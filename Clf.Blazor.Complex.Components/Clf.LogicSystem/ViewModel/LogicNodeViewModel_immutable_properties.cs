//
// LogicNodeViewModel_immutable_properties.cs
//

using Clf.ChannelAccess;
using Clf.LogicSystem.Attributes;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.ViewModel
{

  // These are properties that are set up when the
  // ViewModel is created, and subsequently don't change.

  // We can either implement these as get-only properties,
  // or (with C#11) as required 'init' properties that would be
  // set up in the constructor. But requiring properties as a constructor
  // argument is sometimes preferable, if other properties can be defined
  // based on the value passed in - also, the constructor can do some
  // validation that wouldn't be possibke with a property setter.

  public partial class LogicNodeViewModel
  { 

    // The ViewModel for a Node is 'owned' by the ViewModel
    // that represents the entire diagram of a LogicSystem.
    // The diagram is available via this 'Parent' property.

    // public LogicSystemViewModel Parent { get ; }

    // The ViewModel has a reference to the actual LogicNode
    // that it's a visualisation of, within the Clf.Clf.LogicSystem.

    public LogicNode LogicNode { get ; private set ; }

    // The ViewModel knows the integer identifier that uniquely
    // identifies the LogicNode within the Clf.Clf.LogicSystem.

    public int UniqueIntegerIdentifier => LogicNode.UniqueIntegerIdentifier ;
                                     
    // The stacking order for a Node is always '1',
    // which puts it 'underneath' other visual elements
    // that can have a higher number for their stacking order.

    public override int StackingOrderZ => 1 ;

    // The position-and-size of the box that represents a Node
    // has been assigned by the layout algorithm, and doesn't change.
    // This is the 'nominal' bounding rectangle, which doesn't include
    // space for an extra 'external border' that we might draw around it.

    public Clf.LogicSystem.Common.GeomertyPrimitives.CentredRectangleF NodeOutlineRectangle { get ; }

    // -------------------------------

    public IEnumerable<string> LabelTextLinesTemplate => LogicNode.LabelTextLinesTemplate().ToList() ;

    // public LogicNodeCategory LogicNodeCategory => LogicNode.LogicNodeCategory ;

    public bool ShowAsCollapsed => LogicNode.ShowAsCollapsed() ;

    public string PropertyName => LogicNode.PropertyName ;

    public string ChannelName => LogicNode.ChannelName ;

    public ChannelType ChannelType => LogicNode.ChannelType ;

  }

}

