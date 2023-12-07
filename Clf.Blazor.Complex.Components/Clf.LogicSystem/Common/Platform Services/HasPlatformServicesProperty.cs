//
// HasPlatformServicesProperty.cs
//

namespace Clf.LogicSystem.Common.PlatformServices
{

  public abstract class HasPlatformServicesProperty : IProvidesPlatformServices
  {

    // FIX_THIS_STEVE : Use DI instead !!!

    private IPlatformServices? m_platformServices = null ;

    public IPlatformServices PlatformServices => m_platformServices ??= GetPlatformServices() ;

    private static IPlatformServices GetPlatformServices ( )
    {
      try
      {
        return CommunityToolkit.Mvvm.DependencyInjection.Ioc.Default.GetRequiredService<IPlatformServices>() ;
      }
      catch ( System.Exception x )
      {
        return Clf.LogicSystem.Common.PlatformServices.PlatformServices.Instance ;
      }
    }

  }

}

