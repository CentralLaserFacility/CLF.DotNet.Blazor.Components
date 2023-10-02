using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels
{
  public class ContainerWidgetViewModelBase : WidgetViewModelBase
  {
    public IList<IWidgetViewModelBase> ChildWidgetsList { get; } = new List<IWidgetViewModelBase>();
    
    private string? _title;
    public string? Title
    {
      get { return _title; }
      set { SetProperty(ref _title, value); }
    }
    
    public ContainerWidgetViewModelBase(
          int? width = 0,
          int? height = 0,
          bool isVisible = true,
          string? title = null
          )
          : base(
              isVisible: isVisible,
              width: width,
              height: height
              )
    {
      _title = title ?? string.Empty;
    }
  }
}
