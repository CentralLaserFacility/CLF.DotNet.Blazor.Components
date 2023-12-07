//
// DotTextAndPlainText.cs
//

namespace Clf.GraphViz
{

  //
  // Representation of 'dot' information passed to GraphViz,
  // and the resulting 'plain' text that describes the computed layout.
  //

  public record DotTextAndPlainText ( string DotText, string PlainText ) ;

}
