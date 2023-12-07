//
// LogicHelpers.cs
//

using Clf.Common.ExtensionMethods;
using Clf.Common.Utils;
using System.Linq;

namespace Clf.LogicSystem.Miscellaneous
{

  //
  // You can use '|' and '&' in logical expressions involving 'bool?' types, but beware,
  // the results may not be what you initially expect ! For example, it's possible for
  //
  //   a | b
  //
  // ... to give a non null result even if one of the operands is null !
  //
  // For nullable types other than bool, any 'null' operand will give a null result.
  // For example
  //
  //  true  | null => true
  //  false & null => false
  //
  // When you think about it, the rules make perfect sense. For example if you have
  // an '|' expression, and one of the operands is 'true', then the result should be
  // true regardless of whether the other operand is true or false. If the other operand
  // is null, it means we don't know whether it's true or false - but that's irrelevant
  // to the outcome.
  //
  // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators
  // See the section on 'Nullable Boolean logical operators'
  //
  // The 'conditional' logical operators || and && don't support 'bool?' operands,
  // so to use them you have to convert the operands to bool eg using '==',
  // eg 'a == true' or 'a is true'.
  //
  // But actually, you need to AVOID USING THESE 'CONDITIONAL' LOGICAL OPERATORS
  // because the mechanism that the Clf.Clf.LogicSystem uses to discover the network
  // of dependencies between the nodes expects *all* logical paths to be evaluated.
  // This is what happens with the '|' and '&' operators, which do *not* use
  // the 'short-circuit evaluation' scheme that you get with '||' and '&&'.
  //
  // Instead of using result = condition ? trueExpression : falseExpression,
  // use one of the following alternatives :
  //
  // 1.  result = (
  //        condition & expression-to-use-when-condition-is-true
  //     | ~condition & expression-to-use-when-condition-is-false
  //     ) ;
  //
  // 2.  result = IfThenElse(
  //       condition,
  //       expression-to-use-when-condition-is-true,
  //       expression-to-use-when-condition-is-false
  //     ) ;
  //

  //
  // These helper functions are useful when defining the computed values.
  //
  // They are declared in a static helper class that we
  // make available with a 'using static' declaration.
  //
  //  using static Clf.Clf.LogicSystem.LogicHelpers ;
  //

  //
  // Hmm, in principle we could log the invocations of these methods
  // and build a sub-network that reflects the logical structure !!!
  //
  // Another possibility would be to use Roslyn to explore the lambda,
  // and generate the network dependencies at compile time rather than
  // at runtime.
  //
  // Another possibility would be to have computed nodes that represent 
  // AND and OR etc, and explicitly include these in the network. The downside
  // is that these need to be named ??
  //

  public static class LogicHelpers
  {

    //
    // This helper method ensures we always have a consistent conversion to a string.
    //
    // NOTE THAT WHEN THE VALUE IS AN ENUM, WE RETURN THE INTEGER VALUE AS A STRING,
    // RATHER THAN THE NAME ASSOCIATED WITH THE ENUM VALUE.
    // ARGUABLY WE SHOULD BE RETURNING THE NAME, BUT FOR THE TIME BEING THIS IS NECESSARY
    // TO MAKE THINGS WORK NICELY WITH CHANNEL ACCESS.
    //

    public static string GetValueAsString ( object? value )
    => value switch {
      null               => "null",
      true               => "1", // Channel access likes to represent a boolean 'true'  as 1
      false              => "0", // Channel access likes to represent a boolean 'false' as 0
      int    intValue    => intValue.ToString(),
      double doubleValue => doubleValue.ToString(),
      short  shortValue  => shortValue.ToString(),
      float  floatValue  => floatValue.ToString(),
      string stringValue => stringValue,
      _ => ( 
        value?.GetType().IsEnum == true 
      )
      ? (
        (int) value!
      ).ToString()
    : (
        value?.ToString() ?? "null"
      )
    } ;

    public static string GetValueAsStringForDisplay ( object? value )
    => value switch {
      null               => "null",
      true               => "true (1)", 
      false              => "false (0)",
      int    intValue    => intValue.ToString(),
      double doubleValue => doubleValue.ToString(),
      short  shortValue  => shortValue.ToString(),
      float  floatValue  => floatValue.ToString(),
      string stringValue => stringValue,
      _ => ( 
        value?.GetType().IsEnum == true 
      )
      ? (
          EnumHelpers.GetNameAndIntegerValueAsString(
            value as System.Enum
          )
          // value!.ToString()!
        )
      : (
          value?.ToString() ?? "null"
        )
    } ;

    // Combining boolean values

    // TODO : Unit tests !!!

    public static bool? Combine (
      System.Func<bool?,bool?,bool?> combineFunc,
      params bool?[]                 inputs
    ) => inputs.Aggregate(
      combineFunc
    ) ;

    public static bool Combine (
      System.Func<bool,bool,bool> combineFunc,
      params bool[]               inputs
    ) => inputs.Aggregate(
      combineFunc
    ) ;

    public static T Combine<T> (
      System.Func<T,bool?,T> combineFunc,
      params bool?[]         inputs
    ) 
    where T : struct
    {
      T result = default ;
      inputs.ForEachItem(
        x => result = combineFunc(result,x)
      ) ;
      return result ;
    }

    public static T Combine<T> (
      System.Func<T,bool,T> combineFunc,
      params bool[]         inputs
    ) 
    where T : struct
    {
      T result = default ;
      inputs.ForEachItem(
        x => result = combineFunc(result,x)
      ) ;
      return result ;
    }

