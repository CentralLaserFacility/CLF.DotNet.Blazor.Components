//
// Scenario.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem
{

  //
  // A scenario has a name, which uniquely identifies
  // the scenario within a collection.
  //

  // A scenario relates to a specific Clf.Clf.LogicSystem.
  // A Clf.Clf.LogicSystem can tell us about its associated Scenarios.

  // A scenario can be 'based on' a different scenario.
  // That is, it defines some additional input settings
  // that extend or override the settings inherited from
  // the scenario it's based on.

  public interface IBaseScenario
  {
  }

  public class RootScenario : IBaseScenario
  {
    // In a 'root' scenario' we provide the option of
    // setting 'boolean' input nodes to have a non null value.
    // Inputs of other types, eg double and string,
    // are always initialised to null.
    // public bool? ValueForAllBooleanInputs { get ; init ; }
    public RootScenario ( 
      LogicSystemBase logicSystem//,
      //bool?           valueForAllBooleanInputs 
    ) {
          LogicSystem              = logicSystem ;
      // ValueForAllBooleanInputs = valueForAllBooleanInputs ;
    }
    public LogicSystemBase LogicSystem { get ; }
  }

  // All inputs are set to their default values

  // public class DefaultScenario : IBaseScenario
  // {
  //   public DefaultScenario ( 
  //     LogicSystemBase logicSystem
  //   ) {
  //     Clf.Clf.LogicSystem = logicSystem ;
  //   }
  //   public LogicSystemBase Clf.Clf.LogicSystem { get ; }
  // }

  public class Scenario : IBaseScenario
  {

    // TINE !!!

    // public static RootScenario AllBooleanInputsTrue_OtherInputsNull  ( LogicSystemBase logicSystem ) => new RootScenario(logicSystem,true) ;
    //                                           
    // public static RootScenario AllBooleanInputsFalse_OtherInputsNull ( LogicSystemBase logicSystem ) => new RootScenario(logicSystem,false) ;
    //                                           
    // public static RootScenario AllInputsNull  ( LogicSystemBase logicSystem ) => new RootScenario(logicSystem,null) ;
    // 
    // public static RootScenario AllBooleanInputsSetToSameValue_OtherInputsNull ( LogicSystemBase logicSystem, bool? value ) => new RootScenario(logicSystem,value) ;

    public static RootScenario AllInputsSetToDefaultValues ( LogicSystemBase logicSystem ) => new RootScenario(logicSystem) ;

    public IBaseScenario BasedOn { get ; init ; } 

    // public string? CategoryName = null ;

    public string Name { get ; set ; } 

    //
    // A scenario can be based on 
    // - either a Root scenario, which specifies a Value (null/true/false) 
    //   that is to be initially applied to *all* input nodes
    // - or an existing Scenario
    //
    // Ultimately a Scenario can trace back to its Root.
    //

    public readonly RootScenario RootScenario ;

    public Scenario (
      RootScenario                                               basedOn,
      [System.Runtime.CompilerServices.CallerMemberName] string? name = null
    ) {
      BasedOn      = basedOn ;
      RootScenario = basedOn ;
      Name         = name.VerifiedAsNonNullInstance() ;
    }

    public Scenario (
      Scenario                                                   basedOn,
      [System.Runtime.CompilerServices.CallerMemberName] string? name = null
    ) {
      BasedOn      = basedOn ;
      Name         = name.VerifiedAsNonNullInstance() ;
      RootScenario = basedOn.RootScenario ;
    }

    public string? Description { get ; init ; } = null ;

    //
    // If this scenario is 'B_current', 
    // and it's based on 'A_ancestor',
    // which is based on 'Root,
    // then we return the sequence
    //
    //   B_current
    //   A_ancestor
    //

    public IEnumerable<Scenario> ParentageChain_FromCurrentBackToRoot 
    {
      get
      {
        Scenario? current = this ;
        while ( current != null )
        {
          yield return current ;
          current = current.BasedOn as Scenario ;
        }
      }
    }

    //
    // If this scenario is 'B_current', 
    // and it's based on 'A_ancestor',
    // which is based on 'Root,
    // then we return the sequence
    //
    //   A_ancestor
    //   B_current
    //

    public IEnumerable<Scenario> DerivationChain_FromRootUpToCurrent => ParentageChain_FromCurrentBackToRoot.Reverse() ;

    public IEnumerable<NodeAndValueDescriptor> AllInputChangesRelativeToRootScenario 
    => Resolve(
      (scenario) => scenario.InputNodeValueSettings
    ) ;

    public IEnumerable<NodeAndValueDescriptor> AllComputedValueChangesRelativeToRootScenario 
    => Resolve(
      (scenario) => scenario.ComputedNodeValuesExpected
    ) ;

    public List<NodeAndValueDescriptor> InputNodeValueSettings { get ; init ; } = new() ;

    //
    // For a given scenario, we can declare the inputs whose values
    // are expected to have NO INFLUENCE on the 'expected outcome'.
    // Then in a test of the scenario, we could apply all combinations of those values
    // ie true/false/null, and verify that the outcome is not affected.
    //
    // NOT YET IMPLEMENTED !!!
    //

    // public List<InputLogicNode> InputNodesThatShouldHaveNoInfluence { get ; init ; } = new() ;

    //
    // For some scenarios, we know in advance what the expected outcome is, 
    // ie the Computed output values that correspond to our specified inputs.
    // If we declare these in the Scenario, a Test can verify that
    // these Computed values have actually been achieved. 
    // Note that Computed values that are not mentioned in this list
    // will not be checked in such a test.
    //

    public List<NodeAndValueDescriptor> ComputedNodeValuesExpected { get ; init ; } = new() ;

    #if SUPPORTS_SHADOW_VALUES
    // If we've run the Clf.Clf.LogicSystem with inputs coming from real PV's
    // we might encounter situations where the output PV from our Logic System
    // disagrees with the output generated as a PV by the 'real' Epics logic ...
    public List<NodeAndValueDescriptor> ShadowValuesThatDisagreeWithComputedNodeOutputs { get ; init ; } = new() ;
    #endif

    //
    // Once the scenario has been 'executed', 
    // we'll know whether there are any discrepancies ...
    //
    // Hmm, hard to guarantee that this is up-to-date.
    // Idea - characterise a Clf.Clf.LogicSystem with a 'hash-code' generated from
    // the Model definition, so that we can tell whether it's changed ???
    // That would be more reliable than manually updating a Revision Number.
    // It could be defined via Source Generation ...
    //

    public bool? ComputedNodeValuesAreAllValid { get ; set ; } = null ;

    public ScenarioDiscrepanciesReport? ScenarioDiscrepanciesReport { get ; set ; } = null ;

    //
    // In a scenario that's based on another one, we might no longer care about
    // certain Computed values that were mentioned in the 'base' scenario.
    // It's important to mention these because we can then ensure that a Test
    // does not check those values.
    //
    // NOT YET IMPLEMENTED !!!
    //

    // public List<ComputedNodeBase> ComputedNodesWhoseValuesWeNoLongerCareAbout { get ; init ; } = new() ;

    //
    // This method 'resolves' a Scenario that might be 'based on' other Scenarios,
    // by rolling up all the Input changes and Computed-Values-Expected into
    // a single data set. Scenarios which appear 'later' in the chain
    // take precedence.
    //

    public IEnumerable<NodeAndValueDescriptor> Resolve ( 
      System.Func<Scenario,IEnumerable<NodeAndValueDescriptor>> getChangesFunc 
    ) {
      var derivationChain = this.DerivationChain_FromRootUpToCurrent ;
      // Changes that are specified in scenarios which appear 'earlier' in the chain,
      // ie closer to the root, will be overridden by changes to the same Input node
      // that are specified in 'later' scenarios. To arrange this, we scan the change
      // specifiers in order, adding each newly mentioned InputNodeBase to a dictionary.
      // If that InputNodeBase is referred to again, the value will be overwritten.
      var changesDictionary = new Dictionary<LogicNode,string>() ;
      foreach ( var scenarioChainElement in derivationChain )
      {
        getChangesFunc(scenarioChainElement).ForEachItem(
          change => changesDictionary[change.LogicNode] = change.ValueAsString
        ) ;
      }
      return (
        changesDictionary
        .OrderBy( 
          keyValuePair => keyValuePair.Key.UniqueIntegerIdentifier
        )
        .Select(
          keyValuePair => new NodeAndValueDescriptor(
            keyValuePair.Key,
            keyValuePair.Value
          )
        ).ToList()
      ) ;
    }

    public Scenario Resolved ( )
    => new Scenario(
      basedOn : this.RootScenario,
      name    : this.Name
    ) {
      InputNodeValueSettings      = AllInputChangesRelativeToRootScenario.ToList(),
      ComputedNodeValuesExpected = AllComputedValueChangesRelativeToRootScenario.ToList()
    } ;

    // public Scenario Resolved ( )
    // {
    //   var derivationChain = this.DerivationChain_FromRootUpToCurrent ;
    //   // Changes that are specified in scenarios which appear 'earlier' in the chain,
    //   // ie closer to the root, will be overridden by changes to the same Input node
    //   // that are specified in 'later' scenarios. To arrange this, we scan the change
    //   // specifiers in order, adding each newly mentioned InputNodeBase to a dictionary.
    //   // If that InputNodeBase is referred to again, the value will be overwritten.
    //   var inputChangesDictionary = new Dictionary<LogicNode,bool?>() ;
    //   var outputChangesDictionary = new Dictionary<LogicNode,bool?>() ;
    //   foreach ( var scenarioChainElement in derivationChain )
    //   {
    //     scenarioChainElement.InputValueChanges.ForEachItem(
    //       change => inputChangesDictionary[change.LogicNode] = change.Value
    //     ) ;
    //     scenarioChainElement.ExpectedOutcome.ForEachItem(
    //       change => outputChangesDictionary[change.LogicNode] = change.Value
    //     ) ;
    //   }
    //   // Probably not necessary, but we'll want the changes to be
    //   // presented in a consistent order so that they will be applied
    //   // in a known sequence. The ordering rule doesn't really matter,
    //   // but 'UniqueIntegerIdentifier' and 'PropertyName' would both
    //   // make sense.
    //   Scenario resolvedScenario = new(
    //     basedOn : Scenario.AllInputsNull,
    //     name    : this.Name
    //   ) {
    //     InputValueChanges = inputChangesDictionary.OrderBy( 
    //       keyValuePair => keyValuePair.Key.UniqueIntegerIdentifier
    //     ).Select(
    //       keyValuePair => new NodeValueChange(
    //         keyValuePair.Key,
    //         keyValuePair.Value
    //       )
    //     ).ToList(),
    //     ExpectedOutcome = outputChangesDictionary.OrderBy( 
    //       keyValuePair => keyValuePair.Key.UniqueIntegerIdentifier
    //     ).Select(
    //       keyValuePair => new NodeValueChange(
    //         keyValuePair.Key,
    //         keyValuePair.Value
    //       )
    //     ).ToList() 
    //   } ;
    //   return resolvedScenario ;
    // }

    public ScenarioDescriptor AsResolvedScenarioDescriptor ( )
    => new ScenarioDescriptor(
      ClassNameOfLogicSystem : this.RootScenario.LogicSystem.LogicSystemClassName,
      ScenarioName           : this.Name
    ) {
      Description = this.Description,
      InputNodeValueSettings = AllInputChangesRelativeToRootScenario.Where(
        // We only need to mention an input-node-setting if the value differs
        // from the value we're going to apply by default to all inputs
        change => {
          if ( change.LogicNode.ValueAsString == change.LogicNode.DefaultValueAsString )
          {
            // The input node is specifying a change to a value which is the same
            // as the one we're going to be applying anyway by default, 
            // so we don't need to mention this explicitly in our 'AddInputNodeValueSettings' list.
            return false ;
          }
          else
          {
            // In all other cases we *do* mention this in our list
            return true ;
          }
        }
      ).Select(
        change => $"{change.LogicNode.PropertyName}={change.ValueAsString}"
      ).ToList(),
      ComputedNodeValuesExpected = AllComputedValueChangesRelativeToRootScenario.Select(
        change => $"{change.LogicNode.PropertyName}={change.ValueAsString}"
      ).ToList()
      #if SUPPORTS_SHADOW_VALUES
      ,ShadowValuesThatDisagreeWithComputedNodeOutputs = this.RootScenario.Clf.Clf.LogicSystem.OutputLogicNodes().Where(
        outputNode => outputNode.ComputedValueDisagreesWithShadowValue
      ).Select(
        outputNode => $"{outputNode.PropertyName}={LogicHelpers.GetValueAsString(outputNode.ShadowValue)}"
      ).ToList()
      #endif
    } ;

    public static Scenario FromScenarioDescriptor ( 
      ScenarioDescriptor scenarioDescriptor,
      LogicSystemBase    logicSystem
    ) 
    => new Scenario(
      basedOn : Scenario.AllInputsSetToDefaultValues(
        logicSystem
      ),
      name : scenarioDescriptor.ScenarioName
    ) {
      // Hmm, use a local function here ???
      // ALSO : VERIFY THAT THE CLASS NAME IS AS EXPECTED !!!
      InputNodeValueSettings = scenarioDescriptor.InputNodeValueSettings.Select(
        nodeAndValue_encodedAsString => {
          string[] fields = nodeAndValue_encodedAsString.Split('=') ;
          return new NodeAndValueDescriptor(
            logicSystem.LookupLogicNode(fields[0]),
            fields[1]
          ) ;
        }
      ).ToList(),
      ComputedNodeValuesExpected = scenarioDescriptor.ComputedNodeValuesExpected.Select(
        nodeAndValue_encodedAsString => {
          string[] fields = nodeAndValue_encodedAsString.Split('=') ;
          return new NodeAndValueDescriptor(
            logicSystem.LookupComputedLogicNode(fields[0]),
            fields[1]
          ) ;
        }
      ).ToList()
      #if SUPPORTS_SHADOW_VALUES
      ,ShadowValuesThatDisagreeWithComputedNodeOutputs = scenarioDescriptor.ShadowValuesThatDisagreeWithComputedNodeOutputs.Select(
        nodeAndValue_encodedAsString => {
          string[] fields = nodeAndValue_encodedAsString.Split('=') ;
          return new NodeAndValueDescriptor(
            logicSystem.LookupComputedLogicNode(fields[0]),
            fields[1]
          ) ;
        }
      ).ToList()
      #endif
    } ;

    //
    // NOW SUPERCEDED By 'ScenarioDescriptor'
    //
    // Typical output :
    //
    // [ScenarioCategory("CreatedFromVisualiser")]
    // public Scenario ScenarioName 
    // => new Scenario(
    //   basedOn : Scenario.AllInputsNull(this),
    //   "ScenarioDescription"
    // ) {
    //   AddInputNodeValueSettings = {
    //     ( MS_TIMING_OK,          "true"),          
    //     ( MS_CWMode_Off_BYPASS,  false),
    //     ( FE_PA_1_TRAFFIC_LIGHT, "OK"), 
    //     ( MS_FE_PA_1_OK_BYPASS,  false),
    //     ( CO_PLC_FE_WP_2_FB_OUT, true),
    //   },
    //   ComputedNodeValuesExpected = {
    //     ( OPEN_FE_SHUT_1_Allowed, true )
    //   }
    // } ;
    //
    //
    // public string AsSourceCode ( 
    //   string scenarioName,
    //   string scenarioDescription
    // ) {
    //   System.Text.StringBuilder stringBuilder = new() ;
    //   stringBuilder.AppendLine(
    //   $$"""
    //     [ScenarioCategory('CreatedFromVisualiser')]
    //     public Scenario {{scenarioName}}
    //     => new Scenario(
    //       basedOn : Scenario.AllInputsSetToDefaultValues(this),
    //       '{{scenarioDescription}}
    //     ) {
    //       AddInputNodeValueSettings = {
    //         {{GetInputNodeValueSettings()}}
    //       },
    //       ComputedNodeValuesExpected = {        
    //         {{GetComputedNodeValuesExpected()}}
    //       }
    //   """
    //   ) ;
    //   return stringBuilder.ToString().Replace("'","\"").TrimEnd() ;
    //   string GetInputNodeValueSettings ( )
    //   {
    //     System.Text.StringBuilder inputNodeValueSettings = new() ;
    //     AllInputChangesRelativeToRootScenario.Where(
    //       // We only need to mention an input-change if the value differs
    //       // from the value we're going to apply by default to all inputs
    //       change => {
    //         if ( change.LogicNode.ValueAsString == change.LogicNode.DefaultValueAsString )
    //         {
    //           // The input node is specifying a change to a value which is the same
    //           // as the one we're going to be applying anyway by default, 
    //           // so we don't need to mention this explicitly in our 'AddInputNodeValueSettings' list.
    //           return false ;
    //         }
    //         else
    //         {
    //           // In all other cases we *do* mention this in our list
    //           return true ;
    //         }
    //       }
    //     ).Select(
    //       change => $"    ({change.LogicNode.PropertyName}),'{change.ValueAsString}'),"
    //     ).ForEachItem(
    //       line => inputNodeValueSettings.AppendLine(line)
    //     ) ;
    //     return inputNodeValueSettings.ToString() ;
    //   }
    //   string GetComputedNodeValuesExpected ( )
    //   {
    //     System.Text.StringBuilder computedComputedValueChangesRelativeToRootScenario = new() ;
    //     AllComputedValueChangesRelativeToRootScenario.Select(
    //       change => $"    ({change.LogicNode.PropertyName}),'{change.ValueAsString}'),"
    //     ).ForEachItem(
    //       line => computedComputedValueChangesRelativeToRootScenario.AppendLine(line)
    //     ) ;
    //     return computedComputedValueChangesRelativeToRootScenario.ToString() ;
    //   }
    // }
    // 
    // public string AsSourceCode_OLD ( 
    //   string scenarioName,
    //   string scenarioDescription
    // ) {
    //   System.Text.StringBuilder stringBuilder = new() ;
    //   Add($"[ScenarioCategory('CreatedFromVisualiser')]  ") ;
    //   Add($"public Scenario {scenarioName}               ") ; 
    //   Add($"=> new Scenario(                             ") ;
    //   Add($"  basedOn : Scenario.AllInputsSetToDefaultValues(this),    ") ;
    //   Add($"  '{scenarioDescription}'                    ") ;
    //   Add($") {{                                         ") ;
    //   Add($"  AddInputNodeValueSettings = {{                ") ;
    //   AddInputNodeValueSettings() ;
    //   Add($"  }},                                        ") ;
    //   Add($"  ComputedNodeValuesExpected = {{            ") ;
    //   AddComputedNodeValuesExpected() ;
    //   Add($"  }}                                         ") ;
    //   Add($"}} ;                                         ") ;
    //   return stringBuilder.ToString() ;
    //   void Add ( string line )
    //   {
    //     stringBuilder.AppendLine(
    //       line.Replace("'","\"").TrimEnd()
    //     ) ;
    //   }
    //   void AddInputNodeValueSettings ( )
    //   {
    //     AllInputChangesRelativeToRootScenario.Where(
    //       // We only need to mention an input-change if the value differs
    //       // from the value we're going to apply by default to all inputs
    //       change => {
    //         if ( change.LogicNode.ValueAsString == change.LogicNode.DefaultValueAsString )
    //         {
    //           // The input node is specifying a change to a value which is the same
    //           // as the one we're going to be applying anyway by default, 
    //           // so we don't need to mention this explicitly in our 'AddInputNodeValueSettings' list.
    //           return false ;
    //         }
    //         else
    //         {
    //           // In all other cases we *do* mention this in our list
    //           return true ;
    //         }
    //       }
    //     ).Select(
    //       change => $"    ({change.LogicNode.PropertyName}),'{change.ValueAsString}'),"
    //     ).ForEachItem(
    //       line => Add(line)
    //     ) ;
    //   }
    //   void AddComputedNodeValuesExpected ( )
    //   {
    //     AllComputedValueChangesRelativeToRootScenario.Select(
    //       change => $"    ({change.LogicNode.PropertyName}),'{change.ValueAsString}'),"
    //     ).ForEachItem(
    //       line => Add(line)
    //     ) ;
    //   }
    // }

  }

}
