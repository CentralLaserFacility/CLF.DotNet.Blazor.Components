using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;
using Clf.Common.Graphs;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
  public class GraphViewModel : MonitorWidgetViewModelBase
  {
    private string m_xTitle;
    public string XTitle
    {
      get { return m_xTitle; }
      set { SetProperty(ref m_xTitle, value ?? GraphStyle.DEFAULT_XTitle); }
    }

    private string m_yTitle;
    public string YTitle
    {
      get { return m_yTitle; }
      set { SetProperty(ref m_yTitle, value ?? GraphStyle.DEFAULT_YTitle); }
    }

    private double m_xMinimum;
    public double XMinimum
    {
      get { return m_xMinimum; }
      set { m_xMinimum = value;
        ComputeXLabels();
        UpdatePoints();
      }
    }

    private double m_xMaximum;
    public double XMaximum
    {
      get { return m_xMaximum; }
      set 
      { 
        m_xMaximum = value;
        ComputeXLabels();
        UpdatePoints();
      }
    }

    public double XMidpoint => ((XMaximum - XMinimum) / 2) + XMinimum;

    private double m_xTickSize;
    public double XTickSize
    {
      get { return m_xTickSize; }
      set { m_xTickSize = value; }
    }

    private double m_yMinimum;
    public double YMinimum
    {
      get { return m_yMinimum; }
      set { m_yMinimum = value;
        ComputeYLabels();
        UpdatePoints();
      }
    }

    private double m_yMaximum;
    public double YMaximum
    {
      get { return m_yMaximum; }
      set 
      { 
        m_yMaximum = value;
        ComputeYLabels();
        UpdatePoints();
      }
    }

		public double YMidpoint => ((YMaximum - YMinimum) / 2) + YMinimum;

		private double m_yTickSize;
    public double YTickSize
    {
      get { return m_yTickSize; }
      set { m_yTickSize = value; }
    }

    private bool m_showGrid;
    public bool ShowGrid
    {
      get { return m_showGrid; }
      set { m_showGrid = value; }
    }

    private bool m_autoScale;
    public bool AutoScale
    {
      get { return m_autoScale; }
      set { m_autoScale = value; }
    }

    private BorderStatus m_xborderStatus = BorderStatus.NotConnected;
    public BorderStatus XBorderStatus
    {
      get { return m_xborderStatus; }
      set { SetProperty(ref m_xborderStatus, value); }
    }

    private BorderStatus m_yborderStatus = BorderStatus.NotConnected;
    public BorderStatus YBorderStatus
    {
      get { return m_yborderStatus; }
      set { SetProperty(ref m_yborderStatus, value); }
    }
    private List<double>? m_xData;

    private List<double>? m_yData;

    public List<GraphPoint> GraphPoints { get; private set; } = new List<GraphPoint>();

    private string m_graphPoint = "";
    public string GraphPointsString
    {
      get { return m_graphPoint; }
      set { SetProperty(ref m_graphPoint, value); }
    }

    public double ScaleX { get; private set; }

    public double ScaleY { get; private set; }

    public double ScaleXValue(double x)
    {
      return (x-XMinimum) * ScaleX + PaddingX;
    }

    public double ScaleYValue(double y)
    {
      return (((y-YMinimum) * ScaleY) - Height!.Value) * -1 + PaddingY;
    }

    public Dictionary<double, double> XLabels { get; private set; } = new Dictionary<double, double>();

    public Dictionary<double, double> YLabels { get; private set; } = new Dictionary<double, double>();

    public double PaddingX { get; private set; } = 40;

    public double PaddingY { get; private set; } = 20;

    public GraphType GraphType { get; private set; }

    public Colour TraceColor { get; private set; }

    public bool ShowOverlay { get; private set; } = false;

    private string m_OverlayPointsString = "";
    public string OverlayPointsString
    {
      get { return m_OverlayPointsString; }
      set { SetProperty(ref m_OverlayPointsString, value); }
    }

    private double m_overlayStart = 0;
    public double OverlayStart
    {
      get => m_overlayStart;
      set
      {
        if (value < XMinimum)
        {
          m_overlayStart = XMinimum;
        }
        else if (value > XMaximum)
        {
          m_overlayEnd = XMaximum;
        }
        else
        {
          m_overlayStart = value;
        }
        UpdateOverlayPoints();
      }
    }

    private double m_overlayEnd = 0;
    public double OverlayEnd
    {
      get => m_overlayEnd;
      set
      {
        if (value < XMinimum)
        {
          m_overlayEnd = XMinimum;
        }
        else if (value > XMaximum)
        {
          m_overlayEnd = XMaximum;
        }
        else
        {
          m_overlayEnd = value;
        }
        UpdateOverlayPoints();
      }
    }

    public Colour OverlayColor { get; private set; }

    public int TraceWidth { get; private set; }

    public int OverlayWidth { get; private set; }

    Dictionary<int, int> XPadding = new Dictionary<int, int>() { { 1, 5 }, { 2, 10 }, { 3, 15 }, { 4, 35 } };

    Dictionary<int, int> YPadding = new Dictionary<int, int>() { { 1, 25 }, { 2, 27 }, { 3, 35 }, { 4, 35 } };

    public string m_errorMessage = "";
    public string ErrorMessage
    {
      get { return m_errorMessage; }
      set { SetProperty(ref m_errorMessage, value); }
    }

		public bool ShowXAxis { get; private set; } = true;

		public bool ShowYAxis { get; private set; } = true;

		public ChannelRecord? XChannelRecord { get; private set; }
    public ChannelRecord? YChannelRecord { get; private set; }

    public GraphViewModel(
      double? xMaximum = null,
      double? yMaximum = null,
      double? xMinimum = null,
      double? yMinimum = null,
      double? xTickSize = null,
      double? yTickSize = null,
      Dictionary<int, int>? xPadding = null,
      Dictionary<int, int>? yPadding = null,
      bool showGrid = false,
      bool autoScale = false,
      GraphType? graphType = null,
      Colour? traceColor = null,
      int? overlayWidth = 2,
      int? traceWidth = 1,
      bool showOverlay = false,
      double? overlayStart = 0,
      double? overlayEnd = 0,
      Colour? overlayColor = null,
      int width = 0,
      int height = 0,
      bool isVisible = true,
      string? xTitle = null,
      string? yTitle = null,
      bool showXAxis = true, 
      bool showYAxis = true,
      bool showTooltip = true,
      string? fontStyle = null,
      string? tooltipText = null,
      BorderStatus xborderStatus = BorderStatus.NotConnected,
      BorderStatus yborderStatus = BorderStatus.NotConnected,
      ChannelRecord? xChannelRecord = null,
      ChannelRecord? yChannelRecord = null
      )
    : base(
        isVisible: isVisible,
        fontStyle: FontStyle.DEFAULT_FONT_STYLE,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width <= 0 ? GraphStyle.DEFAULT_WIDTH : width,
        height: height <= 0 ? GraphStyle.DEFAULT_HEIGHT : height
        )
    {
      ShowXAxis = showXAxis;
      ShowYAxis = showYAxis;

      m_xborderStatus = xborderStatus;
      m_yborderStatus = yborderStatus;

      m_xTitle = xTitle ?? GraphStyle.DEFAULT_XTitle;
      m_yTitle = yTitle ?? GraphStyle.DEFAULT_YTitle;

      m_xMaximum = xMaximum ?? GraphStyle.DEFAULT_XMaximum;
      m_yMaximum = yMaximum ?? GraphStyle.DEFAULT_YMaximum;

      m_xMinimum = xMinimum ?? GraphStyle.DEFAULT_XMinimum;
      m_yMinimum = yMinimum ?? GraphStyle.DEFAULT_YMinimum;

      m_xTickSize = xTickSize ?? (XMaximum - XMinimum) / 10;
      m_yTickSize = yTickSize ?? (YMaximum - YMinimum) / 10;

      if (xPadding != null)
      {
        XPadding = xPadding;
      }

      if (yPadding != null)
      {
        YPadding = yPadding;
      }


      m_showGrid = showGrid ;
      m_autoScale = autoScale;

      TraceColor = traceColor ?? GraphStyle.DEFAULT_TRACE_COLOR;
      GraphType = graphType ?? GraphStyle.DEFAULT_GRAPH_TYPE;

      ShowOverlay = showOverlay ;
      OverlayStart = overlayStart ?? XMinimum;
      OverlayEnd = overlayEnd ?? XMinimum;
      OverlayColor = overlayColor ?? GraphStyle.DEFAULT_OVERLAY_COLOR;

      TraceWidth = traceWidth ?? GraphStyle.DEFAULT_TRACE_WIDTH;
      OverlayWidth = overlayWidth ?? GraphStyle.DEFAULT_OVERLAY_WIDTH;

      ComputeXLabels();
      ComputeYLabels();

      // Create or get the Channel/PV object
      XChannelRecord = xChannelRecord;
      XChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { XBorderStatus = isConnected ? BorderStatus.Connected : BorderStatus.NotConnected;},
        (valueInfo, currentState) => SetLineGraphValueX(valueInfo, currentState)
        );

      YChannelRecord = yChannelRecord;
      YChannelRecord?.InitialiseChannel(
        (isConnected, currentState) => { YBorderStatus = isConnected ? BorderStatus.Connected : BorderStatus.NotConnected; },
        (valueInfo, currentState) => SetLineGraphValueY(valueInfo, currentState)
        );

      UpdatePoints();
    }

    void ComputeXLabels()
    {
      ScaleX = Width!.Value / (XMaximum - XMinimum);
      XLabels.Clear();
      // Creating X label co-ordinate relationship
      for (double x = XMinimum; x <= XMaximum; x += XTickSize)
      {
        XLabels.Add(x, ScaleXValue(x));
      }
    }

    void ComputeYLabels()
    {
      ScaleY = Height!.Value / (YMaximum - YMinimum);
      YLabels.Clear();
      // Creating Y label co-ordinate relationship
      for (double y = YMinimum; y <= YMaximum; y += YTickSize)
      {
        YLabels.Add(y, ScaleYValue(y));
      }
    }

    private void SetLineGraphValueX(ValueInfo valueInfo, ChannelState currentState)
    {
        List<double>? values_double;
        try
        {
          values_double = ((IEnumerable)valueInfo.Value).Cast<object>()
                               .Select(x => Convert.ToDouble(x))
                               .ToList();
        }
        catch (Exception x)
        {
          x.ToString(); //TODO: Handle exception in Log... suppressing warning
        values_double = null;
        }
        SetXData(values_double);

      XBorderStatus = currentState.IsConnected ? BorderStatus.Connected : BorderStatus.NotConnected;
    }

    public void SetXData(List<double>? values_double)
    {
      m_xData = values_double;
      UpdatePoints();
    }

    private void SetLineGraphValueY(ValueInfo valueInfo, ChannelState currentState)
    {
        List<double>? values_double;
        try
        {
          values_double = ((IEnumerable)valueInfo.Value).Cast<object>()
                             .Select(x => Convert.ToDouble(x))
                             .ToList();
        }
        catch (Exception x)
        {
          x.ToString(); //TODO: Handle exception in Log... suppressing warning
          values_double = null;
        }
        SetYData(values_double);

      YBorderStatus = currentState.IsConnected ? BorderStatus.Connected : BorderStatus.NotConnected;
    }

    public void SetAutoScaledYData(List<double> values_double)
    {
			var min = values_double.Min();
			var max = values_double.Max();
			double factor = max != 0 ? Math.Abs(1 / max) * 10 : 1.0;

			for (int i = 0; i < values_double.Count; i++)
			{
				values_double[i] = values_double[i] * factor;
			}
			m_yMinimum = Math.Floor(min * factor);
			m_yMaximum = Math.Ceiling(max * factor);
      if(m_yMinimum== m_yMaximum)
      {
        m_yMinimum -= 2; 
        m_yMaximum +=2;

			}
			m_yData = values_double;
			ComputeYLabels();
			UpdatePoints();
		}

		public void SetAutoScaledXData(List<double> values_double)
		{
			var min = values_double.Min();
			var max = values_double.Max();
			double factor = max != 0 ? Math.Abs(1 / max) * 10 : 1.0;

			for (int i = 0; i < values_double.Count; i++)
			{
				values_double[i] = values_double[i] * factor;
			}
			m_xMinimum = Math.Floor(min * factor);
			m_xMaximum = Math.Ceiling(max * factor);
			m_xData = values_double;
			ComputeXLabels();
			UpdatePoints();
		}


		public void SetYData(List<double>? values_double)
    {
      m_yData = values_double;
      UpdatePoints();
    }

    public void SetData(List<double>? xDataValues, List<double>? yDataValues)
    {
      m_xData = xDataValues;
      m_yData = yDataValues;
      UpdatePoints();
    }

    public double PaddingCalculationY(double y)
    {
      var numberOfdigits = y.ToString().Count();
      return YPadding[numberOfdigits];
    }
    public double PaddingCalculationX(double x)
    {
      var numberOfdigits = x.ToString().Count();
      return XPadding[numberOfdigits];
    }

    public void UpdatePoints()
    {
      ErrorMessage = "";
      bool errorFlag = false;
      if (m_xData == null)
      {
        //Display Error Message
        ErrorMessage += "X-Axis data is invalid.";
        errorFlag = true;
      }
      if (m_yData == null)
      {
        //Display Error Message
        if (!String.IsNullOrEmpty(ErrorMessage))
          ErrorMessage += Environment.NewLine;
        ErrorMessage += "Y-Axis data is invalid.";
        errorFlag = true;
      }

      if (errorFlag)
      {
        return;
      }

      if (m_xData!.Count != m_yData!.Count)
      {
        ErrorMessage = m_xData!.Count > m_yData!.Count ?
                        "Y-Axis data count is less than X-Axis data count" :
                        "X-Axis data count is less than Y-Axis data count";
        return;
      }

			GraphPoints = m_xData.Zip(
					m_yData,
					(x, y) => new GraphPoint(x, y)
				).ToList();
	
      if(GraphPoints
        .Count > 10000 ) 
      {
				UpdateGraphPoints(GetResampledGraphPointsList(GraphPoints,10000));
			}
      else
      {
				UpdateGraphPoints(GraphPoints);
			}
      if (ShowOverlay)
      {
        UpdateOverlayPoints();
      }
    }

    List<GraphPoint> GetResampledGraphPointsList(List<GraphPoint> graphPoints, int newCount)
    {
      List<GraphPoint> resampledList = new List<GraphPoint>() { graphPoints[0] };
			double step = (double)(graphPoints.Count - 3) / (newCount - 3);

			for (int i=0; i<newCount-2;i++)
      {
				int index = (int)Math.Round(i * step);
				resampledList.Add(graphPoints[index]);
			}   
      resampledList.Add(graphPoints[^1]);
      return resampledList;
    }

    private void UpdateGraphPoints(List<GraphPoint> graphPointsToPlot)
    {
			System.Text.StringBuilder pointsStringBuilder = new();

			//rather then using ScaleXValue and ScaleYValue, we need implementation similar to GetDataPositionInViewCoordinates_X in IntensityProfileGraph
			foreach (var point in graphPointsToPlot)
      {
				pointsStringBuilder.Append(
						$"{ScaleXValue(point.X):F2},{ScaleYValue(point.Y):F2} "
					);
      }
      if (GraphType == GraphType.Area && graphPointsToPlot.Any())
      {
        //inserting at 0 is slow, only done as a stop gap, needs to handle end points as it is done in IntensityProfileGraph
        pointsStringBuilder.Insert(
              0,
              $"{ScaleXValue(graphPointsToPlot[0].X):F2},{ScaleYValue(YMinimum):F2} "
            );
        pointsStringBuilder.Append(
          $"{ScaleXValue(graphPointsToPlot[^1].X):F2},{ScaleYValue(YMinimum):F2} "
        );
      }
      GraphPointsString = pointsStringBuilder.ToString();
    }

    private void UpdateOverlayPoints()
    {
      string temp = "";
      if (GraphType == GraphType.Line)
      {
        foreach (var point in GraphPoints)
        {
          if (point.X >= OverlayStart && point.X <= OverlayEnd)
          {
            temp += $" {ScaleXValue(point.X)},{ScaleYValue(point.Y)} ";
          }
        }
      }
      else if (GraphType == GraphType.Area)
      {
        temp = $" {ScaleXValue(OverlayStart)},{ScaleYValue(YMinimum)} ";
        foreach (var point in GraphPoints)
        {
          if (point.X >= OverlayStart && point.X <= OverlayEnd)
          {
            temp += $" {ScaleXValue(point.X)},{ScaleYValue(point.Y)} ";
          }
        }
        temp += $" {ScaleXValue(OverlayEnd)},{ScaleYValue(YMinimum)} ";
      }
      if (!OverlayPointsString.Equals(temp))
      {
        OverlayPointsString = temp;
      }
    }

    public string ComputeBorderStatus()
    {
      string borderStatus = Colour.Transparent.HtmlRgbaString;
      if (YChannelRecord?.Channel != null)
      {
        borderStatus = Utilities.GetColourFromBorderStatus(YBorderStatus).HtmlRgbaString;
      }
      if (XChannelRecord?.Channel != null)
      {
        borderStatus = Utilities.GetColourFromBorderStatus(XBorderStatus).HtmlRgbaString;
      }
      return borderStatus;
    }
  }
}
