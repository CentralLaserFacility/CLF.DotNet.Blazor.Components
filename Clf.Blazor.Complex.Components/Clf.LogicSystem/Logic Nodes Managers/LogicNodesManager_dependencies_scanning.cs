//
// LogicNodesManager_dependenciesScanning.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.LogicNodesManagers
{

  internal partial class LogicNodesManager
  {

    // Enumerate all the Computed nodes that are present,
    // and for each one, identify the Source nodes that
    // directly contribute to that Computed node's value.

    public void ScanSourceNodesDirectlyContributingToComputedNodes (
      System.Action<
        ComputedNodeBase,
        IEnumerable<LogicNode>
      > handleResult
    ) => this.ComputedLogicNodes().ForEachItem(
      (computedValueNode) => {
        handleResult(
          computedValueNode,
          Helpers.GetSourceNodesDirectlyContributingTo(
            computedValueNode
          )
        ) ;
      }
    ) ;

    public void ScanSourceNodesDirectlyContributingToComputedNodes (
      System.Action<string,IEnumerable<string>> handleResult
    ) => ScanSourceNodesDirectlyContributingToComputedNodes(
      (computedNode,inputs) => {
        handleResult(
          computedNode.PropertyName,
          inputs.Select(
            node => node.PropertyName
          )
        ) ;
      }
    ) ;

    public void ScanSourceNodesDirectlyContributingToComputedNodes (
      System.Action<string> writeLine
    ) {
      writeLine("Computed node contributions :") ;
      ScanSourceNodesDirectlyContributingToComputedNodes(
        (node,contributors) => writeLine(
          $"Computed node '{node.PropertyName}' is influenced by source nodes : {contributors.BuildItemNamesList()}"
        )
      ) ;
    }

    // Enumerate all the Input nodes and for each one,
    // identify all the Computed nodes that it contributes to, 
    // either directly or indirectly.

    public void ScanAllInputNodeInfluencesOnComputedNodes (
      System.Action<
        LogicNode,
        IEnumerable<ComputedNodeBase>
      > handleResult
    ) => this.InputNodes().ForEachItem(
      (inputNode) => {
        handleResult(
          inputNode,
          Helpers.AccumulateTargetNodesInfluencedBy(
            sourceNode            : inputNode,
            accumulateRecursively : true
          )
        ) ;
      }
    ) ;

    public void ScanAllInputNodeInfluencesOnComputedNodes (
      System.Action<string,IEnumerable<string>> handleResult
    ) => ScanAllInputNodeInfluencesOnComputedNodes(
      (inputNode,targetNodes) => handleResult(
        inputNode.PropertyName,
        targetNodes.Select(
          node => node.PropertyName
        )
      )
    ) ;

    public void ScanAllInputNodeInfluencesOnComputedNodes (
      System.Action<string> writeLine
    ) {
      writeLine("Computed node contributions :") ;
      ScanAllInputNodeInfluencesOnComputedNodes(
        (node,contributors) => writeLine(
          $"Computed node '{node.PropertyName}' is influenced by mutable value nodes : {contributors.BuildItemNamesList()}"
        )
      ) ;
    }

  }

}
