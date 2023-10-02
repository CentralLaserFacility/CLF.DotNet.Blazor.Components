using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_BinningTabViewModel
  {
    private IntensityMapViewerViewModel m_parent;
    private string camPluginPrefix;
    private string roiPluginPrefix;

    public RadioButtonViewModel BinningMethod { get; private set; }
    public TextEntryViewModel CameraBinningXSet { get; }
    public TextUpdateViewModel CameraBinningXRBV { get; set; }
    public TextEntryViewModel CameraBinningYSet { get; }
    public TextUpdateViewModel CameraBinningYRBV { get; set; }
    public TextEntryViewModel SoftwareBinningXSet { get; }
    public TextUpdateViewModel SoftwareBinningXRBV { get; set; }
    public TextEntryViewModel SoftwareBinningYSet { get; }
    public TextUpdateViewModel SoftwareBinningYRBV { get; set; }

    public IntensityMapFeatures_BinningTabViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;
      camPluginPrefix = "cam1:";
      roiPluginPrefix = "ROI1:";

      BinningMethod = new RadioButtonViewModel(
          width: 150,
          items: new ObservableCollection<string> { "Camera Binning", "Software Binning" },
          isHorizontal: false,
          channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "BinningMethod")
      );

      CameraBinningXSet = new TextEntryViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "BinX")
      );

      CameraBinningXRBV = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "BinX_RBV")
      );
      CameraBinningYSet = new TextEntryViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "BinY")
      );

      CameraBinningYRBV = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "BinY_RBV")
      );
      SoftwareBinningXSet = new TextEntryViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "BinX")
      );

      SoftwareBinningXRBV = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "BinX_RBV")
      );
      SoftwareBinningYSet = new TextEntryViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "BinY")
      );

      SoftwareBinningYRBV = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + roiPluginPrefix + "BinY_RBV")
      );
      IntensityMapFeatures_BinningTabViewModel_Logic_Initiliasation();
    }
  }
}