    public static int Combine_Counting (
      System.Func<bool?,bool> countFunc,
      params bool?[]          inputs
    ) {
      int result = 0 ;
      inputs.ForEachItem(
        x => {
          if ( countFunc(x) )
          {
            result++ ;
          }
        }
      ) ;
      return result ;
    }

    public static int Combine_Counting (
      System.Func<bool,bool> countFunc,
      params bool[]          inputs
    ) {
      int result = 0 ;
      inputs.ForEachItem(
        x => {
          if ( countFunc(x) )
          {
            result++ ;
          }
        }
      ) ;
      return result ;
    }

    public static void Combine_Test ( ) 
    {
      int nDefinitelyTrue = Combine<int>(
        (resultSoFar,x) => (
          x == true
          ? resultSoFar + 1
          : resultSoFar
        ),
        new bool?[]{null,true,true,false}
      ) ;
    }

    public static bool? Combine_OR ( params bool?[] inputs )
    => Combine(
      (a,b) => a | b,
      inputs
    ) ;

    public static bool Combine_OR ( params bool[] inputs )
    => Combine(
      (a,b) => a | b,
      inputs
    ) ;

    public static bool? Combine_NOR ( params bool?[] inputs )
    => Combine(
      (a,b) => ! ( a | b ),
      inputs
    ) ;

    public static bool Combine_NOR ( params bool[] inputs )
    => Combine(
      (a,b) => ! ( a | b ),
      inputs
    ) ;

    public static bool? Combine_AND ( params bool?[] inputs )
    => Combine(
      (a,b) => a & b,
      inputs
    ) ;

    public static bool Combine_AND ( params bool[] inputs )
    => Combine(
      (a,b) => a & b,
      inputs
    ) ;

    public static bool? Combine_NAND ( params bool?[] inputs )
    => Combine(
      (a,b) => ! ( a & b ),
      inputs
    ) ;

    public static bool Combine_NAND ( params bool[] inputs )
    => Combine(
      (a,b) => ! ( a & b ),
      inputs
    ) ;

    public static bool? Combine_XOR ( params bool?[] inputs )
    => Combine(
      (a,b) => a ^ b,
      inputs
    ) ;

    public static bool Combine_XOR ( params bool[] inputs )
    => Combine(
      (a,b) => a ^ b,
      inputs
    ) ;

    public static bool? Combine_EQUAL ( params bool?[] inputs )
    => Combine(
      (a,b) => a == b,
      inputs
    ) ;

    public static bool Combine_EQUAL ( params bool[] inputs )
    => Combine(
      (a,b) => a == b,
      inputs
    ) ;

    public static bool? Combine_NOT_EQUAL ( params bool?[] inputs )
    => Combine(
      (a,b) => a != b,
      inputs
    ) ;

    public static bool Combine_NOT_EQUAL ( params bool[] inputs )
    => Combine(
      (a,b) => a != b,
      inputs
    ) ;

    public static bool? IfThenElse (
      bool  conditionToTest,
      bool? ifConditionTrue,
      bool? ifConditionFalse
    ) => (
      conditionToTest
      ? ifConditionTrue
      : ifConditionFalse
    ) ;

    public static bool IfThenElse (
      bool  conditionToTest,
      bool ifConditionTrue,
      bool ifConditionFalse
    ) => (
      conditionToTest
      ? ifConditionTrue
      : ifConditionFalse
    ) ;

    // public static bool? Switch<TCustomAttribute> (
    //   TCustomAttribute                                  valueToTest,
    //   params ( TCustomAttribute option, bool? value )[] options
    // ) where TCustomAttribute: System.IEquatable<TCustomAttribute>
    // {
    //   foreach ( var (option,value) in options )
    //   {
    //     if ( option.Equals(valueToTest) )
    //     {
    //       return value ;
    //     }
    //   }
    //   throw new System.NotSupportedException() ; // ???
    // }

    public static bool? Invert(bool? input) => !input;

    public static bool Invert(bool input) => !input;

    public static string NullBecomes ( string? input, string value ) => input ?? value ;

    public static double NullBecomes ( double? input, double value ) => input ?? value ;

    #if true

      // These revised implementations are slick, correct and nicely symmetrical

      // public static bool NullBecomesFalse ( InputLogicNode node ) => node.Value ?? false ;
      // public static bool NullBecomesTrue  ( InputLogicNode node ) => node.Value ?? true ;
      
      public static bool NullBecomesFalse ( bool? input ) => input ?? false ;
      
      public static bool NullBecomesTrue  ( bool? input ) => input ?? true ;

    #else

      // These previous implementations also looked plausible and nicely symmetrical
      // but were actually WRONG !! Kept here as an example of surprising behaviour ...

      // This implementation works fine ...

      public static bool NullBecomesFalse ( bool? input ) => input is true ;

      // Looks plausible but returns 'false' when the input is null, oops ...

      // public static bool NullBecomesTrue_GIVING_WRONG_RESULT ( bool? input ) => input is false ; 

      // This 'fast' version works fine ...

      public static bool NullBecomesTrue ( bool? input ) // => input is false ; 
      {
        bool fastResult = input ?? true ;
        bool slowButSureResult = input switch
        {
         null => true,
         _    => input.Value
        } ;
        return slowButSureResult.VerifiedEqualTo(fastResult) ;
      }

    #endif

    public static bool IsNull ( bool? x ) => x == null ;

    public static bool IsTrue ( bool? x ) => x == true ;

    public static bool IsFalse ( bool? x ) => x == false ;

    // Might be useful ?

    public static bool IsTrue ( bool x ) => x == true ;

    public static bool IsFalse ( bool x ) => x == false ;

  }
    
}
