using Clf.ChannelAccess;
using Microsoft.AspNetCore.Components.Web;
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

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
    public class TextEntryViewModel : UpdateWidgetViewModelBase
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

    private bool _showUnits=true;
    public bool ShowUnits
    {
      get { return _showUnits; }
      set => SetProperty(ref _showUnits, value);
    }

    private bool _isMultiLine=false;
    public bool IsMultiLine
    {
      get { return _isMultiLine; }
      set { SetProperty(ref _isMultiLine, value); }
    }


    private int _precision=-1;
    public int Precision
    {
        get { return _precision; }
        set { SetProperty(ref _precision, value); }
    }

    private bool m_IsDisabledOnEnter;
    public bool IsDisabledOnEnter
    {
      get { return m_IsDisabledOnEnter; }
      set { m_IsDisabledOnEnter = value; }
    }


    private bool m_waitForAcknowledgement;
    public bool WaitForAcknowledgement
    {
      get { return m_waitForAcknowledgement; }
      set { SetProperty(ref m_waitForAcknowledgement, value); }
    }
   
    //Channel/PV Object
    public ChannelRecord? ChannelRecord { get; private set; }

    public TextEntryViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isDisabled = false,
      bool isDisabledOnEnter = false,
      bool waitForAcknowledgement = true,
      bool showTooltip = true,
      string? text = null,
      string? tooltipText = null,
      bool showUnits = true,
      string? units=null,
      bool isMultiLine = false,
      int precision = -1,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null)
    : base(
        isVisible: isVisible,
        isDisabled: isDisabled,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width:width,
        height:height,
        borderStatus: borderStatus
        )
    {      
      _text = text??string.Empty;
      _showUnits = showUnits;
      _units = units??string.Empty;
      _isMultiLine = isMultiLine;
      _precision =precision;
      m_IsDisabledOnEnter = isDisabledOnEnter;
      m_waitForAcknowledgement = waitForAcknowledgement;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
        (valueInfo, currentState) => SetTextEntry(valueInfo, currentState)
        );
    }
    
    private void SetTextEntry( ValueInfo valueInfo, ChannelState currentState)
    {
      //setting the Text property that is binded to the view -         
      // the GetPreciseText is a helper method
      // that converts the PV value object
      // into a string based on the Precision property
      Text = Converters.GetPrecisionText(valueInfo.Value, Precision);

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);

      //setting Units property that is binded to the view 
      if (!string.IsNullOrEmpty(valueInfo.AuxiliaryInfo?.EGU))
        Units = valueInfo.AuxiliaryInfo.EGU;
    }

    public async Task OnEnterKeyDownAsync(KeyboardEventArgs e)
    {
      var currentState = ChannelRecord?.Channel?.Snapshot().CurrentState;
      //only try to write to the PV if "Enter" key is pressed
      if ((e.Code == "Enter" || e.Code == "NumpadEnter") && currentState != null && currentState.IsConnected && currentState.FieldInfo != null)
      {
        // Before writing new value to Channel, use this currenState as previousState
        var previousState = currentState;
        try
        {
          object? valueToWrite = null;

          if (currentState.FieldInfo.DbFieldDescriptor.DbFieldType == DbFieldType.DBF_CHAR_byte 
            || currentState.FieldInfo.DbFieldDescriptor.TryParseValue(Text,out valueToWrite))
          {
            if (valueToWrite == null)
              valueToWrite = Converters.GetByteArrayFromString(Text, currentState.FieldInfo.DbFieldDescriptor.ElementsCountOnServer);

            if (WaitForAcknowledgement)
            {
              // Disable this component on enter.
              // Make sure that this is re-enabled outside of this viewmodel
              if (IsDisabledOnEnter)
              {
                IsDisabled = true;
              }
              // PutValueAckAsync will wait until the value
              // has been accepted on the server side
              // and that a message has been received notifying
              // us of the new current value.  and returns true if accepted.
              //and if PutValueResult is not Success then Undo change
              if (await ChannelRecord!.Channel!.PutValueAckAsync(valueToWrite) != PutValueResult.Success)
              {
                Text = Converters.GetPrecisionText(previousState?.ValueInfo?.Value, Precision);
                if (IsDisabledOnEnter)
                {
                  IsDisabled = false;
                }
              }
            }
            else
            {
              ChannelRecord!.Channel!.PutValue(valueToWrite);
            }
          }
        }
        catch (Exception ex)
        {
          // TODO: Throw / Log Exception
          ex.ToString(); //TODO: Handle exception in Log... suppressing warning
          //if caught some exception while trying to convert and write to the PV
          //then set the previous value
          Text = Converters.GetPrecisionText(previousState.ValueInfo?.Value, Precision);
        }
      }
    }
  }
}
