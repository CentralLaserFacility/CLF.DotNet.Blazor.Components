//
// DirectionVector.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.GeomertyPrimitives;

namespace Clf.LogicSystem.Common
{

  //
  // Represents a direction.
  //
  // The Y axis increases DOWNWARDS.
  //
  // DeltaX and DeltaY are NORMALISED,
  // such that the pythagorean length 
  // has a magnitude of one.
  //
  // The Delta values can be either positive or negative.
  //

  public sealed class DirectionVector
  {

    public float NormalisedDeltaX { get ; }

    public float NormalisedDeltaY { get ; }

    public DirectionVector ( System.Drawing.PointF from, System.Drawing.PointF to ) :
    this(
      dx : to.X - from.X,
      dy : to.Y - from.Y
    ) {
    }
    
    // Hmm, check this !!

    public DirectionVector ( float dx, float dy )
    {
      float length = (float) System.Math.Sqrt(
        dx * dx
      + dy * dy
      ).Verified(
        value => value > 0.0
      ) ;
      NormalisedDeltaX = dx / length ;
      NormalisedDeltaY = dy / length ;
      // var _ = Length.Verified(
      //   value => value == 1.0
      // ) ;
    }

    // Hmm, should always be 1.0 

    public const float Length = 1.0f ; 

    public float ComputedLength_ExpectedToBeOne => System.MathF.Sqrt(
      NormalisedDeltaX * NormalisedDeltaX
    + NormalisedDeltaY * NormalisedDeltaY
    ) ;

    // We normally (??) think of angles and rotations as being ANTI-CLOCKWISE

    //
    //    |
    //    |   * Rotated anticlockwise
    //    |  /
    //    | /   
    //    |/ theta
    //  --+--------*-----
    //             |
    //             Originally along X axis
    //

    public float AngleInAnticlockwiseRadians => System.MathF.Atan2(NormalisedDeltaY,NormalisedDeltaX) ;
    public float AngleInClockwiseRadians     => -AngleInAnticlockwiseRadians ;

    public float AngleInClockwiseDegrees     => -AngleInAnticlockwiseDegrees; 
    public float AngleInAnticlockwiseDegrees => AngleInAnticlockwiseRadians * 180.0f / System.MathF.PI ; 

    // Hmm, better to write this as a method rather than a property
    // because that causes issues with writing as JSON ...

    public DirectionVector Reversed ( ) => new DirectionVector(
      -NormalisedDeltaX,
      -NormalisedDeltaY
    ) ;

    public DirectionVector RotatedAnticlockwise ( float thetaInDegrees )
    {
      // https://matthew-brett.github.io/teaching/rotation_2d.html
      float anticlockwiseAngleInRadians_beta = 2.0f * System.MathF.PI * thetaInDegrees / 360.0f ;
      float cosBeta = System.MathF.Cos(anticlockwiseAngleInRadians_beta) ;
      float sinBeta = System.MathF.Sin(anticlockwiseAngleInRadians_beta) ;
      float x1 = NormalisedDeltaX ;
      float y1 = NormalisedDeltaY ;
      float x2 =  cosBeta * x1 - sinBeta * y1 ;
      float y2 =  sinBeta * x1 + cosBeta * y1 ;
      return new DirectionVector(
        (float) x2,
        (float) y2
      ) ;
    }

    public DirectionVector RotatedClockwise ( float thetaInDegrees )
    => RotatedAnticlockwise(-thetaInDegrees) ;

    //
    // When we multiply Direction Vector by a 'length', we get a DeltaXY.
    //
    // It wouldn't make sense to get a result type of 'DirectionVector',
    // since the length of a DirectionVector is always 1.0, by definition.
    //

    public static DeltaXY operator * ( DirectionVector directionVector, float length )
    => new DeltaXY(
      directionVector.NormalisedDeltaX * length,
      directionVector.NormalisedDeltaY * length
    ) ;

    public static DeltaXY operator * ( float length, DirectionVector directionVector )
    => directionVector * length ;

    public static DirectionVector operator - ( DirectionVector directionVector )
    => directionVector.RotatedClockwise(180.0f) ;

  }

}

