//
// LogicNodesManager_queries_node_lookup.cs
//

using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Clf.LogicSystem.LogicNodesManagers
{

  internal partial class LogicNodesManager
  {

    public IEnumerable<LogicNode> AllLogicNodes => LogicNodesDictionary.Values ;

    public bool LogicNodeExists (
      string                             logicNodeName,
      [NotNullWhen(true)] out LogicNode? node
    ) => (
      LogicNodesDictionary.TryGetValue(
        logicNodeName,
        out node
      )
    ) ;

    // Returns the nodes that can be cast to type 'TNode'.
    // The name of the LINQ 'OfType()' method is unfortunate,
    // as it would suggest that we'll only get instances whose
    // type is 'exactly' TNode ?

    public IEnumerable<TNode> LogicNodesOfType<TNode> ( ) where TNode : LogicNode
    => AllLogicNodes.OfType<TNode>() ;

    // Returns the nodes that can be cast to type 'InputNodeBase',
    // that is, all Nodes whose type is a subclass of InputNodeBase.

    public IEnumerable<InputNodeBase> InputNodes ( )
    => (
      AllLogicNodes.OfType<InputNodeBase>()
    ) ;

    public IEnumerable<ComputedNodeBase> ComputedLogicNodes ( )
    => (
      AllLogicNodes.OfType<ComputedNodeBase>()
    ) ;

    public bool LogicNodeExists ( string logicNodeName ) 
    => (
      LogicNodeExists(
        logicNodeName,
        out _
      )
    ) ;

    public LogicNode LookupLogicNode ( string logicNodeName ) 
    => (
      LogicNodeExists(
        logicNodeName,
        out var node
      )
      ? node
      : throw new System.ApplicationException(
          $"Node '{logicNodeName}' not found"
        )
    ) ;

  }

}
