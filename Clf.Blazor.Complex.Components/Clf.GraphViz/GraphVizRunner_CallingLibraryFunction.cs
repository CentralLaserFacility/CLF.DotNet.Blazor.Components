//
// GraphVizRunner_CallingLibraryFunction.cs
//

using System ;
using System.Runtime.InteropServices ;

namespace Clf.GraphViz
{

  public class GraphVizRunner_CallingLibraryFunction : IGraphVizRunner 
  {

    public void RunGraphViz_GeneratingPlainAndSvgFilesFromDotFile(
      string pathToDotFile,
      string pathToPlainFile,
      string pathToSvgFile
    ) {
      string dotText = Clf.Common.Helpers.ReadAllTextFromFile(pathToDotFile ) ;
      RunGraphViz_WritingPlainFile(
        dotText,
        pathToPlainFile
      ) ;
      RunGraphViz_WritingSvgFile(
        dotText,
        pathToSvgFile
        ) ;
    }

    public void RunGraphViz_GeneratingSvgFileFromDotFile(
      string pathToDotFile,
      string pathToSvgFile
    ) {
      string dotText = Clf.Common.Helpers.ReadAllTextFromFile(pathToDotFile ) ;
      RunGraphViz_WritingSvgFile(
        dotText,
        pathToSvgFile
      ) ;
    }

    public void RunGraphViz_GeneratingSvgFileFromDotText(
      string dotText,
      string pathToSvgFile
    ) {
      RunGraphViz_WritingSvgFile(
        dotText,
        pathToSvgFile
      ) ;
    }

    public void RunGraphViz_WritingPlainFile(
      string dotText,
      string pathToPlainFile
    ) {
      RenderDotTextToFile(dotText,"plain",pathToPlainFile) ;
    }

    public void RunGraphViz_WritingSvgFile (
      string dotText,
      string pathToSvgFile
    ) {
      RenderDotTextToFile(dotText,"svg",pathToSvgFile) ;
    }

    public string RunGraphViz_GeneratingPlainTextFromDotText (
      string dotText
    ) {
      return GeneratePlainOrSvgTextFromDotText(dotText,"plain") ;
    }

    public string RunGraphViz_GeneratingSvgTextFromDotText (
      string dotText
    ) {
      return GeneratePlainOrSvgTextFromDotText(dotText,"svg") ;
    }

    // ---------- LOW LEVEL FUNCTIONS -----------

    //
    // Invoke the 'dot' processor by invoking functions in a DLL.
    //
    // Using Graphviz in your project to create graphs from dot files
    // https://www.codeproject.com/Articles/1164156/Using-Graphviz-in-your-project-to-create-graphs-fr
    // - Wrapping the Graphviz libraries and performing in-memory data transfers
    //
    // https://graphviz.gitlab.io/_pages/pdf/libguide.pdf
    //

    private void RenderDotTextToFile (
      string dotText,
      string format_plainOrSvg,
      string pathToFile
    ) {
      IntPtr contextHandle = IntPtr.Zero ;
      IntPtr graphHandle   = IntPtr.Zero ;
      try 
      {
        // Create a Graphviz context
        contextHandle = gvContext() ;
        if ( contextHandle == IntPtr.Zero )
          throw new Exception("Failed to create Graphviz context.") ;

        // Load the DOT data into a graph
        graphHandle = agmemread(dotText) ;
        if ( graphHandle == IntPtr.Zero )
          throw new Exception("Failed to create graph from source. Check for syntax errors.") ;

        // Apply a layout
        if ( gvLayout(contextHandle,graphHandle,"dot") != 0 )
          throw new Exception("Layout failed." ) ;

        if ( gvRenderFilename(contextHandle,graphHandle,format_plainOrSvg,pathToFile) != 0 )
          throw new Exception("File render failed." ) ;
      }
      catch ( Exception x ) 
      {
        throw ;
      }
      finally 
      {
        // Free up resources
        if ( 
           contextHandle != IntPtr.Zero 
        && graphHandle   != IntPtr.Zero 
        ) {
          gvFreeLayout(contextHandle,graphHandle) ;
          agclose(graphHandle) ;
          gvFreeContext(contextHandle) ;
        }
      }
    }

