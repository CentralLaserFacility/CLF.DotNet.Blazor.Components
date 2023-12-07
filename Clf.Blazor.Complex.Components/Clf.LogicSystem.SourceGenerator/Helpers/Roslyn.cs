//
// Roslyn.cs
//

using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using System.Collections.Concurrent;
using System.Collections.Generic ;
using System.Linq ;
using System.Text.RegularExpressions;

namespace Clf.LogicSystem.SourceGenerator.Helpers
{

  public static class Roslyn
  {

    public static string GetNamespaceOfSyntaxNode ( SyntaxNode syntaxNode )
    {
      return string.Join(
        ".", 
        syntaxNode
        .Ancestors()
        .OfType<BaseNamespaceDeclarationSyntax>()
        .Reverse()
        .Select(
          baseNamespaceDeclaration => baseNamespaceDeclaration.Name 
        )
      ) ;
    }

    // Typically used to find classes that
    // inherit from 'Clf.LogicSystem.LogicSystemBase'

    public static bool ClassIsSubclassOf ( 
      ClassDeclarationSyntax classDeclaration, 
      string                 baseClassFullyQualifiedName 
    ) {
      BaseListSyntax? baseClassesList = classDeclaration.BaseList ;
      if ( baseClassesList != null ) 
      {
        foreach ( var baseClassType in baseClassesList.Types ) 
        {
          if ( baseClassFullyQualifiedName.Contains($"{baseClassType}") )
          {
            return true ;
          }
        }
      }
      return false ;
    }

    public static Dictionary<string, string> GetAttributes(
      IPropertySymbol propertySymbol
    ) {
      Dictionary<string,string> keyValuePairs = new () ;
      string key, val;

      var attributes = propertySymbol.GetAttributes();
      if (attributes != null)
        foreach (AttributeData attribute in attributes)
        {
          if (attribute?.ConstructorArguments.Length > 0)
          {

            key = $"{attribute.AttributeClass}".Replace("Attribute", "").Split('.').LastOrDefault();
            val = $"{attribute.ConstructorArguments[0].Value}";

            keyValuePairs.Add(key, val);

          }
        }

      return keyValuePairs;
    }

    public static string? GetFullyQualifiedTypeNameFromPropertyDeclarationOrNull ( 
      PropertyDeclarationSyntax propertyDeclarationSyntaxNode,
      SemanticModel             semanticModel
    ) {
      //
      // The 'PropertyDeclarationSyntax' node represents
      // the ENTIRE definition of a property, ie all this text :
      //
      //   public ComputedNode X => GetOrCreateComputedNode(
      //     () => Combine_OR(A,B)
      //   ) ;
      //
      // As such, we have to drill into its various syntactic and semantic components
      // to retrieve the information we want.
      //
      // We can get the name of the type, as mentioned in the source code,
      // from the PropertyDeclarationSyntax node.
      //
      TypeSyntax typeSyntaxNode = propertyDeclarationSyntaxNode.Type ;
      var typeSyntaxKind = typeSyntaxNode.Kind() ;
      if ( typeSyntaxKind == SyntaxKind.IdentifierName ) 
      {
        if ( typeSyntaxNode is IdentifierNameSyntax identifierName )
        {
          SyntaxToken identifierNameToken = identifierName.Identifier ;
          string nameFromText = identifierNameToken.Text ;
          string nameFromValueText = identifierNameToken.ValueText ;
          string? nameFromValue = identifierNameToken.Value as string ;
        }
      }
      string typeName = typeSyntaxNode.ToString() ; // 'ComputedNode'
      //
      // In many case the above will be fine.
      //
      // But ... the user might have specified the fully qualified name,
      // in which case the SyntaxNode will be telling us 'Clf.LogicSystem.ComputedNode'.
      //
      // So it's much more robust to use the SemanticModel to find the 'fully qualified' type name.
      //
      // 'GetDeclaredSymbol()' gets us the semantic 'Symbol' associated with the property
      // we're defining. That 'IPropertySymbol' will be able to tell us the name of
      // the property and also its data type.
      //
      IPropertySymbol? semanticPropertySymbol = semanticModel.GetDeclaredSymbol( 
        propertyDeclarationSyntaxNode 
      ) ;
      //
      // Having retrieved the semantic 'Symbol' associated with the property
      // we're defining, we can now get the fully qualified name of the type,
      // eg 'Clf.LogicSystem.ComputedNode'
      //
      ITypeSymbol? property_typeSymbol= semanticPropertySymbol?.Type ;
      string? fullyQualifiedTypeName = property_typeSymbol?.ToString() ;

      #if true
        //
        // Hmm, finding how to get that fully qualified type name wasn't easy.
        // Here are some failed attempts, which looked plausible but didn't work.
        // Clearly these API's are meant to work with different kinds of SyntaxNode,
        // however the documentation isn't at all clear on this ...
        //
        // 'GetSymbolInfo()' on our 'propertyDeclarationSyntaxNode' just returns
        // a SymbolInfo whose 'Symbol' is null ! That is of course (!!) because the
        // syntax node in question represents the entire definition (see above)
        // so it really doesn't have a Symbol accociated with it.
        //
        // The best way of using 'SymbolInfo' is to query the 'Kind',
        // which will be something like SymbolKind.Property, Field, Method etc. 
        //
        SymbolInfo semanticSymbolInfo = semanticModel.GetSymbolInfo(propertyDeclarationSyntaxNode) ;
        SymbolKind? symbolKind = semanticSymbolInfo.Symbol?.Kind ;
        //
        // 'GetTypeInfo()' also returns null !
        // For the same reason, there's no particular 'type' associated with the
        // entire definition.
        TypeInfo semanticTypeInfo       = semanticModel.GetTypeInfo(propertyDeclarationSyntaxNode) ;
        ITypeSymbol? semanticTypeSymbol = semanticTypeInfo.Type ;
        string? typeAsString            = semanticTypeSymbol?.ToDisplayString() ;
        //
      #endif

      return fullyQualifiedTypeName ;

    }

