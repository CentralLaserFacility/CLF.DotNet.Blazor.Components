//
// PlatformServices_ForXUnitTest.cs
//

using static Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

namespace Clf.LogicSystem.Common.PlatformServices
{

  // https://umplify.github.io/xunit-dependency-injection/

  public sealed class PlatformServices_ForXUnitTest : PlatformServices
  {

    public PlatformServices_ForXUnitTest (
      System.Action<string>       writeMessageLogLine,
      System.Action<bool,string?> assert
    ) :
    base(
      writeMessageLogLine,
      assert
    ) {
    }

    public static Microsoft.Extensions.DependencyInjection.ServiceCollection CreateServices ( 
      System.Action<string>       writeMessageLogLine,
      System.Action<bool,string?> assert
    ) {
      var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection() ;
      services.AddSingleton<IPlatformServices>(
        new PlatformServices_ForXUnitTest(
          writeMessageLogLine,
          assert
        ) 
      ) ;
      return services ;
    }

  }

}

