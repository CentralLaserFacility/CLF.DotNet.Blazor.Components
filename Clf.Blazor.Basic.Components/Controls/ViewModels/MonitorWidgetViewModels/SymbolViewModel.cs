using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
    public class SymbolViewModel : MonitorWidgetViewModelBase
  {
    public List<string> Symbols { get; set; } = new List<string>();

    private int _selectedSymbol;
    public int SelectedSymbol
    {
      get { return _selectedSymbol; }
      set { SetProperty(ref _selectedSymbol, value); }
    }

    private bool _preserveRatio;
    public bool PreserveRatio
    {
      get { return _preserveRatio; }
      set { SetProperty(ref _preserveRatio, value); }
    }

    private BorderStatus _borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get { return _borderStatus; }
      set { SetProperty(ref _borderStatus, value); }
    }

    // Channel/PV object
    public ChannelRecord? ChannelRecord { get; private set; }

    public SymbolViewModel(
      int width=0,
      int height=0,
      List<string>? symbols=null,
      int selectedSymbol = 0,
      bool preserveRatio=true,
      bool showTooltip = true,
      string? fontStyle = null,
      string? tooltipText = null,
      bool isVisible = true,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null
      )
      : base(
        isVisible: isVisible,
        fontStyle: fontStyle,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width <= 0 ? SymbolStyle.DEFAULT_WIDTH : width,
        height: height <= 0 ? SymbolStyle.DEFAULT_HEIGHT : height
        )
    {
      Symbols = symbols ?? new List<string>();
      _selectedSymbol = selectedSymbol;
      _preserveRatio = preserveRatio;
      _borderStatus = borderStatus;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected); },
        (valueInfo, currentState) => SetSelectedSymbol(valueInfo, currentState)
        );
    }

    private void SetSelectedSymbol(ValueInfo valueInfo, ChannelState currentState)
    {
      try
      {
        SelectedSymbol = Convert.ToInt32(valueInfo.Value);
      }
      catch(Exception ex)
      {
        ex.ToString(); //TODO: Handle exception in Log... suppressing warning
      }
      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);

    }

  }
}
