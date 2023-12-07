//
// LogicNodeViewModel_primary_properties_shadowing_the_logic_system.cs
//

using Clf.LogicSystem.LogicNodes;

namespace Clf.LogicSystem.ViewModel
{

  //
  // These properties 'shadow' the equivalent properties in the Logic System
  // and get updated when those Logic System properties suffer a change.
  //
  // For example ValueAsString, ShadowValue, ShadowValueDiffersFromComputedValue. 
  //

  public partial class LogicNodeViewModel 
  { 

    // Whereas the value of a LogicNode might be of type Boolean or String or Double,
    // and might be a 'null' value ... the Value we display in the ViewModel
    // is always a string.

    public string ValueAsString => LogicNode.ValueAsStringForDisplay;

    public System.Type ValueType => LogicNode.ValueType ;

    public object? ValueAsObject => LogicNode.ValueAsObject ;

    #if SUPPORTS_SHADOW_VALUES

    public bool? ShadowValue => (
      LogicNode is ComputedNodeBase computedNode
      ? computedNode.ShadowValue
      : null
    ) ;

    public bool IsComputedOutputNode_WhoseValueDisagreesWithShadowValue 
    => ( 
      LogicNode is ComputedNodeBase computedNode
      ? computedNode.ComputedValueDisagreesWithShadowValue
      : false
    ) ;

    #endif

    public bool IsComputedOutputNode_WhoseValueIsUnexpectedlyNull 
    => ( 
      LogicNode is ComputedNodeBase computedNode
      ? computedNode.ComputedValueIsUnexpectedlyNull
      : false
    ) ;

  }

}

