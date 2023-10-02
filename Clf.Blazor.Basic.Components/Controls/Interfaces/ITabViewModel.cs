using Clf.Blazor.Basic.Components.Controls.ViewModels;
using System.Collections.Generic;

namespace Clf.Blazor.Basic.Components.Controls.Interfaces
{
  public interface ITabViewModel
  {
    IList<IWidgetViewModelBase> ChildWidgetsList { get;}
    bool IsActive { get; set; }
  }
}