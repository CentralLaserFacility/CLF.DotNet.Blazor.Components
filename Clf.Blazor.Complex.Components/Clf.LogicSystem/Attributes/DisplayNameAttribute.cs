//
// DisplayNameAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  //
  // The 'display name' is typically the name of the associated PV,
  // which often can't be represented as a valid C# property name as it
  // contains special characters.
  //
  // If a [DisplayName(...)] attribute is omitted, the Property Name will be used.
  //

  public sealed class DisplayNameAttribute : LogicNodeAttribute
  {
    public DisplayNameAttribute ( string value ) : base(value) { }
  }

}
