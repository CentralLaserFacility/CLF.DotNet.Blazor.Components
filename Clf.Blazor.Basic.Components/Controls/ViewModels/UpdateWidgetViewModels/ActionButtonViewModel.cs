using Clf.Blazor.Basic.Components.Controls.Enums;
using Clf.Blazor.Basic.Components.Controls.Models;
using Microsoft.AspNetCore.Components;
using System;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class ActionButtonViewModel : ButtonViewModel
  {
    public Action OnActionButtonClicked { get; set; } = delegate { };

    public ActionButtonViewModel(
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
        borderStatus: borderStatus,
        text: text,
        buttonType: buttonType
        )
    {
    }
  }
}
