using Clf.Blazor.Basic.Components.Controls.Enums;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class DropDownButtonViewModel: UpdateWidgetViewModelBase
  {
    private string _text = string.Empty;
    public string Text
    {
      get { return _text; }
      set => SetProperty(ref _text, value ?? string.Empty);
    }

    public DropDownButtonViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isDisabled = false,
      string? text = null,
      bool showTooltip = true,
      string? tooltipText = null
      )
    : base(
        isVisible: isVisible,
        isDisabled: isDisabled,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width,
        height: height,
        borderStatus: BorderStatus.Connected
        )
    {
      _text = text ?? string.Empty;
    }


  }
}
