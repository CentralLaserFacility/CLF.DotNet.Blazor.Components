//
// LogicSystemCanvasViewModel.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.ViewModel
{

  //
  // The 'canvas' is the area onto which the diagram is drawn.
  //
  //
  //  0,0
  //   +-----------------------------------------------+  -- OuterBoundingRectangleEnclosingAllNodesAndBothMargins
  //   | Outer Margin                                  |
  //   |   +---------------------------------------+   |  -- InnerBoundingRectangleEnclosingAllNodesAndInnerMargin
  //   |   | Inner Margin                          |   |  
  //   |   |   +------+------------------------+   |   |  -- InnerBoundingRectangleEnclosingAllNodes
  //   |   |   |  A   |             +------+   |   |   |  
  //   |   |   +------+             |  B   |   |   |   |     If there are no nodes present, the innermost
  //   |   |   |                    +------+   |   |   |     rectangle still takes on a non zero size
  //   |   |   |        +------+               |   |   |     and it can still be clicked on.
  //   |   |   |        |  C   |        +------+   |   |  
  //   |   |   |        +------+        |  D   |   |   |     The 'inner margin' provides some space 'outside' of
  //   |   |   +------------------------+------+   |   |     the exact rectangle required for the nodes themselves,
  //   |   |                                       |   |     in order that we can accommodate (A) borders that extend
  //   |   +---------------------------------------+   |     outside of that nominal rectangle (due to their thickness),
  //   |                                               |     and also (B) additional 'external rectangles' around the nodes.
  //   +-----------------------------------------------+
  //

  public sealed partial class LogicSystemCanvasViewModel : LogicSystemViewModelElementBase
  {

    // ?? An improved BackgroundRectanglesDescriptor
    // could describes colour as well as geometry ??

    public LogicSystemCanvasViewModel ( LogicSystemViewModel parent ) :
    base(parent)
    { }

    public Clf.LogicSystem.Common.BoundingRectanglesDescriptor BoundingRectanglesDescriptor { get ; private set ; }

    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour OuterBoundingRectangleEnclosingAllNodesAndBothMargins_Colour 
    => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForOuterBoundingRectangleEnclosingAllNodesAndBothMargins ;

    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour InnerBoundingRectangleEnclosingAllNodesAndInnerMargin_Colour 
    => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForInnerBoundingRectangleEnclosingAllNodesAndInnerMargin ;
    // 

    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour InnerBoundingRectangleEnclosingAllNodes_Colour               
    => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForInnerBoundingRectangleEnclosingAllNodes ;

    // If there are no nodes, we'll still see a non-empty rectangle
    // to enclose the nodes, which will be of a minumum size.
    // However we may want to indicate emptiness by using a different
    // fill colour or by writing something in the inner area ?

    public bool IsEmpty => Parent.LogicNodeViewModels.Any() is false ;

    public override int StackingOrderZ => 0 ;

    public override IEnumerable<string> ToolTipTextLines => new []{
      $"{Parent.LogicSystem.ClassName()} :#{Parent.LogicSystem.InstanceNumber} VM=#{Parent.InstanceNumber}",
      $"{Parent.WhichNodesAreIncluded}",
      $"{Parent.LogicNodeViewModels.Count()} nodes",
      $"{Parent.LogicSystem.Timings}"
      // $"{DependencyLinkViewModelsList.Count()} links",
      // $"Total of {LogicNodeViewModelsList.Count()+DependencyLinkViewModelsList.Count()} elements"
    } ;

    public void LoadPropertiesFrom ( Clf.LogicSystem.Common.NetworkLayoutDescriptor networkLayoutDescriptor )
    {
      BoundingRectanglesDescriptor = networkLayoutDescriptor.BoundingRectanglesDescriptor ;
    }

  }

}

