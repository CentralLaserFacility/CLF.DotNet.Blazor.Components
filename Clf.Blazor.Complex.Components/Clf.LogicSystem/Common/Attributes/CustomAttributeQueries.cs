//
// CustomAttributeQueries.cs
//

using Clf.Common.ExtensionMethods;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Clf.LogicSystem.Common.Attributes
{

  // Attributes from MemberInfo

  public static partial class CustomAttributeQueries
  {

    // Getting all the custom attributes

    public static IEnumerable<T> GetAllCustomAttributesSpecifiedByMemberInfo<T> (
      this System.Reflection.MemberInfo memberInfo
    ) where T : System.Attribute
    {
      object[] attributes = memberInfo.GetCustomAttributes(
        typeof(T),
        false
      ).ToArray() ; ;
      foreach ( T customAttribute in attributes )
      {
        yield return customAttribute as T ;
      }
    }

    // Querying the existence of custom attributes of a specific type

    public static bool MemberInfoSpecifiesCustomAttribute<T> (
      this System.Reflection.MemberInfo memberInfo
    ) where T : System.Attribute
    => memberInfo.MemberInfoSpecifiesCustomAttributeOfType(typeof(T)) ;

    public static bool MemberInfoSpecifiesCustomAttributeOfType (
      this System.Reflection.MemberInfo memberInfo,
      System.Type                       attributeType
    ) => memberInfo.GetCustomAttributes(
      attributeType,
      inherit : true
    ).Any() ;

    // Querying the value of a specified Attribute (if it is present)

    public static bool MemberInfoSpecifiesCustomAttribute<T> (
      this System.Reflection.MemberInfo memberInfo,
      [NotNullWhen(true)] out T?        attribute
    ) where T : System.Attribute
    {
      attribute = memberInfo.GetCustomAttributes(
        typeof(T),
        inherit : true
      ).OfType<T>().FirstOrDefault() ;
      return attribute != null ;
    }

    public static T GetCustomAttributeSpecifiedByMemberInfo<T> (
      this System.Reflection.MemberInfo memberInfo
    ) where T : System.Attribute
    => memberInfo.GetCustomAttributes(
      typeof(T),
      inherit : true
    ).OfType<T>().First() ;

    public static T? GetCustomAttributeSpecifiedByMemberInfoOrNull<T> (
      this System.Reflection.MemberInfo memberInfo
    ) where T : System.Attribute
    => memberInfo.GetCustomAttributes(
      typeof(T),
      inherit : true
    ).OfType<T>().FirstOrDefault() ;

  }

  // Attributes from a Type

  public static partial class CustomAttributeQueries
  {

    public static bool TypeHasCustomAttribute<T> (
      this System.Type type
    ) where T : System.Attribute
    => type.GetCustomAttributes(typeof(T),true).Any() ;

    public static bool TypeHasCustomAttribute<T> (
      this System.Type           type,
      [NotNullWhen(true)] out T? customAttribute 
    ) 
    where T : System.Attribute
    {
      customAttribute = type.GetCustomAttributes(typeof(T),true).FirstOrDefault() as T ;
      return customAttribute != null ;
    }
    public static T? GetCustomAttributeOrNull<T> (
      this System.Type type
    ) 
    where T : System.Attribute
    {
      return type.GetCustomAttributes(typeof(T),true).FirstOrDefault() as T ;
    }
  }

  // Attributes from an Enum value

  public static partial class EnumValueExtensions
  {

    public static bool SpecifiesCustomAttribute<T> (
      this System.Enum           enumValue,
      [NotNullWhen(true)] out T? customAttribute
    ) where T : System.Attribute
    {
      System.Type enumType = enumValue.GetType() ;
      if ( System.Enum.IsDefined(enumType,enumValue) )
      {
        string valueAsString = enumValue.ToString() ;
        // The enum value might be a value that doesn't explicitly appear
        // in the enumerated type definition, in which case we won't be able
        // to find any 'FieldInfo' pertaining to that value ...
        System.Reflection.FieldInfo fieldInfo = enumType.GetField(valueAsString).VerifiedAsNonNullInstance() ;
        return fieldInfo.MemberInfoSpecifiesCustomAttribute<T>(
          out customAttribute
        ) ;
      }
      else
      {
        customAttribute = null ;
        return false ;
      }
    }

    // public static bool SpecifiesCustomAttribute_ORIGINAL<T> (
    //   this System.Enum           enumValue,
    //   [NotNullWhen(true)] out T? customAttribute
    // ) where T : System.Attribute
    // {
    //   customAttribute = null ;
    //   System.Type enumType = enumValue.GetType() ;
    //   string valueAsString = enumValue.ToString() ;
    //   // The enum value might be a value that doesnt explicitly appear
    //   // in the enumerated type definition, in which case we won't be able
    //   // to find any 'FieldInfo' pertaining to that value ...
    //   System.Reflection.FieldInfo? fieldInfo = enumType.GetField(valueAsString) ;
    //   return fieldInfo?.MemberInfoSpecifiesCustomAttribute<T>(
    //     out customAttribute
    //   ) ?? false ;
    // }

    public static T GetRequiredCustomAttribute<T> (
      this System.Enum enumValue
    ) 
    where T : System.Attribute
    {
      if (
        enumValue.SpecifiesCustomAttribute<T>(
          out T? customAttribute
        ) 
      ) {
        return customAttribute ;
      }
      else
      {
        throw new System.ApplicationException("Custom attribute was required but not found") ;
      }
    }

    // public static TCustomAttribute? GetCustomAttributeOrNull<TCustomAttribute> (
    //   this System.Enum enumValueOrNull
    // ) 
    // where TCustomAttribute : System.Attribute
    // {
    //   if (
    //     enumValueOrNull.SpecifiesCustomAttribute<TCustomAttribute>(
    //       out TCustomAttribute customAttribute
    //     ) 
    //   ) {
    //     return customAttribute ;
    //   }
    //   else
    //   {
    //     return enumValueOrNull.GetType().GetCustomAttributeOrNull<TCustomAttribute>() ;
    //   }
    // }

    public static TCustomAttribute? GetCustomAttributeOrNull<TCustomAttribute> (
      this System.Enum? enumValue
    ) 
    where TCustomAttribute : System.Attribute
    {
      if (
        enumValue?.SpecifiesCustomAttribute<TCustomAttribute>(
          out TCustomAttribute? customAttribute
        ) == true
      ) {
        return customAttribute ;
      }
      else
      {
        return enumValue?.GetType().GetCustomAttributeOrNull<TCustomAttribute>() ;
      }
    }

    //
    // Experiments, now obselete as a better solution has been discovered ...
    //
    // public static System.Attribute? GetCustomAttributeOrNullEx<TEnum> (
    //   this TEnum? enumValueOrNull,
    //   System.Type customAttributeType
    // ) 
    // where TEnum : struct, System.Enum
    // {
    //   if ( ! enumValueOrNull.HasValue ) 
    //   {
    //     return null ;
    //   }
    //   else
    //   {
    //     TEnum enumValue = enumValueOrNull.Value;
    //     System.Type enumType = enumValue.GetType() ;
    //     string valueAsString = enumValue.ToString() ;
    //     System.Reflection.FieldInfo? fieldInfo = enumType.GetField(valueAsString) ;
    //     if ( fieldInfo?.IsDefined(customAttributeType,true) == true )
    //     {
    //       return fieldInfo.GetCustomAttributes(customAttributeType,true)[0] as System.Attribute ;
    //     }
    //     else
    //     {
    //       return null ;
    //     }
    //   }
    // }
    // 
    // public static TAttribute? GetCustomAttributeOrNullEx2<TEnum,TAttribute> (
    //   this TEnum? enumValueOrNull
    // ) 
    // where TEnum : struct, System.Enum
    // where TAttribute : System.Attribute
    // {
    //   if ( ! enumValueOrNull.HasValue ) 
    //   {
    //     return null ;
    //   }
    //   else
    //   {
    //     System.Type customAttributeType = typeof(TAttribute) ;
    //     TEnum enumValue = enumValueOrNull.Value;
    //     System.Type enumType = enumValue.GetType() ;
    //     string valueAsString = enumValue.ToString() ;
    //     System.Reflection.FieldInfo? fieldInfo = enumType.GetField(valueAsString) ;
    //     if ( fieldInfo?.IsDefined(customAttributeType,true) == true )
    //     {
    //       return fieldInfo.GetCustomAttributes(customAttributeType,true)[0] as TAttribute ;
    //     }
    //     else
    //     {
    //       return null ;
    //     }
    //   }
    // }
    // 
    // public static TCustomAttribute? GetCustomAttributeOrNullEx3<TCustomAttribute> (
    //   this System.Enum? enumValueOrNull
    // ) 
    // where TCustomAttribute : System.Attribute
    // {
    //   if ( enumValueOrNull is null ) 
    //   {
    //     return null ;
    //   }
    //   else
    //   {
    //     System.Type customAttributeType = typeof(TCustomAttribute) ;
    //     System.Enum enumValue = enumValueOrNull! ;
    //     System.Type enumType = enumValue.GetType() ;
    //     string valueAsString = enumValue.ToString() ;
    //     System.Reflection.FieldInfo? fieldInfo = enumType.GetField(valueAsString) ;
    //     if ( fieldInfo?.IsDefined(customAttributeType,true) == true )
    //     {
    //       return fieldInfo.GetCustomAttributes(customAttributeType,true)[0] as TCustomAttribute ;
    //     }
    //     else
    //     {
    //       return enumValue?.GetType().GetCustomAttributeOrNull<TCustomAttribute>() ;
    //     }
    //   }
    // }
    //

  }

  // Attributes from an object instance (actually defined on the Type of the instance)

  public static partial class ObjectAttributeQueries
  {

    // Not actually very useful : better to just query for custom attributes on the object's Type ...

    public static bool ObjectInstanceHasCustomAttribute<T> ( 
      object                     instance, 
      [NotNullWhen(true)] out T? customAttribute 
    ) where T : System.Attribute
    {
      System.Type type = instance.GetType() ;
      Clf.Common.Utils.Assert.IsTrue(
        type != typeof(System.Type),
        "Object instance was expected"
      ) ;
      customAttribute = type.GetCustomAttributes(type,inherit:true).OfType<T>().FirstOrDefault()! ;
      return customAttribute != null ;
    }

  }

}
