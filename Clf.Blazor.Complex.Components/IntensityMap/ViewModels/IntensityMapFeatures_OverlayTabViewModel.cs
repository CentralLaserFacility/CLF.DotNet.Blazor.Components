using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
  public partial class IntensityMapFeatures_OverlayTabViewModel
  {
    private string centroidPluginPrefix;
    public GroupBoxViewModel OverlayProfileReferenceGroupBox { get; set; }
    public CheckboxViewModel OverlayProfileReferenceShow { get; set; }
    public SliderViewModel OverlayProfileReferenceSizeSlider { get; set; }
    public TextEntryViewModel OverlayProfileReferenceX { get; set; }
    public TextEntryViewModel OverlayProfileReferenceY { get; set; }
    public CheckboxViewModel OverlayProfileReferenceFollowCentroid { get; set; }
    public ColorPickerViewModel OverlayProfileReferenceColor { get; }
    public GroupBoxViewModel OverlaySoftwareReferenceGroupBox { get; set; }
    public CheckboxViewModel OverlaySoftwareReferenceShow { get; set; }
    public CheckboxViewModel OverlaySoftwareReferenceBoxShow { get; set; }
    public SliderViewModel OverlaySoftwareReferenceSizeSlider { get; set; }
    public TextEntryViewModel OverlaySoftwareReferenceX { get; set; }
    public TextEntryViewModel OverlaySoftwareReferenceY { get; set; }
    public ColorPickerViewModel OverlaySoftwareReferenceColor { get; }
    public TextEntryViewModel OverlaySoftwareReferenceWidth { get; set; }
    public TextEntryViewModel OverlaySoftwareReferenceHeight { get; set; }
    public IntensityMapFeatures_OverlayTabViewModel(IntensityMapViewerViewModel parent)
    {
      centroidPluginPrefix = "Centroid1:";
      OverlayProfileReferenceGroupBox = new GroupBoxViewModel(
      title: "Profile Reference"
      );
      OverlayProfileReferenceX = new TextEntryViewModel(
      width: 50,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ProfXhairX")
      );
      OverlayProfileReferenceY = new TextEntryViewModel(
      width: 50,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ProfXhairY")
      );
      OverlayProfileReferenceSizeSlider = new SliderViewModel(
        width: 100,
        showSpinner: true,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ProfXhairSize")
      );
      OverlayProfileReferenceShow = new CheckboxViewModel(
      label: "Show",
      width: 60,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ShowProfXhair")
      );
      OverlayProfileReferenceColor = new ColorPickerViewModel(
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ProfColour")
      );
      OverlayProfileReferenceFollowCentroid = new CheckboxViewModel(
      label: "Follow Centroid",
      width: 120,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "FollowCentroid")
      );
      OverlaySoftwareReferenceGroupBox = new GroupBoxViewModel(
      title: "Software Reference"
        );
      OverlaySoftwareReferenceX = new TextEntryViewModel(
      width: 50,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "SoftX")
      );
      OverlaySoftwareReferenceY = new TextEntryViewModel(
      width: 50,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "SoftY")
      );
      OverlaySoftwareReferenceSizeSlider = new SliderViewModel(
      width: 100,
      showSpinner: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "SoftXhairSize")
      );
      OverlaySoftwareReferenceShow = new CheckboxViewModel(
      label: "Show Reference",
      width: 130,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ShowSoftXhair")
      );
      OverlaySoftwareReferenceBoxShow = new CheckboxViewModel(
      label: "Show Box",
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ShowSoftBox")
      );
      OverlaySoftwareReferenceColor = new ColorPickerViewModel(
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "SoftColour")
      );
      OverlaySoftwareReferenceWidth = new TextEntryViewModel(
      width: 50,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "SoftWidth")
      );
      OverlaySoftwareReferenceHeight = new TextEntryViewModel(
      width: 50,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "SoftHeight")
      );
    }
  }
}
