//
// IncrementalSourceGenerator.cs
//

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;

namespace Clf.LogicSystem.SourceGenerator
{

    //
    // Spec and design for Incremental generators ...
    // https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md
    //

    // If we comment out this 'Generator' attribute, then Visual Studio will NOT use the Generator.
    [Generator]
  public class IncrementalSourceGenerator : IIncrementalGenerator
  {

    public void Initialize ( IncrementalGeneratorInitializationContext initialisationContext )
		{

      //
      // Create a 'provider' that will generate sequences
      // of 'SourceGenerator.LogicSystemSubclassDescriptor' values
      // that pertain to SyntaxNodes of type 'ClassDeclarationSyntax'.
      //

      //
      // Note that this 'SyntaxProvider' will be triggered on ANY change
      // to any source code in the project, and each and every SyntaxNode
      // will be offered to our 'predicate' for testing. If that returns true,
      // indicating that the SyntaxNode is relevant, then the 'transform'
      // is activated and the result it produces is passed along the pipeline.
      //
      // The infrastructure will only propagate this new result if it's different
      // from the current one. But it's up to us to avoid running an expensive
      // 'transform' if the inputs to it haven't actually changed, and/or (B) make
      // sure that an identical output from a transform is recognised as such. 
      //
      IncrementalValuesProvider <LogicSystemSubclassDescriptor> logicSystemSubclassDescriptorsProviderEx = 

        initialisationContext.SyntaxProvider.CreateSyntaxProvider (
          predicate : static (syntaxNode,cancellationToken) => {

            // We're interested in changes that pertain to SyntaxNodes
            // representing classes that are subclasses of 'LogicSystemBase'.
            // At this stage we don't have access to the Semantic Model.
            // This predicate is invoked on pretty much every keystroke,
            // so it needs to be quick.
            return (
               syntaxNode is ClassDeclarationSyntax classDeclaration
            && Clf.LogicSystem.SourceGenerator.Helpers.Roslyn.ClassIsSubclassOf(
                 classDeclaration,
                 "Clf.LogicSystem.LogicSystemBase"
               ) 
            ) ;

          },
          transform : static (
            generatorSyntaxContext,
            cancellationToken
          ) => {

            // When we're told that there's been an interesting update
            // to a SyntaxNode, we know that the SyntaxNode is a
            // Class Declaration and that it represents a LogicSystem.
            // In response, we scan the child nodes (representing properties etc)
            // and build a LogicSystemSubclassDescriptor that summarises that
            // LogicSystem class and its ComputedProperty nodes.
            // At this stage we DO have access to the Semantic Model.
            // This transformation function is invoked on pretty much
            // every keystroke where the 'predicate' has PREVIOUSLY
            // returned 'true' ... ???
            return LogicSystemSubclassDescriptor.CreateFromClassDeclaration(
              (ClassDeclarationSyntax) generatorSyntaxContext.Node,
              generatorSyntaxContext.SemanticModel
            ) ;

          }
        ) ;

      //
      // Register our 'GenerateSourceCode' function so that it will be invoked
      // every time the pipeline produces a new array of 'LogicSystemSubclassDescriptor'
      // values to deal with.
      //
      initialisationContext.RegisterSourceOutput(
        logicSystemSubclassDescriptorsProviderEx, GenerateSourceCode
      ) ;

      return;
      
    }

    public LogicSystemSubclassDescriptor? LogicSystemSubclassDescriptor  { get ; private set ; }

    private void GenerateSourceCode(
      SourceProductionContext sourceProductionContext,
      LogicSystemSubclassDescriptor logicSystemSubclassDescriptor
    )
    {
      LogicSystemSubclassDescriptor = logicSystemSubclassDescriptor;
      logicSystemSubclassDescriptor.GenerateCode(
        (fileName,sourceCode) => sourceProductionContext.AddSource(
          hintName   : fileName,
          sourceText : SourceText.From(
            sourceCode,
            System.Text.Encoding.UTF8
          )
        )
      );
    }

  }

}
