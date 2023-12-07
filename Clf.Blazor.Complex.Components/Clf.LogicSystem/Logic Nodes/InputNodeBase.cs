//
// InputNodeBase.cs
//

using Clf.LogicSystem.ChangeDescriptors;

namespace Clf.LogicSystem.LogicNodes
{

  public abstract class InputNodeBase : LogicNode
  {

    protected InputNodeBase (
      LogicSystemBase logicSystem,
      string          propertyName,
      System.Action?  valueChangedAction = null
    ) :
    base(
      logicSystem,
      propertyName,
      valueChangedAction
    ) {
    }

    /// <summary>
    /// Only used when initialising a Scenario.
    /// Caution: Hmm, this is DANGEROUS ... hence, internal.
    /// </summary>
    internal abstract void SetValueAsDefault_WITHOUT_PROPAGATING_CHANGES ( ) ;


    public abstract bool CanSetValue_ParsedFromString (
      string                                                      valueAsString,
      System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
    ) ;

    /// <summary>
    /// This event is raised via the ChannelsMonitor that is listening for status changes on the PV we're connected to.
    /// </summary>
    public event System.Action<bool>? ChannelConnectionStatusChangedEvent ;

    /// <summary>
    /// Event for handling channel connection status changed
    /// </summary>
    /// <param name="isConnected"></param>
    internal void RaiseChannelConnectionStatusChangedEvent ( bool isConnected )
    {
      ChannelConnectionStatusChangedEvent?.Invoke(isConnected) ;
    }

    /// <summary>
    /// Change input node value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="changesHandler">Handle changes arising from input value changes</param>
    public abstract void SetValue (
      object?                                                     value,
      System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
    ) ;

    /// <summary>
    /// Set the next value to input node
    /// </summary>
    /// <param name="changesHandler">Handle changes arising from input value change</param>
    public virtual void CycleToNextValue ( 
      System.Action<LogicNodeChangesArisingFromInputValueChange>? changesHandler = null
    ) { 
    }

    /// <summary>
    /// Perform type conversion from object or null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="incomingValueOrNull"></param>
    /// <returns></returns>
    protected internal T? PerformTypeConversionFromObjectOrNullTo<T> ( 
      object? incomingValueOrNull
    ) 
    where T : struct
    {
      if ( 
        Clf.Common.Utils.TypeConversionHelpers.CanPerformTypeConversionFromObject(
          incomingValueOrNull,
          typeof(T),
          out object? convertedValue
        )
      ) {
        return (T) convertedValue! ;
      }
      else
      {
        return null ;
      }
    }

    #if SUPPORT_VALUE_VALIDITY_STATUS_CHANGE_EVENTS

    //
    // This mechanism provides a distinct event that client code can hook into
    // to respond to transitions between 'null' 'non-null' states,
    // irrespective of the data type of the Value. The event is raised
    // when the 'value' gets updated with a value that is either freshly null
    // or freshly non-null.
    //

    // 
    // The mechanism works fine and is potentially useful, but the code
    // has been retired in favour of a more direct technique in which we raise an event
    // in response to a 'ConnectionStatusChange' handled by the ChannelsMonitor.
    // See the 'EnsureMonitoredChannelsConfigured' method.
    //

    //
    // This event gets raised when the Value changes
    // between being non-null, ie with the driving PV being connected,
    // and being null, with the driving PV being not connected.
    //

    public event System.Action<bool>? ChannelValueValidityStatusChangedEvent ;

    public void RaiseChannelValueValidityStatusChangedEvent ( bool valueIsNonNull )
    {
      ChannelValueValidityStatusChangedEvent?.Invoke(valueIsNonNull) ;
    }

    protected void HandleValueValidityStatusChange ( bool previousValueWasNull, object? latestIncomingValue ) 
    {
      bool newIncomingValueIsNull = latestIncomingValue is null ;
      if ( previousValueWasNull == newIncomingValueIsNull ) 
      {
        // The 'null' status hasn't changed 
      }
      else
      {
        // The value has become null when it was previously non-null,
        // or it has become non-null when it was previously null.
        RaiseChannelValueValidityStatusChangedEvent(
          newIncomingValueIsNull is false
        ) ;
      }
    }

    #endif

  }

}
