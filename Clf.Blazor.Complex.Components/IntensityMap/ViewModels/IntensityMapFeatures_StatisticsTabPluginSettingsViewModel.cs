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

  public partial class IntensityMapFeatures_StatisticsTabPluginSettingsViewModel
  {
    public IntensityMapFeatures_CommonPluginSettingsViewModel StatisticsCommonPluginSettingsTab { get; }
    public string pageFileTitle;
    private string statsPluginPrefix;
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
    public ComboBoxViewModel StatisticsCentroidEnable { get; set; }
    public TextUpdateViewModel StatisticsCentroidEnableRBV { get; set; }
    public TextEntryViewModel StatisticsCentroidThreshold { get; set; }
    public TextUpdateViewModel StatisticsCentroidThresholdRBV { get; set; }
    public TextUpdateViewModel StatisticsCentroidTotalRBV { get; set; }
    public TextUpdateViewModel StatisticsCentroidXRBV { get; set; }
    public TextUpdateViewModel StatisticsCentroidYRBV { get; set; }
    public TextUpdateViewModel StatisticsSigmaXRBV { get; set; }
    public TextUpdateViewModel StatisticsSigmaYRBV { get; set; }
    public TextUpdateViewModel StatisticsSkewnessXRBV { get; set; }
    public TextUpdateViewModel StatisticsSkewnessYRBV { get; set; }
    public TextUpdateViewModel StatisticsKurtosisXRBV { get; set; }
    public TextUpdateViewModel StatisticsKurtosisYRBV { get; set; }
    public TextUpdateViewModel StatisticsSigmaXYRBV { get; set; }
    public TextUpdateViewModel StatisticsEccentricityRBV { get; set; }
    public TextUpdateViewModel StatisticsOrientationRBV { get; set; }
    public ActionButtonViewModel StatisticsReset { get; set; }
    public ComboBoxViewModel StatisticsConfigure { get; set; }
    public ComboBoxViewModel StatisticsProfileEnable { get; set; }
    public TextUpdateViewModel StatisticsProfileEnableRBV { get; set; }
    public TextUpdateViewModel StatisticsProfileSizeXRBV { get; set; }
    public TextUpdateViewModel StatisticsProfileSizeYRBV { get; set; }
    public TextEntryViewModel StatisticsProfileCursorX { get; set; }
    public TextUpdateViewModel StatisticsProfileCursorXRBV { get; set; }
    public TextEntryViewModel StatisticsProfileCursorY { get; set; }
    public TextUpdateViewModel StatisticsProfileCursorYRBV { get; set; }
    public ComboBoxViewModel StatisticsHistogramEnable { get; set; }
    public TextUpdateViewModel StatisticsHistogramEnableRBV { get; set; }
    public TextEntryViewModel StatisticsHistogramSize { get; set; }
    public TextUpdateViewModel StatisticsHistogramSizeRBV { get; set; }
    public TextEntryViewModel StatisticsHistogramMinimum { get; set; }
    public TextUpdateViewModel StatisticsHistogramMinimumRBV { get; set; }
    public TextEntryViewModel StatisticsHistogramMaximum { get; set; }
    public TextUpdateViewModel StatisticsHistogramMaximumRBV { get; set; }
    public TextUpdateViewModel StatisticsHistogramBelowMinimumRBV { get; set; }
    public TextUpdateViewModel StatisticsHistogramAboveMaximumRBV { get; set; }
    public TextUpdateViewModel StatisticsHistogramEntropyRBV { get; set; }
    public ActionButtonViewModel StatisticsTimeSeriesStart { get; set; }
    public ActionButtonViewModel StatisticsTimeSeriesStop { get; set; }
    public TextUpdateViewModel StatisticsTimeSeriesAcquireRBV { get; set; }
    public TextEntryViewModel StatisticsTimeSeriesNumPoints { get; set; }
    public TextUpdateViewModel StatisticsTimeSeriesCurrentPointsRBV { get; set; }
    public ComboBoxViewModel StatisticsTimeSeriesReadRate { get; set; }
    public ActionButtonViewModel StatisticsTimeSeriesRead { get; set; }
    public ComboBoxViewModel StatisticsTimeSeriesAcquireMode { get; set; }
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
    public IntensityMapFeatures_StatisticsTabPluginSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix)
    {
      statsPluginPrefix = "Stats1:";
      StatisticsCommonPluginSettingsTab = new IntensityMapFeatures_CommonPluginSettingsViewModel(channelsHandler,pvPrefix, statsPluginPrefix);
      pageFileTitle = "Statistics Plugin Settings  " + pvPrefix + statsPluginPrefix;
      StatisticsEnable = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeStatistics", channelsHandler)
        );
      StatisticsEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeStatistics_RBV", channelsHandler)
        );
      StatisticsBackgroundWidth = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "BgdWidth", channelsHandler)
        );
      StatisticsBackgroundWidthRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "BgdWidth_RBV", channelsHandler)
        );
      StatisticsMinimumPixelRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "MinValue_RBV", channelsHandler)
        );
      StatisticsMaximumPixelRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "MaxValue_RBV", channelsHandler)
        );
      StatisticsMinimumXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "MinX_RBV", channelsHandler)
        );
      StatisticsMaximumXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "MaxX_RBV", channelsHandler)
        );
      StatisticsMinimumYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "MinY_RBV", channelsHandler)
        );
      StatisticsMaximumYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "MaxY_RBV", channelsHandler)
        );
      StatisticsTotalValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "Total_RBV", channelsHandler)
        );
      StatisticsNetValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "Net_RBV", channelsHandler)
        );
      StatisticsMeanValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "MeanValue_RBV", channelsHandler)
        );
      StatisticsSigmaValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "Sigma_RBV", channelsHandler)
        );
      StatisticsCentroidEnable = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeCentroid", channelsHandler)
        );
      StatisticsCentroidEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeCentroid_RBV", channelsHandler)
        );
      StatisticsCentroidThreshold = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CentroidThreshold", channelsHandler)
        );
      StatisticsCentroidThresholdRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CentroidThreshold_RBV", channelsHandler)
        );
      StatisticsCentroidTotalRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CentroidTotal_RBV", channelsHandler)
        );
      StatisticsCentroidXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CentroidX_RBV", channelsHandler)
        );
      StatisticsCentroidYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CentroidY_RBV", channelsHandler)
        );
      StatisticsSigmaXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "SigmaX_RBV", channelsHandler)
        );
      StatisticsSigmaYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "SigmaY_RBV", channelsHandler)
        );
      StatisticsSkewnessXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "SkewX_RBV", channelsHandler)
        );
      StatisticsSkewnessYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "SkewY_RBV", channelsHandler)
        );
      StatisticsKurtosisXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "KurtosisX_RBV", channelsHandler)
        );
      StatisticsKurtosisYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "KurtosisY_RBV", channelsHandler)
        );
      StatisticsSigmaXYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "SigmaXY_RBV", channelsHandler)
        );
      StatisticsEccentricityRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "Eccentricity_RBV", channelsHandler)
        );
      StatisticsOrientationRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "Orientation_RBV", channelsHandler)
        );

      StatisticsReset = CreateValueWriteActionButtonViewModel(pvPrefix + statsPluginPrefix + "Reset.PROC", (short)1);
      StatisticsReset.Text = "Reset";
      StatisticsProfileEnable = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeProfiles", channelsHandler)
        );
      StatisticsProfileEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeProfiles_RBV", channelsHandler)
        );
      StatisticsProfileSizeXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ProfileSizeX_RBV", channelsHandler)
        );
      StatisticsProfileSizeYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ProfileSizeY_RBV", channelsHandler)
        );
      StatisticsProfileCursorX = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CursorX", channelsHandler)
        );
      StatisticsProfileCursorXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CursorX_RBV", channelsHandler)
        );
      StatisticsProfileCursorY = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CursorY", channelsHandler)
        );
      StatisticsProfileCursorYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "CursorY_RBV", channelsHandler)
        );

      StatisticsHistogramEnable = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeHistogram", channelsHandler)
        );
      StatisticsHistogramEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "ComputeHistogram_RBV", channelsHandler)
        );
      StatisticsHistogramSize = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistSize", channelsHandler)
        );
      StatisticsHistogramSizeRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistSize_RBV", channelsHandler)
        );
      StatisticsHistogramMinimum = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistMin", channelsHandler)
        );
      StatisticsHistogramMinimumRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistMin_RBV", channelsHandler)
        );
      StatisticsHistogramMaximum = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistMax", channelsHandler)
        );
      StatisticsHistogramMaximumRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistMax_RBV", channelsHandler)
        );
      StatisticsHistogramBelowMinimumRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistBelow_RBV", channelsHandler)
        );
      StatisticsHistogramAboveMaximumRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistAbove_RBV", channelsHandler)
        );
      StatisticsHistogramEntropyRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "HistEntropy_RBV", channelsHandler)
        );
      StatisticsTimeSeriesStart = CreateValueWriteActionButtonViewModel(pvPrefix + statsPluginPrefix + "TS:TSAcquire", (short)1);
      StatisticsTimeSeriesStart.Text = "Start";
      StatisticsTimeSeriesStop = CreateValueWriteActionButtonViewModel(pvPrefix + statsPluginPrefix + "TS:TSAcquire", (short)0);
      StatisticsTimeSeriesStop.Text = "Stop";
      StatisticsTimeSeriesAcquireRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "TS:TSAcquiring", channelsHandler)
        );
      StatisticsTimeSeriesNumPoints = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "TS:TSNumPoints", channelsHandler)
        );
      StatisticsTimeSeriesCurrentPointsRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "TS:TSCurrentPoint", channelsHandler)
        );
      StatisticsTimeSeriesReadRate = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "TS:TSRead.SCAN", channelsHandler)
        );
      StatisticsTimeSeriesRead = CreateValueWriteActionButtonViewModel(pvPrefix + statsPluginPrefix + "TS:TSRead.PROC", (short)1);
      StatisticsTimeSeriesRead.Text = "Read";
      StatisticsTimeSeriesAcquireMode = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: new ChannelRecord(pvPrefix + statsPluginPrefix + "TS:TSAcquireMode", channelsHandler)
        );
    }
  }
}
