using Clf.Blazor.Basic.Components.Controls.Enums;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.Widgets.Monitors;
using Clf.Common.ImageProcessing;

namespace Clf.Blazor.Basic.Components.Controls.Models
{
  public static class LedStyleExtensions
  {
    public static void SetBinaryLedStyle(this LedViewModel ledViewModel)
    {
      ledViewModel.LedType = LedType.Binary;
    }

    public static void SetErrorLedStyle(this LedViewModel ledViewModel)
    {
      ledViewModel.LedType = LedType.Error;
    }

    public static void SetWarningLedStyle(this LedViewModel ledViewModel)
    {
      ledViewModel.LedType = LedType.Warning;
    }
  }
}

