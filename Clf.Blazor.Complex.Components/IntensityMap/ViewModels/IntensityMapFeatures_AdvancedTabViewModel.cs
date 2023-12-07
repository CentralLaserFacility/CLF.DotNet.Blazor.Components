using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;

using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_AdvancedTabViewModel
  {
    private string camPluginPrefix;
    public TextUpdateViewModel CameraFramesDroppedRBV { get; set; }
    public TextEntryViewModel CameraStreamsBytesPerSecond { get; set; }
    public TextUpdateViewModel CameraStreamsBytesPerSecondRBV { get; set; }
    public TextEntryViewModel CameraJumboPacketSize { get; set; }
    public TextUpdateViewModel CameraJumboPacketSizeRBV { get; set; }
    public ComboBoxViewModel CameraPixelFormat { get; set; }
    public TextUpdateViewModel CameraPixelFormatRBV { get; set; }
    public ActionButtonViewModel CameraDriverSettings { get; }
    public ActionButtonViewModel CameraAdvancedDriverSettings { get; }

    public IntensityMapFeatures_AdvancedTabViewModel(IntensityMapViewerViewModel parent)
    {
      camPluginPrefix = ":cam1:";
      CameraStreamsBytesPerSecond = new TextEntryViewModel(
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_StrBytesPerSecond")
      );
      CameraStreamsBytesPerSecondRBV = new TextUpdateViewModel(
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_StrBytesPerSecond_RBV")
      );
      CameraJumboPacketSize = new TextEntryViewModel(
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_GevSCPSPacketSize")
      );
      CameraJumboPacketSizeRBV = new TextUpdateViewModel(
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_GevSCPSPacketSize_RBV")
      );
      CameraPixelFormat = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "PixelFormat")
      );
      CameraPixelFormatRBV = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "PixelFormat_RBV")
      );
      CameraFramesDroppedRBV = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_StatFrameDropped_RBV")
      );
      CameraDriverSettings = new ActionButtonViewModel(
      text: " Camera Driver Settings",
      width: 200,
      height: 50
      );
      CameraAdvancedDriverSettings = new ActionButtonViewModel(
      text: " Camera Advanced Driver Settings",
      width: 200,
      height: 50
      );
    }
  }
}

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
    internal class IntensityMapFeatures_AdvancedTab
  {
    }
}
