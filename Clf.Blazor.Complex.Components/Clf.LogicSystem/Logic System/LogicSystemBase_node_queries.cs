//
// LogicSystemBase_node_queries.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem
{

  //
  // These methods are useful when we're creating visualisations
  // of an 'interesting subset' of a Clf.Clf.LogicSystem, for example a display
  // that shows just the Nodes that 'feed into' a particular Output node.
  //

  public partial class LogicSystemBase
  {

    // Given a single node, typically an Input node ... which Computed nodes are influenced by it ?

    public IEnumerable<ComputedNodeBase> QueryComputedNodesDirectlyOrIndirectlyInfluencedBy (
      LogicNode logicNode 
    ) => (
      Helpers.ComputedTargetNodesDirectlyOrIndirectlyContributedToBy(logicNode) 
    ) ;

    // Given a set of nodes, typically Input nodes ... which Computed nodes are influenced by them ?

    public IEnumerable<ComputedNodeBase> QueryComputedNodesDirectlyOrIndirectlyInfluencedBy (
      IEnumerable<LogicNode> logicNodes 
    ) {
      HashSet<ComputedNodeBase> influencedNodes = new() ;
      logicNodes.ForEachItem(
        logicNode => influencedNodes.UnionWith(
          QueryComputedNodesDirectlyOrIndirectlyInfluencedBy(logicNode)
        )
      ) ;
      return influencedNodes ;
    }

    // Given a single Computed node, typically an 'output' node ... which 'source' nodes influence its Value ?

    public IEnumerable<LogicNode> QuerySourceNodesDirectlyOrIndirectlyContributingTo (
      ComputedNodeBase computedLogicNode 
    ) => (
      Helpers.AccumulateSourceNodesInfluencing(
        computedLogicNode,
        accumulateRecursively : true
      ) 
    ) ;

    // Given a set of Computed nodes, typically 'output' nodes ... which 'source' nodes influence their Values ?

    public IEnumerable<LogicNode> QuerySourceNodesDirectlyOrIndirectlyContributingTo (
      IEnumerable<ComputedNodeBase> computedLogicNodes 
    ) {
      HashSet<LogicNode> influencingNodes = new() ;
      computedLogicNodes.ForEachItem(
        computedLogicNode => influencingNodes.UnionWith(
          QuerySourceNodesDirectlyOrIndirectlyContributingTo(computedLogicNode)
        )
      ) ;
      return influencingNodes ;
    }

    public IEnumerable<LogicNode> QueryAllNodesDirectlyOrIndirectlyContributingToOrInfluencedBy (
      LogicNode logicNode 
    ) => (
      QueryComputedNodesDirectlyOrIndirectlyInfluencedBy(
        logicNode
      ).Concat(
        logicNode is ComputedNodeBase computedLogicNode
        ? QuerySourceNodesDirectlyOrIndirectlyContributingTo(
            computedLogicNode
          )
        : Enumerable.Empty<ComputedNodeBase>()
      )
    ) ;

    // Given a collection of LogicNode instances, identify all 'related' nodes

    public IEnumerable<LogicNode> QueryAllNodesDirectlyOrIndirectlyContributingToOrInfluencedBy (
      IEnumerable<LogicNode> logicNodes
    ) {
      var result = new HashSet<LogicNode>() ;
      logicNodes.ForEachItem(
        logicNode => result.UnionWith(
          QueryAllNodesDirectlyOrIndirectlyContributingToOrInfluencedBy(logicNode)
        )
      ) ;
      return result ;
    }

    // Given a set of Nodes, create a string containing an ordered list of
    // the node ID's. This 'hash' can be used to uniquely characterise a Layout
    // that has been computed for that set of nodes.

    public string GetNodesSubsetHash ( 
      IEnumerable<LogicNode> logicNodes 
    ) 
    => logicNodes.OrderBy(
      id => id
    ).Select(
      id => id.ToString()
    ).AsSingleLine(",") ;

    public ChannelAndValueDescriptorsList GetChannelAndValueDescriptorsList ( )
    {
      ChannelAndValueDescriptorsList result = new() ;
      AllLogicNodes.ForEachItem(
        logicNode => {
          result.Add(
            new ChannelNameAndValueDescriptor(
              logicNode.ChannelName,
              // logicNode.LogicNodeCategory,
              logicNode.ValueAsString
            )
          ) ;
        }
      ) ;
      return result ;
    }

  }

}
