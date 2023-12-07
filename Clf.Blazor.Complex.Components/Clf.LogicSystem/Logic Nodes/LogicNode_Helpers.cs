//
// LogicNode_Helpers.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.LogicNodes
{

  public static partial class Helpers
  {

    //
    // These are useful functions that don't make sense as Extension Methods,
    // largely because it's impossible to think of a name that would properly
    // indicate their purpose when used as 'x.DoSomething(y)'.
    //
    // So, better to package them as static 'Helper' functions 
    // that are invoked as
    //
    //   Helpers.DoSomething(x,y)
    //

    public static IEnumerable<ComputedNodeBase> AccumulateTargetNodesInfluencedBy (
      LogicNode                       sourceNode,
      bool                            accumulateRecursively,
      System.Func<ComputedNodeBase,bool>? filter_optional        = null
    ) => (
      sourceNode.AccumulateLinkedItems(
        node => ComputedTargetNodesDirectlyContributedToBy(node),
        accumulateRecursively,
        filter_optional
      )
    ) ;

    // public static IEnumerable<LogicNode> AccumulateSourceNodesInfluencing (
    //   ComputedNodeBase            computedNode,
    //   bool                         accumulateRecursively,
    //   System.Func<LogicNode,bool>? filter_optional = null
    // ) => (
    //   computedNode.GetLinkedItems(
    //     node => Helpers.GetSourceNodesDirectlyContributingTo(node),
    //     accumulateRecursively,
    //     filter_optional
    //   )
    // ) ;

    // Hmm, needs testing more thoroughly ...

    public static IEnumerable<LogicNode> AccumulateSourceNodesInfluencing (
      LogicNode                    logicNode,
      bool                         accumulateRecursively,
      System.Func<LogicNode,bool>? filter_optional        = null
    ) => (
      logicNode.AccumulateLinkedItems(
        node => Helpers.GetSourceNodesDirectlyContributingTo(node,filter_optional),
        accumulateRecursively,
        filter_optional
      )
    ) ;

    public static IEnumerable<ComputedNodeBase> ComputedTargetNodesDirectlyContributedToBy (
      LogicNode node
    ) => (
      node.LogicNodesManager.Dependencies.Where(
        link => link.FromSource == node
      ).Select(
        link => link.ToTarget
      )
    ) ;

    public static IEnumerable<ComputedNodeBase> ComputedTargetNodesDirectlyOrIndirectlyContributedToBy (
      LogicNode                       sourceNode,
      System.Func<ComputedNodeBase,bool>? filter_optional = null
    ) => (
      AccumulateTargetNodesInfluencedBy(
        sourceNode            : sourceNode,
        accumulateRecursively : true,
        filter_optional
      )
    ) ;

    public static bool IsOutputNode_NotFeedingIntoOtherComputedNodes ( this LogicNode node )
    => (
      node.FeedsIntoOneOrMoreComputedNodesThatAreDirectDependents() is false
    ) ;

  }

}
