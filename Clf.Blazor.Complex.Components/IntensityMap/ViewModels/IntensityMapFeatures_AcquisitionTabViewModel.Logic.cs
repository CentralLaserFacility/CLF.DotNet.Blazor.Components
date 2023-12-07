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
    private void IntensityMapFeatures_AcquisitionTabViewModel_Logic_Initialisation()
    {
      m_parent.ChannelsHandler.InstallChannel(
      _camAcquireStatus = Hub.GetOrCreateChannel(m_parent.PvPrefix + m_parent.StreamPrefix + "AcquireBusy"),
      (valueInfo, _) => OnCamAcquireStatusChange((System.Int16)valueInfo.Value)
        );
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
  }
}
