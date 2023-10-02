//
// IntensityProfileGraphViewModel.cs
//

using System.Collections.Generic;
using System.Linq;

using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Blazor.Basic.Components.Controls.ViewModels;
using Clf.Common.ImageProcessing;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Common.Graphs;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  //
  // Here we're just defining the 'core' area that displays the graph points.
  //
  // Plan for a version that draws Axes and so on ...
  //
  // To properly draw the axes, we'll assign some 'border' areas as follows :
  //
  //
  //     +-------+-------------------------------------------+---+
  //     |                          A                            |
  //     +-------+-------------------------------------------+---+
  //     |       |                                           |   |
  //     |       |                                           |   |
  //     |   L   |                  G                        | R |
  //     |       |                                           |   |
  //     |       |                                           |   |
  //     |       |                                           |   |
  //     +-------+-------------------------------------------+---+
  //     |                                                       |
  //     |                          B                            |
  //     |                                                       |
  //     +-------+-------------------------------------------+---+
  //
  //   A  Above : Overall title
  //   L  Left  : Y axis title, labels and tick marks
  //   G  Graph : Plotted points
  //   R  Right : Right border
  //   B  Below : Y axis title, labels and tick marks
  //
  // The 'Below' area will itself be divided into strips ...
  //
  //    +--------------------------------------------------+
  //    |                Tick marks                        |
  //    +--------------------------------------------------+
  //    |                Labels                            |
  //    +--------------------------------------------------+
  //    |                Axis title                        |
  //    +--------------------------------------------------+
  //
  // Likewise for the 'Left' area that shows the Y axis info.
  //
  // These areas can be distinct components organised with a Grid.
  // They all map onto the same underlying ViewModel and have access
  // to Mapper instances pertaining to the various areas.
  //

  public class IntensityProfileGraphViewModel : MonitorWidgetViewModelBase
  {

    private GraphType DEFAULT_GRAPH_TYPE  = GraphType.Line ;
    private int       DEFAULT_TRACE_WIDTH = 1 ;
    private Colour    DEFAULT_TRACE_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Blue) ;

    private int DEFAULT_NUMBER_OF_GRID_LINES_X = 11 ;
    private int DEFAULT_NUMBER_OF_GRID_LINES_Y = 6 ;

    // Properties pertaining the 'X' data set
    // No longer needed ...

    // public double XDataMinimum     => DataRange_X.Min ;
    // 
    // public double XDataMaximum     => DataRange_X.Max ;
    // 
    // public double XDataSpan        => DataRange_X.Span ;
    // 
    // public double XDataMidPoint    => DataRange_X.MidPoint ;

    // public double XDataTickSpacing => DataRange_X.Span / 11 ;

    public IEnumerable<double> XDataTickCoordinates => XDataRange.TickCoordinates(DEFAULT_NUMBER_OF_GRID_LINES_X) ;

    // Properties pertaining the 'Y' data set
    // No longer needed ...

    // public double YDataMinimum     => DataRange_Y.Min ;
    // 
    // public double YDataMaximum     => DataRange_Y.Max ;
    // 
    // public double YDataSpan        => DataRange_Y.Span ;
    // 
    // public double YDataMidPoint    => DataRange_Y.MidPoint ;
    // 
    // public double YDataTickSpacing => DataRange_Y.Span / 11 ;

    public IEnumerable<double> YDataTickCoordinates => YDataRange.TickCoordinates(DEFAULT_NUMBER_OF_GRID_LINES_Y) ;

    // Visual properties

    public bool ShowGrid { get ; set ; } = false ;
    
    //
    // Hmm, we're currently using 'SetProperty' for a few of these only ...
    // BEST WAIT UNTIL THIS BECOMES EASIER WITH v8 of the Toolkit !!
    // In principle, we should initiate a complete repaint whenever
    // any property changes ...
    //
    // Note that the MVVM Tooklit scheme will raise an event for every
    // property change, whereas for Blazor we really only need ONE invocation
    // of 'StateHasChanged' ...
    //

    private BorderStatus m_xborderStatus = BorderStatus.NotConnected ;
    public BorderStatus XBorderStatus
    {
      get { return m_xborderStatus ; }
      set { SetProperty(ref m_xborderStatus, value) ; }
    }

    private BorderStatus m_yborderStatus = BorderStatus.NotConnected ;
    public BorderStatus YBorderStatus
    {
      get { return m_yborderStatus ; }
      set { SetProperty(ref m_yborderStatus, value) ; }
    }
    private List<double>? m_xDataValues ;

    private List<double>? m_yDataValues ;

    // private List<GraphPointEx> m_graphPointsList = new List<GraphPointEx>() ;

    // Defines a string in the following format ...
    // <polygon points="5,5 195,10 185,185 10,195" />

    private string m_graphPointsString = "" ;
    public string GraphPointsString
    {
      get { return m_graphPointsString ; }
      set { SetProperty(ref m_graphPointsString, value) ; }
    }

    // Hmm, we could consider returning rounded integer values ???

    #if true

      // Faster since these will just be converted to string ???

      public string GetDataPositionInViewCoordinates_X ( double dataValue_X )
      => m_mapper_X[dataValue_X].ToString("F0") ;

      public string GetDataPositionInViewCoordinates_Y ( double dataValue_Y )
      => m_mapper_Y[dataValue_Y].ToString("F0") ;

      // public int GetDataPositionInViewCoordinates_X ( double dataValue_X )
      // => (int) m_mapper_X[dataValue_X] ;
      // 
      // public int GetDataPositionInViewCoordinates_Y ( double dataValue_Y )
      // => (int) m_mapper_Y[dataValue_Y] ;

    #else

      public double GetDataPositionInViewCoordinates_X ( double dataValue_X )
      => m_mapper_X[dataValue_X] ;

      public double GetDataPositionInViewCoordinates_Y ( double dataValue_Y )
      => m_mapper_Y[dataValue_Y] ;

    #endif

    public GraphType GraphType { get ; private set ; }

    public Colour TraceColor { get ; }

    public int TraceWidth { get ; }

    public List<string> m_errorMessageLines = new() ;
    public List<string> ErrorMessageLines
    {
      get { return m_errorMessageLines ; }
      set { SetProperty(ref m_errorMessageLines, value) ; }
    }

    private GraphAxisRange m_xDataRange;
    public GraphAxisRange XDataRange
    {
      get { return m_xDataRange; }
      set { SetProperty(ref m_xDataRange, value); }
    }

    private GraphAxisRange m_yDataRange;
    public GraphAxisRange YDataRange
    {
      get { return m_yDataRange; }
      set { SetProperty(ref m_yDataRange, value); }
    }

    public ChannelRecord? XChannelRecord { get ; }

    public ChannelRecord? YChannelRecord { get ; }

    //public GraphAxisRange XViewBoxRange { get ; }

    //public GraphAxisRange YViewBoxRange { get ; }

    private GraphDataCoordinateMapper m_mapper_X ;
    private GraphDataCoordinateMapper m_mapper_Y ;

    public IntensityProfileGraphViewModel (
      int            width,
      int            height,
      GraphAxisRange axisRange_X,
      GraphAxisRange axisRange_Y,
      bool?          showGrid       = false,
      GraphType?     graphType      = null,
      Colour?        traceColor     = null,
      int?           traceWidth     = 1,
      BorderStatus xborderStatus = BorderStatus.NotConnected,
      BorderStatus yborderStatus = BorderStatus.NotConnected,
      ChannelRecord? xChannelRecord = null,
      ChannelRecord? yChannelRecord = null
    ) : 
    base(
      isVisible             : true,
      width                 : width,
      height                : height
    ) {

      this.PropertyChanged += IntensityProfileGraphViewModel_PropertyChanged;
      XDataRange = axisRange_X ;
      YDataRange = axisRange_Y ;

      m_mapper_X = new(
        FromDataRange: XDataRange,
        ToViewBoxRange: new(0,Width!.Value-1)
      ) ;

      m_mapper_Y = new(
        FromDataRange: YDataRange,
        ToViewBoxRange: new(Height!.Value-1,0)
      ) ;

      m_xborderStatus = xborderStatus;
      m_yborderStatus = yborderStatus;

      ShowGrid = showGrid ?? ShowGrid ;

      GraphType  = graphType ?? DEFAULT_GRAPH_TYPE ;

      TraceColor = traceColor ?? DEFAULT_TRACE_COLOR ;
      TraceWidth = traceWidth ?? DEFAULT_TRACE_WIDTH ;

      // ScaleFactorX = Width  / m_axisRange_X.Span ;
      // ScaleFactorY = Height / m_axisRange_Y.Span ; ;

      XChannelRecord = xChannelRecord ;
      XChannelRecord?.InitialiseChannel(
        connectionStatusChangedHandler : (
          isConnected,
          currentState
        ) => { 
          XBorderStatus = (
            isConnected 
            ? BorderStatus.Connected 
            : BorderStatus.NotConnected 
          ) ;
          if ( ! isConnected )
          {
            SetDataValues_X(null) ;
          }
        },
        valueChangedHandler : (
          valueInfo,
          currentState
        ) => {
          OnDataChanged_X(valueInfo,currentState) ;
        }
      ) ;

      YChannelRecord = yChannelRecord ;
      YChannelRecord?.InitialiseChannel(
        connectionStatusChangedHandler : (
          isConnected,
          currentState
        ) => { 
          YBorderStatus = (
            isConnected 
            ? BorderStatus.Connected 
            : BorderStatus.NotConnected 
          ) ; 
          if ( ! isConnected )
          {
            SetDataValues_Y(null) ;
          }
        },
        valueChangedHandler : (
          valueInfo,
          currentState
        ) => {
          OnDataChanged_Y(valueInfo,currentState) ;
        }
      ) ;

      UpdateGraphPoints() ;

      // GraphPointMapper.DoTests() ;

    }

    //Aoun - Move it to individual properties
    private void IntensityProfileGraphViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(Width) || e.PropertyName == nameof(XDataRange))
      {
        m_mapper_X = new(
          FromDataRange: XDataRange,
          ToViewBoxRange: new(0, Width!.Value - 1)
          );
      }
      else if (e.PropertyName == nameof(Height) || e.PropertyName == nameof(YDataRange))
      {
        m_mapper_Y = new(
          FromDataRange: YDataRange,
          ToViewBoxRange: new(Height!.Value - 1, 0)
          );
      }
    }

    private List<double>? GetValueAsListOfDoubles ( ValueInfo valueInfo )
    {
      try
      {
        // Hmm, surely there's a better way ???
        return (
          (System.Collections.IEnumerable) valueInfo.Value
        ).Cast<object>(
        ).Select(
          x => System.Convert.ToDouble(x)
        ).ToList() ;
      }
      catch ( System.Exception x )
      {
        return null ;
      }
    }

    private void OnDataChanged_X ( ValueInfo valueInfo, ChannelState currentState )
    {
      SetDataValues_X(
        GetValueAsListOfDoubles(valueInfo)
      ) ;
      XBorderStatus = (
        currentState.IsConnected 
        ? BorderStatus.Connected 
        : BorderStatus.NotConnected 
      ) ;
    }

    public void SetDataValues_X ( List<double>? values )
    {
      m_xDataValues = values ;
      UpdateGraphPoints() ;
    }

    private void OnDataChanged_Y ( ValueInfo valueInfo, ChannelState currentState )
    {
      SetDataValues_Y(
        GetValueAsListOfDoubles(valueInfo)
      ) ;
      YBorderStatus = (
        currentState.IsConnected 
        ? BorderStatus.Connected 
        : BorderStatus.NotConnected 
      ) ;
    }

    public void SetDataValues_Y ( List<double>? values )
    {
      m_yDataValues = values ;
      UpdateGraphPoints() ;
    }

    private void UpdateGraphPoints ( )
    {
      ErrorMessageLines.Clear() ;

      if ( 
         m_xDataValues == null
      || m_yDataValues == null
      ) {
        if ( m_xDataValues == null )
        {
          ErrorMessageLines.Add("No X-Axis data") ;
        }
        if  ( m_yDataValues == null ) 
        {
          ErrorMessageLines.Add("No Y-Axis data") ;
        }
        // m_graphPointsList.Clear() ;
      }
      else if (
        m_xDataValues.Count != m_yDataValues.Count
      ) {
        ErrorMessageLines.Add(
          m_xDataValues.Count > m_yDataValues.Count 
          ? $"Y-Axis data count ({m_yDataValues.Count}) is less than X-Axis data count ({m_xDataValues.Count})" 
          : $"X-Axis data count ({m_xDataValues.Count}) is less than Y-Axis data count ({m_yDataValues.Count})" 
        ) ;
        // m_graphPointsList.Clear() ;
        GraphPointsString = "" ;
      }
      else
      {
        //
        // for ( int iPoint = 0 ; iPoint < m_xData.Count ; iPoint++ )
        // {
        //   if ( GraphPointsList.ElementAtOrDefault(iPoint) != null )
        //   {
        //     GraphPointsList[iPoint] = new(
        //       m_xData[iPoint],
        //       m_yData[iPoint]
        //     ) ;
        //   }
        //   else
        //   {
        //     GraphPointsList.Add(
        //       new GraphPointEx(
        //         XDataMinimum,
        //         YDataMinimum
        //       )
        //     ) ;
        //   }
        // }
        //
        var graphPointsList = m_xDataValues.Zip(
          m_yDataValues,
          (x,y) => new GraphPointEx(x,y)
        ).ToArray() ;
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
          #if false
            points.Insert(
              0, 
              $"{GetDataPositionInViewCoordinates_X(m_graphPointsList[0].X)},{GetDataPositionInViewCoordinates_Y(DataRange_Y.Min)} "
            ) ;
            points.Append(
              $"{GetDataPositionInViewCoordinates_X(m_graphPointsList[^1].X)},{GetDataPositionInViewCoordinates_Y(DataRange_Y.Min)} " 
            ) ;
          #else
            pointsStringBuilder.Append(
              $"{GetDataPositionInViewCoordinates_X(graphPointsList[^1].X)},{GetDataPositionInViewCoordinates_Y(YDataRange.Min)} " 
            ) ;
            pointsStringBuilder.Append(
              $"{GetDataPositionInViewCoordinates_X(graphPointsList[0].X)},{GetDataPositionInViewCoordinates_Y(YDataRange.Min)} "
            ) ;
          #endif
        }
        GraphPointsString = pointsStringBuilder.ToString() ;
      }

    }

    public string ComputeBorderColourFromPvStatuses ( )
    {
      // ?????
      // We should report the same value if EITHER of the channels is not connected ???
      string borderColour = Colour.Transparent.HtmlRgbaString ;
      if ( YChannelRecord?.Channel != null )
      {
        borderColour = Utilities.GetColourFromBorderStatus(YBorderStatus).HtmlRgbaString ;
      }
      if ( XChannelRecord?.Channel != null )
      {
        borderColour = Utilities.GetColourFromBorderStatus(XBorderStatus).HtmlRgbaString ;
      }
      return borderColour ;
    }

  }

}
