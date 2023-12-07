//
// Type_ExtensionMethods.cs
//

using Clf.Common.ExtensionMethods;
using System.Diagnostics.CodeAnalysis;

namespace Clf.LogicSystem.Common.ExtensionMethods
{

  public static partial class Type_ExtensionMethods
  {

    // 
    // If we have a 'System.Attribute' subclass called 'SomethingAttribute',
    // then we often want to work with the attribute name as just 'Something',
    // ie with the trailing 'Attribute' part removed.
    //

    public static string AttributeName (
      this System.Type type
    )
    // where TCustomAttribute : System.Attribute
    => (
      type.Name.Replace("Attribute","")
    ) ;

    public static string GetTypeName ( this System.Type type, bool showNamespace = false )
    {
      string typeName ;
      try
      {
        if ( type.IsGenericParameter )
        {
          typeName = type.Name ;
        }
        else if ( type.IsArray )
        {
          typeName =  (
            GetTypeName(
              type.GetElementType()!
            )
          + "[]".Repeated(
              type.GetArrayRank()
            )
          ) ;
        }
        else if ( type.IsGenericType )
        {
          string result = type.Name.Split('`')[0] + "<" ;
          System.Type[] genericArguments = type.GetGenericArguments() ;
          foreach ( System.Type T in genericArguments )
          {
            result += (
              T.IsGenericParameter // TCustomAttribute.ContainsGenericParameters
              ? T.Name
              : GetTypeName(T)
            ) + "," ;
          }
          if ( showNamespace ) 
          {
            result = type.Namespace + "." + result ;
          }
          typeName = result.TrimEnd(',') + ">" ;
        }
        else
        {
          typeName = type.Name ;
        }
        if ( showNamespace ) 
        {
          typeName = type.Namespace + "." + typeName ;
        }
      }
      catch ( System.ApplicationException x )
      {
        typeName = type.Name + x ;
      }
      return (
        // IMPROVE_THIS ???
        // Could use a 'switch' expression instead ???
        typeName
        .Replace( typeof( bool   ).FullName!.VerifiedAsNonNullInstance() , "bool"   )
        .Replace( typeof( byte   ).FullName!.VerifiedAsNonNullInstance() , "byte"   )
        .Replace( typeof( short  ).FullName!.VerifiedAsNonNullInstance() , "short"  )
        .Replace( typeof( ushort ).FullName!.VerifiedAsNonNullInstance() , "ushort" )
        .Replace( typeof( int    ).FullName!.VerifiedAsNonNullInstance() , "int"    )
        .Replace( typeof( uint   ).FullName!.VerifiedAsNonNullInstance() , "uint"   )
        .Replace( typeof( nint   ).FullName!.VerifiedAsNonNullInstance() , "nint"   )
        .Replace( typeof( long   ).FullName!.VerifiedAsNonNullInstance() , "long"   )
        .Replace( typeof( ulong  ).FullName!.VerifiedAsNonNullInstance() , "ulong"  )
        .Replace( typeof( float  ).FullName!.VerifiedAsNonNullInstance() , "float"  )
        .Replace( typeof( double ).FullName!.VerifiedAsNonNullInstance() , "double" )
        .Replace( typeof( string ).FullName!.VerifiedAsNonNullInstance() , "string" )
        .Replace( typeof( char   ).FullName!.VerifiedAsNonNullInstance() , "char"   )
      ) ;
    }

  }

}
