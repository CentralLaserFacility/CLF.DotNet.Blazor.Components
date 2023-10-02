using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using Clf.Common.Arithmatic;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;
using Clf.Blazor.Basic.Components.Controls.Enums;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
    public class ProgressBarViewModel : MonitorWidgetViewModelBase
  {

    private bool _showValue =true;
    public bool ShowValue
    {
      get => _showValue;
      set => SetProperty(ref _showValue, value);
    }

    private double _maximum = ProgressBarStyle.DEFAULT_MAX;
    public double Maximum
    {
      get { return _maximum; }
      set { SetProperty(ref _maximum, value); }
    }

    private double _minimum = ProgressBarStyle.DEFAULT_MIN;
    public double Minimum
    {
      get { return _minimum; }
      set { SetProperty(ref _minimum, value); }
    }

    private double _value;
    public double Value
    {
      get { return _value; }
      set { SetProperty(ref _value, value); }
    }

    private bool _isIndeterminate = false;
    public bool IsIndeterminate
    {
      get { return _isIndeterminate; }
      set { SetProperty(ref _isIndeterminate, value); }
    }

    private BorderStatus _borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get { return _borderStatus; }
      set { SetProperty(ref _borderStatus, value); }
    }

    private ProgressBarSize _progressbarsize = ProgressBarSize.Regular;
    public ProgressBarSize ProgressBarSize
    {
      get { return _progressbarsize; }
      set { SetProperty(ref _progressbarsize, value); }
    }

    //Channel/PV Object
    public ChannelRecord? ChannelRecord { get; private set; }

    public ProgressBarViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isIndeterminate=false,
      bool showTooltip = true,
      string? tooltipText = null,
      double? minimum = null,
      double? maximum = null,
      double? value = null,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ProgressBarSize progressBarSize = ProgressBarSize.Regular,
      ChannelRecord? channelRecord = null
      )
    : base(
        isVisible: isVisible,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width,
        height: height
        )
    {
      _isIndeterminate = isIndeterminate;
      _minimum = minimum ?? ProgressBarStyle.DEFAULT_MIN;
      _maximum = maximum ?? ProgressBarStyle.DEFAULT_MAX;
      _value = value ?? _minimum;
      _borderStatus = borderStatus;
      _progressbarsize = progressBarSize;
 
      TooltipText = TooltipText == string.Empty ? channelRecord!=null? channelRecord.ChannelName:string.Empty : TooltipText;

      // Create or get the Channel/ PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
        (valueInfo, currentState) => SetProgressBar(valueInfo, currentState)
        );
    }

    private void SetProgressBar(ValueInfo valueInfo, ChannelState currentState)
    {
      //setting the Value property that is binded to the view -         
      // the GetPreciseDoubleFromString is a helper method
      // that converts the PV value (already converted to string)
      // into the desired value based on the Precision property

      //NOTE: we don't have any precision, so we are using
      //this method to just convert the string value to double
      Value = Converters.GetPrecisionDoubleFromString(currentState.ValueInfo!.ValueAsString(), -1);

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);
    }

  }
}