    private string GeneratePlainOrSvgTextFromDotText (
      string dotText,
      string format_plainOrSvg
    ) {
      IntPtr contextHandle = IntPtr.Zero ;
      IntPtr graphHandle   = IntPtr.Zero ;
      string result_string = "" ;
      try 
      {
        // Create a Graphviz context
        contextHandle = gvContext() ;
        if ( contextHandle == IntPtr.Zero )
          throw new Exception("Failed to create Graphviz context." ) ;

        // Load the DOT data into a graph
        graphHandle = agmemread(
          dotText
        ) ;
        if ( graphHandle == IntPtr.Zero )
          throw new Exception("Failed to create graph from source. Check for syntax errors.") ;

        // Apply a layout
        if ( gvLayout(contextHandle,graphHandle,"dot") != 0 )
          throw new Exception("Layout failed.") ;

        // Render the graph
        if ( gvRenderData(contextHandle,graphHandle,format_plainOrSvg, out var resultPointer, out var length ) != 0 )
          throw new Exception("Render failed." ) ;

        // Create a byte array to hold the rendered graph
        byte[] bytes = new byte[length] ;

        // Copy the text from the result into a byte array
        Marshal.Copy(resultPointer,bytes,0,length) ;

        // Convert byte array into string
        result_string = System.Text.Encoding.Default.GetString(bytes).Replace("\n","\r\n") ;

        if ( format_plainOrSvg == "svg" )
        {
          // Remove the initial lines that are of no interest
          int iSvg = result_string.IndexOf("<svg") ;
          result_string = result_string.Substring(iSvg) ;
        }
      }
      catch ( Exception x ) 
      {
        throw ;
      }
      finally 
      {
        // Free up resources
        if (
           contextHandle != IntPtr.Zero 
        && graphHandle   != IntPtr.Zero
        ) {
          gvFreeLayout(contextHandle,graphHandle) ;
          agclose(graphHandle) ;
          gvFreeContext(contextHandle) ;
        }
      }
      return result_string ;
    }

    // --------- DLL FUNCTIONS -----------

    private const string LIB_GVC   = @".\external\gvc.dll" ;
    private const string LIB_GRAPH = @".\external\cgraph.dll" ;

    // Create a new Graphviz context.
    [DllImport(LIB_GVC,CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr gvContext( ) ;

    // Release a context's resources.
    [DllImport(LIB_GVC,CallingConvention = CallingConvention.Cdecl)]
    private static extern int gvFreeContext ( IntPtr gvc ) ;

    // Read a graph from a string.
    [DllImport(LIB_GRAPH,CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr agmemread ( string data ) ;

    // Release the resources used by a graph.
    [DllImport(LIB_GRAPH,CallingConvention=CallingConvention.Cdecl)]
    private static extern void agclose ( IntPtr g ) ;

    // Apply a layout to a graph using the given engine.
    [DllImport(LIB_GVC,CallingConvention=CallingConvention.Cdecl)]
    private static extern int gvLayout ( IntPtr gvc, IntPtr g, string engine ) ;

    // Release the resources used by a layout.
    [DllImport(LIB_GVC,CallingConvention=CallingConvention.Cdecl)]
    private static extern int gvFreeLayout ( IntPtr gvc, IntPtr g ) ;

    // Render a graph to a file.
    [DllImport(LIB_GVC,CallingConvention=CallingConvention.Cdecl)]
    private static extern int gvRenderFilename ( IntPtr gvc, IntPtr g, string format, string fileName ) ;

    // Render a graph in memory.
    [DllImport(LIB_GVC,CallingConvention=CallingConvention.Cdecl)]
    private static extern int gvRenderData ( IntPtr gvc, IntPtr g, string format, out IntPtr result, out int length ) ;

    // Release render resources.
    [DllImport(LIB_GVC,CallingConvention=CallingConvention.Cdecl)]
    private static extern int gvFreeRenderData ( IntPtr result ) ;

  }

}
