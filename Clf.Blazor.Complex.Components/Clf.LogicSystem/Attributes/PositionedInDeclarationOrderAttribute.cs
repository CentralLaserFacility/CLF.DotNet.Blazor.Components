//
// PositionedInDeclarationOrderAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  public sealed class PositionedInDeclarationOrderAttribute : LogicNodeAttribute
  {
    public PositionedInDeclarationOrderAttribute ( 
      // A possible mechanism for governing the order-of-declaration of Properties ?
      [System.Runtime.CompilerServices.CallerFilePath]   string? filePath   = null,
      [System.Runtime.CompilerServices.CallerLineNumber] int     lineNumber = 0
    ) : 
    base(
      $"{filePath}#{lineNumber:D6}"
    ) { 
    }
  }

}
