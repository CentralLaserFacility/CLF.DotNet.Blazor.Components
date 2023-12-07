//
// DescriptionAttribute.cs
//

namespace Clf.LogicSystem.Common.Attributes
{

  [System.AttributeUsage(System.AttributeTargets.All)]
  public sealed class DescriptionAttribute : System.Attribute
  {

    // The description can have a '$$' string which indicates
    // that we'll substitute a different value at that point ...

    public readonly string Description ;

    public DescriptionAttribute ( string description )
    {
      Description = description ;
    }

  }

}

