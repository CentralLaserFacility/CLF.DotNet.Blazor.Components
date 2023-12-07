//
// LogicSystemBase_submitting_input_changes.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase 
  {

    //
    // This is the primary API for submitting an Input-Node change.
    //
    // The consequences of the change, ie changes in Computed nodes,
    // are reported 'all at once' via a collection that identifies the
    // Computed nodes whose Value suffered a change. 
    //
    // This is a safer mechanism than the alternative design whereby
    // you might install a 'ValuePropertyChanged' action handler in each Node.
    // Those event handlers would need to be individually de-registered 
    // when a client that had installed them is about to cease to exist.
    //

    public void SubmitInputChange<T> (
      InputNode<T>                                                  valuedInputNode,
      T?                                                            changedValue,
      System.Action<LogicNodeChangesArisingFromInputValueChange>?   changesHandler = null
    ) where T:struct {
      valuedInputNode.SetValue(
        changedValue,
        changesHandler
      ) ;
    }

    public bool CanSubmitInputChange_ParsedFromString (
      InputNodeBase                                               inputNode,
      string                                                      changedValue,
      System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
    ) {
      return inputNode.CanSetValue_ParsedFromString(
        changedValue,
        changesHandler
      ) ;
    }

    public void SetAllBooleanInputValues ( bool? value )
    {
      NodesOfType<InputNode<bool>>().ForEachItem(
        node => SubmitInputChange(
          node,
          value
        )
      ) ;
    }

    public void SetAllNullableInputValuesNull ( )
    {
      // Safest way is to specify a string, as this will
      // propagate the change for any Value type ...
      NodesOfType<InputNodeBase>().ForEachItem(
        node => CanSubmitInputChange_ParsedFromString(
          node,
          null
        ) // .WithDebugBreakOnFalseValue()
      ) ;
    }

    #if SUPPORTS_SHADOW_VALUES
    public void SubmitShadowOutputChange (
      ComputedNodeBase outputNode,
      bool?        shadowValue
    ) {
      outputNode.ShadowValue = shadowValue ;
    }
    #endif

  }

}
