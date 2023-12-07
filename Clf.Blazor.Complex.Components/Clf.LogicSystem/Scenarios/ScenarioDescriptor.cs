//
// ScenarioDescriptor.cs
//

using Clf.LogicSystem.Common.ExtensionMethods ;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem
{

  //
  // A 'ScenarioDescriptor' is always 'resolved' ... that is,
  // it represents the entire set of changes required to recreate
  // a given 'derived' scenario, and *all* the expected outcomes
  // that we care about.
  //
  // In a given Clf.Clf.LogicSystem, there may be additional 'Computed' values
  // that aren't mentioned in the 'ExpectedOutcomes' list.
  //
  // We record the Class Name of the Clf.Clf.LogicSystem to which this scenario applies,
  // so that we can verify that it's applicable to a Clf.Clf.LogicSystem of that type.
  //

  //
  // Note that we could in principle use Source Generation to create
  // strongly typed Scenarios in C# code, directly from JSON
  // that would have been created either by hand editing
  // or from the Visualiser.
  //
  // https://github.com/Json2CSharp/Json2CSharpCodeGenerator/blob/master/JsonClassGeneratorLib/JsonClassGenerator.cs
  //

  public record ScenarioDescriptor ( 
    string ClassNameOfLogicSystem,
    string ScenarioName
  ) {

    private string? m_description ;
    public string? Description
    {
      get  => m_description ?? "No description provided" ;
      init => m_description = value ;
    }

    // Each element here is a name-value pair eg 'propertyName=value'

    public List<string> InputNodeValueSettings { get ; init ; } 
    = new List<string>() ;

    public List<string> ComputedNodeValuesExpected { get ; init ; } 
    = new List<string>() ;

    #if SUPPORTS_SHADOW_VALUES
    public List<string> ShadowValuesThatDisagreeWithComputedNodeOutputs { get ; init ; } 
    = new List<string>() ;
    #endif

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
    //     ( FE_PA_1_TRAFFIC_LIGHT, "OK"), 
    //   },
    //   ComputedNodeValuesExpected = {
    //     ( OPEN_FE_SHUT_1_Allowed, "true" )
    //   }
    // } ;
    //

    public string AsSourceCode ( 
      string scenarioCategory = "CreatedFromVisualiser" 
    ) {
      var namespaceAndClassName = this.ClassNameOfLogicSystem.Split('.') ;
      System.Text.StringBuilder stringBuilder = new() ;
      stringBuilder.AppendLine(
      $$"""
      //
      // {{ScenarioName}}.cs
      //
      // {{Description}}
      //

      using Clf.Clf.LogicSystem ;
 
      namespace {{namespaceAndClassName[0]}}
      {

        public partial class {{namespaceAndClassName[1]}}
        {

          [ScenarioCategory('{{scenarioCategory}}')]
          public Scenario {{ScenarioName}} 
          => new Scenario(
            basedOn : Scenario.AllInputsSetToDefaultValues(this)
          ) {
            Description = '{Description}',
            AddInputNodeValueSettings = {
      {{GetInputNodeValueSettings().TrimEnd()}}
            },
            ComputedNodeValuesExpected = {
      {{GetComputedNodeValuesExpected().TrimEnd()}}
            }
          }

        }
 
      }
      """
      ) ;
      return stringBuilder.ToString().Replace("'","\"").TrimEnd() ;
      string GetInputNodeValueSettings ( )
      {
        // System.Text.StringBuilder inputNodeValueSettings = new() ;
        // InputNodeValueSettings.ForEachItem(
        //   nodeAndValue_encodedAsString => {
        //     inputNodeValueSettings.AppendLine(
        //       GetNodeValueDescriptor(nodeAndValue_encodedAsString)
        //     ) ;
        //   }
        // ) ;
        // return inputNodeValueSettings.ToString() ;
        return string.Join(
          System.Environment.NewLine,
          InputNodeValueSettings.Select(
            GetNodeValueDescriptor
          )
        ) ;
      }
      string GetComputedNodeValuesExpected ( )
      {
        // System.Text.StringBuilder computedComputedValueChangesRelativeToRootScenario = new() ;
        // ComputedNodeValuesExpected.ForEachItem(
        //   nodeAndValue_encodedAsString => {
        //     computedComputedValueChangesRelativeToRootScenario.AppendLine(
        //       GetNodeValueDescriptor(nodeAndValue_encodedAsString)
        //     ) ;
        //   }
        // ) ;
        // return computedComputedValueChangesRelativeToRootScenario.ToString()
        return string.Join(
          System.Environment.NewLine,
          ComputedNodeValuesExpected.Select(
            nodeAndValue_encodedAsString => GetNodeValueDescriptor(nodeAndValue_encodedAsString)
          )
        ) ;

      }
      string GetNodeValueDescriptor ( string nodeAndValue_encodedAsString )
      {
        string[] fields = nodeAndValue_encodedAsString.Split('=') ;
        return $"        ({fields[0]},'{fields[1]}')," ;
      }
    }

    // public string AsSourceCode_OLD ( 
    //   string scenarioCategory = "CreatedFromVisualiser" 
    // ) {
    //   // if ( RootValueToInitiallyApplyToAllBooleanInputNodes != null )
    //   // {
    //   //   return "Root value must be null" ;
    //   // }
    //   var namespaceAndClassName = this.ClassNameOfLogicSystem.Split('.') ;
    //   System.Text.StringBuilder stringBuilder = new() ;
    //   Add($"//                                                         ") ;
    //   Add($"// {ScenarioName}.cs                                       ") ;
    //   Add($"//                                                         ") ;
    //   Add($"// {Description}                                           ") ;
    //   Add($"//                                                         ") ;
    //   Add($"                                                           ") ;
    //   Add($"using MachineSafety ;                                      ") ;
    //   Add($"                                                           ") ;
    //   Add($"namespace {namespaceAndClassName[0]}                       ") ;
    //   Add($"{{                                                         ") ;
    //   Add($"                                                           ") ;
    //   Add($"  public partial class {namespaceAndClassName[1]}          ") ;
    //   Add($"  {{                                                       ") ;
    //   Add($"                                                           ") ;
    //   Add($"    [ScenarioCategory('{scenarioCategory}')]               ") ;
    //   Add($"    public Scenario {ScenarioName}                         ") ; 
    //   Add($"    => new Scenario(                                       ") ;
    //   Add($"      basedOn : Scenario.AllInputsSetToDefaultValues(this) ") ;
    //   Add($"      // '{ScenarioName}'                                  ") ;
    //   Add($"    ) {{                                                   ") ;
    //   Add($"      Description = '{Description}',                       ") ;
    //   Add($"      AddInputNodeValueSettings = {{                       ") ;
    //   AddInputNodeValueSettings() ;                                    
    //   Add($"      }},                                                  ") ;
    //   Add($"      ComputedNodeValuesExpected = {{                      ") ;
    //   AddComputedNodeValuesExpected() ;                                
    //   Add($"      }}                                                   ") ;
    //   Add($"    }} ;                                                   ") ;
    //   Add($"                                                           ") ;
    //   Add($"  }}                                                       ") ;
    //   Add($"                                                           ") ;
    //   Add($"}}                                                         ") ;
    //   return stringBuilder.ToString() ;
    //   void Add ( string line )
    //   {
    //     stringBuilder.AppendLine(
    //       line.Replace("'","\"").TrimEnd()
    //     ) ;
    //   }
    //   void AddInputNodeValueSettings ( )
    //   {
    //     InputNodeValueSettings.ForEachItem(
    //       AddNodeValueDescriptor
    //     ) ;
    //   }
    //   void AddComputedNodeValuesExpected ( )
    //   {
    //     ComputedNodeValuesExpected.ForEachItem(
    //       AddNodeValueDescriptor
    //     ) ;
    //   }
    //   void AddNodeValueDescriptor ( string nodeAndValue_encodedAsString )
    //   {
    //     string[] fields = nodeAndValue_encodedAsString.Split('=') ;
    //     Add($"        ({fields[0]},'{fields[1]}'),") ;
    //   }
    // }

  }

  public record DerivedScenarioDescriptor ( 
    string ClassNameOfLogicSystem,
    string ScenarioName,
    string BaseScenarioIdentifier
  ) {

  }

}
