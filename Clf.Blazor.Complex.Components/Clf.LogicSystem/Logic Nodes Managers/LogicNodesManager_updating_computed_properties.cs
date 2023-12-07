//
// LogicNodesManager_computedProperties.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using Clf.LogicSystem.LogicNodes.ExtensionMethods;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.LogicNodesManagers
{

  internal partial class LogicNodesManager
  {

    private readonly List<ComputedNodeBase> m_computedNodesThatHaveBeenEvaluated = new List<ComputedNodeBase>() ;
    
    public IEnumerable<ComputedNodeBase> ComputedNodesThatHaveBeenRefreshed => m_computedNodesThatHaveBeenEvaluated ;

    public void ComputedNodesThatHaveBeenRefreshed_Add ( ComputedNodeBase computedNode )
    {
      m_computedNodesThatHaveBeenEvaluated.Add(computedNode) ;
    }

    //
    // !!!!!
    //
    // Rather than recomputing all the affected values here and now,
    // we just flag them as invalidated. Recomputation of the updated values
    // occurs in a separate pass ; in some cases this can be deferred
    // until an 'observer' actually queries the value.
    //
    // We can potentially raise change-notifications on the affected properties
    // without necessarily having recomputed their values. The values
    // will be refreshed as and when a client tries to access the value
    // and the infrastructure discovers that the cached value is not valid.
    // At this point the new value would be computed, and it's only then
    // that we'd discover whether the 'new' value is actually different.
    //
    // We'd have to propagate a change notification to all the other properties
    // whose values could potentially be affected, even if when we find out 
    // that a value change has not actually occurred.
    //

    internal void PropagateSourceValueChange (
      InputNodeBase                                       sourceNodeWhoseValueChanged,
      out LogicNodeChangesArisingFromInputValueChange changesDescriptor
    ) {

      // The 'computedNodesWhoseValueChanged' list will contain references to the Computed nodes
      // that have actually suffered a change in value as a consequence of the input-value change
      // that we submitted. The ordering of the nodes in the list reflects the order in which
      // the Computed nodes were visited as the changes were propagated. 

      DoChangePropagation(
        sourceNodeWhoseValueChanged,
        out var computedNodesWhoseValueChanged
      ) ;

      //
      // All the affected nodes have been invalidated.
      //
      // Now query their Values, in order to trigger a recomputation.
      // Here we guarantee to 'visit' each node that might have changed,
      // and if there are no interdependencies, this will be the order in which
      // updated values are computed. However when evaluating a lambda involves
      // querying other Computed nodes, those will be evaluated first. It's interesting
      // to see the order in which the evaluations happen, so we enable this by setting
      // a flag that will get the nodes added to a list as they're evaluated.
      //

      try
      {
        // When this flag is 'true', the act of accessing the Value of a computed node
        // will cause that node to be added to our 'm_computedNodesThatHaveBeenRefreshed' list,
        // in the 'OnNodeValueBeingComputed_Begin' method. This is useful for debugging,
        // as we can then determine which nodes were recomputed. Note that if a node is recomputed,
        // it won't necessarily appear in the 'computedNodesWhoseValueChanged' list because the
        // new computed value might be identical to the previous computed value.
        IsRefreshingInvalidatedComputedNodes = true ;
        m_computedNodesThatHaveBeenEvaluated.Clear() ;
        //
        computedNodesWhoseValueChanged.ForEachItem(
          (computedNode) => {
            _ = computedNode.ValueAsObject ;  
            computedNode.CachedComputedValueIsBelievedToBeCorrect.Should().BeTrue() ;
            computedNode.CachedComputedValueIsDefinitelyCorrect.Should().BeTrue() ;
          }
        ) ;
      }
      finally
      {
        IsRefreshingInvalidatedComputedNodes = false ;
      }

      //
      // Were there any changes in Computed values ?
      // If yes, then we'll invoke any 'Value-Changed' actions
      // that have been installed into the affected nodes.
      //

      // We expect that the cached values for all Computed properties
      // will have been populated with up-to-date values.

      if ( IsRestoringInputsFromSavedScenario )
      {
        // Hmm, while we're in the process of restoring a Scenario,
        // when we have 'invalidated' all the Computed properties,
        // we won't expect all the cached values to be up-to-date
        // until that process has completed ...
      }
      else
      {
        VerifyComputedPropertyCachedValuesAllUpToDate() ;
      }

      changesDescriptor = new LogicNodeChangesArisingFromInputValueChange(
        sourceNodeWhoseValueChanged,
        computedNodesWhoseValueChanged
      ) ;

      changesDescriptor.WriteConsequentChangesSummary(
        System.Console.WriteLine
      ) ;

    }

    private void DoChangePropagation (
      LogicNode                            sourceNode,
      out IReadOnlyList<ComputedNodeBase> computedNodesWhoseValueChanged
    ) {
      //
      // Proceed as follows :
      // 1. Identify the entire tree of computed nodes whose values
      //    might be affected either directly or indirectly, by the change
      //    in the 'source' value.
      // 2. Visit all those nodes and (A) copy the current 'cached' value
      //    into the 'previous' value, and (B) mark the  node as needing a refresh.
      // 3. Identify all the computed nodes that are 'leaves' of the tree.
      //    That is, nodes which don't feed into any other nodes.
      // 4. Visit each of these leaf nodes and access the Value. 
      //    This will cause the Value to be refreshed, and any computed nodes
      //    whose values feed into the leaf node will also be refreshed.
      // 5. Verify that the entire tree of nodes now has valid Values.
      // 6. Visit every node in the tree and compare the new Value with the
      //    saved 'previous' value. Build a list of those nodes whose values
      //    have actually suffered a change.
      // 7. Invoke a user-supplied function to handle the changed nodes.
      //    This function is passed the entire list of changed nodes,
      //    and it can perform any desired operation on those nodes,
      //    such as immediately invoking a node's 'ValueChanged' action
      //    or scheduling an action to be invoked at some future time.
      //
      var treeOfAffectedNodes = Helpers.AccumulateTargetNodesInfluencedBy(
        sourceNode            : sourceNode,
        accumulateRecursively : true
      ) ;
      treeOfAffectedNodes.ForEachItem(
        computedNode => {
          computedNode.SetPreviousValueFromCurrentValue() ;
          computedNode.DeclareCachedComputedValueMightBeOutOfDate() ;
        }
      ) ;
      treeOfAffectedNodes.ForEachItem(
        computedNode => _ = computedNode.ValueAsObject
      ) ;
      treeOfAffectedNodes.ForEachItem(
        computedNode => _ = computedNode.CachedComputedValueIsDefinitelyCorrect.Should().BeTrue()
      ) ;
      computedNodesWhoseValueChanged = treeOfAffectedNodes.Where(
        node => node.CurrentValueIsDifferentFromPrevious
      ).ToList() ;
    }

    private void PropagateSourceValueChange_InvalidatingAffectedNodes (
      LogicNode                sourceNode,
      IList<ComputedNodeBase> computedNodesThatSufferedChanges
    ) {
      /*PlatformServices.WriteMessageLogLine(
        $"Source node is '{sourceNode.PropertyName}'"
      ) ;*/
      Helpers.ComputedTargetNodesDirectlyContributedToBy(sourceNode).ForEachItem(
        (computedNodeTarget) => PropagateComputedValueChange_InvalidatingAffectedNodes(
          computedNodeTarget,
          computedNodesThatSufferedChanges
        )
      ) ;
    }

    private void PropagateComputedValueChange_InvalidatingAffectedNodes (
      ComputedNodeBase        computedNodeWhoseSourceValueChanged,
      IList<ComputedNodeBase> computedNodesThatSufferedChanges
    ) {
      /*PlatformServices.WriteMessageLogLine(
        $"  Computed target node is '{computedNodeWhoseSourceValueChanged.PropertyName}'"
      ) ;*/
      bool propagateToDependentNodes ;
      /*PlatformServices.WriteMessageLogLine(
        $"  Declaring cached computed value as out-of-date"
      ) ;*/
      computedNodeWhoseSourceValueChanged.DeclareCachedComputedValueMightBeOutOfDate() ;
      propagateToDependentNodes = true ;
      if ( propagateToDependentNodes )
      {
        /*PlatformServices.WriteMessageLogLine(
          $"  Propagating to dependent nodes"
        ) ;*/
        computedNodesThatSufferedChanges.Add(computedNodeWhoseSourceValueChanged) ;
        // Hmm, rather than propagate immediately, we could wait until
        // we've visited all the items at this level ? Because it's possible
        // that an item might be visited more than once.
        PropagateSourceValueChange_InvalidatingAffectedNodes(
          computedNodeWhoseSourceValueChanged,
          computedNodesThatSufferedChanges
        ) ;
      }
    }

    internal void VerifyComputedPropertyCachedValuesAllUpToDate ( )
    {
      ComputedLogicNodes().ForEachItem(
        (node) => node.VerifyCachedComputedPropertyValueIsUpToDate()
      ) ;
    }

    internal void PropagateSourceValueChange (
      InputNodeBase                                                   sourceNodeWhoseValueChanged,
      System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
    ) {
      PropagateSourceValueChange(
        sourceNodeWhoseValueChanged,
        out LogicNodeChangesArisingFromInputValueChange changesDescriptor
      ) ;
      // Note that at this point, the 'changesDescriptor' tells us which nodes
      // have suffered a change in value as a consequence of the change
      // in the Input value. We still need to inform any observers 
      // that a change has occurred - eg to update a View, or to trigger
      // any Actions or Events that need to be actioned given that a change has occurred.
      m_logicSystem.ConsiderInvokingNodeSpecificValueChangedActionsOnAllChangedNodes(changesDescriptor) ;
      m_logicSystem.RaiseValueChangedEventsOnAllChangedNodes(changesDescriptor) ;
      m_logicSystem.LogicNodeValueChangesHandler?.Invoke(changesDescriptor) ;
      changesHandler?.Invoke(changesDescriptor) ;
    }

  }

}
