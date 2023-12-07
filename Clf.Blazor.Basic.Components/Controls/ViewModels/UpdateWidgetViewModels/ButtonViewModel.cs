using Clf.Blazor.Basic.Components.Controls.Enums;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class ButtonViewModel: UpdateWidgetViewModelBase
  {
    private string _text = string.Empty;
    public string Text
    {
      get { return _text; }
      set => SetProperty(ref _text, value ?? string.Empty);
    }

    private ButtonType _type = ButtonType.Default;
    public ButtonType Type
    {
      get { return _type; }
      set => SetProperty(ref _type, value);
    }

    public ButtonViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isDisabled = false,
      BorderStatus borderStatus = BorderStatus.Connected,
      string? text = null,
      bool showTooltip = true,
      string? tooltipText = null,
      ButtonType buttonType = ButtonType.Default
      )
    : base(
        isVisible: isVisible,
        isDisabled: isDisabled,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width,
        height: height,
        borderStatus: borderStatus
        )
    {
      _text = text ?? string.Empty;
      _type = buttonType;
    }
  }
}
