// 
// UiServices.cs
//

using Clf.Common.MenuHandling;
using Clf.Common.UI;
using Clf.Common.Utils ;

namespace Clf.LogicSystem.Common.UI
{

  public record UiServices (
    ILogEntriesCollection           LogOfInterestingEvents,
    IProvidesMenuDescriptors        MenuDescriptorsProvider,
    IDialogHandler                  DialogHandler,
    IContextMenuService?            ContextMenuService = null,
    IWindowOpeningService?          WindowOpeningService   = null,
    IProvidesAvailableLogicSystems? AvailableLogicSystemsProvider = null
  ) : IProvidesUiServices ;

}