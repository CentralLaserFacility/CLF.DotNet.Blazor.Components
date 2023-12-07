//
// LogicSystemBase_channel_monitoring.cs
//

using Clf.LogicSystem.Common;
using Clf.LogicSystem.Common.ExtensionMethods;

using Clf.ChannelAccess.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using Clf.Common.ExtensionMethods;
using Clf.ChannelAccess;
using Clf.LogicSystem.Common.Utils;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase 
  {

    private Clf.ChannelAccess.ChannelsMonitor? m_channelsMonitor  ;

    public void EnsureMonitoredChannelsConfigured ( )
    {
      if ( m_channelsMonitor == null )
      {
        m_channelsMonitor = new Clf.ChannelAccess.ChannelsMonitor() ;
        m_channelsMonitor.CanPerformNumericTypeConversion = CanPerformTypeConversion_FromIncomingChannelValue ;
        //
        // Scan the current Clf.Clf.LogicSystem, visiting each and every Node.
        // According to the data type of the Node, add a 'Monitored Channel'
        // using that Node's PV name, to the Channels Monitor, so that we'll be
        // listening for value changes and connect/disconnect events for that PV.
        //
        // For a Computed node, we don't create a Monitor ; instead, we create a Local Channel
        // that gets updated with a new value whenever the computed Value changes.
        //
        // Configure the event handling for each Channel, such that when an
        // updated value arrives, that value is transmitted to the Logic System.
        //
        AllLogicNodes.ForEachItem(
          logicNode => {

            var channelType = logicNode.ChannelType ;
              switch (logicNode)
              {

                  case Clf.LogicSystem.LogicNodes.InputNodeBase inputNode:

                      // Input node with local channel: When local channel gets created, it does not have any initial value. Although any node that gets its value from some logic will have an initial value. For example: TEST-PM-201-FE-1:FrontEndCW =  null; whereas LOC:IsPA1Allowed_LOGIC = NotAllowed(#0)

                      bool isInvalid = false ;

                      if (channelType == ChannelType.Local)
                      {

                          if (Clf.ChannelAccess.Helpers.CanInferLogicNodeDbFieldDescriptorFromValueType(
                          logicNode.ValueType,
                          out Clf.ChannelAccess.DbFieldDescriptor? _dbFieldDescriptor
                          ))
                              // We are interested in creating local channel, if it fails due to some reason (for example: a remote channel already exists), then we should NOT create a remote channel monitor in the next step)
                              isInvalid = Clf.ChannelAccess.Hub.GetOrCreateLocalChannel(
                                    new Clf.ChannelAccess.ChannelDescriptor(
                                        ChannelName: logicNode.ChannelName,
                                        DbFieldDescriptor: _dbFieldDescriptor,
                                        InitialValueAsString: logicNode.ValueAsString == "null" ? null : logicNode.ValueAsString
                                    ),
                                    initiallyConnected: true
                                  ).IsInvalid(out var whyNotValid);

                      }

                      if (!isInvalid)
                      {
                          var monitorChannel = new Clf.ChannelAccess.ChannelMonitor_ObservingChannelValueAsObject(
                            logicNode.ChannelName,
                            (name, valueOrNull) => inputNode.SetValue(
                              valueOrNull
                            ),
                            (name, isConnected) => inputNode.RaiseChannelConnectionStatusChangedEvent(isConnected)
                          );
                          m_channelsMonitor.AddChannel(monitorChannel);
                      }

                      break;

                  case Clf.LogicSystem.LogicNodes.ComputedNodeBase computedNodeBase:

                      Clf.ChannelAccess.IChannel? nodeChannel = null;
                      Clf.ChannelAccess.DbFieldDescriptor? cDbFieldDescriptor = null;

                      if (Clf.ChannelAccess.Helpers.CanInferLogicNodeDbFieldDescriptorFromValueType(
                                  computedNodeBase.ValueType,
                                  out cDbFieldDescriptor))
                      {
                          if (channelType == ChannelType.Remote)
                          {
                              nodeChannel = Clf.ChannelAccess.Hub.GetOrCreateChannel(logicNode.ChannelName);
                          }
                          else
                          {
                              nodeChannel = Clf.ChannelAccess.Hub.GetOrCreateLocalChannel(
                                  new Clf.ChannelAccess.ChannelDescriptor(
                                      ChannelName: computedNodeBase.ChannelName,
                                      DbFieldDescriptor: cDbFieldDescriptor,
                                      InitialValueAsString: computedNodeBase.ValueAsString
                                  ),
                                  initiallyConnected: true
                              );
                          }

                          computedNodeBase.ValueChangedEvent += () =>
                          {
                              if (computedNodeBase.ValueAsObject is null)
                              {
                                  // A computed node's Value will never be null.
                                  throw new System.Diagnostics.UnreachableException(
                                    $"Computed node {computedNodeBase.PropertyName} has a null value !!"
                                  );
                              }
                              else
                              {
                                  // TINE's VERSION - ????????
                                  if (cDbFieldDescriptor.TryParseValue(
                                      computedNodeBase.ValueAsString,
                                      out var valueToWrite
                                      ))
                                  {
                                      nodeChannel.PutValue(valueToWrite!);
                                  }
                                  else
                                      throw new System.Diagnostics.UnreachableException(
                                          $"Computed node {computedNodeBase.PropertyName} value cannot be parsed !!"
                                      );

                                  // ORIGINAL VERSION ...
                                  // localChannel.PutValue(
                                  //   computedNodeBase.ValueAsObject
                                  // ) ;
                              }
                          };
                      }
                      else
                      {
                          throw new System.Diagnostics.UnreachableException(
                          $"Failed to infer value type of {computedNodeBase.ChannelName} node !!"
                          );
                      }
                      break;
              }
          }
        ) ;
      }
    }

    public bool AllChannelsHaveBeenCreated (
      out List<string> namesOfChannelsNotCreated
    ) {
      List<string> namesOfChannelsNotRegistered = new() ;
      var allRegisteredChannels = Clf.ChannelAccess.Hub.GetRegisteredChannelsSnapshot() ;
      var allRegisteredChannelNames = allRegisteredChannels.Select(
        channel => channel.ChannelName.WithOptionalValSuffixRemoved().Name
      ) ;
      int nChannelsFound = 0 ;
      int nChannelsNotFound = 0 ;
      AllLogicNodes.ForEachItem(
        logicNode => {
          bool foundChannelForThisNode = allRegisteredChannelNames.Contains(
            logicNode.ChannelName.ToString().Replace(".VAL","")
          ) ;
          if ( foundChannelForThisNode )
          {
            nChannelsFound++ ;
          }
          else
          {
            nChannelsNotFound++ ;
            namesOfChannelsNotRegistered.Add(
             logicNode.ChannelName
            ) ;
          }
        }
      ) ;
      namesOfChannelsNotCreated = namesOfChannelsNotRegistered ;
      return (
        nChannelsNotFound == 0
        ? true  // All the channels we asked for do exist
        : false // One or more channels were *not* created
      ) ;
    }

    private static bool CanPerformTypeConversion_FromIncomingChannelValue ( 
      object      incomingValue,
      System.Type desiredType, // Numeric or bool
      out object? convertedValue
    ) {
      // We perform conversions to the desired Type
      // if the provided value is 'compatible'.
      // These rules are as needed for Machine Safety.
      convertedValue = null ;
      if ( desiredType == typeof(bool) )
      {
        if ( incomingValue is int newIntValue ) 
        {
          // If the incoming value is an int and we're expecting bool,
          // treat 0/1 as false/true and anything else as null ...
          convertedValue = newIntValue switch {
            0 => false,
            1 => true,
            _ => (bool?) null
          } ;
        }
        else if ( incomingValue is short newShortValue ) 
        {
          // If the incoming value is an int and we're expecting bool,
          // treat 0/1 as false/true and anything else as null ...
          convertedValue = newShortValue switch {
            0 => false,
            1 => true,
            _ => (bool?) null
          } ;
        }
        else if ( incomingValue is string newStringValue ) 
        {
          // If the incoming value is a string ...
          newStringValue = newStringValue.ToLower() ;
          if ( 
              newStringValue.Contains("on") 
          // || newStringValue.Contains("yes") 
          ) {
            convertedValue = true ;
          }
          else if ( 
              newStringValue.Contains("off") 
          // || newStringValue.Contains("no") 
          ) {
            convertedValue = false ;
          }
          else if ( newStringValue.Contains("null") )
          {
            convertedValue = null ;
          }
          else
          {
            convertedValue = newStringValue switch {
              "false" => false,
              "f"     => false,
              "0"     => false,
              "no"    => false, // ??
              "out"   => false, // ??
              "true"  => true,
              "t"     => true,
              "1"     => true,
              "yes"   => true, // ??
              "in"    => true, // ??
              _       => null
            } ;
            // bool? ReturnNull_LoggingStringValue ( string s )
            // {
            //   System.Console.WriteLine(
            //     $"**** Incoming string value '{s}' not recognised as boolean ; PV is '{channelName}'"
            //   ) ;
            //   return null ;
            // }
          }
        }
        else if ( incomingValue is double newDoubleValue ) 
        {
          // If the incoming value is a double and we're expecting bool,
          // treat 0 as false and anything else as true ...
          convertedValue = (
            newDoubleValue != 0.0 
          ) ;
        }
        else if ( incomingValue is float newFloatValue ) 
        {
          // If the incoming value is a double and we're expecting bool,
          // treat 0 as false and anything else as true ...
          convertedValue = (
            newFloatValue != 0.0f 
          ) ;
        }
      }
      else if ( desiredType == typeof(double) )
      {
        if ( incomingValue is float newFloatValue )
        {
          convertedValue = (double) newFloatValue ;
        }
        else if ( incomingValue is string newStringValue )
        {
          convertedValue = (
            newStringValue.CanParseAs<double>( out double doubleValue )
            ? doubleValue
            : null
          ) ;
        }
      }
      return (
        convertedValue != null 
      ) ;
    }

  }

}
