//
// NodeLayoutDescriptor.cs
//

namespace Clf.LogicSystem.Common
{

  public sealed class NodeLayoutDescriptor
  {

    public string NodeId { get ; init ; }

    public System.Drawing.PointF CentrePosition { get ; private set ; }

    public System.Drawing.SizeF Size { get ; init ; }

    public System.Drawing.PointF TopLeftPoint     => CentrePosition - ( Size / 2 ) ;

    public System.Drawing.PointF BottomRightPoint => CentrePosition + ( Size / 2 ) ;

    public Clf.LogicSystem.Common.GeomertyPrimitives.CentredRectangleF NodeOutlineRectangle => new Clf.LogicSystem.Common.GeomertyPrimitives.CentredRectangleF(
      CentrePosition,
      Size
    ) ;

    public NodeLayoutDescriptor ( 
      string                nodeId,
      System.Drawing.PointF centrePosition,
      System.Drawing.SizeF  size    
      ) {
      NodeId         = nodeId ;
      CentrePosition = centrePosition ;
      Size           = size ;
    }

    // public NodeLayoutDescriptor MovedBy ( 
    //   System.Drawing.SizeF delta    
    // ) {
    //   return new NodeLayoutDescriptor(
    //     NodeId,
    //     CentrePosition + delta,
    //     Size
    //   ) ;
    // }

    public void MoveCentrePointBy ( 
      System.Numerics.Vector2 delta    
    ) {
      CentrePosition = new(
        CentrePosition.X + delta.X,
        CentrePosition.Y + delta.Y
      ) ;
    }

    public override string ToString ( ) => $"{NodeId}" ;

    // When we come to draw text on a node box, we'll want to know
    // the coordinates of the reference point at which we start
    // drawing the characters.

    // public System.Drawing.PointF TextReferencePoint ( LayoutSizingParameters layoutSizingParameters ) ;

  }

}
