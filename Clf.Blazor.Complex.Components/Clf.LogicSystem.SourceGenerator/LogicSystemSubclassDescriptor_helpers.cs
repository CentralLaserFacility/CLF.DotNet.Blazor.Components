//
// LogicSystemSubclassDescriptorEx_helpers.cs
//

using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using System.Collections.Immutable ;
using System.Runtime.CompilerServices ;

namespace Clf.LogicSystem.SourceGenerator
{

  partial class LogicSystemSubclassDescriptor
  {

    private static void Assert (
      bool                                         x, 
      [CallerArgumentExpression(nameof(x))] string expression = ""
    ) { 
      if ( x is false ) 
      {
        throw new System.ApplicationException(
          $"Assert failed : {expression}"
        ) ;
      }
    }

    public void Experiments ( )
    {
      SemanticModel semanticModel = null! ;
      ClassDeclarationSyntax classDeclaration = null! ;
      // classDeclaration.GetType
      ISymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration)! ;
      SymbolKind kind = classSymbol.Kind ;
      if ( kind is SymbolKind.NamedType ) 
      {
        INamedTypeSymbol? namedTypeSymbol = classSymbol as INamedTypeSymbol ;
        System.Collections.Immutable.ImmutableArray<ISymbol> allMembers = namedTypeSymbol!.GetMembers() ;
        foreach ( ISymbol memberSymbol in allMembers ) 
        {
          if ( memberSymbol is IPropertySymbol propertySymbol )
          {
            ImmutableArray<SyntaxReference> syntaxNodeReferences = propertySymbol.DeclaringSyntaxReferences ;
            SyntaxNode propertySyntaxNode = syntaxNodeReferences[0].GetSyntax() ;
            if ( propertySyntaxNode.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PropertyDeclaration) )
            {
              var propertySyntaxNode2 = propertySyntaxNode as PropertyDeclarationSyntax ;
            }
          }
        }
      }
    }

  }


}