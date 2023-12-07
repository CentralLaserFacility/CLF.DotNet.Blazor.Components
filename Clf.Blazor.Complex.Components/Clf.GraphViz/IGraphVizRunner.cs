//
// IGraphVizRunner.cs
//

namespace Clf.GraphViz
{

  //
  // Sometimes we have the 'dot' info as a string,
  // and sometimes as a file ; and sometimes we want
  // the output as a string, and other times as a file ...
  //

  public interface IGraphVizRunner
  {

    //
    // When we run GraphViz to convert a 'dot' file into 'plain' and 'svg',
    // it's useful to have the data available as files so that they can be
    // viewed in a text editor and in online viewers.
    //

    void RunGraphViz_GeneratingPlainAndSvgFilesFromDotFile ( 
      string pathToDotFile, 
      string pathToPlainFile, 
      string pathToSvgFile 
    ) ;

    void RunGraphViz_GeneratingSvgFileFromDotFile ( 
      string pathToDotFile, 
      string pathToSvgFile 
    ) ;

    void RunGraphViz_GeneratingSvgFileFromDotText ( 
      string dotText, 
      string pathToSvgFile 
    ) ;

    string RunGraphViz_GeneratingPlainTextFromDotText ( 
      string dotText
    ) ;

    string RunGraphViz_GeneratingSvgTextFromDotText ( 
      string dotText
    ) ;

    DotTextAndPlainText RunGraphViz_GeneratingDotTextAndPlainTextFromDotText ( 
      string dotText
    ) => new DotTextAndPlainText(
      DotText   : dotText,
      PlainText : RunGraphViz_GeneratingPlainTextFromDotText(dotText)
    ) ;

    void RunGraphViz_DisplayingSvgFromDotFile ( 
      string pathToDotFile, 
      string pathToSvgFile 
    ) {
      RunGraphViz_GeneratingSvgFileFromDotFile ( 
        pathToDotFile, 
        pathToSvgFile 
      ) ;
      System.Diagnostics.Process.Start(
        new System.Diagnostics.ProcessStartInfo(
          fileName : pathToSvgFile
        ){
          UseShellExecute = true,
          Verb            = "open"
        }
      ) ; 
    }

    void RunGraphViz_DisplayingSvgFromDotText ( 
      string  dotText, 
      string  diagramName,
      string? pathToSvgFile = null
    ) {
      pathToSvgFile ??= Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath(
        $"GraphViz_{diagramName}.svg"
      ) ;
      RunGraphViz_GeneratingSvgFileFromDotText ( 
        dotText, 
        pathToSvgFile 
      ) ;
      System.Diagnostics.Process.Start(
        new System.Diagnostics.ProcessStartInfo(
          fileName : pathToSvgFile
        ){
          UseShellExecute = true,
          Verb            = "open"
        }
      ) ; 
    }

    void RunGraphViz_DisplayingSideBySideSvgsFromDotTexts ( 
      string  dotText_A, 
      string  dotText_B,
      string  title,
      string? descriptionTextLine = null
    ) {
      descriptionTextLine ??= $"Versions A and B : '{title}'" ;
      string svgText_A = RunGraphViz_GeneratingSvgTextFromDotText(dotText_A) ;
      string svgText_B = RunGraphViz_GeneratingSvgTextFromDotText(dotText_B) ;
      string pathToHtmlFile = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath(
       $"SideBySideSvgs_{title}.html"
     ) ;
      string htmlText = (
      $$"""
      <!DOCTYPE html>
      <html>
      <body>
        <table style="width:100%">
          <tr>
            {{descriptionTextLine}}
            <hr>
          </tr>
          <tr>
            <td>
              {{svgText_A}}
            </td>
            <td>
              {{svgText_B}}
            </td>
          </tr>
        </table>
      </body>
      </html>      
      """
      ) ;
      Clf.Common.Helpers.WriteStringToFile(
        htmlText,
        pathToHtmlFile
      ) ;
      System.Diagnostics.Process.Start(
        new System.Diagnostics.ProcessStartInfo(
          fileName : pathToHtmlFile
        ){
          UseShellExecute = true,
          Verb            = "open"
        }
      ) ; 
    }

  }

}
