//
// ScenarioCategoryAttribute.cs
//

namespace Clf.LogicSystem
{

  //
  // This attribute can be applied to a Scenario, and determines
  // which 'ScenarioCollection' it will be placed into.
  //

  public sealed class ScenarioCategoryAttribute : System.Attribute
  {

    public readonly string Value ; // ScenarioCategoryName, as a record ???

    public ScenarioCategoryAttribute ( string value ) 
    { 
      Value = value ;
    }

  }

}
