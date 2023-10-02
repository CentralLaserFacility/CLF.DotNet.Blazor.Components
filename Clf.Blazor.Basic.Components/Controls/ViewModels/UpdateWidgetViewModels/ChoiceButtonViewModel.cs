using Clf.Blazor.Basic.Components.Controls.Models;
using System.Collections.ObjectModel;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class ChoiceButtonViewModel : ChoiceWidgetViewModelBase
  {
    private bool _isHorizontal = false;
    public bool IsHorizontal
    {
      get { return _isHorizontal; }
      set { SetProperty(ref _isHorizontal, value); }
    }

    private int? _buttonWidth = null;
    public int? ButtonWidth
    {
      get { return _buttonWidth; }
      set { SetProperty(ref _buttonWidth, value); }
    }

    private int? _buttonHeight = null;
    public int? ButtonHeight
    {
      get { return _buttonHeight; }
      set { SetProperty(ref _buttonHeight, value); }
    }

    public ChoiceButtonViewModel(
      ObservableCollection<string>? items = null,
      int? buttonWidth = null,
      int? buttonHeight = null,
      bool itemsFromPv = false,
      bool showTooltip = true,
      bool isHorizontal = false,
      string? selectedItem = null,
      string? tooltipText = null,
      bool isVisible = true,
      bool isDisabled = false,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null)
    : base(
      isVisible: isVisible,
      isDisabled: isDisabled,
      tooltipText: tooltipText,
      showTooltip: showTooltip,
      borderStatus: borderStatus,
      items:items,
      selectedItem:selectedItem,
      itemsFromPv:itemsFromPv,
      channelRecord:channelRecord
      )
    {
      _isHorizontal = isHorizontal;
      _buttonWidth = buttonWidth;
      _buttonHeight = buttonHeight;
    }
  }
}

