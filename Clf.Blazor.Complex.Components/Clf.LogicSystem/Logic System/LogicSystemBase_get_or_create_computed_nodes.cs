//
// LogicSystemBase_get_or_create_computed_nodes.cs
//

using Clf.LogicSystem.LogicNodes;
using System.Runtime.CompilerServices;

namespace Clf.LogicSystem
{

  partial class LogicSystemBase
  {

    protected ComputedNode<T> GetOrCreateComputedNode<T> (
      System.Func<T>                                               computeValueFunc,
      System.Action?                                               valueChangedAction                 = null,
      [CallerMemberName]                                   string? propertyNameSuppliedByCompiler     = null,
      [CallerArgumentExpression(nameof(computeValueFunc))] string  formulaTextLinesSuppliedByCompiler = ""
    ) where T : struct // , System.Numerics.INumber<TCustomAttribute>
    => this.GetOrCreateLogicNode<ComputedNode<T>>(
      createAndInstallLogicNodeIfNotFound : 
      propertyName => new ComputedNode<T>(
        logicSystem        : this,
        propertyName       : propertyName,
        computeValueFunc   : computeValueFunc,
        formulaTextLines   : formulaTextLinesSuppliedByCompiler,
        valueChangedAction : valueChangedAction
      ),
      propertyNameSuppliedByCompiler
    ) ;

  }

}
