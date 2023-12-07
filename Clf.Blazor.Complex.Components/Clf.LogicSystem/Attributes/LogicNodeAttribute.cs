//
// LogicNodeAttribute.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;

namespace Clf.LogicSystem.Attributes
{

  [System.AttributeUsage(System.AttributeTargets.Property)]
  public abstract class LogicNodeAttribute : System.Attribute
  {
    public string Name => this.GetType().AttributeName() ;
    public readonly string Value ;
    protected LogicNodeAttribute ( string value )
    {
      Value = value ;
    }
  }

}
