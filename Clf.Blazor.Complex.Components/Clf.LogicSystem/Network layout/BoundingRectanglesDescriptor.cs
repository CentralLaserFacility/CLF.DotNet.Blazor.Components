//
// BoundingRectanglesDescriptor.cs
//

namespace Clf.LogicSystem.Common
{

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
  //   |   |   |        +------+        |  D   |   |   |  
  //   |   |   +------------------------+------+   |   |  
  //   |   |                                       |   |  
  //   |   +---------------------------------------+   |  
  //   |                                               |  
  //   +-----------------------------------------------+
  //
  //  0,0
  //   +-----------------------------------------------+  OuterBoundingRectangleEnclosingAllNodesAndBothMargins
  //   | Outer Margin                                  |
  //   |   +---------------------------------------+   |  InnerBoundingRectangleEnclosingAllNodesAndInnerMargin
  //   |   | Inner Margin                          |   |  
  //   |   |   +-------------------------------+   |   |  InnerBoundingRectangleEnclosingAllNodes
  //   |   |   |                               |   |   |  
  //   |   |   |   NodesRectangleMinimumSize   |   |   |  
  //   |   |   |                               |   |   |  
  //   |   |   +-------------------------------+   |   |  
  //   |   |                                       |   |  
  //   |   +---------------------------------------+   |  
  //   |                                               |  
  //   +-----------------------------------------------+
  //

  public record BoundingRectanglesDescriptor (
    System.Drawing.RectangleF OuterBoundingRectangleEnclosingAllNodesAndBothMargins,
    System.Drawing.RectangleF InnerBoundingRectangleEnclosingAllNodesAndInnerMargin,
    System.Drawing.RectangleF InnerBoundingRectangleEnclosingAllNodes              
  ) ;

}
