using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
    public class ColorPickerViewModel: UpdateWidgetViewModelBase
  {
    private string _selectedColorString;
    public string SelectedColorString
    {
      get { return _selectedColorString; }
      set
      {
        SetProperty(ref _selectedColorString, value ?? ColorPickerStyle.DEFAULT_COLOR);
        WriteSelectedColor();
      }
    }

    private bool m_waitForAcknowledgement;
    public bool WaitForAcknowledgement
    {
      get { return m_waitForAcknowledgement; }
      set { SetProperty(ref m_waitForAcknowledgement, value); }
    }

    //Channel/PV Object
    public ChannelRecord? ChannelRecord { get; private set; }

    public ColorPickerViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isDisabled = false,
      bool waitForAcknowledgement = true,
      bool showTooltip = true,
      string? selectedColorString = null,
      string? tooltipText = null,
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
        borderStatus: borderStatus
        )
    {
      _selectedColorString = selectedColorString?? ColorPickerStyle.DEFAULT_COLOR;
      m_waitForAcknowledgement = waitForAcknowledgement;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
        (valueInfo, currentState) => { SelectedColorString = valueInfo.ValueAsString(); }
        );
    }

    private async void WriteSelectedColor()
    {
      var currentState = ChannelRecord?.Channel?.Snapshot().CurrentState;
      var currentValue = ChannelRecord?.Channel?.Snapshot().CurrentState.ValueInfo?.Value.ToString();
      if (currentState != null && currentState.IsConnected && currentState.FieldInfo != null
            && currentValue != null && currentValue != SelectedColorString)
      {
        try
        {
          if (currentState.FieldInfo.DbFieldDescriptor.TryParseValue(SelectedColorString, out var valueToWrite))
          {
            if (WaitForAcknowledgement)
            {
              if (await ChannelRecord!.Channel!.PutValueAckAsync(valueToWrite) != PutValueResult.Success)
              {
                SelectedColorString = currentValue.ToString();
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
          ex.ToString(); //TODO: Handle exception in Log... suppressing warning
          SelectedColorString = currentValue.ToString();
        }
      }
    }
  }
}
