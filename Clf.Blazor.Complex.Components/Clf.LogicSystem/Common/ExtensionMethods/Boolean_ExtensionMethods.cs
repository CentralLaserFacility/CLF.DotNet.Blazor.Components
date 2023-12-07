//
// Boolean_ExtensionMethods.cs
//

namespace Clf.LogicSystem.Common.ExtensionMethods
{

  public static partial class Boolean_ExtensionMethods
  {

    public static bool WithActionOnFalseValue ( this bool value, System.Action action )
    {
      if ( value is false )
      {
        action() ;
      }
      return value ;
    }

    public static bool WithDebugBreakOnFalseValue ( this bool value )
    {
      if ( value is false )
      {
        System.Diagnostics.Debugger.Break() ;
      }
      return value ;
    }

    // TODO : WithActionOnSuccess ... CanXXX => TryXXX ...

    public static bool WithActionOnTrueValue ( this bool value, System.Action action )
    {
      if ( value is true )
      {
        action() ;
      }
      return value ;
    }

  }

}
