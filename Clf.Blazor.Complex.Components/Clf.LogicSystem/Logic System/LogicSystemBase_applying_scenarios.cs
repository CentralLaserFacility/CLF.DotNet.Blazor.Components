//
// LogicSystemBase_applying_scenarios.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase
  {

    //
    // As part of the definition of a Clf.Clf.LogicSystem, we can install 'scenarios'
    // that define expected inputs and outputs. These can act as illustrations
    // of the desired behaviour, and can also be applied as 'tests'.
    //

    private Dictionary<string,ScenariosCollection> m_scenarioCollections = new() ;

    public virtual IEnumerable<ScenariosCollection> ScenarioCollections => m_scenarioCollections.Values ;

    public void Add ( ScenariosCollection scenariosCollection ) 
    => m_scenarioCollections[
      scenariosCollection.Name
    ] = scenariosCollection ;

    public void VerifyAllScenarios ( )
    {
      foreach ( var scenarioCollection in ScenarioCollections )
      {
        scenarioCollection.AvailableScenarios.ForEachItem(
          scenario => ApplyScenario(scenario)
        ) ;
      }
    }

    public TLogicSystem WithScenarioApplied<TLogicSystem> ( 
      System.Func<TLogicSystem,Scenario> getScenarioFunc 
    ) 
    where TLogicSystem : LogicSystemBase
    {
      return (TLogicSystem) ApplyScenario(
        getScenarioFunc(
          (TLogicSystem) this
        )
      ) ;
    }

    public static void LogUnexpectedOutcome ( 
      Scenario         scenario,
      ComputedNodeBase computedNode,
      string           expectedValue
    ) {
      System.Diagnostics.Debug.WriteLine(
        $"Scenario '{scenario.Name}' ; expected '{computedNode.PropertyName}' to be '{expectedValue}' but value was '{computedNode.ValueAsString}'"
      ) ;
    }

    public LogicSystemBase ApplyScenario ( 
      Scenario                                         scenario, 
      System.Action<Scenario,ComputedNodeBase,string>? scenarioOutcomeNotAsExpected = null // HMM - DON'T PERMIT NULL HERE !!!
    ) {
      RestoreInputsAsSpecifiedInScenario(scenario) ;
      // Now check that the computed values are as specified in the scenario
      scenario.AllComputedValueChangesRelativeToRootScenario.ForEachItem(
        changeExpected => {
          if ( changeExpected.LogicNode_AsComputedLogicNode().ValueAsString != changeExpected.ValueAsString )
          {
            scenarioOutcomeNotAsExpected?.Invoke(
              scenario,
              changeExpected.LogicNode_AsComputedLogicNode(),
              changeExpected.ValueAsString
            ) ;
          }
        }
      ) ;
      #if SUPPORTS_SHADOW_VALUES
      // Set any 'shadow' values that are present ...
      scenario.ShadowValuesThatDisagreeWithComputedNodeOutputs.ForEachItem(
        nodeAndValue => {
          nodeAndValue.LogicNode_AsComputedLogicNode().ShadowValue = nodeAndValue.ValueAsString.ParsedAs<bool?>() ;
        }
      ) ;
      #endif
      return this ;
    }

    public void ApplyInputValueChange ( NodeAndValueDescriptor inputValueChange )
    {
      if ( 
         inputValueChange.LogicNode is InputNodeBase inputNode
      && inputNode.CanSetValue_ParsedFromString(
           inputValueChange.ValueAsString
         )
      ) {
        // OK
      }
      else
      {
        // Hmm, failed ...
      }

    }

    // A subclass can define an override that will set up non-default values
    // on certain Input nodes, and specify the expected outcome.

    [ScenarioCategory("Default")]
    public virtual Scenario DefaultScenario => new Scenario(
      basedOn : Scenario.AllInputsSetToDefaultValues(this),
      "Setting all inputs to default values"
    ) ;

  }

}
