//
// LogicNode_DeclarationExamples.cs
//

namespace Clf.LogicSystem.LogicNodes
{

  //
  // TINE !!!
  //

  public class LogicNode_DeclarationExamples : LogicSystemBase
  {
    //PS: we discarded proposed new syntax `InputNode<bool>.GetOrCreate(this) ;`, as it would require passing `Clf.LogicSystem` reference each time. Existing approach is much concise.
    InputNode<bool> A => GetOrCreateInputNode<bool>() ;
    
    // A 'ChannelInputNode' has a Value of type object
    InputNode channelInputNode => GetOrCreateInputNode();
    
    ComputedNode<int> ZA_int => GetOrCreateComputedNode<int>(()=>123) ;

    enum MyEnum { AA,BB } 

    ComputedNode<MyEnum> ZD => GetOrCreateComputedNode<MyEnum>(()=>MyEnum.AA) ;
  }

}
