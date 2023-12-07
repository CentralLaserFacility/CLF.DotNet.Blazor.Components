//
// ReliesOnObservablePropertyAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  //
  // This gives us a way of annotating 'derived' properties
  // with an indication of which [Observable] properties they depend on.
  // Each of those [Observable] properties should have an attribute
  // that mentions our property as a dependent.
  //
  // For now this is just a comment, but in principle we could run a check
  // that scans all the properties and ensures that the expected attributes
  // have actually been supplied.
  //
  // For example :
  //
  //   [ObservableProperty]
  //   
  //   [NotifyPropertyChangedFor(nameof(B))] // THIS NEEDS TO BE PRESENT !!!
  //   public int m_A = 123 ;
  //
  //   [ReliesOnObservableProperty(nameof(A))]
  //   public int B => A * 2 ;
  //

  public sealed class ReliesOnObservablePropertyAttribute : System.Attribute
  {
    public ReliesOnObservablePropertyAttribute ( string propertyName ) { }
  }

}
