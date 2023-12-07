//
// ExecutionLog.cs
//

using Microsoft.CodeAnalysis ;

namespace Clf.LogicSystem.SourceGenerator.Helpers
{

  public class ExecutionLog : System.IDisposable
  {

    private GeneratorExecutionContext m_generatorExecutionContext ;

    private System.Text.StringBuilder m_stringBuilder = new() ;

    private string m_hintName ;

    public ExecutionLog ( 
      GeneratorExecutionContext generatorExecutionContext,
      string                    title = "SourceGeneratorExecutionLog" 
    ) {
      m_generatorExecutionContext = generatorExecutionContext ;
      m_hintName = $"{title}.g.cs" ;
      m_stringBuilder.Append(
      $$"""
        //
        // {{title}}.g.cs ; generated on {{System.DateTime.Now}}
        //

        public static class {{title}}
        {
          // If you mention this in source code,
          // clicking on the definition will take you to the file
          public static void HasBeenGenerated ( ) 
          { }
        }

        #if false


        """
      ) ;
    }

    public void WriteLine ( string line ) 
    {
      m_stringBuilder.AppendLine(
        line
      ) ;
    }

    public static ExecutionLog operator + ( ExecutionLog log, string line ) 
    {
      log.WriteLine(line) ;
      return log ;
    }

    public string TextLines => m_stringBuilder.ToString() ;

    public void Dispose ( )
    {
      m_stringBuilder.AppendLine("") ;
      m_stringBuilder.AppendLine("#endif") ;
      // NORMALLY WE'D WRITE THE LOG TO A FILE,
      // AND THAT USED TO WORK FINE - BUT NOWADAYS FOR SOME REASON
      // m_generatorExecutionContext.AddSource(
      //   hintName : $"{m_hintName}",
      //   source   : TextLines
      // ) ;
    }

    public class TestClass
    {
      private ExecutionLog log = null! ;
      public void Test ( ) 
      {
        log = new ExecutionLog(
          new GeneratorExecutionContext()
        ) ;
        log += "Hello" ;
        log.WriteLine("Hello") ;
      }
    }

    public class TestClass2
    {
      private ExecutionLog Log_A = null! ;
      private ExecutionLog Log_B = null! ;
      public void Test ( ) 
      {
        Log_A = new ExecutionLog(
          new GeneratorExecutionContext()
        ) ;
        Log_B = new ExecutionLog(
          new GeneratorExecutionContext()
        ) ;
        using var disposer = new Disposer(Log_A,Log_B) ;
        Log_A.WriteLine("Hello") ;
        Log_B += "Hello" ;
      }
    }

  }

}