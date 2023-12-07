//
// LogicSystemViewModel_logging.cs
//

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicSystemViewModel
  {

    internal static void LogMouseEventAction ( Clf.Common.UI.MouseEventDescriptor mouseEvent, ILogicSystemViewModelElement viewModelElement )
    {
      Clf.Common.Utils.DebugHelpers.WriteDebugLines(
        $"{mouseEvent} ; {viewModelElement}"
      ) ;
    }

    //
    // Here we define the default for the action to be performed whenever a Value suffers a change.
    // The default is to write a message line to the debug log.
    //

    internal static System.Action<string>? ValueWasChangedLoggingAction_Default = (
      // null
      (line) => Clf.Common.Utils.DebugHelpers.WriteDebugLine(line)
    ) ;

  }

}

