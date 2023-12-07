// 
// IProvidesUiServices.cs
//

using Clf.Common.MenuHandling;
using Clf.Common.UI;
using Clf.Common.Utils;

namespace Clf.LogicSystem.Common.UI
{
  public interface IProvidesUiServices
  {
    
    ILogEntriesCollection           LogOfInterestingEvents  { get ; }

    IProvidesMenuDescriptors        MenuDescriptorsProvider { get ; }

    IDialogHandler                  DialogHandler           { get ; }

    IContextMenuService?            ContextMenuService { get; }

    IWindowOpeningService?          WindowOpeningService { get ; } 

    IProvidesAvailableLogicSystems? AvailableLogicSystemsProvider { get ; }

  }

}