﻿//
// NetworkLayoutDescriptor.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.Common.GeomertyPrimitives;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.Common
{

  // The NetworkLayoutDescriptor will not necessarily have been created with GraphViz,
  // it might have been generated by a different tool such as Dagre or GraphRe.

  public abstract class NetworkLayoutDescriptor
  {

    public readonly LayoutSizingParameters LayoutSizingParameters ;

    public BoundingRectanglesDescriptor BoundingRectanglesDescriptor { get ; private set ; }

    protected List<NodeLayoutDescriptor> m_nodeLayoutDescriptors = new() ;

    protected List<EdgeLayoutDescriptor> m_edgeLayoutDescriptors = new() ;

    // Provided the NetworkLayoutDescriptor has been built correctly,
    // each NodeLayoutDescriptor will tell us its position and size
    // in terms of the overall 'diagram' coordinates ; that is, its
    // position will reflect the Margin that's been asked for
    // in the LayoutSizingParameters.

    public IEnumerable<NodeLayoutDescriptor> NodeLayoutDescriptors => m_nodeLayoutDescriptors ;

    public IEnumerable<EdgeLayoutDescriptor> EdgeLayoutDescriptors => m_edgeLayoutDescriptors ;

    // Default constructor would be required for JSON persistence ???

    protected NetworkLayoutDescriptor ( LayoutSizingParameters layoutSizingParameters ) 
    {
      LayoutSizingParameters = layoutSizingParameters ;
    }

    //
    // When a NetworkLayoutDescriptor is initially created, eg from parsing the '.plain' file
    // generated by GraphViz, the nodes won't necessarily be placed at exactly the right positions
    // for displaying on our diagram.
    //
    //     0,0
    //    --+----------------------------------------
    //      | +-----------+
    //      | | Phantom_A |       +--------+
    //      | +-----------+       | ID = 2 |
    //      |                     +--------+
    //      | +---------+
    //      | | ID = 1  |       +--------+
    //      | +---------+       | ID = 3 |
    //      |                   +--------+
    //      |        +--------+
    //      |        | ID = 4 |
    //      |        +--------+      
    //      |                      +-----------+
    //      |                      | Phantom_B |
    //      |                      +-----------+
    //      |
    //
    // In the case of a GraphViz-generated layout, that's because our NetworkLayoutDescriptor
    // will only be mentioning the nodes that have 'integer' id's. The '.dot' file we submitted
    // to GraphViz will have defined 'phantom' nodes and links whose id's are not integers,
    // which we have added just to persuade GraphViz to produce a more amenable layout.
    // These 'phantom' nodes will be present in the '.plain' file, and will have been discarded
    // when we built the NetworkLayoutDescriptor. But coordinates of the nodes we *do* see
    // may still be influenced by those 'phantom' nodes.
    //
    // So, we'll adjust the nodeLayoutDescriptor positions so that there's a well defined margin
    // around the non-phantom nodes, in an overall diagram that has its
    // top left origin at (0,0) and accommodates various 'margins' :
    //
    //    [0,0]
    //    --+---------------------------------
    //      |######################################## <-- The 'OuterBoundingRectangleIncludingAllNodesAndMargin'         
    //      |########################################     has its top left point at [0,0], and accommodates        
    //      |########################################     the margin added around the nodes.       
    //      |#####+-------------------+--------+#####
    //      |#####|                   | ID = 2 |#####
    //      |#####|                   +--------+#####
    //      |#####+---------+                  |#####      
    //      |#####| ID = 1  |       +--------+ |#####     The 'InnerBoundingRectangleEnclosingAllNodes' 
    //      |#####+---------+       | ID = 3 | |#####     lies inside the 'outer' rectangle. If there are 
    //      |#####|                 +--------+ |#####     no nodes at all, its size will be zero.
    //      |#####|      +--------+            |#####
    //      |#####|      | ID = 4 |            |#####
    //      |#####+------+--------+------------+#####     The coordinates of the Nodes themselves are adjusted
    //      |########################################     so that they are located with the central area.
    //      |########################################           
    //      |########################################            
    //      |
    //

    public void NormalisePositionsAndApplyMargins (  )
    {

      //
      // Our goal here is to compute an InnerBoundingRectangle
      // that lies at the centre of the diagram, enclosing all the nodes.
      // With all coordinates adjusted so that the top left point of the rectangle
      // is located not at [0,0] but at a position inset by an amount
      // corresponding to the 'total margin'.
      //

      var innerBoundingRectangleEnclosingAllNodes = (
        NodeLayoutDescriptors.Any()
        ? ComputeInnerBoundingRectangleEnclosingAllNodes()
        : new System.Drawing.RectangleF(
            new System.Drawing.PointF(
              LayoutSizingParameters.DiagramTotalMarginSize.Width,
              LayoutSizingParameters.DiagramTotalMarginSize.Height
            ),
            LayoutSizingParameters.NodeOutlineRectangleMinimumSize
          )
      ) ;

      var outerBoundingRectangleEnclosingAllNodesAndBothMargins = new System.Drawing.RectangleF(
        new System.Drawing.PointF(0,0),
        innerBoundingRectangleEnclosingAllNodes.Size
        + new System.Drawing.SizeF(
          LayoutSizingParameters.DiagramTotalMarginSize.Width * 2,
          LayoutSizingParameters.DiagramTotalMarginSize.Height * 2
        )
      ) ;
      // The intermediate 'inner' margin can be derived from expanding the Nodes rectangle
      // all round by an amount corresponding to the DiagramInnerMarginSizeInPixels.
      var innerBoundingRectangleEnclosingAllNodesAndInnerMargin = new System.Drawing.RectangleF(
          x      : innerBoundingRectangleEnclosingAllNodes.X      - LayoutSizingParameters.DiagramInnerMarginSize.Width,
          y      : innerBoundingRectangleEnclosingAllNodes.Y      - LayoutSizingParameters.DiagramInnerMarginSize.Height,
          width  : innerBoundingRectangleEnclosingAllNodes.Width  + LayoutSizingParameters.DiagramInnerMarginSize.Width * 2,
          height : innerBoundingRectangleEnclosingAllNodes.Height + LayoutSizingParameters.DiagramInnerMarginSize.Height * 2
      ) ;

      BoundingRectanglesDescriptor = new(
        OuterBoundingRectangleEnclosingAllNodesAndBothMargins : outerBoundingRectangleEnclosingAllNodesAndBothMargins,
        InnerBoundingRectangleEnclosingAllNodesAndInnerMargin : innerBoundingRectangleEnclosingAllNodesAndInnerMargin,
        InnerBoundingRectangleEnclosingAllNodes               : innerBoundingRectangleEnclosingAllNodes              
      ) ;
    }

    private System.Drawing.RectangleF ComputeInnerBoundingRectangleEnclosingAllNodes ( )
    {
      // Scan the nodes to find the extreme points
      ComputeExtremePointsAndBoundingRectangle(
        NodeLayoutDescriptors,
        out float                     topLeftMinX,
        out float                     topLeftMinY,
        out float                     bottomRightMaxX,
        out float                     bottomRightMaxY,
        out System.Drawing.RectangleF boundingRectangleEnclosingAllNodes
      ) ;
      // Adjust the bounding rectangle so that its size is
      // at least the minimum size specified by the LayoutSizingParameters.
      boundingRectangleEnclosingAllNodes = boundingRectangleEnclosingAllNodes.ExpandedToMinimumSize(
        LayoutSizingParameters.NodeOutlineRectangleMinimumSize
      ) ;
      //
      // Compute the offset that we'll need to subtract from all coordinates
      // in order move the top-left point of the top-left-most nodeLayoutDescriptor
      // towards the [0,0] position at the top left of the diagram.
      //
      System.Numerics.Vector2 nominalOffsetToSubtract = new System.Numerics.Vector2(
        topLeftMinX,
        topLeftMinY
      ) ;
      //
      // But in order to create the desired 'margin' around the nodes, we'll need to ADD
      // an offset to each coordinate to move it towards the bottom right ...
      //
      System.Numerics.Vector2 offsetToAddForTotalMargin = (
        LayoutSizingParameters.DiagramTotalMarginSize.ToVector2()
      - nominalOffsetToSubtract
      ) ;
      //
      // Apply the necessary offset to each and every coordinate
      // (A) in all the Nodes, and (B) in all the points that define the Edges.
      //
      foreach ( var node in NodeLayoutDescriptors )
      {
        node.MoveCentrePointBy(offsetToAddForTotalMargin) ;
      }
      foreach ( var edge in EdgeLayoutDescriptors )
      {
        edge.MovePointsBy(offsetToAddForTotalMargin) ;
      }
      //
      // We can now define the Inner Bounding Rectangle
      // that contains all the nodes, offset by the desired total boundary.
      //
      var innerBoundingRectangleEnclosingAllNodes = new System.Drawing.RectangleF(
        location : new System.Drawing.PointF(
          LayoutSizingParameters.DiagramTotalMarginSize.Width,
          LayoutSizingParameters.DiagramTotalMarginSize.Height
        ),
        size : boundingRectangleEnclosingAllNodes.Size
      ) ;

      // Sanity check : all our nodes should lie inside this rectangle
      foreach ( var node in NodeLayoutDescriptors )
      {
        var nodeOutlineRectangle = node.NodeOutlineRectangle.ToSystemDrawingRectangleF() ;
        // nodeOutlineRectangle.LiesCompletelyInside(
        //   innerBoundingRectangleEnclosingAllNodes
        // ).Should().BeTrue() ;
      }

      return innerBoundingRectangleEnclosingAllNodes ;

    }

    private void ComputeExtremePointsAndBoundingRectangle (
      IEnumerable<NodeLayoutDescriptor> nodeLayoutDescriptors,
      out float                         topLeftMinX,      // X_Left
      out float                         topLeftMinY,      // Y_Top
      out float                         bottomRightMaxX,  // X_Right
      out float                         bottomRightMaxY,  // Y_Bottom
      out System.Drawing.RectangleF     boundingRectangleEnclosingAllNodes
    ) {
      topLeftMinX = NodeLayoutDescriptors.Select(
        node => node.TopLeftPoint.X
      ).Min() ;
      topLeftMinY = NodeLayoutDescriptors.Select(
        node => node.TopLeftPoint.Y
      ).Min() ;
      bottomRightMaxX = NodeLayoutDescriptors.Select(
        node => node.BottomRightPoint.X
      ).Max() ;
      bottomRightMaxY = NodeLayoutDescriptors.Select(
        node => node.BottomRightPoint.Y
      ).Max() ;
      float X_Left   = topLeftMinX ;
      float Y_Top    = topLeftMinY ;
      float X_Right  = bottomRightMaxX ;
      float Y_Bottom = bottomRightMaxY ;
      foreach ( var node in nodeLayoutDescriptors ) 
      {
        node.TopLeftPoint.X.Should().BeGreaterThanOrEqualTo(X_Left) ;
        node.TopLeftPoint.Y.Should().BeGreaterThanOrEqualTo(Y_Top) ;
        node.BottomRightPoint.X.Should().BeLessThanOrEqualTo(X_Right) ;
        node.BottomRightPoint.Y.Should().BeLessThanOrEqualTo(Y_Bottom) ;
      }
      // Compute the the bounding rectangle that would enclose all the nodes
      boundingRectangleEnclosingAllNodes = new System.Drawing.RectangleF(
        new System.Drawing.PointF(
          topLeftMinX,
          topLeftMinY
        ),
        new System.Drawing.SizeF(
          bottomRightMaxX - topLeftMinX,
          bottomRightMaxY - topLeftMinY
        ) 
      ) ;
      // Sanity check - make sure that all the nodes really do
      // lie inside the bounding rectangle we've computed
      foreach ( var nodeLayoutDescriptor in NodeLayoutDescriptors )
      {
        var nodeRectangle = nodeLayoutDescriptor.NodeOutlineRectangle.ToSystemDrawingRectangleF() ;
        // This was failing occasionally, because comparisons were being performed
        // with 'float' values without a specified tolerance !!!
        // bool liesInside = boundingRectangleEnclosingAllNodes.Contains(nodeRectangle) ;
        bool liesInside = nodeRectangle.LiesCompletelyInside(
          parent    : boundingRectangleEnclosingAllNodes,
          tolerance : 0.01f,
          out bool leftX_isOutsideOfParent,
          out bool rightX_isOutsideOfParent,
          out bool topY_isOutsideOfParent,
          out bool bottomY_isOutsideOfParent
        ) ;
        liesInside.Should().BeTrue() ;
        if ( ! liesInside ) 
        {
          string boundingRectangleTopLeft = boundingRectangleEnclosingAllNodes.TopLeftPoint().ToString() ;
          string boundingRectangleBottomRight = boundingRectangleEnclosingAllNodes.BottomRightPoint().ToString() ;
          string nodeTopLeft     = nodeRectangle.TopLeftPoint().ToString() ;
          string nodeBottomRight = nodeRectangle.BottomRightPoint().ToString() ;
        }
      }
    }

  }

}
