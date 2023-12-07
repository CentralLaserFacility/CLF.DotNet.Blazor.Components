//
// LogicSystemBase_value_changed_actions.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.Common.ExtensionMethods;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase
  {

    public void ConsiderInvokingNodeSpecificValueChangedActionsOnAllChangedNodes ( LogicNodeChangesArisingFromInputValueChange changes )
    {
      if ( ShouldInvokeNodeSpecificValueActionChangeHandlers )
      {
        changes.ChangedInputNode.ValueChangedAction?.Invoke() ;
        changes.ChangedComputedNodes.ForEachItem(
          computedNodeThatWasAffected => computedNodeThatWasAffected.ValueChangedAction?.Invoke() 
        ) ;
      }
    }

    public void RaiseValueChangedEventsOnAllChangedNodes ( LogicNodeChangesArisingFromInputValueChange changes )
    {
      changes.ChangedInputNode.RaiseValueChangedEvent() ;
      changes.ChangedComputedNodes.ForEachItem(
        computedNodeThatWasAffected => computedNodeThatWasAffected.RaiseValueChangedEvent() 
      ) ;
    }

  }

}

