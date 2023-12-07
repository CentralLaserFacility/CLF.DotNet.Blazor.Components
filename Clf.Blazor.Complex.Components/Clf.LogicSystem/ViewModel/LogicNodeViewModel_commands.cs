//
// LogicNodeViewModel_commands.cs
//

using CommunityToolkit.Mvvm.Input;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicNodeViewModel
  { 

    public IRelayCommand DoSomethingCommand { get ; set ; }

    private void InitialiseCommands ( )
    {
      DoSomethingCommand = new RelayCommand(
        execute : () => { 
          // This lambda function can access properties and methods
          // defined in the host object. Note that there's no way
          // to pass in a parameter, but that doesn't matter.
          // The original ICommand.Execute() function can take 
          // an 'object' argument, but that was never a good idea.
        },
        canExecute : () => (
          // Here we can access properties defined in the host object
          // and decide whether this command should be made available.
          true
        )
      ) ;
    }

    private void RefreshCommandCanExecuteProperties ( )
    {
      DoSomethingCommand.NotifyCanExecuteChanged() ;
    }

  }

}

