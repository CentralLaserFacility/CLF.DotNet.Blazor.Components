//
// ShowAsCollapsedAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  //
  // Should the Node be displayed in a compact 'collapsed' form
  // as a tiny square box, rather than as a full-sized box that is
  // large enough to accommodate a 'label' ?
  //

  public sealed class ShowAsCollapsedAttribute : LogicNodeAttribute
  {
    public ShowAsCollapsedAttribute ( bool value ) : 
    base(
      value
      ? "yes"
      : "no"
    ) { 
    }
  }

}
