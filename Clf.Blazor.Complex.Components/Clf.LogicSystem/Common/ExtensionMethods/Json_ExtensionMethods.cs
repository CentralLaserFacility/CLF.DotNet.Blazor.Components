//
// Json_ExtensionMethods.cs
//

using Clf.Common.ExtensionMethods;

namespace Clf.LogicSystem.Common.ExtensionMethods
{

  // Hmm, an [Attribute] would be better ...
  public interface IPreserveReferencesOnJsonSerialisation
  { }

}

namespace Clf.LogicSystem.Common.ExtensionMethods
{

  public static partial class Json_ExtensionMethods
  {

    public static string AsJsonTextForPersistence<T> ( 
      this T instance
    )
    where T : class
    {
      // We ignore 'read-only' properties and fields,
      // but we *do* include 'read/write' fields.
      // We include explicit 'null' values. 
      string json = System.Text.Json.JsonSerializer.Serialize(
        instance,
        new System.Text.Json.JsonSerializerOptions(){
          WriteIndented            = true,
          IgnoreReadOnlyProperties = true,
          IgnoreReadOnlyFields     = true,
          IncludeFields            = true,
          ReferenceHandler = (
              instance is IPreserveReferencesOnJsonSerialisation
              ? System.Text.Json.Serialization.ReferenceHandler.Preserve
              : null
          ),
          // IgnoreNullValues = false 
          DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        }
      ) ;
      return json ;
    }

    public static string AsJsonTextForDisplay ( this object instance )
    {
      try
      {
        return System.Text.Json.JsonSerializer.Serialize(
          instance,
          new System.Text.Json.JsonSerializerOptions(){
            WriteIndented            = true,
            IgnoreReadOnlyFields     = false,
            IgnoreReadOnlyProperties = false,
            // IgnoreNullValues = false 
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            IncludeFields            = true,
            ReferenceHandler = (
                instance is IPreserveReferencesOnJsonSerialisation
                ? System.Text.Json.Serialization.ReferenceHandler.Preserve
                : null
            )
          }
        ) ;
      } 
      catch  ( System.Exception x )
      {
        return x.Message ;
      }
    }

    // If parsing fails, an exception is thrown

    public static T ParsedAsJsonToCreateInstanceOf<T> ( this string jsonText ) where T : class
    {
      return System.Text.Json.JsonSerializer.Deserialize<T>(
        jsonText,
        new System.Text.Json.JsonSerializerOptions(){}        
      ).VerifiedAsNonNullInstanceOf<T>() ;
    }

  }

}
