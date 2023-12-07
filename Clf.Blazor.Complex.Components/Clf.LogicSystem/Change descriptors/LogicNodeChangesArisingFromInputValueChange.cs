//
// LogicNodeChangesArisingFromInputValueChange.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.ChangeDescriptors
{

  public sealed class LogicNodeChangesArisingFromInputValueChange 
  {

    public InputNodeBase ChangedInputNode { get ; }

    public IEnumerable<ComputedNodeBase> ChangedComputedNodes { get ; }

    public IEnumerable<LogicNode> AllChangedNodes => (
      (
        ChangedInputNode as LogicNode
      ).WithAdditionalItems(
        ChangedComputedNodes.Cast<LogicNode>()
      )
    ) ;

    public LogicNodeChangesArisingFromInputValueChange (
      InputNodeBase                 inputNodeThatChanged,
      IEnumerable<ComputedNodeBase> computedNodesThatWereAffected
    ) {
      ChangedInputNode     = inputNodeThatChanged ;
      ChangedComputedNodes = computedNodesThatWereAffected ;
    }

    public override string ToString ( )
    => Clf.Common.Helpers.BuildMultiLineString(
      WriteConsequentChangesSummary
    ) ;

    public string AsSingleLineSummary 
    => (
      $"Input node '{ChangedInputNode.PropertyName}' => '{ChangedInputNode.ValueAsString}' ; "
    + $"{ChangedComputedNodes.Count()} computed nodes changed"
    ) ;

    public void WriteConsequentChangesSummary ( System.Action<string> writeLineFunc )
    {
      writeLineFunc(
        ""
      ) ;
      if ( ChangedComputedNodes.Any() )
      {
        writeLineFunc(
          $"{ChangedInputNode.PropertyName} => {ChangedInputNode.ValueAsString} HAD {ChangedComputedNodes.Count()} CONSEQUENCES :"
        ) ;
        ChangedComputedNodes.ForEachItem(
          changedComputedNode => writeLineFunc(
            $"  {changedComputedNode.PropertyName} => {changedComputedNode.ValueAsString}"
          )
        ) ;
      }
      else
      {
        writeLineFunc(
          $"{ChangedInputNode.PropertyName} => {ChangedInputNode.ValueAsString} had no consequences"
        ) ;
      }
    }

  }

}
