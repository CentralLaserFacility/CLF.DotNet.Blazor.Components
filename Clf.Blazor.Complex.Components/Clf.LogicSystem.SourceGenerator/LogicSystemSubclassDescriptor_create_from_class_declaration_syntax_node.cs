//
// LogicSystemSubclassDescriptorEx_create_from_class_declaration_syntax_node.cs
//

using System;
using System.Collections.Generic ;
using System.Collections.Immutable;
using System.Linq ;

using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace Clf.LogicSystem.SourceGenerator
{

  // Every time we're woken up with a notification regarding a ClassDeclarationSyntax node
  // that pertains to a class that derives from 'Clf.LogicSystem.LogicSystemBase',
  // we interrogate the semantic model and build a view of the class properties.

  //
  // With a partial class, we get an exception 'Syntax node is not within syntax tree',
  // as the Semantic Model we've been passed only pertains to the ClassDeclarationSyntax node
  // that we've been handed ??
  //

  partial class LogicSystemSubclassDescriptor
  {
    public const string LOGIC_SYSTEM_BASE_CLASS = "Clf.LogicSystem.LogicSystemBase";
    public static readonly string[] IGNORE_BASE_CLASSES = new[] {
      "ComputedNodeBase",
      "InputNodeBase"
    };
    public const string COMPUTED_NODE_CLASS = "ComputedNode";
    public const string INPUT_NODE_CLASS = "InputNode";

    public static LogicSystemSubclassDescriptor CreateFromClassDeclaration (
      ClassDeclarationSyntax classDeclarationSyntaxNode,
      SemanticModel          semanticModel
    ) {
      LogicSystemSubclassDescriptor logicSystemSubclassDescriptor = new(){
        ClassName     = classDeclarationSyntaxNode.Identifier.ValueText,
        NamespaceName = Clf.LogicSystem.SourceGenerator.Helpers.Roslyn.GetNamespaceOfSyntaxNode(classDeclarationSyntaxNode)
      } ; 
      string propertyNameBeingHandled = "?PROPERTY?" ;
      try
      {
        bool isLogicSystem = Clf.LogicSystem.SourceGenerator.Helpers.Roslyn.ClassIsSubclassOf(
          classDeclarationSyntaxNode,
          LOGIC_SYSTEM_BASE_CLASS
        ) ;
        Assert(isLogicSystem) ;
        // Interrogate the semantic model
        ISymbol class_symbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntaxNode)! ;
        Assert(class_symbol.Kind is SymbolKind.NamedType) ;
        INamedTypeSymbol classSymbol = (INamedTypeSymbol) class_symbol  ;
        ImmutableArray<ISymbol> symbolsForAllClassMembers = classSymbol.GetMembers() ;

        string? fullyQualifiedPropertyTypeName;
        foreach (IPropertySymbol propertySymbol in symbolsForAllClassMembers.OfType<IPropertySymbol>())
        {
          propertyNameBeingHandled = propertySymbol.Name;
          ImmutableArray<SyntaxReference> syntaxNodeReferences = propertySymbol.DeclaringSyntaxReferences;
          SyntaxNode property_syntaxNode = syntaxNodeReferences[0].GetSyntax();
          Assert(property_syntaxNode.IsKind(SyntaxKind.PropertyDeclaration));
          PropertyDeclarationSyntax propertySyntaxNode = (PropertyDeclarationSyntax)property_syntaxNode;

          fullyQualifiedPropertyTypeName = Clf.LogicSystem.SourceGenerator.Helpers.Roslyn.GetFullyQualifiedTypeNameFromPropertyDeclarationOrNull(
              propertySyntaxNode,
              semanticModel
            );

          if (fullyQualifiedPropertyTypeName != null)
          {
            if (IGNORE_BASE_CLASSES.Any(fullyQualifiedPropertyTypeName.Contains))
            {
              logicSystemSubclassDescriptor.ExceptionMessages.Add($"Logic node {propertyNameBeingHandled} should not be defined using abstract type {fullyQualifiedPropertyTypeName}");
            }
            else
            {

              if (
                  fullyQualifiedPropertyTypeName.StartsWith(INPUT_NODE_CLASS)
                  || fullyQualifiedPropertyTypeName.Contains($".{INPUT_NODE_CLASS}")
              )
              {

                //
                // Create an instance of 'InputNodeDescriptor' that we'll populate.
                //
                var inputNodeDescriptorBeingBuilt =
                    new InputNodeDescriptor()
                    {
                      PropertyName = propertyNameBeingHandled,
                      FullyQualifiedPropertyTypeName = fullyQualifiedPropertyTypeName
                    };

                //
                // Retrieve the attributes of the InputNode property 
                //
                inputNodeDescriptorBeingBuilt.OverwriteAttributes(
                  Clf.LogicSystem.SourceGenerator.Helpers.Roslyn.GetAttributes(propertySymbol)
                );

                logicSystemSubclassDescriptor.InputNodeDescriptors.Add(inputNodeDescriptorBeingBuilt);

              }
              else if (
                fullyQualifiedPropertyTypeName.StartsWith(COMPUTED_NODE_CLASS)
                || fullyQualifiedPropertyTypeName.Contains($".{COMPUTED_NODE_CLASS}")
              )
              {

                //
                // Create an instance of 'ComputedNodeDescriptor' that we'll populate.
                //
                var computedNodeDescriptorBeingBuilt = new ComputedNodeDescriptor();

                //
                // Retrieve the name of the ComputedNode property
                //
                string computedNodePropertyName = propertySyntaxNode.Identifier.ValueText;
                computedNodeDescriptorBeingBuilt.PropertyName = computedNodePropertyName;

                //
                // Retrieve the attributes of the ComputedNode property
                //
                computedNodeDescriptorBeingBuilt.OverwriteAttributes(
                  Clf.LogicSystem.SourceGenerator.Helpers.Roslyn.GetAttributes(propertySymbol)
                );

                //
                // Retrieve the type of the ComputedNode property
                //
                computedNodeDescriptorBeingBuilt.FullyQualifiedPropertyTypeName = fullyQualifiedPropertyTypeName;

                //
                // Next we need to drill into the definition of the property declaration
                // and extract the body of the Lambda Expression that defines the Formula.
                //
                ParenthesizedLambdaExpressionSyntax? lambdaExpressionRepresentingComputedNodeFormula = (
                propertySyntaxNode.DescendantNodes(
                    ).OfType<ParenthesizedLambdaExpressionSyntax>(
                  ).FirstOrDefault()
                );
                computedNodeDescriptorBeingBuilt.LambdaBodyText = Clf.LogicSystem.SourceGenerator.Helpers.Roslyn.CompressMultiLineExpressionToSingleLine(
                  lambdaExpressionRepresentingComputedNodeFormula?.Body.ToString() ?? ""
                );
                //
                // Finally, we need to extract the names of all identifiers that appear
                // in the body of the lambda expression. The identifiers we're interested in
                // are ones that pertain to LogicNode properties - but we'll also encounter
                // identifiers that refer to function names (eg 'Combine_XX') and other items
                // that might be in scope, such as constants ot other properties of the class.
                //
                IEnumerable<IdentifierNameSyntax>? lambdaBodyIdentifierNames = lambdaExpressionRepresentingComputedNodeFormula?.DescendantNodes().OfType<IdentifierNameSyntax>() ?? Enumerable.Empty<IdentifierNameSyntax>();
                string lambdaBodyIdentifierNamesList = string.Join(
                  ",",
                  lambdaBodyIdentifierNames
                  .Where(identifier => identifier.Identifier.ValueText != "Value")
                  .Select(
                    identifier => identifier.Identifier.ValueText
                  ).Distinct().OrderBy(x => x)
                );
                computedNodeDescriptorBeingBuilt.LambdaBodyArgs = lambdaBodyIdentifierNamesList;

                //
                IEnumerable<IdentifierNameSyntax>? lambdaBodyPropertyNames = lambdaExpressionRepresentingComputedNodeFormula?.DescendantNodes().OfType<IdentifierNameSyntax>(
                ).Where(
                  // We're only interested in properties, not methods ...
                  // EITHER OF THESE FILTER PREDICATES WORK !!!
                  identifierName => semanticModel.GetSymbolInfo(identifierName).Symbol?.Kind == SymbolKind.Property
                // identifierName => semanticModel.GetSymbolInfo(identifierName).Symbol is IPropertySymbol
                ) ?? Enumerable.Empty<IdentifierNameSyntax>();
                string lambdaBodyPropertyNamesList = string.Join(
                  ",",
                  lambdaBodyPropertyNames
                  .Where(identifier => identifier.Identifier.ValueText != "Value")
                  .Select(
                    identifier => identifier.Identifier.ValueText
                  ).Distinct().OrderBy(x => x)
                );
                computedNodeDescriptorBeingBuilt.LambdaBodyProperties = lambdaBodyPropertyNamesList;

                logicSystemSubclassDescriptor.ComputedNodeDescriptors.Add(computedNodeDescriptorBeingBuilt);
              }
            }
          }
        }
      }
      catch ( System.Exception x )
      {
        logicSystemSubclassDescriptor.ExceptionMessages.Add(
          $"{logicSystemSubclassDescriptor.FullyQualifiedClassName} {propertyNameBeingHandled} {x.Message}"
        ) ; 
      }
      return logicSystemSubclassDescriptor ;
    }

  }


}