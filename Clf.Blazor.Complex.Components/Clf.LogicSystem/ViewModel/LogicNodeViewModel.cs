//
// LogicNodeViewModel.cs
//

using FluentAssertions;

namespace Clf.LogicSystem.ViewModel
{

  //
  // The visualisation presents various aspects of the 'status' of a Node :
  //
  //  - its 'category', ie Input node vs Computed node
  //
  //  - is the node being hovered over ?
  //  - is the node 'selected' ?
  // 
  // If a Computed node is showing a different value than the one that was predicted
  // as the proper 'expected' value, in the current Scenario, we indicate this
  // discrepancy by (A) showing the Expected value and also the Actual value in the Tooltip,
  // and (B) drawing an additional coloured border outside of the nominal border.
  //

  public sealed partial class LogicNodeViewModel 
  : LogicSystemViewModelElementBase
  , ILogicNodeViewModel
  {

    //
    // Constructor can mention the LogicNode,
    // and we can populate everything else from that ?
    //
    // BUT : even if we pass in the logicNode, we still need
    // the Parent ViewModel instance.
    //
    // We could continue to support JSON persisence if we pass in the integer ID,
    // and configure the LogicSystemViewModel with an API call once the
    // tree of instances has been created ... ???
    //

    public LogicNodeViewModel ( 
      LogicSystemViewModel                    parent,
      int                                     uniqueIntegerIdentifier, 
      Clf.LogicSystem.Common.GeomertyPrimitives.CentredRectangleF boundingRectangle // Assigned by the layout algorithm
    ) :
    base(parent)
    {
      // The 'uniqueIntegerIdentifier' lets us look up the LogicNode
      // within the overall Clf.Clf.LogicSystem. 
      LogicNode = Parent.LogicSystem.LookupLogicNode_FromUniqueIntegerIdentifier(
        uniqueIntegerIdentifier
      ) ;
      UniqueIntegerIdentifier.Should().Be(uniqueIntegerIdentifier) ;
      NodeOutlineRectangle = boundingRectangle ;
      this.PropertyChanged += (sender,propertyChangedEventArgs) => {
        System.Diagnostics.Debug.WriteLine(
          $"Node {PropertyName} : PropertyChanged raised, property name is '{propertyChangedEventArgs.PropertyName}'"
        ) ;
      } ;
    }

    // Default constructor is required for JSON persistence.
    // But we can't call the main constructor, as that assumes
    // that we have a valid parent. Hmm, tricky.

    // [RequiredForJsonPersistence]
    // public LogicNodeViewModel ( ) :
    // this(
    //   parent                  : null!,
    //   uniqueIntegerIdentifier : -1,
    //   boundingRectangle       : null!
    // ) {
    // }

    public override string ToString ( ) => $"Node '{PropertyName}'" ;

    // TODO : RaisePropertyChanged_All ??

    public void RaisePropertyChanged ( string? propertyName = null )
    {
      OnPropertyChanged(propertyName??"") ;
    }

    public void HandleLogicNodeVisualisedPropertyChanged ( )
    {
      // this.ValueAsString = LogicNode.ValueAsString ;
      // if ( LogicNode is ComputedNodeBase computedNode )
      // {
      //   this.ShadowValue = computedNode.ShadowValue ;
      // }
      // This is equivalent to raising 'PropertyChanged'
      // on each and every property ...
      RaisePropertyChanged() ;
    }
  
    public void RegisterWithLogicNodeVisualisedPropertyChangedEvent ( )
    {
      // A check to ensure the handler has been added
      // is probably unnecessary, but it doesn't do any harm ...
      int nBefore = LogicNode.HowManyVisualisedPropertyChangedEventHandlersCurrentlyAttached ;
      LogicNode.VisualisedPropertyChangedEvent += HandleLogicNodeVisualisedPropertyChanged ;
      int nAfter = LogicNode.HowManyVisualisedPropertyChangedEventHandlersCurrentlyAttached ;
      nAfter.Should().Be(nBefore+1) ;
    }

    public void DeregisterFromLogicNodeValueChangedEvent ( )
    {
      LogicNode.VisualisedPropertyChangedEvent -= HandleLogicNodeVisualisedPropertyChanged ;
    }

  }

}

