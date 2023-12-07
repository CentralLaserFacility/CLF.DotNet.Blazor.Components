//
// PermissibleStringValuesAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  // Not used anywhere, but let's keep this in our back pocket ...

  public sealed class PermissibleStringValuesAttribute : LogicNodeAttribute
  {

    //
    // Example :
    //
    //   [PermissibleStringValues("ERROR,WARNING,OK")]
    //   public InputNode_StringOrNull => GetOrCreateInputNode_String() ;
    //

    public PermissibleStringValuesAttribute ( string permissibleStringValues_commaSeparated ) : 
    base(permissibleStringValues_commaSeparated) 
    { }

  }

}
