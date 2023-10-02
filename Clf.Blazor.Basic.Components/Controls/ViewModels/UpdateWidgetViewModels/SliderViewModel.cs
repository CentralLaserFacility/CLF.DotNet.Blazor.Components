using Clf.ChannelAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Threading.Tasks;
using Clf.Common.Arithmatic;
using Clf.Common.Drawing;
using System.ComponentModel;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
	public class SliderViewModel : UpdateWidgetViewModelBase
	{
    private double _minimum = SliderStyle.DEFAULT_MIN;
		public double Minimum
		{
			get { return _minimum; }
			set { SetProperty(ref _minimum, value); }
		}

		private double _maximum = SliderStyle.DEFAULT_MAX;
		public double Maximum
		{
			get { return _maximum; }
			set { SetProperty(ref _maximum, value); }
		}

		private double _value;
		public double Value
		{
			get { return _value; }
			set { SetProperty(ref _value, value); }
		}

		private double _increment = SliderStyle.DEFAULT_INCREMENT;
		public double Increment
		{
			get { return _increment; }
			set { SetProperty(ref _increment, value); }
		}

		public bool LimitsFromPv { get; set; } = false;

		public ChannelRecord? ChannelRecord { get; private set; }

		public SpinnerViewModel SpinnerViewModel { get; private set; }

		public bool ShowValue { get; private set; }

    public bool ShowSpinner { get; private set; }

    public bool ShowTicks { get; private set; }
    public int TickInterval { get; private set; }

		public SliderViewModel(
			bool isVisible = true,
			bool isDisabled = false,
			BorderStatus borderStatus = BorderStatus.NotConnected,
			bool showTooltip = true,
			string? tooltipText = null,
			int? width = null,
			int? height = null,
			double? minimum = null,
			double? maximum = null,
			double? value = null,
			double? increment = 1,
			bool limitsFromPv = false,
			bool showValue = true,
			bool showSpinner = false,
			bool showTicks = false,
			int tickInterval = 10,
      ChannelRecord? channelRecord = null)
		: base(
					isVisible: isVisible,
					isDisabled: isDisabled,
					tooltipText: tooltipText,
					showTooltip: showTooltip,
					width: width,
					height: height,
					borderStatus: borderStatus
				)
		{
			_minimum = minimum ?? SliderStyle.DEFAULT_MIN;
			_maximum = maximum ?? SliderStyle.DEFAULT_MAX;
			_value = value ?? _minimum;
			_increment = increment ?? SliderStyle.DEFAULT_INCREMENT;
			LimitsFromPv = limitsFromPv;
			ShowValue = showValue;
			ShowSpinner = showSpinner;
			ShowTicks = showTicks;
			TickInterval = tickInterval;

			TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      SpinnerViewModel = new SpinnerViewModel
                              (isVisible: ShowSpinner,
                                isDisabled: GetDisableStatus(),
                                showUnits: false,
                                width: 80,
                                tooltipText: TooltipText,
                                showTooltip: ShowTooltip,
                                borderStatus: BorderStatus.Connected,
                                minimum: Minimum,
                                maximum: Maximum,
                                value: Value,
                                increment: Increment);

			this.PropertyChanged += OnSliderPropertyChanged;

      // Create or get the Channel/PV object
      ChannelRecord = channelRecord;
			ChannelRecord?.InitialiseChannel(
				(isConnected, currentState) =>
				{
					BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected);
				},
				(valueInfo, currentState) => SetSlider(valueInfo, currentState)
				);

		}

		private void OnSliderPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Value):
					SpinnerViewModel.Value = Value;
					break;
				case nameof(Minimum):
					SpinnerViewModel.Minimum = Minimum;
					break;
				case nameof(Maximum):
					SpinnerViewModel.Maximum = Maximum;
					break;
				case nameof(Increment):
					SpinnerViewModel.Increment = Increment;
					break;
				case nameof(IsDisabled):
					SpinnerViewModel.IsDisabled = GetDisableStatus();
					break;
				case nameof(IsVisible):
					SpinnerViewModel.IsVisible = IsVisible;
					break;
				case nameof(BorderStatus):
					SpinnerViewModel.IsDisabled = GetDisableStatus();
					break;
				default:
					break;
			}
		}

    private void SetSlider(ValueInfo valueInfo, ChannelState currentState)
		{
			//Setting minimum and maximum limits from PV only once
			if (LimitsFromPv && valueInfo.AuxiliaryInfo != null)
			{
				Maximum = Converters.GetDoubleFromObject(valueInfo.AuxiliaryInfo.DRVH);
				Minimum = Converters.GetDoubleFromObject(valueInfo.AuxiliaryInfo.DRVL);
			}
			Value = Converters.GetDoubleFromObject(valueInfo.Value);
			BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);
		}

		public async void OnSliderValueChange(ChangeEventArgs e)
		{
			var currentState = ChannelRecord?.Channel?.Snapshot().CurrentState;
			var input = Converters.GetDoubleFromObject(e.Value);
			if (input != double.NaN)
			{
				Value = input;
				if (currentState != null && currentState.IsConnected && currentState.FieldInfo != null)
				{
					var previousValue = Value;
					try
					{
						if (currentState.FieldInfo.DbFieldDescriptor.TryParseValue(
								input.ToString(),
								out var valueToWrite
							))
							//writing value to the PV and if PutValueResult is not Success then undo
							if (await ChannelRecord!.Channel!.PutValueAsync(valueToWrite) != PutValueResult.Success)
								Value = previousValue;
					}
					catch (Exception ex)
					{
						ex.ToString(); //TODO: Handle exception in Log... suppressing warning
						Value = previousValue;
					}
				}
			}
		}
  }
}
