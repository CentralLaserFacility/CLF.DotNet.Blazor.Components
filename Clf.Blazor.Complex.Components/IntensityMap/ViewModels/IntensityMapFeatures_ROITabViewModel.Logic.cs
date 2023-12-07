using Clf.ChannelAccess;
using Clf.ChannelAccess.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
  public partial class IntensityMapFeatures_ROITabViewModel
  {
    IChannel? _roiMethod;
    IChannel? _cameraROIStartXSet;
    IChannel? _cameraROIStartYSet;
    IChannel? _cameraROISizeX;
    IChannel? _cameraROISizeXRBV;
    IChannel? _cameraROISizeY;
    IChannel? _cameraROISizeYRBV;
    IChannel? _cameraMaxSizeXRBV;
    IChannel? _cameraMaxSizeYRBV;
    IChannel? _softwareROIStartXSet;
    IChannel? _softwareROIStartYSet;
    IChannel? _softwareROISizeXRBV;
    IChannel? _softwareROISizeYRBV;

    private void IntensityMapFeatures_ROITabViewModel_Logic_Initiliasation()
    {

      m_parent.ChannelsHandler.InstallChannel(
      _roiMethod = Hub.GetOrCreateChannel(m_parent.PvPrefix + ":ROIMethod"),
      (valueInfo, _) => OnROIMethodChange((System.Int16)valueInfo.Value)
        );

      m_parent.ChannelsHandler.InstallChannel(
        _cameraROIStartXSet = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "MinX"),
        (valueInfo, _) => OnCameraROIStartXChange((int)valueInfo.Value)
        );
      m_parent.ChannelsHandler.InstallChannel(
        _cameraROIStartYSet = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "MinY"),
        (valueInfo, _) => OnCameraROIStartYChange((int)valueInfo.Value)
        );
      m_parent.ChannelsHandler.InstallChannel(
        _softwareROIStartXSet = Hub.GetOrCreateChannel(m_parent.PvPrefix + roiPluginPrefix + "MinX"));
      m_parent.ChannelsHandler.InstallChannel(
        _softwareROIStartYSet = Hub.GetOrCreateChannel(m_parent.PvPrefix + roiPluginPrefix + "MinY"));
      m_parent.ChannelsHandler.InstallChannel(
        _cameraROISizeX = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "SizeX"));
      m_parent.ChannelsHandler.InstallChannel(
        _cameraROISizeXRBV = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "SizeX_RBV"));
      m_parent.ChannelsHandler.InstallChannel(
        _cameraROISizeY = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "SizeY"));
      m_parent.ChannelsHandler.InstallChannel(
        _cameraROISizeYRBV = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "SizeY_RBV"));
      m_parent.ChannelsHandler.InstallChannel(
        _softwareROISizeXRBV = Hub.GetOrCreateChannel(m_parent.PvPrefix + roiPluginPrefix + "SizeX_RBV"));
      m_parent.ChannelsHandler.InstallChannel(
        _softwareROISizeYRBV = Hub.GetOrCreateChannel(m_parent.PvPrefix + roiPluginPrefix + "SizeY_RBV"));
      m_parent.ChannelsHandler.InstallChannel(
        _cameraMaxSizeXRBV = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "MaxSizeX_RBV"));
      m_parent.ChannelsHandler.InstallChannel(
        _cameraMaxSizeYRBV = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "MaxSizeY_RBV"));

    }

    private async void OnCameraROIStartXChange(int value)
    {
      if (_cameraMaxSizeXRBV != null)
      {
        var cameraMaxSizeXRBVVal = await _cameraMaxSizeXRBV!.GetValueAsync();
        if(cameraMaxSizeXRBVVal != null && cameraMaxSizeXRBVVal.IsSuccess)
        {
          var cameraMaxSizeXRBV = Convert.ToInt16(cameraMaxSizeXRBVVal.ValueInfo.Value);
          _cameraROISizeX.PutValue((cameraMaxSizeXRBV - value));
        }
      }     
    }

    private async void OnCameraROIStartYChange(int value)
    {
      if (_cameraMaxSizeYRBV != null)
      {
        var cameraMaxSizeYRBVVal = await _cameraMaxSizeYRBV!.GetValueAsync();
        if (cameraMaxSizeYRBVVal != null && cameraMaxSizeYRBVVal.IsSuccess)
        {
          var cameraMaxSizeYRBV = Convert.ToInt16(cameraMaxSizeYRBVVal.ValueInfo.Value);
          _cameraROISizeY.PutValue((cameraMaxSizeYRBV - value));
        }
      }
    }
    private async void OnROIMethodChange(int value)
    {

      if (value == 1)
      {
        ROIPluginSettings.IsVisible = true;
        SoftwareROISizeXSet.IsVisible = true;
        SoftwareROISizeXRBV.IsVisible = true;
        SoftwareROISizeYSet.IsVisible = true;
        SoftwareROISizeYRBV.IsVisible = true;
        SoftwareROIStartXSet.IsVisible = true;
        SoftwareROIStartXRBV.IsVisible = true;
        SoftwareROIStartYSet.IsVisible = true;
        SoftwareROIStartYRBV.IsVisible = true;
        CameraROISizeXSet.IsVisible = false;
        CameraROISizeXRBV.IsVisible = false;
        CameraROISizeYSet.IsVisible = false;
        CameraROISizeYRBV.IsVisible = false;
        CameraROIStartXSet.IsVisible = false;
        CameraROIStartXRBV.IsVisible= false;
        CameraROIStartYSet.IsVisible = false;
        CameraROIStartYRBV.IsVisible = false;
      }
      else
      {
        ROIPluginSettings.IsVisible = false;
        SoftwareROISizeXSet.IsVisible = false;
        SoftwareROISizeXRBV.IsVisible = false;
        SoftwareROISizeYSet.IsVisible = false;
        SoftwareROISizeYRBV.IsVisible = false;
        SoftwareROIStartXSet.IsVisible = false;
        SoftwareROIStartXRBV.IsVisible = false;
        SoftwareROIStartYSet.IsVisible = false;
        SoftwareROIStartYRBV.IsVisible = false;
        CameraROISizeXSet.IsVisible = true;
        CameraROISizeXRBV.IsVisible = true;
        CameraROISizeYSet.IsVisible = true;
        CameraROISizeYRBV.IsVisible = true;
        CameraROIStartXSet.IsVisible = true;
        CameraROIStartXRBV.IsVisible = true;
        CameraROIStartYSet.IsVisible = true;
        CameraROIStartYRBV.IsVisible = true;
      }
    }
  }
}
