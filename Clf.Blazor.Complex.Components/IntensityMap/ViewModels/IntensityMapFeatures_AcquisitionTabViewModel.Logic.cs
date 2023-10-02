using Clf.ChannelAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
  public partial class IntensityMapFeatures_AcquisitionTabViewModel
  {
    IChannel? _camAcquireStatus;
    IChannel? _camAcquire;
    private void IntensityMapFeatures_AcquisitionTabViewModel_Logic_Initiliasation()
    {
      m_parent.ChannelsHandler.InstallChannel(
      _camAcquireStatus = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "AcquireBusy"),
      (valueInfo, _) => OnCamAcquireStatusChange((System.Int16)valueInfo.Value)
        );
      m_parent.ChannelsHandler.InstallChannel(
      _camAcquire = Hub.GetOrCreateChannel(m_parent.PvPrefix + camPluginPrefix + "Acquire"));
      StopAcquisition.OnActionButtonClicked = OnStopAcquisition;
    }
    private void OnCamAcquireStatusChange(int value)
    {
      if (value == 0)
      {
        StartAcquisition.IsDisabled = false;
        StopAcquisition.IsDisabled = true;
      }
      else
      {
        StartAcquisition.IsDisabled = true;
        StopAcquisition.IsDisabled = false;
      }
    }

    private async void OnStopAcquisition()
    {
      _camAcquire.PutValue(Convert.ToInt16("0"));
    }
  }
}
