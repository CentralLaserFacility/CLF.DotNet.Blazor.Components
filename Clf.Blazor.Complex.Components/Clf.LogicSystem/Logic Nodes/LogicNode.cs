//
// LogicNode.cs
//

using Clf.ChannelAccess;
using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Attributes;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodesManagers;
using Clf.LogicSystem.Miscellaneous;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.LogicNodes
{

  //
  // A 'node' always provides a Value of some type.
  //
  // The value might be a mutable 'input' value, or a 'computed' value.
  //
  // This abstract base class provides concrete implementations of various properties
  // that are common to the 'input node' and 'computed node' subclasses.
  //

  public abstract class LogicNode // : ILogicNode
  {

    private static LogicSystemBase? g_parentLogicSystem = null ;
    public static LogicSystemBase? ParentLogicSystemBeingConfigured
    {
      get => g_parentLogicSystem.VerifiedAsNonNullInstance() ;
      set 
      {
        if ( value == null )
        {
          // We're setting the value to null,
          // so we'd expect it to have been set non null.
          g_parentLogicSystem.Should().NotBeNull() ;
          g_parentLogicSystem = null ;
        }
        else
        {
          // We're setting the value to non-null,
          // se we'd expect it to be currently null.
          g_parentLogicSystem.Should().BeNull() ;
          g_parentLogicSystem = value ;
        }
      }
    }

    public int UniqueIntegerIdentifier { get ; init ; }

    public string UniqueIntegerIdentifierAsString => UniqueIntegerIdentifier.ToString("D3") ;

    // The 'PropertyName' of a node is the name of the C# property, which is always available.

    public string PropertyName { get ; private set ; } 

    // The 'ChannelName' of a node is taken from the '[ChannelName]' attribute
    // if that has been supplied, otherwise it defaults to the PropertyName.

    public ChannelName ChannelName => this.OptionalAttributeValueOrNull(NodeAttributeNames.ChannelName) ?? PropertyName ; 
        
    public bool DefinesAnActualChannelName => this.HasAttribute(NodeAttributeNames.ChannelName) ;

    public ChannelAccess.ChannelType? _channelType;
    public ChannelType ChannelType
    {
        get
        {
            if (_channelType.HasValue)
                return _channelType.Value;
            else
            {
                _channelType = DefinesAnActualChannelType ?

                //Get the channel type as ChannelType attribute is defined
                (System.Enum.TryParse(
                        typeof(ChannelType),
                        this.OptionalAttributeValueOrNull(NodeAttributeNames.ChannelType),
                        out object chnlType
                    ) ? (ChannelType)chnlType : ChannelType.Undefined)

                //By default, computed node's creates local and input node's creates remote.
                : (this is ComputedNodeBase ? ChannelType.Local
                    : this is InputNodeBase ? ChannelType.Remote
                        : ChannelType.Undefined);

                return _channelType.Value;
            }
        }
    }

    public bool DefinesAnActualChannelType => this.HasAttribute(NodeAttributeNames.ChannelType) ;

    public LogicSystemBase LogicSystem { get ; init ; }

    internal LogicNodesManager LogicNodesManager => LogicSystem.LogicNodesManager;

    protected LogicNode (
      LogicSystemBase logicSystem,
      string          propertyName,
      System.Action?  valueChangedAction
    ) {
      // At the time we create instance of LogicNode,
      // in the 'LogicNodesManager.ScanTargetObjectProperties_BuildingLogicNodesAndDependencies'
      // method, we expect the static 'ParentLogicSystemBeingConfigured' property to match
      try
      {
        ParentLogicSystemBeingConfigured.Should().Be(logicSystem);
     
        LogicSystem = logicSystem ;
        this.PropertyName = propertyName ;

        UniqueIntegerIdentifier = LogicNodesManager.RegisterNewlyCreatedNode(
          this,
          propertyName
        ) ;
      }
      catch (Exception ex)
      {
        ex.ToString();
      }
      ValueChangedAction = valueChangedAction ;
    }

    public override string ToString ( ) => this.PropertyName ;

    public abstract System.Type ValueType { get ; }

    public abstract object? ValueAsObject { get ; }

    public abstract string ValueAsString { get ; }
    
    public virtual object? DefaultValueAsObject => null ;
    
    public string DefaultValueAsString => LogicHelpers.GetValueAsString(DefaultValueAsObject) ;
    
    public string ValueAsStringForDisplay => LogicHelpers.GetValueAsStringForDisplay(ValueAsObject) ;

    public bool ValueIsNull => ValueAsObject is null ;

    // NEEDS_FIXING
    // These methods really only apply to Boolean values
    // but they do return correct results for *any* type.
    // On balance it's probably better to NOT define these
    // in the base class, and make them available only in
    // concrete subclasses that hold Boolean values ...
    public bool ValueIsTrue => ValueAsObject is true ;
    public bool ValueIsFalse => ValueAsObject is false ;

    public virtual string SummaryText ( ) => $"'{this.PropertyName}' class={this.ClassName()} ; '{this.ValueAsString}'" ;

    // Every LogicNode can define a single action that will be invoked
    // when any and all of the changes provoked by a single input change
    // have propagated through the network.

    public System.Action? ValueChangedAction { get ; }

    //
    // For any Node, we can install an event handler that will be invoked
    // when changes provoked by an input change have propagated
    // through the network.
    //
    // Typically we'll use this to propagate changes to a ViewModel.
    // Note that we need to ensure that when a ViewModel registers
    // its interest in a certain set of Nodes, via '+=' on the event,
    // it performs a matching '-=' when it's no longer interested.
    //
    // Hmm, Messenger would be a more robust scheme here !!!
    //

    //
    // Gotcha !! We need to raise this event not only when the Value changes,
    // but whenever any other property changes that affects the Visual representation.
    // Such as, the 'Shadow discrepancy' value.
    //

    //
    // TODO : CLARIFY THAT THIS EVENT IS ONLY SUBSCRIBED TO IN THE LOGIC SYSTEM ITSELF.
    // CHANGES TO VISUALISED PROPERTIES ARE HANDLED BY A SEPARATE EVENT !!! SEE BELOW ...
    //

    public event System.Action? ValueChangedEvent ;

    public int HowManyValueChangedEventHandlersCurrentlyAttached 
    => ValueChangedEvent?.GetInvocationList().Length ?? 0 ;
    
    public void RaiseValueChangedEvent ( )
    {
      ValueChangedEvent?.Invoke() ;
      VisualisedPropertyChangedEvent?.Invoke() ;
    }

    // This event pertains to changes that relate to properties
    // other than the 'Value' of the logic node, which nevertheless
    // affect the visualisation of the LogicNode. For example,
    // the status of the 'ShadowValue' property.

    public event System.Action? VisualisedPropertyChangedEvent ;

    public int HowManyVisualisedPropertyChangedEventHandlersCurrentlyAttached 
    => VisualisedPropertyChangedEvent?.GetInvocationList().Length ?? 0 ;
    
    public void RaiseVisualisedPropertyChangedEvent ( )
    {
      VisualisedPropertyChangedEvent?.Invoke() ;
    }

    // FIX_THIS : ALSO NEED TO DO THIS FOR VISUALISED-PROPERTY-CHANGED EVENTS !!!

    private void DetachAllValueChangedEventHandlers ( )
    {
      // https://stackoverflow.com/questions/447821/how-do-i-unsubscribe-all-handlers-from-an-event-for-a-particular-class-in-c
      System.Delegate[]? delegatesToRemove = ValueChangedEvent?.GetInvocationList() ;
      if ( 
         delegatesToRemove != null 
      && delegatesToRemove.Any()
      ) {
        foreach ( System.Delegate delegateToRemove in delegatesToRemove )
        {
          ValueChangedEvent -= (System.Action) delegateToRemove ;
        }
      }
    }

    public void EnsureNoValueChangedEventHandlersStillAttached ( ) 
    { 
      if ( HowManyValueChangedEventHandlersCurrentlyAttached != 0 )
      {
        DetachAllValueChangedEventHandlers() ;
        AssertNoValueChangedEventHandlersStillAttached() ;
      }
    }

    public void AssertNoValueChangedEventHandlersStillAttached ( ) 
    { 
      HowManyValueChangedEventHandlersCurrentlyAttached.Should().Be(0) ;
    }

    // Attributes

    private Dictionary<string,string> m_attributesDictionary = new Dictionary<string,string>() ;

    public IReadOnlyDictionary<
      string, // Name of the attribute
      string  // Value, as a string
    > AttributesDictionary => m_attributesDictionary ;

    public void AddAttribute ( string name, string value )
    {
      m_attributesDictionary.Add(name,value) ;
    }

    public void ReplaceAttribute ( string name, string value )
    {
      m_attributesDictionary[name] = value ;
    }

  }

}

