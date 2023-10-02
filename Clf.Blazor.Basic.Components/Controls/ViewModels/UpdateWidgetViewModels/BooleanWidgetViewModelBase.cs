using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using Clf.Common.Arithmatic;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
    public class BooleanWidgetViewModelBase: UpdateWidgetViewModelBase
  {
    private bool _value = false;
    public bool Value
    {
      get => _value;
      set => SetProperty(ref _value, value);
    }

    public ChannelRecord? ChannelRecord { get; private set; }

    public BooleanWidgetViewModelBase(
      int? width = null,
      int? height = null,
      bool value = false,
      bool showTooltip = true,
      bool isVisible = true,
      bool isDisabled = false,
      string? fontStyle = null,
      string? tooltipText = null,    
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null
      )
      :base(
        isVisible: isVisible,
        isDisabled: isDisabled,
        fontStyle: fontStyle,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width, 
        height: height,
        borderStatus: borderStatus
        )
    {
      _value = value;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
        (valueInfo, currentState) => SetBooleanWidget(valueInfo, currentState)
        );
    }

    private void SetBooleanWidget(ValueInfo valueInfo, ChannelState currentState)
    {
      
      if (valueInfo.Value != null)
      {
        Value = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
      }

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);
    }

    public async void OnBooleanWidgetClicked(MouseEventArgs e)
    {
      var currentState = ChannelRecord?.Channel?.Snapshot().CurrentState;
      //e.Button: Left button=0, middle button=1 (if present), right button=2
      if (e.Button == 0 && currentState != null && currentState.IsConnected && currentState.FieldInfo != null)
      {
        bool previousValue = Value;
        try
        {
          // Toggle the boolen value
          Value = !Value;

          string stringToWrite = Value ? "1" : "0";
          //converting the stringToWrite to the compatible datatype that can be written to the PV
          if (currentState.FieldInfo.DbFieldDescriptor.TryParseValue(
              stringToWrite,
              out var valueToWrite
            ))
            //writing value to the PV and if PutValueResult is not Success that undo toggle
            if (await ChannelRecord!.Channel!.PutValueAsync(valueToWrite) != PutValueResult.Success)
              Value = previousValue;
        }
        catch (Exception ex)
        {
          ex.ToString(); //TODO: Handle exception in Log... suppressing warning
          // Undo the boolen toggle if an exeption is thrown
          Value = previousValue;
          // TODO: Log Exception
        }
      }
    }

  }
}
