//
// LogicSystemViewModel_services.cs
//

using Clf.LogicSystem.Common ;
using Clf.LogicSystem.Common.UI;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicSystemViewModel
  {

    // COULD BE USING DI !!!

    public IProvidesUiServices UiServices { get ; }

    // This synchronisation should be performed by a Service we inject ???

    private void Invoke ( System.Action action )
    {
      // System.Windows.Application.Current.Dispatcher.Invoke(action) ;
      action() ;
    }

  }

}
