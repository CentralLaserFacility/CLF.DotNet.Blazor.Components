using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class RadioButtonViewModel : ChoiceWidgetViewModelBase
  {    
      public RadioButtonViewModel(
      ObservableCollection<string>? items = null,
      int? width = null,
      int? height = null,
      bool isVisible=true,
      bool isDisabled = false,
      bool isHorizontal=true,
      bool itemsFromPv=false,
      Colour? foregroundColor = null,
      string? selectedItem = null,
      bool showTooltip = true,
      string? fontStyle = null,
      string? tooltipText = null,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null)
    : base(
      isVisible: isVisible,
      isDisabled: isDisabled,
      fontStyle: fontStyle,
      tooltipText: tooltipText,
      showTooltip: showTooltip,
      width: width,
      height: height,
      borderStatus: borderStatus,
      items: items,
      selectedItem: selectedItem,
      itemsFromPv: itemsFromPv,
      channelRecord: channelRecord
      )
    {
    }
  }
}
