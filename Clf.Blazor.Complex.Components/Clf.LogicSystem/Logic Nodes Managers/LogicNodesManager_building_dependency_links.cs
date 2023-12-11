//
// LogicNodesManager_buildingDependencyLinks.cs
//

using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Clf.LogicSystem.LogicNodesManagers
{

  internal partial class LogicNodesManager
  {

    public bool AllNodesShouldHaveBeenCreated { get ; private set ; }

    public bool IsBuildingDependencies { get ; private set ; }

    public bool IsRefreshingInvalidatedComputedNodes { get ; internal set ; } = false ;

    // Hmm, this is necessary to avoid running inappropriate validity checks
    // whilst we're restoring a Scenario ... maybe we could avoid this hack
    // by visiting the Scenario properties in the order we established
    // during the initial pass, which accommodates the dependencies ??

    public bool IsRestoringInputsFromSavedScenario { get ; internal set ; } = false ;

    private readonly Stack<ComputedNodeBase> m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed = new () ;

    public bool OneOrMoreComputedNodeValuesAreCurrentlyBeingComputed ( )
    => (
      m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.Any()
    ) ;

    public bool OneOrMoreComputedNodeValuesAreCurrentlyBeingComputed (
      [NotNullWhen(true)] out ComputedNodeBase? computedNodeCurrentlyBeingComputed
    ) => (
      m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.TryPeek(
        out computedNodeCurrentlyBeingComputed
      )
    ) ;

    public ComputedNodeBase? ComputedNodeWhoseValueIsCurrentlyBeingComputed ( )
    => (
      m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.TryPeek(
        out var computedNodeCurrentlyBeingComputed
      )
      ? computedNodeCurrentlyBeingComputed
      : null
    ) ;

    // public bool ComputedNodeValueIsCurrentlyBeingComputed ( ComputedNodeBase node )
    // => m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.Contains(node) ;

    public bool ComputedNodeValueIsCurrentlyBeingComputed (
      [NotNullWhen(true)] out ComputedNodeBase? nodeCurrentlyBeingComputed
    ) => (
      nodeCurrentlyBeingComputed = ComputedNodeWhoseValueIsCurrentlyBeingComputed()
    ) != null ;

    private void MaybeCheckForIllegalCircularDependency ( LogicNode nodeProvidingValue )
    {
      // This code, if enabled, checks for a circular dependency and issues
      // a warning message if it finds one. 
      // We could easily enable this via a configuration variable,
      // but currently the feature is disabled at compile time.
      #if false
        if ( m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.Contains(nodeProvidingValue) )
        {
          PlatformServices.WriteMessageLogLine(
            $"*** Illegal circular dependency detected, involving {nodeProvidingValue.PropertyName}"
          ) ;
          PlatformServices.WriteMessageLogLine(
            "Nodes whose values are currently being computed :"
          ) ;
          PlatformServices.WriteMessageLogLines(
            m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.Select(
              node => $"  {node.PropertyName}"
            ).ToArray()
          ) ;
          PlatformServices.WriteMessageLogLine(
            "*** CONTINUING ***"
          ) ;
          // throw new System.ApplicationException(
          //   $"Circular dependency detected, involving {nodeProvidingValue.Name()}"
          // ) ;
        }
      #else
        return ;
      #endif
    }

    //
    // These three methods are the cornerstone of the 'dependency discovery' scheme.
    // 
    // Hmm, could make this a lambda function in the Node, that is set to null when we're confident that
    // all dependencies have been captured and that this operation is not actually necessary.
    // eg
    //
    //    m_onNodeValueBeingComputed_Begin?.Invoke(this) ;
    //
    // That would mean that rather than always invoke this method only to return immediately
    // in most cases (once the initial discovery had completed), we'd avid the call completely.
    // Might be worth benchmarking that idea ??
    //
    
    //
    // Even better, we should entirely avoid having to discover the dependencies at runtime
    // and use the Roslyn API's to build them from the semantic model as it's built.
    //

    internal void OnNodeValueBeingComputed_Begin ( ComputedNodeBase computedNode )
    {
      if ( IsBuildingDependencies )
      {
        /*PlatformServices.WriteMessageLogLine(
          $"OnNodeValueBeingComputed_BEGIN : {computedNode.PropertyName}"
        ) ;*/
        MaybeCheckForIllegalCircularDependency(computedNode) ;
        m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.Push(computedNode) ;
      }
    }

    //
    // This method is invoked just prior to our returning a Value. 
    // The call informs the LogicNodesManager that a property value is being accessed.
    // If the reason for the access was to fulfil a computation of a Computed property,
    // the Links Manager can register a dependency of that Computed node on the one
    // whose value is being accessed. This registration only needs to be done once,
    // and it will be performed during the initial 'dependency-building scan' 
    // that visits all the properties of the Logic System.
    //

    internal void OnNodeValueBeingAccessed ( LogicNode nodeProvidingValue )
    {
      if ( IsBuildingDependencies )
      {
        if (
          OneOrMoreComputedNodeValuesAreCurrentlyBeingComputed(
            out ComputedNodeBase? computedNodeCurrentlyBeingComputed
          )
        ) {
          MaybeCheckForIllegalCircularDependency(nodeProvidingValue) ;
          // We're computing a refreshed value for a computed 'target' node,
          // and as part of that process we're accessing the value of another
          // node which is a 'source'. So we need to make sure that the
          // dependency is recorded.
          EnsureDependencyExists(
            fromSource : nodeProvidingValue,
            toTarget   : computedNodeCurrentlyBeingComputed
          ) ;
        }
      }
    }

    internal void OnNodeValueBeingComputed_End ( ComputedNodeBase computedNode )
    {
      if ( IsBuildingDependencies )
      {
        /*PlatformServices.Assert(
          m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.Peek() == computedNode
        ) ;*/
        m_computedValueNodesWhoseValuesAreCurrentlyBeingComputed.Pop() ;
       /* PlatformServices.WriteMessageLogLine(
          $"OnNodeValueBeingComputed_END : {computedNode.PropertyName}"
        ) ;*/
      }
    }

  }

}
