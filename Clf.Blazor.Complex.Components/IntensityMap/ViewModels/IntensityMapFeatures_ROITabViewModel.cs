using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_ROITabViewModel
  {
    private string camPluginPrefix;
    private string roiPluginPrefix;
    private IntensityMapViewerViewModel m_parent;
    ChannelsHandler m_channelsHandler;

    public RadioButtonViewModel ROIMethod { get; private set; }
    public TextEntryViewModel LocalROICentreXSet { get; }
    public TextEntryViewModel LocalROICentreYSet { get; }
    public TextEntryViewModel CameraROISizeXSet { get; }
    public TextUpdateViewModel CameraROISizeXRBV { get; set; }
    public TextEntryViewModel CameraROIStartXSet { get; }
    public TextUpdateViewModel CameraROIStartXRBV { get; set; }
    public TextEntryViewModel CameraROISizeYSet { get; }
    public TextUpdateViewModel CameraROISizeYRBV { get; set; }
    public TextEntryViewModel CameraROIStartYSet { get; }
    public TextUpdateViewModel CameraROIStartYRBV { get; set; }
    public TextEntryViewModel SoftwareROISizeXSet { get; }
    public TextUpdateViewModel SoftwareROISizeXRBV { get; set; }
    public TextEntryViewModel SoftwareROIStartXSet { get; }
    public TextUpdateViewModel SoftwareROIStartXRBV { get; set; }
    public TextEntryViewModel SoftwareROISizeYSet { get; }
    public TextUpdateViewModel SoftwareROISizeYRBV { get; set; }
    public TextEntryViewModel SoftwareROIStartYSet { get; }
    public TextUpdateViewModel SoftwareROIStartYRBV { get; set; }
    public ActionButtonViewModel ROIPluginSettings { get; }

    public IntensityMapFeatures_ROITabViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;
      camPluginPrefix = ":cam1:";
      roiPluginPrefix = ":ROI1:";

      ROIMethod = new RadioButtonViewModel(
          items: new ObservableCollection<string> { "Camera ROI", "Software ROI" },
          isHorizontal: false,
          channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":ROIMethod")
      );

      CameraROISizeXSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "SizeX")
      );
      CameraROISizeXRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "SizeX_RBV")
      );
      CameraROIStartXSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "MinX")
      );
      CameraROIStartXRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "MinX_RBV")
      );
      CameraROISizeYSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "SizeY")
      );
      CameraROISizeYRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "SizeY_RBV")
      );
      CameraROIStartYSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "MinY")
      );
      CameraROIStartYRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "MinY_RBV")
      );
      SoftwareROISizeXSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "SizeX")
      );
      SoftwareROISizeXRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "SizeX_RBV")
      );
      SoftwareROIStartXSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "MinX")
      );
      SoftwareROIStartXRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "MinX_RBV")
      );
      SoftwareROISizeYSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "SizeY")
      );
      SoftwareROISizeYRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "SizeY_RBV")
      );
      SoftwareROIStartYSet = new TextEntryViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "MinY")
      );
      SoftwareROIStartYRBV = new TextUpdateViewModel(
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "MinY_RBV")
      );

      ROIPluginSettings = new ActionButtonViewModel(
      text: "ROI Plugin Settings",
      isVisible: true
      );
      IntensityMapFeatures_ROITabViewModel_Logic_Initiliasation();

    }

  }
}
