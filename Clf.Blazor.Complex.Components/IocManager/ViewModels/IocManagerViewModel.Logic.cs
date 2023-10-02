using Clf.ChannelAccess;

namespace Clf.Blazor.Complex.IocManager.ViewModels
{
  public partial class IocManagerViewModel
  {
    private void IocManagerLogicInitialization()
    {
        _channelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(Prefix + "-STS:HEARTBEAT"),
        (isConnected, _) => {
          IOCStatusLedViewModel.LedValue = isConnected;
        }
        );
    }
    
  }
}
