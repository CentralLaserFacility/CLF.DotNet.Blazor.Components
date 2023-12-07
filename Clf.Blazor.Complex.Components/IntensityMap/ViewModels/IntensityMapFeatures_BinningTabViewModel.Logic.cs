using Clf.ChannelAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
  public partial class IntensityMapFeatures_BinningTabViewModel
  {
    IChannel? _binningMethod;
    IChannel? _cameraAcquisitionStatus;

    private void IntensityMapFeatures_BinningTabViewModel_Logic_Initiliasation()
    {

      m_parent.ChannelsHandler.InstallChannel(
      _binningMethod = Hub.GetOrCreateChannel(m_parent.PvPrefix + ":BinningMethod"),
       (valueInfo, _) => OnBinningMethodChange((System.Int16)valueInfo.Value)
        );
      m_parent.ChannelsHandler.InstallChannel(
      _cameraAcquisitionStatus = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "Acquire"),
       (valueInfo, _) => OnCameraAcquisitionStatusChange((System.Int16)valueInfo.Value)
        );

    }
    private void OnCameraAcquisitionStatusChange(int value)
    {
      if (value == 1)
      {
        CameraBinningXSet.IsDisabled = true;
        CameraBinningYSet.IsDisabled = true;
      }
      else
      {
        CameraBinningXSet.IsDisabled = false;
        CameraBinningYSet.IsDisabled = false;
      }

    }
    private void OnBinningMethodChange(int value)
    {
      if(value == 1)
      {
        SoftwareBinningXSet.IsVisible = true;
        SoftwareBinningXRBV.IsVisible = true;
        SoftwareBinningYSet.IsVisible = true;
        SoftwareBinningYRBV.IsVisible = true;
        CameraBinningXSet.IsVisible = false;
        CameraBinningXRBV.IsVisible = false;
        CameraBinningYSet.IsVisible = false;
        CameraBinningYRBV.IsVisible = false;
      }
      else
      {
        SoftwareBinningXSet.IsVisible = false;
        SoftwareBinningXRBV.IsVisible = false;
        SoftwareBinningYSet.IsVisible = false;
        SoftwareBinningYRBV.IsVisible = false;
        CameraBinningXSet.IsVisible = true;
        CameraBinningXRBV.IsVisible = true;
        CameraBinningYSet.IsVisible = true;
        CameraBinningYRBV.IsVisible = true;
      }
    }
  }
}
