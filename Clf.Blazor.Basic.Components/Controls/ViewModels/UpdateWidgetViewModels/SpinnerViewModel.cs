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
    public class SpinnerViewModel : UpdateWidgetViewModelBase
  {

    private double _value;
    public double Value
    {
      get => _value;
      set => SetProperty(ref _value, value);
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

    private int _precision = -1;
    public int Precision
    {
      get { return _precision; }
      set { SetProperty(ref _precision, value); }
    }
    private double _minimum = SpinnerStyle.DEFAULT_MIN;
    public double Minimum
    {
      get => _minimum;
      set => SetProperty(ref _minimum, value);
    }

    private double _maximum = SpinnerStyle.DEFAULT_MAX;
    public double Maximum
    {
      get => _maximum;
      set => SetProperty(ref _maximum, value);
    }

    private double _increment = SpinnerStyle.DEFAULT_INCREMENT;
    public double Increment
    {
      get => _increment;
      set => SetProperty(ref _increment, value);
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

    public bool LimitsFromPv { get; set; } = false;

    //Channel/PV Object
    public ChannelRecord? ChannelRecord { get; private set; }

    public SpinnerViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isDisabled = false,
      bool isDisabledOnEnter = false,
      bool waitForAcknowledgement = true,
      double? value = null,
      double? increment = null,
      double? minimum = null,
      double? maximum = null,
      bool showUnits = true,
      string? units = null,
      bool showTooltip = true,
      string? tooltipText = null,
      int precision = -1,     
      BorderStatus borderStatus = BorderStatus.NotConnected,
      bool limitsFromPv=false,
      ChannelRecord? channelRecord = null)
    : base(
        width:width,
        height:height,  
        isVisible: isVisible,
        isDisabled: isDisabled,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        borderStatus: borderStatus
        )
    {
      _value = value??0;
      _minimum = minimum ?? SpinnerStyle.DEFAULT_MIN;
      _maximum = maximum ?? SpinnerStyle.DEFAULT_MAX;
      _increment = increment ?? SpinnerStyle.DEFAULT_INCREMENT;
      _showUnits = showUnits;
      _units = units ?? string.Empty;
      _precision = precision;
      m_IsDisabledOnEnter = isDisabledOnEnter;
      m_waitForAcknowledgement = waitForAcknowledgement;

      LimitsFromPv = limitsFromPv;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
        (valueInfo, currentState) => SetSpinner(valueInfo, currentState)
        );
    }

    private void SetSpinner(ValueInfo valueInfo, ChannelState currentState)
    {
      if (LimitsFromPv && valueInfo.AuxiliaryInfo != null)
      {
        Maximum = Converters.GetDoubleFromObject(valueInfo.AuxiliaryInfo.DRVH);
        Minimum = Converters.GetDoubleFromObject(valueInfo.AuxiliaryInfo.DRVL);
      }

      //setting the Value property that is binded to the view -         
      // the GetPreciseDoubleFromString is a helper method
      // that converts the PV value (already converted to string)
      // into the desired value based on the Precision property
      Value = Converters.GetPrecisionDoubleFromString(valueInfo.ValueAsString(), Precision);

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);

      //setting Units property that is binded to the view 
      if (valueInfo.AuxiliaryInfo?.EGU != null)
        Units = valueInfo.AuxiliaryInfo.EGU;
    }

		public async void OnEnterKeyDownAsync()
		{

			var currentState = ChannelRecord?.Channel?.Snapshot().CurrentState;
			//only try to write to the PV if "Enter" key is pressed
			if (currentState != null && currentState.IsConnected && currentState.FieldInfo != null)
			{
				// Before writing new value to Channel, use this currenState as previousState
				var previousState = currentState;
				try
				{
					//only try to write if the Value to be written is in range
					if (Value >= Minimum && Value <= Maximum)
					{
						//converting the value to the compatible datatype that can be written to the PV
						if (currentState.FieldInfo.DbFieldDescriptor.TryParseValue(Value.ToString(), out var valueToWrite))
						{
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
									Value = Converters.GetPrecisionDoubleFromString(previousState.ValueInfo!.ValueAsString(), Precision);
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
					else // if value is outside the range
						Value = Converters.GetPrecisionDoubleFromString(previousState.ValueInfo!.ValueAsString(), Precision);
				}
				catch (Exception ex)
				{
					ex.ToString(); //TODO: Handle exception in Log... suppressing warning

					//if caught some exception while trying to convert and write to the PV
					//then set the previous value
					Value = Converters.GetPrecisionDoubleFromString(previousState.ValueInfo!.ValueAsString(), Precision);
				}
			}
		}


		public void SetValueFromString(string stringValue)
    {
      var parsedValue = Converters.GetPrecisionDoubleFromString(stringValue, Precision);
      if (double.IsNaN(parsedValue)==false)
      {
        if(parsedValue < Minimum)
          _value = Minimum;
        else if(parsedValue > Maximum)
          _value = Maximum;
        else
          _value = parsedValue;
      }
    }
  }
}
