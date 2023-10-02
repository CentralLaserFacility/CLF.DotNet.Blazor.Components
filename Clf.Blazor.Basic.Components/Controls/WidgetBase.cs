using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Common.Drawing;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Basic.Components.Controls
{
  public class WidgetBase : ComponentBase
  {
    [Parameter]
    public string Class { get; set; } = "";

    [Obsolete]
    [Parameter]
    public bool IsPositionRelative { get; set; } = true;

    [Obsolete]
    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public bool IsVisible { get; set; } = true;

    [Obsolete]
    [Parameter]
    public int Width { get; set; }

    [Obsolete]
    [Parameter]
    public int Height { get; set; }

    [Obsolete]
    [Parameter]
    public string FontStyle { get; set; } = Models.FontStyle.DEFAULT_FONT_STYLE;

    public void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      InvokeAsync(StateHasChanged);
    }
    public void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
      InvokeAsync(StateHasChanged);
    }
  }
}
