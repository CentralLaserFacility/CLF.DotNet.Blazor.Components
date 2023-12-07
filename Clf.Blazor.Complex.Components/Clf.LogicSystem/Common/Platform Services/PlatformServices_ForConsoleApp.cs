//
// PlatformServices_ForConsoleApp.cs
//

using static Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

namespace Clf.LogicSystem.Common.PlatformServices
{

  public sealed class PlatformServices_ForConsoleApp : PlatformServices
  {

    public PlatformServices_ForConsoleApp ( ) :
    base(
      (messageLogLine) => {
        System.Console.WriteLine(messageLogLine) ;
        System.Diagnostics.Debug.WriteLine(messageLogLine) ;
      },
      (expectedToBeTrue,arg) => System.Diagnostics.Debug.Assert(expectedToBeTrue,arg)
    ) {
      System.Console.OutputEncoding = System.Text.Encoding.UTF8 ;
    }

    // FIX_THIS ...

    public static Microsoft.Extensions.DependencyInjection.ServiceCollection CreateServices ( 
      System.Action<string> writeMessageLogLine,
      System.Action<bool>   assert
    ) {
      var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection() ;
      services.AddSingleton<IPlatformServices>(
        new PlatformServices_ForConsoleApp() 
      ) ;
      return services ;
    }

  }

}

