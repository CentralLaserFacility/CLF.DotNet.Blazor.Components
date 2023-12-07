//
// Colour.cs
//

using Clf.Common.ImageProcessing;

namespace Clf.LogicSystem.Common.GeomertyPrimitives
{

  //
  // This definition overcomes the problem whereby System.Drawing.Color
  // can't be successfully persisted as JSON, because of the
  // presence of a Type Converter ...
  //
  // https://stackoverflow.com/questions/27584530/json-net-serialization-of-system-drawing-color-with-typenamehandling
  //
  // Add further definitions as necessary.
  //

  public record Colour ( uint Argb )
  {

    public static Colour Red         => System.Drawing.Color.Red ;
    public static Colour Green       => System.Drawing.Color.Green ;
    public static Colour Blue        => System.Drawing.Color.Blue ;
    public static Colour Yellow      => System.Drawing.Color.Yellow ;
    public static Colour Orange      => System.Drawing.Color.Orange ;
    public static Colour Cyan        => System.Drawing.Color.Cyan ;

    public static Colour White       => System.Drawing.Color.White ;
    public static Colour LightGrey   => System.Drawing.Color.LightGray ;
    public static Colour DarkGrey    => System.Drawing.Color.DarkGray ;
    public static Colour Black       => System.Drawing.Color.Black ;

    public static Colour LightPink   => System.Drawing.Color.LightPink ;
    public static Colour LightGreen  => System.Drawing.Color.LightGreen ;
    public static Colour LightBlue   => System.Drawing.Color.LightBlue ;
    public static Colour LightYellow => System.Drawing.Color.LightYellow ;

    // public static Colour From ( System.Drawing.Color color ) 
    // => new Colour(
    //   color.ToArgb()
    // ) ;

    public static implicit operator Colour ( System.Drawing.Color color )
    => new Colour(
      (uint) color.ToArgb()
    ) ;

    public static implicit operator System.Drawing.Color ( Colour colour )
    => System.Drawing.Color.FromArgb(
      (int) colour.Argb
    ) ;

    public Colour ( System.Drawing.Color color ) :
    this(
      (uint) color.ToArgb()
    ) {
    }

    // https://www.w3schools.com/cssref/css_colors_legal.asp

    public string AsRgbText ( )
    {
      System.Drawing.Color color = this ;
      return $"rgba({color.R},{color.G},{color.B})" ;
    }

    public static Colour FromName ( string name )
    {
      return System.Drawing.Color.FromName(name) ;
    }

    //
    // Format is '#rrggbb' or "0xrrggbb"
    //

    public static Colour FromHexEncodedString ( string hexEncodedString_RGB )
    {
      return System.Drawing.Color.FromArgb(
        (int) RgbByteValues.FromHexEncodedString(hexEncodedString_RGB).AsPackedInteger_ARGB
      ) ;
    }

    // public string AsNameOrRgbText ( )
    // => this switch {
    // Colour.Red => "red",
    // _ => AsRgbText()
    // } ;

    public static Colour FromNameOrHexEncodedString ( string nameOrHexEncodedString )
    {
      if ( 
         nameOrHexEncodedString.StartsWith("0x")
      || nameOrHexEncodedString.StartsWith("#")
      ) {
        return FromHexEncodedString(nameOrHexEncodedString) ;
      }
      else
      {
        return System.Drawing.Color.FromName(nameOrHexEncodedString) ;
      }
    }

    public string AsRgbaText ( )
    {
      System.Drawing.Color color = this ;
      return $"rgba({color.R},{color.G},{color.B},{color.A/255.0})" ;
    }

    // Required for JSON persistence

    public Colour ( ) :
    this(
      0
    ) {
    }

  }

}

