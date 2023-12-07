//
// ComputedNodeValueDiscrepancy.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;

namespace Clf.LogicSystem
{

  public record ComputedNodeValueDiscrepancy (
    Scenario Scenario,
    string   ComputedLogicNodePropertyName, 
    string   ExpectedValueDefinedInScenario,
    string   ActualValueProducedByLogicSystem
  ) {

    public string ToString_ForMessageBoxBodyText ( )
    => new[]{
      $"{ComputedLogicNodePropertyName} :",
      $"",
      $"Scenario tells us to expect {ExpectedValueDefinedInScenario}", 
      $"",
      $"Actual computed value is    {ActualValueProducedByLogicSystem}"
    }.AsSingleLine("\r\n") ;

  } ;

}
