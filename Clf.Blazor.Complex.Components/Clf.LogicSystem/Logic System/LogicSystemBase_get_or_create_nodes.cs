//
// LogicSystemBase_get_or_create_nodes.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase
  {

    //
    // The Logic System contains a 'flat' collection of Logic Nodes
    // that represent input 'value' properties and computed 'value' properties.
    //

    //
    // The 'LogicSystemBase' class owns a LogicNodesManager instance that maintains a repository
    // of nodes and links that represent the Properties declared in the concrete
    // subclass.
    //
    // The subclass constructor is expected to invoke 'BuildDependencies()'
    // which will scan the Properties and build the nodes and the dependencies. 
    //

    internal TLogicNode GetOrCreateLogicNode<TLogicNode> (
      System.Func<string,TLogicNode>                             createAndInstallLogicNodeIfNotFound,
      [System.Runtime.CompilerServices.CallerMemberName] string? nodeNameSuppliedByCompiler           = null
    )
    where TLogicNode : LogicNode
    {
      string nodeName = nodeNameSuppliedByCompiler.VerifiedAsNonNullInstance() ;
      if (
        LogicNodesManager.LogicNodeExists(
          logicNodeName :  nodeName,
          out var logicNode
        ) is false
      ) {
        if ( LogicNodesManager.AllNodesShouldHaveBeenCreated )
        {
          throw new Clf.Common.Utils.UnreachableException(
            $"Node '{nodeName}' should have already been created !!"
          ) ;
        }
        // When a 'flat' node of the specified name doesn't already exist,
        // we invoke a client-supplied function that creates the node
        // and installs it. This is done simply by invoking the constructor
        // that takes the name. The execution of the constructor will register
        // the new node at the 'flat' level.
        {
          TLogicNode logicNodeJustCreated = createAndInstallLogicNodeIfNotFound(nodeName) ;
          bool logicNodeNowExists = LogicNodesManager.LogicNodeExists(
            logicNodeName : nodeName,
            out var logicNodeFound
          ) is true ;
          /*PlatformServices.Assert(logicNodeNowExists) ;
          PlatformServices.Assert(
            logicNodeFound == logicNodeJustCreated
          ) ;*/
        }
      }
      return (TLogicNode) LogicNodesManager.LookupLogicNode(nodeName) ;
    }
    
  }

}
