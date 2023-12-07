//
// NodeAndValueDescriptor.cs
//

using Clf.LogicSystem.LogicNodes;
using Clf.LogicSystem.Miscellaneous;

namespace Clf.LogicSystem
{

  //
  // This is used to represent
  // 1. InputValue changes 
  // 2. 'expected Computed values'
  //

  public record NodeAndValueDescriptor (
    LogicNode LogicNode, 
    string    ValueAsString 
  ) {

    public NodeAndValueDescriptor (
      LogicNode logicNode, 
      bool      value 
    ) :
    this(
      logicNode,
      LogicHelpers.GetValueAsString(value)
    ) {
    }

    public NodeAndValueDescriptor (
      LogicNode logicNode, 
      double    value 
    ) :
    this(
      logicNode,
      LogicHelpers.GetValueAsString(value)
    ) {
    }

    public NodeAndValueDescriptor (
      LogicNode logicNode, 
      int       value 
    ) :
    this(
      logicNode,
      LogicHelpers.GetValueAsString(value)
    ) {
    }

    //
    // Bizarrely, if this conversion is expressed as a property ... 
    // an inexplicable exception gets thrown in a totally unrelated test ... 
    //
    // public MachineSafety.InputLogicNode InputLogicNode => (MachineSafety.InputLogicNode) LogicNode ;
    //
    //   D100X_MachineSafety_UnitTests.UnitTest_FE_SHUT_2.Test_01_Check_all_Computed_Nodes_have_correct_Inputs
    //
    // ... even if this property is never accessed !!!
    // 
    // The stack trace in xUnit shows 
    //
    //   GenericEnumerableEquivalencyStep.Handle(IEquivalencyValidationContext context ... )
    //
    // ... and presumably xUnit is using reflection to examine properties ???
    //
    // Note that this 'NodeValueChange' definition is only ever accessed as part
    // of defining a Scenario, and the test doesn't use this feature at all.
    //
    
    public InputNodeBase LogicNode_AsInputNode ( )
    => (InputNodeBase) LogicNode ;
   
    public InputNode<bool> LogicNode_AsInputNode_Boolean ( )
    => (InputNode<bool>) LogicNode ;
   
    public ComputedNodeBase LogicNode_AsComputedLogicNode ( ) 
    => (ComputedNodeBase) LogicNode  ;

    public static implicit operator NodeAndValueDescriptor ( 
      (
        LogicNode logicNode, 
        string    value
      ) change 
    ) 
    => new NodeAndValueDescriptor(
      change.logicNode,
      change.value
    ) ;

    public static implicit operator NodeAndValueDescriptor ( 
      (
        LogicNode logicNode, 
        bool      value
      ) change 
    ) 
    => new NodeAndValueDescriptor(
      change.logicNode,
      change.value
    ) ;

    public static implicit operator NodeAndValueDescriptor ( 
      (
        LogicNode logicNode, 
        double    value
      ) change 
    ) 
    => new NodeAndValueDescriptor(
      change.logicNode,
      change.value
    ) ;

  }

}
