using Clf.ChannelAccess;
using Microsoft.AspNetCore.Components.Web;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class CheckboxViewModel : BooleanWidgetViewModelBase
  {
    private string _label = string.Empty;
    public string Label
    {
      get => _label;
      set => SetProperty(ref _label, value ?? string.Empty);
    }

    public CheckboxViewModel(
      int? width = null,
      int? height = null,
      bool value = false,
      bool isVisible = true,
      bool isDisabled = false,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      string? label = null,
      bool showTooltip = true,
      string? fontStyle = null,
      string? tooltipText = null,
      ChannelRecord? channelRecord = null)
    : base(
        isVisible: isVisible,
        isDisabled: isDisabled,
        fontStyle: fontStyle,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width,
        height: height,
        borderStatus: borderStatus,
        value: value,
        channelRecord: channelRecord
        )
    {
      _label = label ?? string.Empty;
    }

  }
}