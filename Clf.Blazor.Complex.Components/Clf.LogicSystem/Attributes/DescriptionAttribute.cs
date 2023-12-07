//
// Description.cs
//

namespace Clf.LogicSystem.Attributes
{

  //
  // This can be provided in addition to the '[Formula]' attribute.
  //

  public sealed class DescriptionAttribute : LogicNodeAttribute
  {
    public DescriptionAttribute ( string value ) : base(value) { }
  }

}
