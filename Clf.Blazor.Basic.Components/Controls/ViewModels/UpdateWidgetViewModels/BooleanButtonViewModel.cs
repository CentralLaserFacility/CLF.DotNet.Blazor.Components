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
  public class BooleanButtonViewModel : BooleanWidgetViewModelBase
  {

		private Colour? _offColor;
		public Colour? OffColor
		{
			get => _offColor;
			set { SetProperty(ref _offColor, value); }
		}

		private Colour? _onColor;
		public Colour? OnColor
		{
			get => _onColor;
			set { SetProperty(ref _onColor, value); }
		}
		
    private string? _offLabel = BooleanButtonStyle.DEFAULT_OFF_LABEL;
    public string? OffLabel
    {
      get => _offLabel;
      set => SetProperty(ref _offLabel, value ?? BooleanButtonStyle.DEFAULT_OFF_LABEL);
    }

    private string? _onLabel = BooleanButtonStyle.DEFAULT_ON_LABEL;
    public string? OnLabel
    {
      get => _onLabel;
      set => SetProperty(ref _onLabel, value ?? BooleanButtonStyle.DEFAULT_ON_LABEL);
    }

    public BooleanButtonViewModel(
    int? width = null,
    int? height = null,
		Colour? offColor = null,
		Colour? onColor = null,
		string? onLabel = null,
    string? offLabel = null,
    bool showTooltip = true,
    string? tooltipText = null,
    bool isVisible = true,
    bool isDisabled = false,
    bool value=false,
    BorderStatus borderStatus = BorderStatus.NotConnected,
    ChannelRecord? channelRecord = null)
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
      _offLabel = offLabel ?? BooleanButtonStyle.DEFAULT_OFF_LABEL;
      _onLabel = onLabel ?? BooleanButtonStyle.DEFAULT_ON_LABEL;
      _onColor = onColor;
      _offColor = offColor;
    }
  }
}

