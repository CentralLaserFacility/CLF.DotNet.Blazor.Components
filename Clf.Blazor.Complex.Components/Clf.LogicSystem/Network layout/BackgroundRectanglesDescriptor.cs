//
// BackgroundRectanglesDescriptor.cs
//

using Clf.LogicSystem.Common.GeomertyPrimitives;

namespace Clf.LogicSystem.Common
{

  //
  // This describes the rectangles we want to show as the 'background'
  // of a network diagram, in terms of their bounding rectangles
  // and their fill colours.
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

  public record BackgroundRectanglesDescriptor (
    ColouredRectangleDescriptor OuterRectangleEnclosingAllNodesAndBothMargins,
    ColouredRectangleDescriptor InnerRectangleEnclosingAllNodesAndInnerMargin,
    ColouredRectangleDescriptor InnerRectangleEnclosingAllNodes              
  ) ;

  public record ColouredRectangleDescriptor (
    System.Drawing.RectangleF BoundingRectangle,
    Colour                    FillColour
  ) ;

}
