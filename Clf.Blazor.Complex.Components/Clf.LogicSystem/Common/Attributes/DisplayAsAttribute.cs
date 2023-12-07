//
// DisplayAsAttribute.cs
//

namespace Clf.LogicSystem.Common.Attributes
{

  [System.AttributeUsage(System.AttributeTargets.Field|System.AttributeTargets.Property)]
  public sealed class DisplayAsAttribute : System.Attribute
  {

    public readonly string DisplayText ;

    public DisplayAsAttribute ( string displayText )
    {
      DisplayText = displayText ;
    }

  }

}

