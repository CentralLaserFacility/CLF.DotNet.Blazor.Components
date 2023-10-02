using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

//TODO: Use the ActionButton and bind the OnClickAction to call

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class ProcessPVButtonViewModel : ActionButtonViewModel
  {
    public ChannelRecord? ChannelRecord { get; private set; }

    public ProcessPVButtonViewModel(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isDisabled = false,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      string? text = null,
      bool showTooltip = true,
      string? tooltipText = null,
      ChannelRecord? channelRecord = null
      )
      : base(
          width: width,
          height: height,
          isVisible: isVisible,
          isDisabled: isDisabled,
          borderStatus: borderStatus,
          text: text,
          showTooltip: showTooltip,
          tooltipText: tooltipText
          )
    {
      if (!string.IsNullOrWhiteSpace(channelRecord?.ChannelName) && !channelRecord!.ChannelName.EndsWith(".PROC"))
        ChannelRecord = new(channelRecord.ChannelName + ".PROC", channelRecord.ChannelsHandler);
      else
        ChannelRecord = channelRecord;

      TooltipText = TooltipText == string.Empty ? ChannelRecord != null ? ChannelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
        (valueInfo, currentState) => SetProcessPV(valueInfo, currentState)
        );
      OnActionButtonClicked = OnProcessPVButtonClicked; // TODO: Make it not accesible by outside the class
    }

    public void OnProcessPVButtonClicked()
    {
      ChannelRecord?.Channel?.PutValue(Byte.MinValue);
    }
    private void SetProcessPV(ValueInfo valueInfo, ChannelState currentState)
    {
      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);
    }
  }
}
