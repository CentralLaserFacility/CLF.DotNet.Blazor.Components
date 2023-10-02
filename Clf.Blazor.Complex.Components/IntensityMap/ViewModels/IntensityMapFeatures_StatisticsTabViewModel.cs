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

  public partial class IntensityMapFeatures_StatisticsTabViewModel
  {
    private IntensityMapViewerViewModel m_parent;
    public ComboBoxViewModel StatisticsEnable { get; set; }
    public TextUpdateViewModel StatisticsEnableRBV { get; set; }
    public TextEntryViewModel StatisticsBackgroundWidth { get; set; }
    public TextUpdateViewModel StatisticsBackgroundWidthRBV { get; set; }
    public TextUpdateViewModel StatisticsMinimumPixelRBV { get; set; }
    public TextUpdateViewModel StatisticsMaximumPixelRBV { get; set; }
    public TextUpdateViewModel StatisticsMinimumXRBV { get; set; }
    public TextUpdateViewModel StatisticsMaximumXRBV { get; set; }
    public TextUpdateViewModel StatisticsMinimumYRBV { get; set; }
    public TextUpdateViewModel StatisticsMaximumYRBV { get; set; }
    public TextUpdateViewModel StatisticsTotalValueRBV { get; set; }
    public TextUpdateViewModel StatisticsNetValueRBV { get; set; }
    public TextUpdateViewModel StatisticsMeanValueRBV { get; set; }
    public TextUpdateViewModel StatisticsSigmaValueRBV { get; set; }
    public TextEntryViewModel StatisticsEnergyCalibrationFactor { get; set; }
    public TextUpdateViewModel StatisticsEnergyCalibrationFactorRBV { get; set; }
    public ActionButtonViewModel StatisticsPluginSettings { get; set; }

    public IntensityMapFeatures_StatisticsTabViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;
      StatisticsEnable = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:ComputeStatistics")
        );
      StatisticsEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:ComputeStatistics_RBV")
        );
      StatisticsBackgroundWidth = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:BgdWidth")
        );
      StatisticsBackgroundWidthRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:BgdWidth_RBV")
        );
      StatisticsMinimumPixelRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:MinValue_RBV")
        );
      StatisticsMaximumPixelRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:MaxValue_RBV")
        );
      StatisticsMinimumXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:MinX_RBV")
        );
      StatisticsMaximumXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:MaxX_RBV")
        );
      StatisticsMinimumYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:MinY_RBV")
        );
      StatisticsMaximumYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:MaxY_RBV")
        );
      StatisticsTotalValueRBV = new TextUpdateViewModel(
        width: 70,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:Total_RBV")
        );
      StatisticsNetValueRBV = new TextUpdateViewModel(
        width: 70,
        precision: 2,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:Net_RBV")
        );
      StatisticsMeanValueRBV = new TextUpdateViewModel(
        width: 50,
        precision: 4,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:MeanValue_RBV")
        );
      StatisticsSigmaValueRBV = new TextUpdateViewModel(
        width: 50,
        precision: 4,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:Sigma_RBV")
        );
      StatisticsEnergyCalibrationFactor = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:EnergyCalibrationFactor")
        );
      StatisticsEnergyCalibrationFactorRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Stats1:EnergyCalibrationFactorRBV")
        );
      StatisticsPluginSettings = new ActionButtonViewModel(
        text: "Statistics Plugin Settings",
        height: 50,
        width: 250
        );
    }
  }
}
