//
// GeneratedPropertyNameIsAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  // We can decorate an [ObservableProperty] with this attribute
  // to provide a way of navigating to the actual property
  // eg when we want to see all references to that property.

  public sealed class GeneratedPropertyNameIsAttribute : System.Attribute
  {
    public GeneratedPropertyNameIsAttribute ( string? generatedPropertyName = null ) 
    { }
  }

}
