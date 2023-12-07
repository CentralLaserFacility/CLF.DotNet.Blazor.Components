//
// GeneratedSourceFile.cs
//

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Clf.LogicSystem.SourceGenerator.Helpers
{
  
  // https://stackoverflow.com/questions/33090499/how-to-get-the-base-class-name-of-a-class-via-roslyn

  // TODO : add Methods ??

  public class ClassDescriptor
  {
    public string ClassName { get ; set ; }
    public string Namespace { get ; set ; }
    public List<PropertyDescriptor> Properties { get ; set ; }
    public List<string> BaseClassNames { get ; set ; }
    public ClassDescriptor ( )
    {
      Properties = new List<PropertyDescriptor>();
      BaseClassNames = new List<string>();
    }
  }

  public class PropertyDescriptor
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public PropertyDescriptor ( string name, string type )
    {
      Name = name ;
      Type = type ;
    }
  }

  public static class ClassExplorer
  {
    public static ClassDescriptor Parse ( string classDefinitionText )
    {
      var classDescriptor = new ClassDescriptor() ; // !!!!!!!!!!!!!!!!!!!!
      var syntaxTree = CSharpSyntaxTree.ParseText(classDefinitionText) ;
      var memberDeclarations = syntaxTree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>() ;
      foreach ( var memberDeclaration in memberDeclarations )
      {
        if ( memberDeclaration is PropertyDeclarationSyntax property )
        {
          classDescriptor.Properties.Add(
            new PropertyDescriptor(
              property.Identifier.ValueText, 
              property.Type.ToString()
            )
          ) ;
        }
        else if ( memberDeclaration is NamespaceDeclarationSyntax namespaceDeclaration )
        {
          classDescriptor.Namespace = namespaceDeclaration.Name.ToString() ;
        }
        else if ( memberDeclaration is ClassDeclarationSyntax classDeclaration )
        {
          classDescriptor.ClassName = classDeclaration.Identifier.ValueText ;
          classDescriptor.BaseClassNames = GetBaseClassNames(classDeclaration).ToList() ;
        }
        else if ( memberDeclaration is MethodDeclarationSyntax method )
        {
          // Console.WriteLine("Method: " + method.Identifier.ValueText);
        }
      }
      return classDescriptor ;
    }

    private static IEnumerable<string> GetBaseClassNames (
      ClassDeclarationSyntax classDeclaration
    ) {
      if ( classDeclaration.BaseList == null )
      {
        return Enumerable.Empty<string>() ;
      }
      else
      {
        return classDeclaration.BaseList.Types.Select(
          x => x.Type.ToString()
        ) ;
      }
    }

  }

}