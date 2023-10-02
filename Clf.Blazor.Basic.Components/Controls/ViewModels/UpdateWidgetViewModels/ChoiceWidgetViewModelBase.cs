using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
    public class ChoiceWidgetViewModelBase : UpdateWidgetViewModelBase
  {
    public Action OnSelectionChange { get; set; } = delegate { };
    public bool ItemsFromPv { get; private set; } = false;
    public bool HasItemsSetFromPv { get; private set; } = false;
    public ObservableCollection<string> Items { get; init; }

    private string _selectedItem = string.Empty;
    public string SelectedItem
    {
      get => _selectedItem;
      set => SetProperty(ref _selectedItem, value ?? string.Empty);
    }

    //Channel/PV Object
    public ChannelRecord? ChannelRecord { get; private set; }

    public ChoiceWidgetViewModelBase(
      ObservableCollection<string>? items = null,
      int? width = null,
      int? height = null,
      bool itemsFromPv = false,
      bool showTooltip = true,
      string? selectedItem = null,
      string? fontStyle = null,
      string? tooltipText = null,
      bool isVisible = true,
      bool isDisabled = false,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? channelRecord = null
      )
    : base(
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
      Items = items ?? new ObservableCollection<string>();
      _selectedItem = selectedItem ?? Items.FirstOrDefault() ?? string.Empty;
      ItemsFromPv = itemsFromPv;

      TooltipText = TooltipText == string.Empty ? channelRecord != null ? channelRecord.ChannelName : string.Empty : TooltipText;

      //Create or get the Channel/PV object
      ChannelRecord = channelRecord;
      ChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => 
        { 
          BorderStatus = Utilities.GetBorderStatus(currentState.ValueInfo?.AlarmStatusAndSeverity, isConnected);
          //If Channel is disconnected, when the next time it will again get connected,
          //we should fetch the Items again if ItemsFromPv is true
          if (!isConnected)
            HasItemsSetFromPv = false;
        },
        (valueInfo, currentState) => SetChoiceWidgets(valueInfo, currentState)
        );
    }

    private void SetChoiceWidgets(ValueInfo valueInfo, ChannelState currentState)
    {
      //populating items list if ItemsFromPv is true and there is enum information from the PV
      //only add items if HasItemsSetFromPv is false
      //so that we only add to the Items once from the PV

      if (!HasItemsSetFromPv && ItemsFromPv && currentState.FieldInfo?.DbFieldDescriptor?.EnumNames != null)
      {
        Items.Clear();
        //adding all the items in the EnumOptionNames to the Items property that is binded to the view
        currentState.FieldInfo.DbFieldDescriptor.EnumNames.ToList().ForEach(x => Items.Add(x));
        HasItemsSetFromPv = true;
      }
      //setting the SelectedItem property from the PV value that is binded to the view
      SetSelectedItem(valueInfo.Value);

      // Setting BorderStatus - which shows the border around the control based on the connection and alarm status
      BorderStatus = Utilities.GetBorderStatus(valueInfo.AlarmStatusAndSeverity, currentState.IsConnected);
    }

    private void SetSelectedItem(object? value)
    {
      //Should we throw exceptions from components? Check same for ComboBox
      if (value is null || value is System.Exception exception)
      {
        //TODO: Log it somewhere
      }
      else if (value is short || value is int)
      {
        //if value is short or int, it means that PV has index value
        int index = Convert.ToInt32(value);

        //assigning the SelectedItem based on the index value
        if (index >= 0 && index < Items!.Count)
          SelectedItem = Items[index];
        else
        {
          //TODO: Log it somewhere
        }
      }
      else
      {
        //for when PV is of any other type apart from short and int
        //the valid value can be of type string containing the selected item's string
        var item = value?.ToString() ?? string.Empty;

        //in that case check if Items list contain the PV value (item)
        //if true then assign that to the SelectedItem
        if (Items.Contains(item))
          SelectedItem = item;
        else
        {
          //TODO: Log it somewhere
        }
      }
    }

    public async void WriteSelectedItemToPV()
    {
      var currentState = ChannelRecord?.Channel?.Snapshot().CurrentState;
      if (currentState != null && currentState.IsConnected && currentState.FieldInfo != null)
      {
        try
        {
          string stringToWrite = string.Empty;

          //converting the SelectedItem to the string that can be passed to the helper method from ChannelAccessHelpers
          if (currentState.ValueInfo?.Value is short || currentState.ValueInfo?.Value is int)
            stringToWrite = Items.IndexOf(SelectedItem).ToString();
          else
            stringToWrite = SelectedItem;

          //converting the stringToWrite to the compatible datatype that can be written to the PV
          if (currentState.FieldInfo.DbFieldDescriptor.TryParseValue(
              stringToWrite,
              out var valueToWrite
            ))
          {
            //writing value to the PV and if PutValueResult is not Success then Undo change
            if (await ChannelRecord!.Channel!.PutValueAsync(valueToWrite) != PutValueResult.Success)
              SetSelectedItem(currentState.ValueInfo?.Value);
          }
        }
        catch (Exception ex)
        {
          ex.ToString(); //TODO: Handle exception in Log... suppressing warning
          //if caught some exception while trying to convert and write to the PV
          //then set the previous value as the SelectedItem
          SetSelectedItem(currentState.ValueInfo?.Value);
        }
      }
    }
  }
}
