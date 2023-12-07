//
// LogicSystemBase_discovering_scenario_properties.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase
  {
    /// <summary>
    /// Discover scenario properties using *reflection* and add to scenario collection
    /// </summary>
    /// <param name="logicSystemWhosePropertiesWeAreScanning"></param>
    public void ScanTargetObjectProperties_BuildingScenarioCollections ( 
      LogicSystemBase logicSystemWhosePropertiesWeAreScanning 
    ) {
      // Use reflection to build a list of PropertyInfo descriptors
      // for all the nested properties defined in this class
      // that are of type 'Scenario'.
      List<System.Reflection.PropertyInfo> scenarioPropertyInfos = (
        logicSystemWhosePropertiesWeAreScanning.GetType().GetProperties(
          System.Reflection.BindingFlags.Public
        | System.Reflection.BindingFlags.Instance
        ).Where(
          // We want to include all properties whose type is a subclass
          // of 'Scenario', avoiding the indexer if it's present
          (propertyInfo) => (
              propertyInfo.Name != "Item" // Avoid this 'indexer' !!!
          && typeof(Scenario).IsAssignableFrom(propertyInfo.PropertyType)
          )
        )
      ).ToList() ;

      // There are 'Scenario' properties to visit"
      if (scenarioPropertyInfos.Count > 0 )
      {
        // Scan the 'Scenario' properties and add them to the appropriate ScenariosCollection, 
        // as indicated by an optional [ScenarioCategory] attribute. 
        scenarioPropertyInfos.Where(
          // Hmm, ignore the 'DefaultScenario' property as this doesn't need to be put into a ScenariosCollection ??
          propertyInfo => true // ???? propertyInfo.Name != nameof(DefaultScenario)
        ).ForEachItem(
          (propertyInfo,i) => {
            
            var scenario = (Scenario) propertyInfo.GetValue(
              logicSystemWhosePropertiesWeAreScanning
            ).VerifiedAsNonNullInstance() ; 
            string scenarioCategoryName = (
              propertyInfo.GetCustomAttributeSpecifiedByMemberInfoOrNull<ScenarioCategoryAttribute>()?.Value
              ?? "Uncategorised"
            ) ;
            if ( ! m_scenarioCollections.ContainsKey(scenarioCategoryName) )
            {
              m_scenarioCollections[scenarioCategoryName] = new ScenariosCollection(
                scenarioCategoryName
              ) ;
            }
            if ( scenario.Name is null )
            {
              scenario.Name = propertyInfo.Name ;
            }
            m_scenarioCollections[scenarioCategoryName].Add(scenario) ;

          }
        ) ;
      }
    }

  }

}
