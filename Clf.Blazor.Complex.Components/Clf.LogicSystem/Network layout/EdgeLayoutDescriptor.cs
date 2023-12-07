//
// EdgeLayoutDescriptor.cs
//

using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.Common
{

  // FIX_THIS ??
  // This could be an immutable record ???
  // But how best to present the Points list as an immutable ??

  public sealed class EdgeLayoutDescriptor
  {

    public string SourceNodeId { get ; init ; }

    public string TargetNodeId { get ; init ; }

    public List<System.Drawing.PointF> Points { get ; private set ; }
    
    public System.Drawing.PointF StartPoint => Points[0] ;

    public System.Drawing.PointF EndPoint => Points[^1] ;
    
    public Clf.LogicSystem.Common.DirectionVector DirectionVectorAtEndPoint 
    => new Clf.LogicSystem.Common.DirectionVector(
      Points[^1], 
      Points[^2] 
    ) ;

    // // If we need to draw a Bezier curve, it might be necessary
    // // to access 'control points' that lie before and after the
    // // first and last point. Hmm, we could try faking these
    // // by interpolating back and forward linearly ?
    // 
    // public System.Drawing.PointF ControlPointBeforeStartPoint 
    // {
    //   get
    //   {
    //     var delta = DeltaXY.FromPoints(
    //       Points[0],
    //       Points[1]
    //     ) ;
    //     return Points[0].MovedBy(-delta) ;
    //   }
    // }
    // 
    // public System.Drawing.PointF ControlPointAfterEndPoint 
    // {
    //   get
    //   {
    //     var delta = DeltaXY.FromPoints(
    //       Points[^2],
    //       Points[^1]
    //     ) ;
    //     return Points[^1].MovedBy(delta) ;
    //   }
    // }

    public EdgeLayoutDescriptor ( 
      string                      sourceNodeId,
      string                      targetNodeId,
      List<System.Drawing.PointF> points
    ) {
      SourceNodeId = sourceNodeId ;
      TargetNodeId = targetNodeId ;
      Points = points.ToList() ;
    }

    // public EdgeLayoutDescriptor MovedBy ( 
    //   System.Drawing.SizeF delta    
    // ) {
    //   return new EdgeLayoutDescriptor(
    //     SourceNodeId,
    //     TargetNodeId,
    //     Points.Select(
    //       point => point + delta
    //     ).ToList()
    //   ) ;
    // }

    // ??? This prevents immutability ???

    public void MovePointsBy ( 
      System.Numerics.Vector2 delta    
    ) {
      Points = Points.Select(
        point => new System.Drawing.PointF(
          point.X + delta.X,
          point.Y + delta.Y
        )
      ).ToList() ;
    }

    public override string ToString ( ) => $"{SourceNodeId} -> {TargetNodeId}" ;

  }

}
