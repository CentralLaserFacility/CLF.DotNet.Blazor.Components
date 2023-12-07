//
// Geometry_ExtensionMethods.cs
//

namespace Clf.LogicSystem.Common.GeomertyPrimitives
{

  public static class Geometry_ExtensionMethods
  {

    //
    // Helper functions encapsulating common operations on System.Drawing.RectangleF.
    //

    public static System.Drawing.PointF TopLeftPoint ( this System.Drawing.RectangleF rectangle ) 
    => (
      rectangle.Location 
    ) ;

    public static System.Drawing.PointF BottomRightPoint ( this System.Drawing.RectangleF rectangle ) 
    => (
      new System.Drawing.PointF(
        rectangle.Location.X + rectangle.Width,
        rectangle.Location.Y + rectangle.Height
      ) 
    ) ;

    public static System.Drawing.PointF CentrePoint ( this System.Drawing.RectangleF rectangle )
    => (
      new System.Drawing.PointF(
        rectangle.Location.X + rectangle.Width / 2 ,
        rectangle.Location.Y + rectangle.Height / 2
      )
    ) ;

    public static void ExpandToAccommodate ( 
      this ref System.Drawing.RectangleF? rectangle, 
      System.Drawing.RectangleF           other 
    ) {
      rectangle = (
        rectangle.HasValue
        ? System.Drawing.RectangleF.Union(
            rectangle.Value,
            other
          ) 
        : other
      ) ;
    }

    public static System.Drawing.RectangleF ExpandedToMinimumSize ( 
      this System.Drawing.RectangleF rectangle, 
      System.Drawing.SizeF           minimumSize 
    ) {
      return new System.Drawing.RectangleF(
        rectangle.X,
        rectangle.Y,
        System.Math.Max(
          rectangle.Width,
          minimumSize.Width
        ),
        System.Math.Max(
          rectangle.Height,
          minimumSize.Height
        )
      ) ;
    }

    public static System.Drawing.RectangleF ExpandedToAccommodate ( 
      this System.Drawing.RectangleF? rectangle, 
      System.Drawing.RectangleF       other 
    ) => (
      rectangle.HasValue
      ? System.Drawing.RectangleF.Union(
          rectangle.Value,
          other
        ) 
      : other
    ) ;

    // The top left and bottom right points are both adjusted
    // by 'delta' ; the centre point stays in the same place, 
    // and the size increases by TWICE the specified delta.

    public static System.Drawing.RectangleF ExpandedAllRoundBy ( 
      this System.Drawing.RectangleF rectangle, 
      System.Drawing.SizeF           delta 
    ) {
      // Hmm, this seems to be the best way to create 
      // a clone that we can 'inflate' without affecting
      // the source rectangle ...
      var clone = new System.Drawing.RectangleF(
        rectangle.ToVector4()
      ) ;
      clone.Inflate(delta) ;
      return clone ;
    }

    public static bool LiesCompletelyInside ( 
      this System.Drawing.RectangleF rectangle, 
      System.Drawing.RectangleF      parent,       
      float                          tolerance
    ) {
      // return parent.Contains(rectangle) ;
      return LiesCompletelyInside(
        rectangle, 
        parent,
        tolerance,
        out bool leftX_isOutsideOfParent,
        out bool rightX_isOutsideOfParent,
        out bool topY_isOutsideOfParent,
        out bool bottomY_isOutsideOfParent
      ) ;
    }

    public static bool LiesCompletelyInside ( 
      this System.Drawing.RectangleF rectangle, 
      System.Drawing.RectangleF      parent,
      float                          tolerance,
      out bool                       leftX_isOutsideOfParent,
      out bool                       rightX_isOutsideOfParent,
      out bool                       topY_isOutsideOfParent,
      out bool                       bottomY_isOutsideOfParent
    ) {
      leftX_isOutsideOfParent   = rectangle.Left   < ( parent.Left   - tolerance ) ;
      rightX_isOutsideOfParent  = rectangle.Right  > ( parent.Right  + tolerance ) ;
      topY_isOutsideOfParent    = rectangle.Top    < ( parent.Top    - tolerance ) ;
      bottomY_isOutsideOfParent = rectangle.Bottom > ( parent.Bottom + tolerance ) ;
      bool notInside = (
         leftX_isOutsideOfParent
      || rightX_isOutsideOfParent
      || topY_isOutsideOfParent
      || bottomY_isOutsideOfParent
      ) ;
      bool inside = ! notInside ;
      // bool nominalResult = parent.Contains(rectangle) ;
      // nominalResult.Should().Be(inside) ;
      return inside ;
    }

    //
    // Helper functions encapsulating common operations on System.Drawing.PointF.
    //

    public static System.Drawing.PointF MovedBy ( this System.Drawing.PointF point, DeltaXY delta )
    => (
      new System.Drawing.PointF(
        point.X + delta.DeltaX,
        point.Y + delta.DeltaY
      )
    ) ;

  }

}

