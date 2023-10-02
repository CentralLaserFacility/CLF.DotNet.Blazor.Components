using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using Clf.Common.Drawing;
using Clf.Common.ImageProcessing;
using System;
using System.Collections.Generic;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
  public class MultiStateLedViewModel : MonitorWidgetViewModelBase
  {
    private string _ledValue = "";
    public string LedValue
    {
      get => _ledValue;
      set
      {
        SetProperty(ref _ledValue, value);
      }
    }

    private bool _isBlink = false;
    public bool IsBlink
    {
      get => _isBlink;
      set => SetProperty(ref _isBlink, value);
    }

    private bool _isSquare = false;
    public bool IsSquare
    {
      get { return _isSquare; }
      set { SetProperty(ref _isSquare, value); }
    }

    private bool _showIcon;
    public bool ShowIcon
    {
      get { return _showIcon; }
      set { SetProperty(ref _showIcon, value); }
    }

    private BorderStatus _borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get { return _borderStatus; }
      set { SetProperty(ref _borderStatus, value); }
    }

    public ChannelRecord? ChannelRecord { get; private set; }

    public MultiStateLedViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      string? ledValue = null,
      bool ledBlink = false,
      bool isSquare = false,
      bool showIcon = true,
      bool showTooltip = true,
      string? tooltipText = null,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null)
    : base(
        isVisible: isVisible,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width,
        height: height
        )
    {
      _ledValue = ledValue ?? "";
      _isBlink = ledBlink;
      _isSquare = isSquare;
      _showIcon = showIcon;
      _borderStatus = borderStatus;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;
      //Channel/PV creation and LedRecord initialisation
      if (channelRecord?.ChannelsHandler != null
        && !string.IsNullOrWhiteSpace(channelRecord?.ChannelName))
      {
        ChannelRecord = channelRecord;
        ChannelRecord?.InitialiseChannel(
          (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
          (valueInfo, currentState) => SetLed(valueInfo, currentState)
          );
      }
    }

    private void SetLed(ValueInfo valueInfo, ChannelState currentState)
    {
      LedValue = valueInfo.ValueAsString();

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);
    }
  }
}
