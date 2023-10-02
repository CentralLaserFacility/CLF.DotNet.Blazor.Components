using Clf.Blazor.Basic.Components.Controls.Enums;
using Clf.Blazor.Basic.Components.Controls.Models;
using Microsoft.AspNetCore.Components;
using System;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class ActionButtonViewModel : UpdateWidgetViewModelBase
  {

    // 2 enums for action type -> style
    // icon id for icon
    private ActionButtonType _actionButtonType = ActionButtonType.Default;
    public ActionButtonType Type {
      get { return _actionButtonType; }
      set => SetProperty(ref _actionButtonType, value);
    }

    private string _text=string.Empty;
    public string Text
    {
      get { return _text; }
      set => SetProperty(ref _text, value ?? string.Empty);
    }

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
      ActionButtonType buttonType = ActionButtonType.Default
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
      _text = text??string.Empty;
      _actionButtonType = buttonType ;
    }
  }
}
