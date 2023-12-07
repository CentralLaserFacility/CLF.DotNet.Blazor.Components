//
// GroupNameAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  // Not currently used - but will provide a way of
  // grouping a bunch or related nodes together, to make
  // a network display more intelligible ...

  public sealed class GroupNameAttribute : LogicNodeAttribute
  {
    public GroupNameAttribute ( string value ) : base(value) { }
  }

}
