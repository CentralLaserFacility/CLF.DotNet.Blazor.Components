//
// GeneratedSourceFile.cs
//

using Microsoft.CodeAnalysis ;

namespace Clf.LogicSystem.SourceGenerator.Helpers
{

  public class GeneratedSourceFile : System.IDisposable
  {

    private GeneratorExecutionContext m_generatorExecutionContext ;

    private string m_hintName ;

    private System.Text.StringBuilder m_stringBuilder = new() ;

    public GeneratedSourceFile ( 
      GeneratorExecutionContext generatorExecutionContext,
      string                    namespaceName,
      string                    className 
    ) {

      m_generatorExecutionContext = generatorExecutionContext ;
      m_hintName = $"{namespaceName}_{className}.g.cs" ;
      m_stringBuilder.Append(
      $$"""
        //
        // {{m_hintName}} ; generated on {{System.DateTime.Now}}
        //

        namespace {{namespaceName}} ;

        public partial class {{className}}
        {


        """
      ) ;
    }
    
    public void AddCodeLines ( params string[] codeLines ) 
    {
      foreach ( var codeLine in codeLines )
      {
        AddCodeLine(codeLine) ;
      }
    }

    public void AddCodeLine ( string codeLine ) 
    {
      m_stringBuilder.AppendLine(
        "  " 
      + codeLine
      ) ;
    }

    public static GeneratedSourceFile operator + ( GeneratedSourceFile generatedSourceFile, string codeLine ) 
    {
      generatedSourceFile.AddCodeLine(codeLine) ;
      return generatedSourceFile ;
    }

    public string TextLines => m_stringBuilder.ToString() ;

    public void Dispose ( )
    {
      m_stringBuilder.AppendLine("") ;
      m_stringBuilder.AppendLine("}") ;
      m_generatorExecutionContext.AddSource(
        hintName : $"{m_hintName}",
        source   : TextLines
      ) ;
    }

  }

}