//
// PlatformServices.cs
//

using System.Runtime.CompilerServices;
using Clf.Common.ExtensionMethods;
using Clf.Common.Utils;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.Common.Utils;

namespace Clf.LogicSystem.Common.PlatformServices
{

  // FIX_THIS : Use DI instead !!!

  public class PlatformServices : IPlatformServices
  {

    public void Assert ( bool expectedToBeTrue, [CallerArgumentExpression(nameof(expectedToBeTrue))] string? arg = null )
    {
      m_assertAction(expectedToBeTrue,arg) ;
    }

    private readonly System.Action<bool,string?> m_assertAction ;

    public bool ShowCurrentThreadIdentifierInLogMessages = false ;

    public static int CurrentThreadID => System.Environment.CurrentManagedThreadId ;

    //
    // Useful if we want to log an exception in a 'when' clause.
    // We return false, so that the 'catch' doesn't actually catch the exception
    // but we'll have had an opportunity to log a message.
    //

    public bool LogExceptionAndContinue ( string messageLine )
    {
      m_messageWriter.WriteMessageLogLine(messageLine) ;
      return false ;
    }

    //
    // Hmm, would be nice to have distinct streams
    // for 'informational' messages vs 'debug' messages 
    // vs exceptions.
    //

    private readonly IMessageWriter m_messageWriter ;

    public bool WriteMessageLogLines_Enabled { get ; set ; } 
    #if DEBUG
    = false;
    #else
    = false ;
    #endif

    public void WriteMessageLogLine ( string messageLine = "" )
    {
      if ( WriteMessageLogLines_Enabled )
      {
        m_messageWriter.WriteMessageLogLine(messageLine) ;
      }
    }

    public void WriteMessageLogLines ( params string[] messageLines )
    {
      messageLines.ForEachItem(
        WriteMessageLogLine
      ) ;
    }

    private static PlatformServices? m_instance = null ;

    public static PlatformServices Instance => m_instance ??= new PlatformServices_ForDebugging() ;

    internal PlatformServices (
      System.Action<string>         writeMessageLogLine,
      System.Action<bool,string?>   assert
    ) {
      m_instance = this ;
      m_assertAction = assert ;
      // We use the client-supplied function to write physical message lines.
      m_messageWriter = new MessageWriter(writeMessageLogLine) ;
      Clf.Common.Utils.Log.WriteLineAction = this.WriteMessageLogLine ;
    }

  }

}

