//
// DialogHandler.cs
//

using Clf.Common.UI;

namespace Clf.Blazor.Common.Dialogs;

public class DialogHandler : IDialogHandler
{

  public void ShowNameAndDescriptionEditingPanel (
    string                        prompt, 
    string                        initialNameValue, 
    string                        initialDescriptionValue, 
    System.Action<string,string>  okAction, 
    System.Action?                cancelledAction = null
  ) {
    // TODO !!!
  }

  public void ShowTextEntryPanel ( 
    string                 prompt, 
    string                 initialValue, 
    System.Action<string>? okAction        = null, 
    System.Action?         cancelledAction = null
  ) {
    // TODO !!!
  }

  public void ShowOptionSelectionPanel ( 
    string                 prompt, 
    string                 initialValue, 
    System.Action<string>? okAction        = null, 
    System.Action?         cancelledAction = null
  ) {
    // TODO !!!
  }

  public void ShowChannelValueChangeSimulationPanel ( 
    string                 prompt, 
    string                 initialValue, 
    System.Action<string>? okAction        = null, 
    System.Action?         cancelledAction = null
  ) {
    // TODO !!!
  }

  public void ShowMessageBox ( string caption, string textLines )
  {
    // TODO !!!
    throw new System.NotImplementedException();
  }

  public bool ShowMessageBox_AskingYesOrNo ( string caption, string textLines )
  {
    return false ;
  }

}


