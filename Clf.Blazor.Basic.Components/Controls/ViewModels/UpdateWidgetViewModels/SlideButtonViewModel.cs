using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class SlideButtonViewModel : BooleanWidgetViewModelBase
  {
    private Colour? _offColor;
    public Colour? OffColor
    {
      get => _offColor;
      set => _offColor = value;
    }

    private Colour? _onColor;
    public Colour? OnColor
    {
      get => _onColor;
      set => _onColor = value;
    }

    private string _label = string.Empty;
    public string Label
    {
      get => _label;
      set => SetProperty(ref _label, value ?? string.Empty);
    }

    public SlideButtonViewModel(
			int? width = null,
			int? height = null,
			bool value = false,
      bool showTooltip = true,
      bool isVisible = true,
      bool isDisabled = false,
      string? label=null,
      string? tooltipText = null,
      Colour? onColor = null,
      Colour? offColor = null,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null
      )
    : base(
        isVisible: isVisible,
        isDisabled: isDisabled,
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
      _offColor = offColor;
      _onColor = onColor;
    }   
  }
}

