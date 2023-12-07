//
// GraphVizRunner_UsingInstalledExe.cs
//

using Clf.Common.ExtensionMethods ;

namespace Clf.GraphViz
{

  public class GraphVizRunner_UsingInstalledExe : IGraphVizRunner
  {

    //
    // Would be neater to use stdin/stdout ???
    // https://github.com/timothy-shields/graphviz/blob/master/Shields.GraphViz/Components/Renderer.cs
    //

    // Graphviz Command Line
    // https://graphviz.org/doc/info/command.html

    private const string PathToDotExe = @".\external\dot.exe" ;

    //
    // The SVG file is useful for debugging, 
    // as we can open it in a browser and see the layout.
    //

    public void RunGraphViz_GeneratingPlainAndSvgFilesFromDotFile ( string pathToDotFile, string pathToPlainFile, string pathToSvgFile )
    {

      System.Diagnostics.Process process = new System.Diagnostics.Process() ;

      process.StartInfo.FileName = PathToDotExe ;
      // https://www.graphviz.org/doc/info/command.html
      process.StartInfo.Arguments = (
        $"-Tplain -o {pathToPlainFile.EnclosedInDoubleQuotes()} "
      + $"-Tsvg -o {pathToSvgFile.EnclosedInDoubleQuotes()} "
      + $"{pathToDotFile.EnclosedInDoubleQuotes()}" 
      ) ;
      process.StartInfo.UseShellExecute        = false ;
      process.StartInfo.CreateNoWindow         = true ;
      process.StartInfo.RedirectStandardOutput = true ;
      process.StartInfo.RedirectStandardInput  = true ;
      process.StartInfo.RedirectStandardError  = true ;
      
      var startTime = System.DateTime.Now ;
      process.Start() ;
      process.WaitForExit() ;

      var finishTime = System.DateTime.Now ;
      var timeInSecs = ( finishTime - startTime ).TotalSeconds ;
      // System.Console.WriteLine(
      //   $"Creation of PLAIN and SVG files took {timeInSecs:F2} secs"
      // ) ;

      string stdout = process.StandardOutput.ReadToEnd() ;
      string stderr = process.StandardError.ReadToEnd() ;
    }

    public void RunGraphViz_GeneratingSvgFileFromDotFile ( string pathToDotFile, string pathToSvgFile )
    {

      System.Diagnostics.Process process = new System.Diagnostics.Process() ;

      process.StartInfo.FileName = PathToDotExe ;
      // https://www.graphviz.org/doc/info/command.html
      process.StartInfo.Arguments = (
        $"-Tsvg -o {pathToSvgFile.EnclosedInDoubleQuotes()} "
      + $"{pathToDotFile.EnclosedInDoubleQuotes()}" 
      ) ;
      process.StartInfo.UseShellExecute        = false ;
      process.StartInfo.CreateNoWindow         = true ;
      process.StartInfo.RedirectStandardOutput = true ;
      process.StartInfo.RedirectStandardInput  = true ;
      process.StartInfo.RedirectStandardError  = true ;
      
      var startTime = System.DateTime.Now ;
      process.Start() ;
      process.WaitForExit() ;

      var finishTime = System.DateTime.Now ;
      var timeInSecs = ( finishTime - startTime ).TotalSeconds ;
      // System.Console.WriteLine(
      //   $"Creation of SVG file took {timeInSecs:F2} secs"
      // ) ;

      string stdout = process.StandardOutput.ReadToEnd() ;
      string stderr = process.StandardError.ReadToEnd() ;
    }

    public string RunGraphViz_GeneratingPlainTextFromDotText ( 
      string dotText
    ) {
      string dotFileSpec   = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath("GraphVizTmpFile.dot") ;
      string plainFileSpec = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath("GraphVizTmpFile.plain") ;
      string svgFileSpec   = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath("GraphVizTmpFile.svg") ;
      Clf.Common.Helpers.WriteStringToFile(
        dotText,
        dotFileSpec
      ) ;
      RunGraphViz_GeneratingPlainAndSvgFilesFromDotFile(
        pathToDotFile   : dotFileSpec,
        pathToPlainFile : plainFileSpec,
        pathToSvgFile   : svgFileSpec
      ) ;
      string plainTextCreated = Clf.Common.Helpers.ReadAllTextFromFile(plainFileSpec) ;
      return plainTextCreated.Trim() ;
    }

    public string RunGraphViz_GeneratingSvgTextFromDotText ( 
      string dotText
    ) {
      string dotFileSpec   = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath("GraphVizTmpFile.dot") ;
      string plainFileSpec = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath("GraphVizTmpFile.plain") ;
      string svgFileSpec   = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath("GraphVizTmpFile.svg") ;
      Clf.Common.Helpers.WriteStringToFile(
        dotText,
        dotFileSpec
      ) ;
      RunGraphViz_GeneratingSvgFileFromDotFile(
        pathToDotFile : dotFileSpec,
        pathToSvgFile : svgFileSpec
      ) ;
      string svgTextCreated = Clf.Common.Helpers.ReadAllTextFromFile(svgFileSpec) ;
      // Remove the initial lines that are of no interest
      int iSvg = svgTextCreated.IndexOf("<svg") ;
      svgTextCreated = svgTextCreated.Substring(iSvg) ;
      return svgTextCreated.Trim() ;
    }

    public void RunGraphViz_GeneratingSvgFileFromDotText ( 
      string dotText,
      string svgFileSpec
    ) {
      string dotFileSpec = Clf.Common.Helpers.BuildFileSpecificationBasedOnTempFileRootPath("GraphVizTmpFile.dot") ;
      Clf.Common.Helpers.WriteStringToFile(
        dotText,
        dotFileSpec
      ) ;
      RunGraphViz_GeneratingSvgFileFromDotFile(
        pathToDotFile : dotFileSpec,
        pathToSvgFile : svgFileSpec
      ) ;
    }

  }

}
