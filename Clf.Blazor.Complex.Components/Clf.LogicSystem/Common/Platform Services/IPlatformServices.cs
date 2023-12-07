//
// IPlatformServices.cs
//

using System.Runtime.CompilerServices;

namespace Clf.LogicSystem.Common.PlatformServices
{

  // FIX_THIS_STEVE : Use DI instead !!!

  public interface IPlatformServices
  {
    
    void Assert ( bool expectedToBeTrue, [CallerArgumentExpression(nameof(expectedToBeTrue))] string? arg = null ) ;

    // FIX_THIS : use an injected Logger !!!

    bool LogExceptionAndContinue ( string messageLine ) ;

    void WriteMessageLogLine ( string messageLine = "" ) ;

    void WriteMessageLogLines ( params string[] messageLines ) ;

  }

}