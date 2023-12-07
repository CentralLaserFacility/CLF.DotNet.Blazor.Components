//
// DirectionVector.cs
//

namespace Clf.LogicSystem.Common.GeomertyPrimitives
{
  //
  // Represents the difference between two points in 2D space.
  //
  // A positive DeltaY represents a DOWNWARD shift.
  //

  public sealed record DeltaXY ( float DeltaX, float DeltaY )
  {

    public DeltaXY ( ) : 
    this(0,0) 
    { } 

    public static readonly DeltaXY Zero = new DeltaXY(0.0f,0.0f) ;

    public DeltaXY ( System.Drawing.PointF from, System.Drawing.PointF to ) :
    this(
      DeltaX : to.X - from.X,
      DeltaY : to.Y - from.Y
    ) {
    }

    public static DeltaXY FromSize ( System.Drawing.SizeF size )
    => new DeltaXY(size.Width,size.Height) ;

    public float Length => (float) System.Math.Sqrt(
      DeltaX * DeltaX
    + DeltaY * DeltaY
    ) ;

    public DeltaXY Inverted ( )
    => new DeltaXY(
      DeltaX : -this.DeltaX,
      DeltaY : -this.DeltaY
    ) ;

    public static DeltaXY operator * ( DeltaXY delta, float factor )
    => new DeltaXY(
      delta.DeltaX * factor,
      delta.DeltaY * factor
    ) ;

    public static DeltaXY operator / ( DeltaXY delta, float factor )
    => new DeltaXY(
      delta.DeltaX / factor,
      delta.DeltaY / factor
    ) ;

    public static DeltaXY operator + ( DeltaXY a, DeltaXY b )
    => new DeltaXY(
      a.DeltaX + b.DeltaX,
      a.DeltaY + b.DeltaY
    ) ;

    public static DeltaXY operator - ( DeltaXY a, DeltaXY b )
    => new DeltaXY(
      a.DeltaX - b.DeltaX,
      a.DeltaY - b.DeltaY
    ) ;

    public static DeltaXY operator - ( DeltaXY delta )
    => new DeltaXY(
      - delta.DeltaX,
      - delta.DeltaY
    ) ;

    public static DeltaXY FromPoints ( System.Drawing.PointF from, System.Drawing.PointF to )
    => new DeltaXY(
      to.X - from.X,
      to.Y - from.Y
    ) ;

  }

}

