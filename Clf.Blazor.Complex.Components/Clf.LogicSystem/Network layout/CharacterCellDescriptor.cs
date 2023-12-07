//
// CharacterCellDescriptor.cs
//

namespace Clf.LogicSystem.Common
{

  // 
  // When we draw a character (or a line of text) the 'reference point'
  // generally corresponds to a position on the character bounding-box
  // that isn't exactly at the bottom, but a little bit above - so that
  // we can accommodate 'descenders'.
  //
  //   +---------+
  //   |         |
  //   |         |
  //   |         |
  //   |         |
  //   |*        |
  //   |         |
  //   +---------+
  //

  //
  // The size of a font is traditionally specified via the height of a box
  // that will comfortably contain an upper-case 'M'.
  //
  // The box includes space above, below and to the right of the 'M'.
  //
  // The height is in 'points' where each point represents 1/96th of an inch.
  // 
  //   +-------------------+
  //   |                   | <-- space above
  //   |    #       #      |
  //   |   # #     # #     |
  //   |  #   #   #   #    |
  //   | #     # #     #   |
  //   +#       #       #  +---- baseline
  //   |                   |
  //   |                   | <-- space below for the 'descender'
  //   +-------------------+
  //
  // Typically a font size of '1-em' is 16 pixels high.
  //

  public record CharacterCellDescriptor ( 
    float CharacterHeightInPixels,                                         
    float CharacterWidthAsFractionOfHeight               = CharacterCellDescriptor.WidthAsFractionOfHeight_Default,
    float FractionalPositionOfTextBaseLineFromCellBottom = CharacterCellDescriptor.FractionalPositionOfTextBaseLineFromCellBottom_Default
  ) {

    public static CharacterCellDescriptor Instance = new CharacterCellDescriptor() ;

    public const float CharacterHeightInPixels_Default                        = 10.0f ;
    public const float WidthAsFractionOfHeight_Default                        =  0.6f ;
    public const float FractionalPositionOfTextBaseLineFromCellBottom_Default =  0.1f ;

    public float FractionalPositionOfTextBaseLineFromCellTop => ( 1.0f - FractionalPositionOfTextBaseLineFromCellBottom ) ;

    public float OffsetOfTextBaseLineFromCellTop => CharacterHeightInPixels * FractionalPositionOfTextBaseLineFromCellTop ;

    private static readonly CharacterCellDescriptor Default = new(CharacterHeightInPixels_Default) ;

    public float SizeInEms => CharacterHeightInPixels / 16.0f ;

    public float CharacterWidthInPixels => CharacterHeightInPixels * CharacterWidthAsFractionOfHeight ;

    public System.Drawing.SizeF SizeInPixels => new System.Drawing.SizeF(CharacterWidthInPixels,CharacterHeightInPixels) ;

    public float OffsetInPixelsFromCellTopToTextBaseline => CharacterHeightInPixels * ( 1.0f - FractionalPositionOfTextBaseLineFromCellBottom ) ;

    System.Drawing.PointF TextAnchorPointRelativeToCellTopLeftPoint ( System.Drawing.PointF cellTopLeft )
    => new System.Drawing.PointF(
      cellTopLeft.X,
      cellTopLeft.Y + OffsetInPixelsFromCellTopToTextBaseline
    ) ;

    // Required for JSON persistence ???

    public CharacterCellDescriptor ( ) : 
    this(
      CharacterCellDescriptor.CharacterHeightInPixels_Default
    ) {
    }

  }

}
