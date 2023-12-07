//
// Integer_ExtensionMethods.cs
//

namespace Clf.LogicSystem.Common.ExtensionMethods
{

  public static partial class Integer_ExtensionMethods
  {

    public static void Repeat ( this int n, System.Action action )
    {
      for ( int i = 0 ; i < n ; i++ )
      {
        action() ;
      }
    }

  }

}
