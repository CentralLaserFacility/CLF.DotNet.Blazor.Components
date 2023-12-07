//
// LogicSystemBase_channel_publishing.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.ChannelAccess;
using System.Linq;
using Clf.LogicSystem.LogicNodes;
using Clf.Common.ExtensionMethods;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase 
  {

    //
    // Channel Publication provides a mechanism for creating PV's via ThinIOC
    // that publish the values defined by the Clf.Clf.LogicSystem UI. So when we
    // click on an Input node to change the Input value, that value actually 
    // gets published via Channel Access.
    //
    // The primary use for this is to verify that a 2nd 'visualiser'
    // is correctly receiving and displaying the PV values via Channel Access.
    // This is more convenient that writing a special UI as we've done previously,
    // eg the panel of check-boxes used to exercise the 'two-out-of-three' test.
    //
    // However, this also opens the possibility of using the Visualiser UI
    // to drive 'simulated' PV's for other purposes. You'd define a Clf.Clf.LogicSystem
    // that only has 'input' nodes, and use the UI to define their values.
    //

    //
    // Procedure :
    //
    //  1. Create a ChannelDescriptorsList accommodating Input nodes and Output nodes
    //  2. Set up an event handler on each of those nodes, to transmit the value
    //     of the PV whenever it changes. Note that 'null' values can't be transmitted,
    //     so unfortunately we can't use this mechanism to simulate dropouts.
    //  3. Run 'ThinIOC' to set up a Server publishing those nodes.
    //

    [System.Obsolete("NEEDS REFACTORING AFTER THINIOC CHANGES")]
    private void StartThinIocPublishingChangesToInputAndOutputNodes (
    ) {
      Clf.ChannelAccess.ChannelDescriptorsList channelDescriptors = new() ;
      AllLogicNodes.Where(
        logicNode => (
          logicNode is InputNodeBase
        || logicNode.IsOutputNodeWithNoDirectDependents()
        )
      ).ForEachItem(
        logicNode => {
          channelDescriptors.Add(
            ChannelDescriptor.FromEncodedString(
              logicNode.ChannelName
            + GetTypeCodeFor(logicNode.ValueType)
            ) 
          ) ;
          logicNode.ValueChangedEvent += async () => {
            // Hmm, we'll be discarding any exceptions ...
            if ( logicNode.ValueAsObject != null )
            {
              await Clf.ChannelAccess.Hub.PutValueAsync(
                logicNode.ChannelName,
                logicNode.ValueAsObject
              ) ;
            }
          } ;
        }
      ) ;
      /////////////////////////////////////////////// channelDescriptorsList.StartThinIoc() ;
      string GetTypeCodeFor ( System.Type type )
      => (
        type switch
        {
        System.Type when type == typeof(int)    => "i32",
        System.Type when type == typeof(double) => "f64",
        System.Type when type == typeof(string) => "s39",
        System.Type when type == typeof(short)  => "i16",
        System.Type when type == typeof(bool)   => "i32",
        _                                       => throw new System.ArgumentException($"Type {type} is not supported")
        }
      ) ;
    }

    // Having obtained a ChannelDescriptorsList, we can pass it to 'ThinIOC' ...
    // NEED TO FIND REFACTOR THIS !!!
    //
    // Procedure :
    //
    //  1. Create a ChannelDescriptorsList accommodating Input nodes and Output nodes
    //  2. Set up an event handler on each of those nodes, to transmit the value
    //     of the PV whenever it changes. Note that 'null' values can't be transmitted,
    //     so unfortunately we can't use this mechanism to simulate dropouts.
    //

    public Clf.ChannelAccess.ChannelDescriptorsList 
    CreateChannelDescriptorsListFromLogicNodes_AddingEventHandlers ( ) 
    {
      Clf.ChannelAccess.ChannelDescriptorsList channelDescriptorsList = new() ;
      AllLogicNodes.Where(
        logicNode => (
          logicNode is InputNodeBase
        || logicNode.IsOutputNodeWithNoDirectDependents()
        )
      ).ForEachItem(
        logicNode => {
          channelDescriptorsList.Add(
            ChannelDescriptor.FromEncodedString(
              logicNode.ChannelName
            + GetTypeCodeFor(logicNode.ValueType)
            ) 
          ) ;
          logicNode.ValueChangedEvent += async () => {
            // Hmm, we'll be discarding any exceptions ...
            if ( logicNode.ValueAsObject != null )
            {
              var putValueResult_IGNORED = await Clf.ChannelAccess.Hub.PutValueAsync(
                logicNode.ChannelName,
                logicNode.ValueAsObject
              ) ;
            }
          } ;
        }
      ) ;
      return channelDescriptorsList ;
      string GetTypeCodeFor ( System.Type type )
      => (
        type switch
        {
        System.Type when type == typeof(int)    => "i32",
        System.Type when type == typeof(double) => "f64",
        System.Type when type == typeof(string) => "s39",
        System.Type when type == typeof(short)  => "i16",
        System.Type when type == typeof(bool)   => "i32",
        _                                       => throw new System.ArgumentException($"Type {type} is not supported")
        }
      ) ;
    }

  }

}
