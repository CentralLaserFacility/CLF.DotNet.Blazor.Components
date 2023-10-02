using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class ComboBoxViewModel : ChoiceWidgetViewModelBase
  {
        public ComboBoxViewModel(
          ObservableCollection<string>? items = null,
          int? width = null,
          int? height = null,
          bool itemsFromPv = false,
          bool isVisible = true,
          bool isDisabled = false,
          BorderStatus borderStatus = BorderStatus.NotConnected,
          string? selectedItem = null,
          bool showTooltip = true,
          string? tooltipText = null,
          ChannelRecord? channelRecord = null)
        : base(
          isVisible: isVisible,
          isDisabled: isDisabled,
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
