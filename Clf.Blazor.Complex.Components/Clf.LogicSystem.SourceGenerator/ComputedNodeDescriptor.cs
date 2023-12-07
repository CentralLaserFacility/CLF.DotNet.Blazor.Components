//
// ComputedNodeDescriptor.cs
//

namespace Clf.LogicSystem.SourceGenerator
{

  /// <summary>
  /// This describes a 'ComputedNode' property declared in a LogicSystem.
  /// </summary>
  public class ComputedNodeDescriptor : LogicNodeDescriptor, System.IEquatable<ComputedNodeDescriptor>
  {

    public string? LambdaBodyText                 { get ; set ; } = null ; 

    public string? LambdaBodyArgs                 { get ; set ; } = null ; // All arguments, including methods etc
    
    public string? LambdaBodyProperties           { get ; set ; } = null ; // Just the properties !!

    public bool Equals ( ComputedNodeDescriptor other )
    {
      return (
        this.PropertyName                   == other.PropertyName 
     && this.FullyQualifiedPropertyTypeName == other.FullyQualifiedPropertyTypeName
     && this.LambdaBodyText                 == other.LambdaBodyText
     ) ;
    }

  }

}