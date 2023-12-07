//
// PlatformServices_ForDebugging.cs
//

using Microsoft.Extensions.DependencyInjection;

namespace Clf.LogicSystem.Common.PlatformServices
{

  public sealed class PlatformServices_ForDebugging : PlatformServices
  {

    //
    // For a console app we need to ensure that there is only ONE INSTANCE.
    // This needs to be created in 'Main' and installed as the PlatformServices.Instance.
    //

    public PlatformServices_ForDebugging ( ) :
    base(
      (messageLogLine)       => System.Diagnostics.Debug.WriteLine(messageLogLine),
      (expectedToBeTrue,arg) => System.Diagnostics.Debug.Assert(expectedToBeTrue,arg)
    ) {
    }

    public static Microsoft.Extensions.DependencyInjection.ServiceCollection CreateServices ( 
      System.Action<string> writeMessageLogLine,
      System.Action<bool>   assert
    ) {
      var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection() ;
      services.AddSingleton<IPlatformServices>(
        new PlatformServices_ForDebugging() 
      ) ;
      return services ;
    }

    public new static PlatformServices_ForDebugging Instance = new() ;

  }

}

