using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;

using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_KafkaTabViewModel
  {
    private string kafkaPluginPrefix;
    public ActionButtonViewModel StartKafkaStreaming { get; }
    public ActionButtonViewModel StopKafkaStreaming { get; }
    public TextUpdateViewModel KafkaStreamingRBV { get; set; }
    public TextUpdateViewModel KafkaConnectionStatusRBV { get; set; }
    public ActionButtonViewModel KafkaPluginSettings { get; }
    private ActionButtonViewModel CreateValueWriteActionButtonViewModel(string channelName, object valueToWrite)
    {
      return new ActionButtonViewModel(
        width: 45,
        height: 20
        )
      {
        OnActionButtonClicked = async () => {
          await ChannelAccess.Hub.PutValueAsync(
            channelName,
            valueToWrite
          );
        }
      };
    }
    public IntensityMapFeatures_KafkaTabViewModel(IntensityMapViewerViewModel parent)
    {
      kafkaPluginPrefix = ":Kafka1:";
      StartKafkaStreaming = CreateValueWriteActionButtonViewModel(parent.PvPrefix + kafkaPluginPrefix + "EnableCallbacks", (short)1);
      StartKafkaStreaming.Text = "Start";
      StartKafkaStreaming.Width = 60;

      StopKafkaStreaming = CreateValueWriteActionButtonViewModel(parent.PvPrefix + kafkaPluginPrefix + "EnableCallbacks", (short)0);
      StopKafkaStreaming.Text = "Stop";
      StopKafkaStreaming.Width = 60;

      KafkaStreamingRBV = new TextUpdateViewModel(
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + kafkaPluginPrefix + "EnableCallbacks_RBV")
      );
      KafkaConnectionStatusRBV = new TextUpdateViewModel(
        width: 100,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + kafkaPluginPrefix + "ConnectionStatus_RBV")
      );
      KafkaPluginSettings = new ActionButtonViewModel(
        text: "Kafka Plugin Settings",
        width: 150
        );


    }
  }
}
