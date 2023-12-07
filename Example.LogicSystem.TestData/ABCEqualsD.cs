using Clf.LogicSystem;
using Clf.LogicSystem.Attributes;
using Clf.LogicSystem.LogicNodes;
using System.ComponentModel;
using static Clf.LogicSystem.Miscellaneous.LogicHelpers;

namespace Example.LogicSystem.TestData
{
  public partial class ABCEqualsD : LogicSystemBase
  {
    // Input nodes
    [Clf.LogicSystem.Attributes.DisplayName("Input A")]
    [Clf.LogicSystem.Attributes.ChannelType(Clf.ChannelAccess.ChannelType.Local)]
    public InputNode<bool> InputA => GetOrCreateInputNode<bool>();

    [Clf.LogicSystem.Attributes.DisplayName("Input B")]
    [Clf.LogicSystem.Attributes.ChannelType(Clf.ChannelAccess.ChannelType.Local)]
    public InputNode<bool> InputB => GetOrCreateInputNode<bool>();

    [Clf.LogicSystem.Attributes.DisplayName("Input C")]
    [Clf.LogicSystem.Attributes.ChannelType(Clf.ChannelAccess.ChannelType.Local)]
    public InputNode<bool> InputC => GetOrCreateInputNode<bool>();

    [Clf.LogicSystem.Attributes.DisplayName("Output D")]
    [Clf.LogicSystem.Attributes.ChannelType(Clf.ChannelAccess.ChannelType.Local)]
    public ComputedNode<bool> A_B_C_Equals_D => GetOrCreateComputedNode<bool>(
      () => Combine_AND(InputA,InputB,InputC) == true
    );
  }
}
