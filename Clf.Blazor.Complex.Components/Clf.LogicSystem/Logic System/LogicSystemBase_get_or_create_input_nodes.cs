//
// LogicSystemBase_get_or_create_input_nodes.cs
//

using Clf.LogicSystem.LogicNodes;

namespace Clf.LogicSystem
{

  partial class LogicSystemBase
  {
    /// <summary>
    /// Get or create input node
    /// </summary>
    /// <param name="valueChangedAction"></param>
    /// <param name="propertyNameSuppliedByCompiler"></param>
    /// <returns></returns>
    protected InputNode GetOrCreateInputNode (
      System.Action?                                             valueChangedAction             = null,
      [System.Runtime.CompilerServices.CallerMemberName] string? propertyNameSuppliedByCompiler = null
    ) 
    => this.GetOrCreateLogicNode(
      createAndInstallLogicNodeIfNotFound : 
      propertyName => new InputNode(
        logicSystem : this,
        propertyName,
        valueChangedAction
      ),
      propertyNameSuppliedByCompiler
    ) ;

    /// <summary>
    /// Generic method to get or create input node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="valueChangedAction"></param>
    /// <param name="propertyNameSuppliedByCompiler"></param>
    /// <returns></returns>
    protected InputNode<T> GetOrCreateInputNode<T> (
      System.Action?                                             valueChangedAction             = null,
      [System.Runtime.CompilerServices.CallerMemberName] string? propertyNameSuppliedByCompiler = null
    ) where T : struct
    => this.GetOrCreateLogicNode(
      createAndInstallLogicNodeIfNotFound : 
      propertyName => new InputNode<T>(
        logicSystem : this,
        propertyName,
        valueChangedAction
      ),
      propertyNameSuppliedByCompiler
    ) ;
  }

}
