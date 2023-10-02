using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;

using Clf.Blazor.Basic.Components.Controls.Widgets.Monitors;
using Clf.Blazor.Basic.Components.Controls.ViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_KafkaTabPluginSettingsViewModel
  {
    public IntensityMapFeatures_CommonPluginSettingsViewModel KafkaCommonPluginSettingsTab { get; }
    public string pageFileTitle;
    private string kafkaPluginPrefix;
    public TextUpdateViewModel KafkaConnectionStatusRBV { get; set; }
    public TextUpdateViewModel KafkaStatusMessageRBV { get; set; }
    public TextEntryViewModel KafkaBrokerAddress { get; set; }
    public TextUpdateViewModel KafkaBrokerAddressRBV { get; set; }
    public TextEntryViewModel KafkaBrokerTopic { get; set; }
    public TextUpdateViewModel KafkaBrokerTopicRBV { get; set; }
    public TextEntryViewModel KafkaStatsInterval { get; set; }
    public TextUpdateViewModel KafkaStatsIntervalRBV { get; set; }
    public TextUpdateViewModel KafkaArrayCounterRBV { get; set; }
    public IntensityMapFeatures_KafkaTabPluginSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix)
    {
      kafkaPluginPrefix = "Kafka1:";
      KafkaCommonPluginSettingsTab = new IntensityMapFeatures_CommonPluginSettingsViewModel(channelsHandler,pvPrefix, kafkaPluginPrefix);
      pageFileTitle = "Kafka Plugin Settings  " + pvPrefix + kafkaPluginPrefix;
      KafkaConnectionStatusRBV = new TextUpdateViewModel(
        width: 300,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "ConnectionStatus_RBV", channelsHandler)
      );
      KafkaStatusMessageRBV = new TextUpdateViewModel(
        width: 300,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "ConnectionMessage_RBV", channelsHandler)
      );
      KafkaBrokerAddress = new TextEntryViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "KafkaBrokerAddress", channelsHandler)
      );
      KafkaBrokerAddressRBV = new TextUpdateViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "KafkaBrokerAddress_RBV", channelsHandler)
      );
      KafkaBrokerTopic = new TextEntryViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "KafkaTopic", channelsHandler)
      );
      KafkaBrokerTopicRBV = new TextUpdateViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "KafkaTopic_RBV", channelsHandler)
      );
      KafkaStatsInterval = new TextEntryViewModel(
        width: 75,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "KafkaStatsIntervalTime", channelsHandler)
      );
      KafkaStatsIntervalRBV = new TextUpdateViewModel(
        width: 75,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "KafkaStatsIntervalTime_RBV", channelsHandler)
      );
      KafkaArrayCounterRBV = new TextUpdateViewModel(
        width: 75,
        channelRecord: new ChannelRecord(pvPrefix + kafkaPluginPrefix + "ArrayCounter_RBV", channelsHandler)
      );
    }
  }
}