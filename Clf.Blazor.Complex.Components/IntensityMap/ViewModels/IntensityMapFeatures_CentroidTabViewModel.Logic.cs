using Clf.ChannelAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
    public partial class IntensityMapFeatures_CentroidTabViewModel
    {
    IChannel? _centroidAlgorithm;
    IChannel? _beamShape;
    IChannel? _thresholdAlgorithm;
    private void IntensityMapFeatures_CentroidTabViewModel_Logic_Initiliasation()
    {
      m_parent.ChannelsHandler.InstallChannel(
      _centroidAlgorithm = Hub.GetOrCreateChannel(m_parent.PvPrefix + centroidPluginPrefix + "CentroidAlgorithm"),
      (valueInfo, _) => OnCentroidAlgorithmChange((System.Int16)valueInfo.Value)
        );
      m_parent.ChannelsHandler.InstallChannel(
      _beamShape = Hub.GetOrCreateChannel(m_parent.PvPrefix + "BeamShape"),
      (valueInfo, _) => OnBeamShapeChange((System.Int16)valueInfo.Value)
        );
      m_parent.ChannelsHandler.InstallChannel(
      _thresholdAlgorithm = Hub.GetOrCreateChannel(m_parent.PvPrefix + centroidPluginPrefix + "ThresholdAlgorithm"),
      (valueInfo, _) => OnThresholdAlgorithmChange((System.Int16)valueInfo.Value)
        );
    }

    private void OnThresholdAlgorithmChange(int value)
    {
      if (value == 1)
      {
        ThresholdManualSet.IsVisible = true;
      }
      else
      {
        ThresholdManualSet.IsVisible = false;
      }
    }
    private void OnBeamShapeChange(int value)
    {
      if (value == 1)
      {
        BeamDiameter.IsVisible = false;
        BeamHeight.IsVisible = true;
        BeamWidth.IsVisible = true;
        BeamTilt.IsVisible = true;
        BeamDiameterLabel.IsVisible = false;
        BeamHeightLabel.IsVisible = true;
        BeamWidthLabel.IsVisible = true;
        BeamTiltLabel.IsVisible = true;
      }
      else
      {
        BeamDiameter.IsVisible = true;
        BeamHeight.IsVisible = false;
        BeamWidth.IsVisible = false;
        BeamTilt.IsVisible = false;
        BeamDiameterLabel.IsVisible = true;
        BeamHeightLabel.IsVisible = false;
        BeamWidthLabel.IsVisible = false;
        BeamTiltLabel.IsVisible = false;
      }
    }

    private void OnCentroidAlgorithmChange(int value)
    {
      if (value == 1)
      {
        EdgeDetectionGroupBox.IsVisible = true;
      }
      else
      {
        EdgeDetectionGroupBox.IsVisible = false;
      }
    }
  }
}
