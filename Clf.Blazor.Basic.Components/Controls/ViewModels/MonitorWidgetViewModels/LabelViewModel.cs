using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
    public class LabelViewModel : MonitorWidgetViewModelBase
  {
    private string _text = string.Empty;
    public string Text
    {
      get => _text;
      set => SetProperty(ref _text, value ?? string.Empty);
    }

    public LabelViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool showTooltip = true,
      string? text = null,
      string? tooltipText = null)
    : base(
        isVisible: isVisible,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width,
        height: height
        )
    {
      _text = text ?? string.Empty;
    }
  }
}
