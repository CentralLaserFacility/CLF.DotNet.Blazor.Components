//
// LogicNodeViewModel_action_methods.cs
//

using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.LogicNodes;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicNodeViewModel 
  { 

    public bool IsBooleanInputNode => LogicNode is InputNode<bool> booleanInputNode ;

    public void ToggleBooleanInputNodeValue ( System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null )
    {
      if ( this.LogicNode is InputNode<bool> booleanOrNullInputNode )
      {
        booleanOrNullInputNode.ToggleValue(
          valueToSetIfNull : true,
          changesHandler   : changesHandler
        ) ;
      }
    }

    public void RemoveHighlighting ( ) => ApplyHighlighting(false) ;

    public void ApplyHighlighting ( bool applyOrRemove = true )
    {
      HighlightingChoice = (
        applyOrRemove is true
        ? Clf.Common.UI.HighlightingOption.Highlighted
        : Clf.Common.UI.HighlightingOption.NotHighlighted
      ) ;
    }

  }

}

