//
// PixelMappingProfileGraph.razor.cs
//

using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Common.Graphs;
using Clf.Common.ImageProcessing;
using Microsoft.AspNetCore.Components;

namespace Clf.Blazor.ExamplesServerApp.Pages
{

  public partial class PixelMappingProfileGraph
  {

    [Parameter]
    public int HowManyGridLines_X { get ; set ; } = 11 ;

    [Parameter]
    public int HowManyGridLines_Y { get ; set ; } = 6 ;

    public IEnumerable<double> XDataTickCoordinates => XDataRange.TickCoordinates(HowManyGridLines_X) ;

    public IEnumerable<double> YDataTickCoordinates => YDataRange.TickCoordinates(HowManyGridLines_Y) ;

    [Parameter]
    public string Title { get ; set ; } = "Pixel mapping profile" ; 

    [Parameter]
    public bool ShowGrid { get ; set ; } = false ;
    
    [Parameter]
    public IEnumerable<double> XValues { get ; set ; } = new double[]{0,1,2,3,4,5}.ToList() ;

    [Parameter]
    public IEnumerable<double> YValues { get ; set ; } = new double[]{0,1,2,3,4,5}.ToList() ;

    // Defines a string in the following format ...
    // <polygon points="5,5 195,10 185,185 10,195" />

    private string? m_graphPointsString = "" ;
    public string GraphPointsString
    {
      get { 
        if ( m_graphPointsString == null )
        {
          UpdateGraphPoints() ;
        }
        return m_graphPointsString! ; 
      }
      set { m_graphPointsString = value ; }
    }

    // Hmm, dodgy way to ensure that the visuals get updated,
    // but it'll serve the purpose for the time being ...

    public void InvalidateGraphPoints ( ) 
    {
      m_graphPointsString = null ;
      m_xDataRange = null ;
      m_yDataRange = null ;
    }

    // Hmm, we could consider returning rounded integer values ???

    public string GetDataPositionInViewCoordinates_X ( double dataValue_X )
    => m_valueToCoordinateMapper_X[dataValue_X].ToString("F0") ;

    public string GetDataPositionInViewCoordinates_Y ( double dataValue_Y )
    => m_valueToCoordinateMapper_Y[dataValue_Y].ToString("F0") ;

    public GraphType GraphType { get ; private set ; }= GraphType.Line ;
     
    public Colour TraceColor { get ; set ; } = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black) ;

    public int TraceWidth { get ; set ; } = 1 ;

    private GraphAxisRange GetGraphAxisRangeFromDataValues ( IEnumerable<double> values )
    {
      return new(
        values.Min(),
        values.Max()
      ) ;
    }

    private GraphAxisRange? m_xDataRange ;
    public GraphAxisRange XDataRange
    {
      get { return m_xDataRange ?? GetGraphAxisRangeFromDataValues(XValues) ; }
      set { m_xDataRange = value ; }
    }

    private GraphAxisRange? m_yDataRange ;
    public GraphAxisRange YDataRange
    {
      get { return m_yDataRange ?? GetGraphAxisRangeFromDataValues(YValues) ; }
      set { m_yDataRange = value ; }
    }

    private GraphDataCoordinateMapper m_valueToCoordinateMapper_X ;
    private GraphDataCoordinateMapper m_valueToCoordinateMapper_Y ;

    [Parameter]
    public int Width  { get ; set ; } = 300 ;

    [Parameter]
    public int Height { get ; set ; } = 100 ;

    [Parameter]
    public double[] ReferencePointsAlongX { get ; set ; }

    public string? TooltipText = "Tooltip" ;
   
    public bool ShowTooltip => TooltipText != null ;

    //
    // The constructor for a component is best left as empty,
    // as Properties will be set up when the component is
    // instantiated in the razor file.
    //
    // Initialisation that depends on how the Properties have been configured
    // can then be performed in the 'Initialise' method.
    //
    // Unfortunately this means we'll get warnings telling us that
    // certain properties haven't been initialised.
    //

    public PixelMappingProfileGraph ( )
    {
    }

    public void SetDataValues_X ( List<double> values )
    {
      XValues = values ;
      UpdateGraphPoints() ;
    }

    public void SetDataValues_Y ( List<double> values )
    {
      YValues = values ;
      UpdateGraphPoints() ;
    }

    private void UpdateGraphPoints ( )
    {
      if (
        XValues.Count() != YValues.Count()
      ) {
        GraphPointsString = "" ;
      }
      else
      {
        //
        // Build the 'points' string that will be supplied to the SVG 'polyline'
        // (for a line graph) or 'polygon' (for an 'area' graph).
        //
        // Hmm, nasty business which is horrible slow, but that's SVG for you.
        //
        // One possible optimisation would be to precompute the strings that represent the
        // mapped X values, instead of computing them afresh every time ???
        //
        // Another possibility might be to round the values to integers,
        // which can be converted to strings with less overhead ??
        //
        var graphPointsList = XValues.Zip(
          YValues,
          (x,y) => new GraphPointEx(x,y)
        ).ToArray() ;
        System.Text.StringBuilder pointsStringBuilder = new() ;
        foreach ( GraphPointEx graphPoint in graphPointsList )
        {
          pointsStringBuilder.Append(
            $"{GetDataPositionInViewCoordinates_X(graphPoint.X)},{GetDataPositionInViewCoordinates_Y(graphPoint.Y)} " 
          ) ;
        }
        if ( 
           GraphType == GraphType.Area
        && graphPointsList.Any()
        ) {
          // For an 'area' graph, make sure that the first and last
          // points go down to the 'minimum' Y value, so that the
          // entire area is filled.
          //
          //     1-2       To display a 'line' graph, we just need
          //    /   \      the points corresponding to the data values
          //   0     3
          //
          //     2-3
          //    /   \      To display an 'area' graph, we need to add
          //   1     4     additional points at the beginning and the end
          //   |     |     so that we'll get a 'closed' polygon
          //   0     5
          //  
          //     1-2
          //    /   \      To display an 'area' graph, we need to add
          //   0     3     additional points at the beginning and the end
          //   |     |     but they can both be added to the END of the list
          //   5     4     which is better because inserting at 0 is slow ...
          //  
          pointsStringBuilder.Append(
            $"{GetDataPositionInViewCoordinates_X(graphPointsList[^1].X)},{GetDataPositionInViewCoordinates_Y(YDataRange.Min)} " 
          ) ;
          pointsStringBuilder.Append(
            $"{GetDataPositionInViewCoordinates_X(graphPointsList[0].X)},{GetDataPositionInViewCoordinates_Y(YDataRange.Min)} "
          ) ;
        }
        GraphPointsString = pointsStringBuilder.ToString() ;
      }

    }

    protected override void OnInitialized ( )
    {
      base.OnInitialized() ;
      m_valueToCoordinateMapper_X = new(
        FromDataRange  : XDataRange,
        ToViewBoxRange : new GraphAxisRange(0,Width-1)
      ) ;
      m_valueToCoordinateMapper_Y = new(
        FromDataRange  : YDataRange,
        ToViewBoxRange : new GraphAxisRange(Height-1,0)
      ) ;
      UpdateGraphPoints() ;
    }

  }

}