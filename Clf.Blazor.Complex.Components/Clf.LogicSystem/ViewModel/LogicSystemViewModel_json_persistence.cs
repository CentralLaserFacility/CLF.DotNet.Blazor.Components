//
// LogicSystemViewModel_json_persistence.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicSystemViewModel
  {

    public static LogicSystemViewModel CreateFromJsonText ( string jsonText )
    {
      return jsonText.ParsedAsJsonToCreateInstanceOf<LogicSystemViewModel>() ;
    }

    // [System.Text.Json.Serialization.JsonIgnore]
    // public string? JsonFileFromWhichThisWasGenerated { get ; set ; } = null ;

  }

}

