//
// LogicSystemVisualisationSettings.cs
//

namespace Clf.LogicSystem.Miscellaneous
{

  public class LogicSystemConfigurationSettings
  {

    public static readonly LogicSystemConfigurationSettings Instance = new() ;

    public bool DoParanoidCheckWhenReturningCachedComputedValue = (
      #if DEBUG
        true 
      #else
        false
      #endif
    ) ;

  }

}