    // 
    // Taken from Clf.LogicSystem.ComputedNodeBase
    //
    public static string CompressMultiLineExpressionToSingleLine(
      string expressionTextLines
    )
    {
      // Should remove comments, if present ...
      Regex rgx = new(@"\/\*[\s\S]*?\*\/|\/\/.*");
      expressionTextLines = rgx.Replace(expressionTextLines, string.Empty);

      return new string(
        expressionTextLines.ToCharArray().Where(
          ch => !char.IsWhiteSpace(ch)
        ).ToArray()
      ).Replace("()", "").Replace("=>", "").TrimStart();
    }

    public static IEnumerable<TResult> ScanAllSyntaxTreesInCompilation_OLD<TSyntaxNode,TResult> (
      Compilation                       compilation,
      System.Func<TSyntaxNode,TResult?> handler
    ) 
    where TSyntaxNode : SyntaxNode
    where TResult : class
    {
      List<TResult> results = new List<TResult>() ;
      foreach ( SyntaxTree syntaxTree in compilation.SyntaxTrees ) 
      {
        foreach ( 
          TSyntaxNode interestingSyntaxNode 
          in syntaxTree.GetRoot().DescendantNodes().OfType<TSyntaxNode>()
        ) {
          TResult? result = handler(interestingSyntaxNode) ;
          if ( result != null )
          {
            results.Add(result) ;
          }
        }
      }
      return results ;
    }

    // Hmm, return IEnumerable vs IReadOnlyList ???

    public static IEnumerable<TResult> ScanAllSyntaxTreesInCompilation<TSyntaxNode,TResult> (
      Compilation                       compilation,
      System.Func<TSyntaxNode,TResult?> handler
    ) 
    where TSyntaxNode : SyntaxNode
    where TResult : class
    {
      IEnumerable<IEnumerable<TResult>> resultsPerSyntaxTree = compilation.SyntaxTrees.Select(
        syntaxTree => (
          syntaxTree.GetRoot(
          ).DescendantNodes(
          ).OfType<TSyntaxNode>(
          ).Select(
            interestingSyntaxNode => handler(interestingSyntaxNode)
          ).Where(
            result => result != null
          ).Select(
            result => result!
          )
        )
      ) ;
      IEnumerable<TResult> allResultsCombined = resultsPerSyntaxTree.SelectMany(
        z => z
      ) ;
      return allResultsCombined.ToList() ; // Leave the 'ToList()' up to the client code ???
    }
    
    // Support for logging

    public static bool HandleLogMessages = true ;

    public static ConcurrentQueue<string> g_logMessages = new() ;

    public static void WriteLogMessage ( string message )
    {
      if ( HandleLogMessages ) 
      {
        g_logMessages.Enqueue(message) ;
      }
    }

    public static void ExtractAllAvailableLogMessages ( System.Action<string> handleLogMessage  ) 
    {
      while (
        g_logMessages.TryDequeue(
          out string logMessage 
        )
      ) {
        handleLogMessage(
          logMessage
        ) ;
      }
    }

    public static void HandleAllAvailableLogMessages ( System.Action<string> handleLogMessage  ) 
    {
      string[] allAvailableLogMessages_snapshot = g_logMessages.ToArray() ;
      foreach ( string logMessage in allAvailableLogMessages_snapshot )
      {
        handleLogMessage(
          logMessage
        ) ;
      }
    }

  }

}