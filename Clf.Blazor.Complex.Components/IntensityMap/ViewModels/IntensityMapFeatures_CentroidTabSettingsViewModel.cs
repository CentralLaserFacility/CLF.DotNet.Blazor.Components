using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;


namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_CentroidTabSettingsViewModel
  {
    private string centroidPluginPrefix;
    public TextEntryViewModel MinEnclosingAreaSet { get; }
    public TextUpdateViewModel MinEnclosingAreaRBV { get; set; }
    public TextEntryViewModel NumIterationSet { get; }
    public TextUpdateViewModel NumIterationRBV { get; set; }
    public TextEntryViewModel DistanceCentroidsSet { get; }
    public TextUpdateViewModel DistanceCentroidsRBV { get; set; }
    public TextEntryViewModel InitialThresholdSet { get; }
    public TextUpdateViewModel InitialThresholdRBV { get; set; }
    public TextEntryViewModel PixelIncrementSet { get; }
    public TextUpdateViewModel PixelIncrementRBV { get; set; }
    public TextEntryViewModel MinPixelAboveThresholdSet { get; }
    public TextUpdateViewModel MinPixelAboveThresholdRBV { get; set; }
    public TextEntryViewModel MaxPixelAboveThresholdSet { get; }
    public TextUpdateViewModel MaxPixelAboveThresholdRBV { get; set; }
    public TextEntryViewModel DownsampleXSet { get; }
    public TextUpdateViewModel DownsampleXRBV { get; set; }
    public TextEntryViewModel DownsampleYSet { get; }
    public TextUpdateViewModel DownsampleYRBV { get; set; }
    public TextEntryViewModel EDOverlayColourSet { get; }
    public TextUpdateViewModel EDOverlayColourRBV { get; set; }

    public IntensityMapFeatures_CentroidTabSettingsViewModel(IntensityMapViewerViewModel parent)
    {
      centroidPluginPrefix = "Centroid1:";
      MinEnclosingAreaSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "SetCentroidAreaPercent")
      );
      MinEnclosingAreaRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "SetCentroidAreaPercent")
      );

      NumIterationSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidSuccessive")
      );
      NumIterationRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidSuccessive")
      );

      DistanceCentroidsSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidCogDist")
      );
      DistanceCentroidsRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidCogDist")
      );

      InitialThresholdSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidThreshold")
      );
      InitialThresholdRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidThreshold")
      );

      PixelIncrementSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidThreshInc")
      );
      PixelIncrementRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidThreshInc")
      );

      MinPixelAboveThresholdSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidMinFract")
      );
      MinPixelAboveThresholdRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidMinFract")
      );

      MaxPixelAboveThresholdSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidMaxFract")
      );
      MaxPixelAboveThresholdRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidMaxFract")
      );

      DownsampleXSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidDownsampleX")
      );
      DownsampleXRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidDownsampleX")
      );

      DownsampleYSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidDownsampleY")
      );
      DownsampleYRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidDownsampleY")
      );

      EDOverlayColourSet = new TextEntryViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "EdgeDetectOverlayColour")
      );
      EDOverlayColourRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "EdgeDetectOverlayColour")
      );

    }


    }
}
