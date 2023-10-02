using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;
using Clf.Blazor.Basic.Components.Controls.Enums;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
    public class LedViewModel : MonitorWidgetViewModelBase
  {
    private bool _ledValue = false;
    public bool LedValue
    {
      get => _ledValue;
      set => SetProperty(ref _ledValue, value);
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

    private string _offLabel;
    public string OffLabel
    {
      get { return _offLabel; }
      set { SetProperty(ref _offLabel, value ?? LedStyle.DEFAULT_OFF_LABEL); }
    }

    private string _onLabel;
    public string OnLabel
    {
      get { return _onLabel; }
      set { SetProperty(ref _onLabel, value ?? LedStyle.DEFAULT_ON_LABEL); }
    }

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

    private BorderStatus _borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get { return _borderStatus; }
      set { SetProperty(ref _borderStatus, value); }
    }

    private LedType _ledType = LedType.Default;
    public LedType LedType
    {
      get { return _ledType; }
      set { SetProperty(ref _ledType, value); }
    }

    public LedChannelRecord? LedChannelRecord { get; private set; }

    public LedViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool ledValue = false,
      bool ledBlink = false,
      bool isSquare = false,
      bool showIcon = true,
      string? offLabel = null,
      string? onLabel = null,
      bool showTooltip = true,
      string? tooltipText = null,
      Colour? offColor = null,
      Colour? onColor = null,
      LedType ledType = LedType.Default,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      LedChannelRecord? ledChannelRecord = null)
    : base(
        isVisible: isVisible,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width,
        height: height
        )
    {
      _ledValue = ledValue;
      _isBlink = ledBlink;
      _isSquare = isSquare;
      _showIcon = showIcon;
      _offColor = offColor;
      _onColor = onColor;
      _offLabel = offLabel ?? LedStyle.DEFAULT_OFF_LABEL;
      _onLabel = onLabel ?? LedStyle.DEFAULT_ON_LABEL;
      _ledType = ledType;
      _borderStatus = borderStatus;

      TooltipText = TooltipText == string.Empty ? ledChannelRecord != null ? ledChannelRecord.ChannelName : string.Empty : TooltipText;
      //Channel/PV creation and LedRecord initialisation
      if (ledChannelRecord?.ChannelsHandler != null
        && !string.IsNullOrWhiteSpace(ledChannelRecord?.ChannelName)
        && !string.IsNullOrWhiteSpace(ledChannelRecord?.LedOnValue))
      {
        LedChannelRecord = ledChannelRecord;
        LedChannelRecord?.InitialiseChannel(
          (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
          (valueInfo, currentState) => SetLed(valueInfo, currentState)
          );
      }
    }

    private void SetLed(ValueInfo valueInfo, ChannelState currentState)
    {
      // Comparing the value from PV to the LedOnValue and setting the UI LedValue property
      LedValue = valueInfo.ValueAsString() == LedChannelRecord!.LedOnValue ? true : false;

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      // Led does not need Major or Minor Alarms, even if the underlying PV has these alarms set.
      var borderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);
      if(borderStatus== BorderStatus.MajorAlarm || borderStatus== BorderStatus.MinorAlarm) 
      {  
        BorderStatus = BorderStatus.Connected;
      }
      else
        BorderStatus = borderStatus;
    }
  }
}
