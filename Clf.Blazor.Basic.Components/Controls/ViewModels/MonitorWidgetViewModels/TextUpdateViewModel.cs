using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Clf.Common.Arithmatic;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
  public class TextUpdateViewModel : MonitorWidgetViewModelBase
  {
  
    private string _text = string.Empty;
    public string Text
    {
      get => _text;
      set => SetProperty(ref _text, value ?? string.Empty);
    }

    private string _units = string.Empty;
    public string Units
    {
      get => _units;
      set => SetProperty(ref _units, value ?? string.Empty);
    }

    private bool _showUnits = true;
    public bool ShowUnits
    {
      get { return _showUnits; }
      set => SetProperty(ref _showUnits, value);
    }

    private BorderStatus _borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get { return _borderStatus; }
      set { SetProperty(ref _borderStatus, value); }
    }


    private int _precision = -1;
    public int Precision
    {
      get { return _precision; }
      set 
      { 
        SetProperty(ref _precision, value);
        var isParsed = double.TryParse(Text, out var result);
        if (isParsed)
          Text = Converters.GetPrecisionText(result, _precision);
      }
    }

    
    //Channel/PV Object
    public ChannelRecord? ChannelRecord { get; private set; }

    public TextUpdateViewModel(
      int? width = null,
      int? height = null,
      bool isVisible=true,
      string? text = null,
      bool showTooltip = true,
      string? tooltipText = null,
      bool showUnits = true,
      string? units = null,
      int precision = -1,
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
      _text = text ?? string.Empty;
      _showUnits = showUnits;
      _units = units ?? string.Empty;
      _precision = precision;
      _borderStatus = borderStatus;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) =>{ BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected);},
        (valueInfo, currentState) => SetTextUpdate(valueInfo, currentState)
        );
    }

    private void SetTextUpdate(ValueInfo valueInfo, ChannelState currentState)
    {
      //setting the Text property that is binded to the view -         
      SetText(valueInfo);

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
        BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);

      //setting Units property that is binded to the view 
      if (!string.IsNullOrEmpty(valueInfo.AuxiliaryInfo?.EGU))
        Units = valueInfo.AuxiliaryInfo.EGU;
    }

    private void SetText(ValueInfo valueInfo)
    {
      // If the PV is of type enum
      if (valueInfo.FieldInfo?.DbFieldDescriptor?.EnumNames != null
          && valueInfo.Value != null
          && valueInfo.Value is short
          && ((short)valueInfo.Value) >= 0
          && ((short)valueInfo.Value) < valueInfo.FieldInfo?.DbFieldDescriptor?.EnumCount)
        Text = valueInfo.FieldInfo?.DbFieldDescriptor?.EnumNameAsString((short)valueInfo.Value)??string.Empty;
      // When PV is of any other type
      else
        // GetPreciseText is a helper method
        // that converts the PV value object
        // into a string based on the Precision property
        Text = Converters.GetPrecisionText(valueInfo.Value, Precision);
    }
  }
}
