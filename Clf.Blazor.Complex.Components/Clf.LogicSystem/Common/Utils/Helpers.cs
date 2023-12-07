using Clf.Common.ExtensionMethods;
using Clf.Common.Utils;
using Clf.LogicSystem.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.LogicSystem.Common.Utils
{
  public static class Helpers
  {
    //
    // For a given type, such as int or an enum, return the number
    // of characters typically required to represent the value
    //

    public static int GetEstimatedLengthOfStringValueOfType(System.Type type)
    {
      if (
        type.IsNullableType(
          out var nullableType
        )
      )
      {
        type = nullableType;
      }
      if (type.IsEnum)
      {
        var strings = EnumHelpers.GetNamesAndIntegerValuesAsStringsForEnumType(type);
        return strings.Max(
          s => s.Length
        );
      }
      else if (type == typeof(bool))
      {
        return LogicHelpers.GetValueAsStringForDisplay(false).Length; // false (0) or true (1)
      }
      else if (type.IsNumericType())
      {
        return 10; // ???
        // return type switch 
        // {
        // System.Type when type == typeof(int)   => int.MaxValue.ToString().Length,
        // System.Type when type == typeof(short) => 8,
        // System.Type when type == typeof(int)   => 8,
        // System.Type when type == typeof(int)   => 8,
        // System.Type when type == typeof(int)   => 8,
        // _                                      => 6
        // } ;
      }
      else
      {
        return 8;
      }
    }
  }
}
