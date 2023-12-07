//
// PlainTextParser.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem.Common
{

  //
  // This variant uses 'Split()' to segment
  // the source text into a sequence of lines,
  // and then decode each line as a sequence of fields.
  //
  // It's much faster than the 'original' version.
  //

  //
  // Typical syntax of a '.plain' file :
  //
  //   graph 1.000 92.819 17.958
  //   node "AA"  33.389 17.333 5.557 1.237 AudioController filled ellipse black NavajoWhite
  //   node "BB"  89.528 15.069 4.471 1.277 SkinControl     filled ellipse black PaleGreen
  //   edge "AA" "BB" 3 82.042 16.431 87.097 15.708 87.347 15.667 solid black
  //   stop
  //
  // https://graphviz.org/docs/outputs/plain/
  //
  // https://graphviz.org/doc/info/output.html#d:plain
  //

  //
  // The 'plain' coordinate origin is at the bottom left, 
  // whereas in WPF it is at top left left.
  //
  // So we adjust the Y coordinate so that it increases in a downward direction
  // to match the WPF visual coordinate system, with [0,0] at the top left.
  //
  // Then, we apply an offset to shift the point down and to the right,
  // by an amount equivalent to the desired 'margin' that we want to have
  // surrounding the diagram.
  //

  public sealed class PlainTextParser 
  {

    public static NetworkLayoutDescriptor ParsePlainTextFile ( 
      string                                          source,
      Clf.LogicSystem.Common.LayoutSizingParameters layoutSizingParameters 
    ) {
      using Clf.Common.Utils.Stopwatch timer = new(
        "PlainTextParser.ParsePlainTextFile",
        (elapsedMillseconds) => System.Diagnostics.Debug.WriteLine(
          $"PlainTextParser.ParsePlainTextFile took {elapsedMillseconds} mS"
        ) 
      ) ;
      var networkLayoutDescriptor = new NetworkLayoutDescriptor_CreatedFromParsingGraphVizPlainFile(
        layoutSizingParameters
      ) ;
      //
      // Split the source into a sequence of lines
      // For each line :
      // - Split into fields delimited by spaces
      // - The first field identifies the role of the line
      // - Decode the remaining fields into an appropriately typed record
      //
      string[] lines = source.Split(
        "\r\n",
        System.StringSplitOptions.RemoveEmptyEntries
      ) ;
      string[] fields ;
      float heightY = 0.0f ;
      foreach ( string line in lines ) 
      {
        fields = line.Split(' ',System.StringSplitOptions.RemoveEmptyEntries) ;
        switch ( fields[0] )
        {
        case "graph":
          GraphDescriptor graph = CreateGraph() ;
          heightY = graph.HeightY ;
          break ;
        case "node":
          var nodeLayoutDescriptor = CreateNode() ;
          if ( int.TryParse(nodeLayoutDescriptor.NodeId, out _ ) )
          {
            networkLayoutDescriptor.AddNode(
              nodeLayoutDescriptor
            ) ;
          }
          break ;
        case "edge":
          var edgeLayoutDescriptor = CreateEdge() ;
          if ( 
             int.TryParse( edgeLayoutDescriptor.SourceNodeId, out _ )
          && int.TryParse( edgeLayoutDescriptor.TargetNodeId, out _ )
          ) {
            networkLayoutDescriptor.AddEdge(
              edgeLayoutDescriptor
            ) ;
          }
          break ;
        case "stop":
          break ;
        default:
          break ;
        }
      }
      networkLayoutDescriptor.NormalisePositionsAndApplyMargins() ;
      return networkLayoutDescriptor ;
      GraphDescriptor CreateGraph ( )
      {
        //
        // Read fields from a 'graph' line.
        //
        //   graph 1.000 92.819 17.958
        //         |     |      |
        //         |     |      height
        //         |     width
        //         scale
        //
        return new GraphDescriptor() {
          // Note that these 'property initialiser' statements
          // are indeed executed in the order specified here ...
          ScaleFactor = ParseFloat(1),
          WidthX      = ParseFloat(2),
          HeightY     = ParseFloat(3)
        } ;
      }
      NodeLayoutDescriptor CreateNode ( )
      {
        //
        // Read fields from a 'node' line.
        //
        // Note that 'x' and 'y' refer to the 'position' of the node, 
        // ie its centre point - not the 'top left' point ...
        //
        //                 x      y      width height
        //                 |      |      |     |
        //  node "node-id" 89.528 15.069 4.471 1.277 "label-text" filled ellipse black PaleGreen
        //  |    |         |      |      |     |     
        //  0    1         2      3      4     5
        //
        var nodeId         = ParseString(1) ;
        var centrePosition = ParsePoint_AdjustingY(2) ;
        var widthX         = ParseFloat(4) ;
        var heightY        = ParseFloat(5) ;
        return new Clf.LogicSystem.Common.NodeLayoutDescriptor( 
          nodeId,
          centrePosition,
          new System.Drawing.SizeF(
            widthX,
            heightY
          )
        ) ;
      }
      EdgeLayoutDescriptor CreateEdge ( )
      {
        //
        // Read fields from an 'edge' line.
        //
        //      source node ID
        //      |
        //      |    target node ID                                   style
        //      |    |                                                |
        //      |    |    nPoints                                     |     colour
        //      |    |    |                                           |     |
        // edge "AA" "BB" 3 82.042 16.431 87.097 15.708 87.347 15.667 solid black
        // |    |    |    | |      |      |      |      |      |    
        // 0    1    2    3 4      5      6      7      8      9
        //
        //                  |           | |           | |           |
        //                  +-----------+ +-----------+ +-----------+ 
        //                   start point         other points
        //
        // There are always at least 3 points.
        //
        // The points defined for an Edge don't include the centre points of the
        // source and target nodes. The first and last points coincide with a point
        // on the rectangle pertaining to the Source and Target nodes respectively.
        //
        // If the 'dot' file has specified 'arrowhead=normal', GraphViz will
        // assign the 'last' point to fall short by a small amount. This doesn't
        // happen though if you specify 'arrowhead=none'.
        //
        var sourceNodeId = ParseString(1) ;
        var targetNodeId = ParseString(2) ;
        int nPoints      = ParseInt(3) ;
        var points_modelUnits = new List<System.Drawing.PointF>(nPoints) ;
        int iPointField = 4 ;
        for ( int iPoint = 0 ; iPoint < nPoints ; iPoint++ )
        {
          points_modelUnits.Add(
            ParsePoint_AdjustingY(iPointField)
          ) ;
          iPointField += 2 ;
        }
        return new Clf.LogicSystem.Common.EdgeLayoutDescriptor(
          sourceNodeId,
          targetNodeId,
          points_modelUnits
        ) ;
      }
      string ParseString ( int iField )
      {
        return fields[iField].Trim('"') ;
      }
      float ParseFloat ( int iField )
      {
        return float.Parse(
          fields[iField]
        ) ;
      }
      int ParseInt ( int iField )
      {
        return int.Parse(
          fields[iField]
        ) ;
      }
      System.Drawing.PointF ParsePoint_AdjustingY ( int iField )
      {
        return new System.Drawing.PointF(
          x : ParseFloat(iField),
          y : heightY - ParseFloat(iField+1)
        ) ;
      }
    }

  }

}
