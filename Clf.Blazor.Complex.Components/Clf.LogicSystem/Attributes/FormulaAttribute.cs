//
// Formula.cs
//

namespace Clf.LogicSystem.Attributes
{

  // TODO : issue an warning if this [Formula] attribute
  // is inappropriately applied to an InputNodeBase ...

  public sealed class FormulaAttribute : LogicNodeAttribute
  {
    public FormulaAttribute ( string value ) : base(value) { }
  }

}
