

using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.ViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_CommonPluginSettingsViewModel
  {
    public TextUpdateViewModel PluginAsynPortRBV { get; set; }
    public TextUpdateViewModel PluginTypeRBV { get; set; }
    public TextUpdateViewModel PluginADCoreVersionRBV { get; set; }
    public TextUpdateViewModel PluginVersionRBV { get; set; }
    public TextEntryViewModel PluginArrayPortSet { get; }
    public TextUpdateViewModel PluginArrayPortRBV { get; set; }
    public TextEntryViewModel PluginArrayAddressSet { get; }
    public TextUpdateViewModel PluginArrayAddressRBV { get; set; }
    public ComboBoxViewModel PluginEnableSet { get; }
    public TextUpdateViewModel PluginEnableRBV { get; set; }
    public TextEntryViewModel PluginMinTimeSet { get; }
    public TextUpdateViewModel PluginMinTimeRBV { get; set; }
    public TextEntryViewModel PluginQueueSizeSet { get; }
    public TextUpdateViewModel PluginQueueSizeRBV { get; set; }
    public TextEntryViewModel PluginArrayCounterSet { get; }
    public TextUpdateViewModel PluginArrayCounterRBV { get; set; }
    public TextUpdateViewModel PluginArrayRateRBV { get; set; }
    public TextUpdateViewModel PluginExecutionTimeRBV { get; set; }
    public TextEntryViewModel PluginDroppedArraySet { get; }
    public TextUpdateViewModel PluginDroppedArrayRBV { get; set; }
    public TextUpdateViewModel PluginDimensionsRBV { get; set; }
    public TextUpdateViewModel PluginArraySizeXRBV { get; set; }
    public TextUpdateViewModel PluginArraySizeYRBV { get; set; }
    public TextUpdateViewModel PluginArraySizeZRBV { get; set; }
    public TextUpdateViewModel PluginDataTypeRBV { get; set; }
    public TextUpdateViewModel PluginUniqueIDRBV { get; set; }
    public TextUpdateViewModel PluginTimestampRBV { get; set; }
    public TextUpdateViewModel PluginColourModeRBV { get; set; }
    public ComboBoxViewModel PluginArrayCallbacksSet { get; }
    public TextUpdateViewModel PluginArrayCallbacksRBV { get; set; }
    public IntensityMapFeatures_CommonPluginSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix, string pluginPrefix)
    {
      PluginEnableSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "EnableCallbacks", channelsHandler)
      );
      PluginArrayCallbacksSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArrayCallbacks", channelsHandler)
      );

      PluginAsynPortRBV = new TextUpdateViewModel(
      width: 120,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "PortName_RBV", channelsHandler)
      );
      PluginTypeRBV = new TextUpdateViewModel(
      width: 120,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "PluginType_RBV", channelsHandler)
      );
      PluginADCoreVersionRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ADCoreVersion_RBV", channelsHandler)
      );
      PluginVersionRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "DriverVersion_RBV", channelsHandler)
      );
      PluginArrayPortRBV = new TextUpdateViewModel(
      width: 120,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "NDArrayPort_RBV", channelsHandler)
      );
      PluginArrayAddressRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "NDArrayAddress_RBV", channelsHandler)
      );
      PluginEnableRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "EnableCallbacks_RBV", channelsHandler)
      );
      PluginMinTimeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "MinCallbackTime_RBV", channelsHandler)
      );
      PluginQueueSizeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "QueueFree", channelsHandler)
      );
      PluginArrayCounterRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArrayCounter_RBV", channelsHandler)
      );
      PluginArrayRateRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArrayRate_RBV", channelsHandler)
      );
      PluginExecutionTimeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ExecutionTime_RBV", channelsHandler)
      );
      PluginDroppedArrayRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "DroppedArrays_RBV", channelsHandler)
      );
      PluginDimensionsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "NDimensions_RBV", channelsHandler)
      );
      PluginArraySizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArraySize0_RBV", channelsHandler)
      );
      PluginArraySizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArraySize1_RBV", channelsHandler)
      );
      PluginArraySizeZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArraySize2_RBV", channelsHandler)
      );
      PluginDataTypeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "DataType_RBV", channelsHandler)
      );
      PluginUniqueIDRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "UniqueId_RBV", channelsHandler)
      );
      PluginTimestampRBV = new TextUpdateViewModel(
      width: 100,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "TimeStamp_RBV", channelsHandler)
      );
      PluginColourModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ColorMode_RBV", channelsHandler)
      );
      PluginArrayCallbacksRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArrayCallbacks_RBV", channelsHandler)
      );

      PluginArrayPortSet = new TextEntryViewModel(
      width: 100,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "NDArrayPort", channelsHandler)
      );
      PluginArrayAddressSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "NDArrayAddress", channelsHandler)
      );
      PluginMinTimeSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "MinCallbackTime", channelsHandler)
      );
      PluginQueueSizeSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "QueueSize", channelsHandler)
      );
      PluginArrayCounterSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "ArrayCounter", channelsHandler)
      );
      PluginDroppedArraySet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + pluginPrefix + "DroppedArrays", channelsHandler)
      );
    }
    } 
  }