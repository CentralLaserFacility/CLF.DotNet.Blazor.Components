//
// IntensityMapViewerViewModel.cs
//

using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Complex.IntensityMap.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Common.ImageProcessing;
using System.ComponentModel;
using Clf.Blazor.Basic.Components.Controls.ViewModels;
using Clf.Common.Drawing;
using Clf.Common.Graphs;
using Clf.ChannelAccess;
using Clf.Blazor.Common.FilePicker;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapViewerViewModel : ObservableObject, IDisposable
  {

    // 
    // See 'view_cam.bob' opened (for editing) in Phoebus
    //
    // C:\tmp\AVT-cameras-development_epac\ui\bob
    // From https://github.com/CentralLaserFacility/AVT-cameras/tree/development_epac
    //

    public static int GraphHeight = 100;
    public static int TitleLineHeight = 24;
    public static int ControlsPanelWidth = 300;
    public static double DisplayImageScalingFactor = 0.3;
    private DisplaySize m_displaySize;
    public DisplaySize DisplaySize
    {
      get { return m_displaySize; }
      set 
      { 
        SetProperty(ref m_displaySize, value);
        HorizontalProfileGraph.IntensityProfileGraph.Width = m_displaySize.Width;
        VerticalProfileGraph.IntensityProfileGraph.Width = m_displaySize.Height;
      }
    }

    private int[] m_contourDataSet;
    public int[] ContourDataSet
    {
      get { return m_contourDataSet; }
      set 
      { 
        SetProperty(ref m_contourDataSet, value);
        UpdateContourDataSetsOverlay();
      }
    }

    private int m_profileCrosshairSize;
    public int ProfileCrosshairSize
    {
      get { return m_profileCrosshairSize; }
      set 
      { 
        SetProperty(ref m_profileCrosshairSize, value);
        UpdateProfileCrosshairOverlay();
      }
    }

    private Colour m_profileColour = Colour.SystemDrawingColorToColour(System.Drawing.Color.Red);
    public Colour ProfileColour
    {
      get { return m_profileColour; }
      set
      {
        SetProperty(ref m_profileColour, value);
        UpdateProfileCrosshairOverlay();
      }
    }

    private Colour m_contourColour = Colour.SystemDrawingColorToColour(System.Drawing.Color.Red);
    public Colour ContourColour
    {
      get { return m_contourColour; }
      set
      {
        SetProperty(ref m_contourColour, value);
        UpdateContourDataSetsOverlay();
      }
    }

    private bool m_contourCrosshairThick;
    public bool ContourCrosshairThick
    {
      get { return m_contourCrosshairThick; }
      set
      {
        SetProperty(ref m_contourCrosshairThick, value);
        UpdateContourDataSetsOverlay();
      }
    }
    private bool m_showContourCrosshair;
    public bool ShowContourCrosshair
    {
      get { return m_showContourCrosshair; }
      set
      {
        SetProperty(ref m_showContourCrosshair, value);
        if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[3] != null)
          IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[3]!.CanDraw = ShowContourCrosshair;
      }
    }
    private bool m_EDBeamShapeThick;
    public bool EDBeamShapeThick
        {
        get { return m_EDBeamShapeThick; }
        set
        {
            SetProperty(ref m_EDBeamShapeThick, value);
            UpdateEDBeamShapeOverlay();
        }
    }
    private bool m_EDBeamShape;
    public bool EDBeamShape
    {
        get { return m_EDBeamShape; }
        set
        {
            SetProperty(ref m_EDBeamShape, value);
            UpdateEDBeamShapeOverlay();
        }
    }

    private Colour m_EDBeamShapeColour = Colour.SystemDrawingColorToColour(System.Drawing.Color.Red);
    public Colour EDBeamShapeColour
        {
        get { return m_EDBeamShapeColour; }
        set
        {
            SetProperty(ref m_EDBeamShapeColour, value);
            UpdateEDBeamShapeOverlay();
        }
    }

    private bool m_showEDBeamShape;
    public bool ShowEDBeamShape
    {
        get { return m_showEDBeamShape; }
        set
        {
            SetProperty(ref m_showEDBeamShape, value);
            UpdateEDBeamShapeOverlay();
        }
    }

    private bool m_centroidAlgorithm;
    public bool CentroidAlgorithm
        {
        get { return m_centroidAlgorithm; }
        set
        {
            SetProperty(ref m_centroidAlgorithm, value);
            UpdateEDBeamShapeOverlay();
        }
    }

    private int m_EDCentroidX;
    public int EDCentroidX
        {
        get { return m_EDCentroidX; }
        set
        {
            SetProperty(ref m_EDCentroidX, value);
            UpdateEDBeamShapeOverlay();
        }
    }

    private int m_EDCentroidY;
    public int EDCentroidY
    {
        get { return m_EDCentroidY; }
        set
        {
            SetProperty(ref m_EDCentroidY, value);
            UpdateEDBeamShapeOverlay();
        }
    }
    private int m_EDRectangleHeight;
    public int EDRectangleHeight
        {
        get { return m_EDRectangleHeight; }
        set
        {
            SetProperty(ref m_EDRectangleHeight, value);
            UpdateEDBeamShapeOverlay();
        }
    }
    private int m_EDRectangleWidth;
    public int EDRectangleWidth
        {
        get { return m_EDRectangleWidth; }
        set
        {
            SetProperty(ref m_EDRectangleWidth, value);
            UpdateEDBeamShapeOverlay();
        }
    }
    private double m_EDRectangleTilt;
    public double EDRectangleTilt
    {
        get { return m_EDRectangleTilt; }
        set
        {
            SetProperty(ref m_EDRectangleTilt, value);
            UpdateEDBeamShapeOverlay();
        }
    }
    private double m_EDCircleRadius;
    public double EDCircleRadius
        {
        get { return m_EDCircleRadius; }
        set
        {
            SetProperty(ref m_EDCircleRadius, value);
            UpdateEDBeamShapeOverlay();
        }
    }
    private void UpdateContourDataSetsOverlay()
    {
        if (m_contourDataSet != null)
        {
            IntensityMapImage.ImageViewer.OverlaysDescriptor.Overlays[3] = OverlayClosedPolygonDescriptor.Create(new(Convert.ToByte(ContourColour.Red), Convert.ToByte(ContourColour.Green), Convert.ToByte(ContourColour.Blue)), ContourCrosshairThick, m_contourDataSet);
            if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[3] != null)
                IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[3]!.CanDraw = ShowContourCrosshair;
        }

    }

    private void UpdateEDBeamShapeOverlay()
    {
        if (CentroidAlgorithm && ShowEDBeamShape)
        {
            if (m_EDBeamShape)
            {
                IntensityMapImage.ImageViewer.OverlaysDescriptor.Overlays[4] = OverlayClosedPolygonDescriptor.CreateTiltedBox(EDCentroidX, EDCentroidY, EDRectangleHeight, EDRectangleWidth, EDRectangleTilt, new(Convert.ToByte(EDBeamShapeColour.Red), Convert.ToByte(EDBeamShapeColour.Green), Convert.ToByte(EDBeamShapeColour.Blue)), EDBeamShapeThick);
                if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[4] != null)
                {
                    IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[4]!.CanDraw = ShowEDBeamShape;
                    IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[5]!.CanDraw = !ShowEDBeamShape;
                }
            }
            else
            {
                IntensityMapImage.ImageViewer.OverlaysDescriptor.Overlays[5] = new OverlayCircleDescriptor(new(Convert.ToByte(EDBeamShapeColour.Red), Convert.ToByte(EDBeamShapeColour.Green), Convert.ToByte(EDBeamShapeColour.Blue)), EDBeamShapeThick, EDCentroidX, EDCentroidY, Convert.ToInt16(EDCircleRadius));
                if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[5] != null)
                {
                    IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[5]!.CanDraw = ShowEDBeamShape;
                    IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[4]!.CanDraw = !ShowEDBeamShape;
                }

            }
        }
        else
            {
                IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[5]!.CanDraw = false;
                IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[4]!.CanDraw = false;
            }
    }
    private void UpdateProfileCrosshairOverlay()
    {
      IntensityMapImage.ImageViewer.OverlaysDescriptor.Overlays[0] = OverlayCrossDescriptor.FromCentrePoint(ProfileCrosshairX, ProfileCrosshairY, ProfileCrosshairSize, new(Convert.ToByte(ProfileColour.Red), Convert.ToByte(ProfileColour.Green), Convert.ToByte(ProfileColour.Blue)), false);
      if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[0] != null)
        IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[0]!.CanDraw = ShowProfileCrosshair;
    }

    private bool m_showProfileCrosshair;
    public bool ShowProfileCrosshair
    {
      get { return m_showProfileCrosshair; }
      set 
      { 
        SetProperty(ref m_showProfileCrosshair, value);
        if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[0] != null)
          IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[0]!.CanDraw = ShowProfileCrosshair;
      }
    }

    private int m_profileCrosshairX=0;
    public int ProfileCrosshairX
    {
      get { return m_profileCrosshairX; }
      set
      {
        SetProperty(ref m_profileCrosshairX, value);
        //Redraw IntensityMapImage.ImageViewer.OverlaysDescriptor[0]
        UpdateProfileCrosshairOverlay();
        SetVerticalProfileGraph(ProfileCrosshairX);

      }
    }

    private void SetVerticalProfileGraph(int x)
    {
      var yData = IntensityMapImage.ImageViewer.DisplayImageData.GetColumnOfPixelsAtOffsetFromLeft(x);
      var yDataDouble = yData?.Select(x => Convert.ToDouble(x)).ToList();
      if (yDataDouble != null)
      {
        var xDataDouble = Enumerable.Range(0, yDataDouble.Count).Select(x => Convert.ToDouble(x)).ToList();
        VerticalProfileGraph.IntensityProfileGraph.SetDataValues_X(xDataDouble);
        VerticalProfileGraph.IntensityProfileGraph.SetDataValues_Y(yDataDouble);
        VerticalProfileGraph.IntensityProfileGraph.XDataRange = new GraphAxisRange(0, yDataDouble.Count);
      }
    }

    private int m_profileCrosshairY =0;
    public int ProfileCrosshairY
    {
      get { return m_profileCrosshairY ; }
      set 
      { 
        SetProperty(ref m_profileCrosshairY , value);
        //Redraw IntensityMapImage.ImageViewer.OverlaysDescriptor[0]
        UpdateProfileCrosshairOverlay();
        SetHorizontalProfileGraph(ProfileCrosshairY);
      }
    }

    private void SetHorizontalProfileGraph(int x)
    {
      var yData = IntensityMapImage.ImageViewer.DisplayImageData.GetRowOfPixelsAtOffsetFromTop(x);
      var yDataDouble = yData?.Select(x => Convert.ToDouble(x)).ToList();
      if (yDataDouble != null)
      {
        var xDataDouble = Enumerable.Range(0, yDataDouble.Count).Select(x => Convert.ToDouble(x)).ToList();
        HorizontalProfileGraph.IntensityProfileGraph.SetDataValues_X(xDataDouble);
        HorizontalProfileGraph.IntensityProfileGraph.SetDataValues_Y(yDataDouble);
        HorizontalProfileGraph.IntensityProfileGraph.XDataRange = new GraphAxisRange(0, yDataDouble.Count);
      }
    }


    private int m_softwareCrosshairSize;
    public int SoftwareCrosshairSize
    {
      get { return m_softwareCrosshairSize; }
      set 
      { 
        SetProperty(ref m_softwareCrosshairSize, value);
        UpdateSoftwareCrosshairOverlay();
      }
    }

    private Colour m_softwareColour = Colour.SystemDrawingColorToColour(System.Drawing.Color.Red);
    public Colour SoftwareColour
    {
      get { return m_softwareColour; }
      set
      {
        SetProperty(ref m_softwareColour, value);
        UpdateSoftwareCrosshairOverlay();
        UpdateSoftwareBoxOverlay();
      }
    }

    private void UpdateSoftwareBoxOverlay()
    {
      IntensityMapImage.ImageViewer.OverlaysDescriptor.Overlays[2] = OverlayBoxDescriptor.FromCentrePoint(SoftwareX, SoftwareY, SoftwareBoxHeight, SoftwareBoxWidth, new(Convert.ToByte(SoftwareColour.Red), Convert.ToByte(SoftwareColour.Green), Convert.ToByte(SoftwareColour.Blue)), false);
      if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[2] != null)
        IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[2]!.CanDraw = ShowSoftwareBox;
    }

    private void UpdateSoftwareCrosshairOverlay()
    {
      IntensityMapImage.ImageViewer.OverlaysDescriptor.Overlays[1] = OverlayCrossDescriptor.FromCentrePoint(SoftwareX, SoftwareY, SoftwareCrosshairSize, new(Convert.ToByte(SoftwareColour.Red), Convert.ToByte(SoftwareColour.Green), Convert.ToByte(SoftwareColour.Blue)), false);
      if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[1] != null)
        IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[1]!.CanDraw = ShowSoftwareCrosshair;
    }

    private bool m_showSoftwareCrosshair;
    public bool ShowSoftwareCrosshair
    {
      get { return m_showSoftwareCrosshair; }
      set 
      { 
        SetProperty(ref m_showSoftwareCrosshair, value);
        if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[1] != null)
          IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[1]!.CanDraw = ShowSoftwareCrosshair;
      }
    }


    private bool m_showSoftwareBox;
    public bool ShowSoftwareBox
    {
      get { return m_showSoftwareBox; }
      set 
      {
        SetProperty(ref m_showSoftwareBox, value);
        if (IntensityMapImage.ImageViewer.OverlaysDescriptor?.Overlays[2] != null)
          IntensityMapImage.ImageViewer.OverlaysDescriptor!.Overlays[2]!.CanDraw = ShowSoftwareBox;
      }
    }

    private int m_softwareX;
    public int SoftwareX
    {
      get { return m_softwareX; }
      set 
      { 
        SetProperty(ref m_softwareX, value);
        UpdateSoftwareCrosshairOverlay();
        UpdateSoftwareBoxOverlay();

      }
    }

    private int m_softwareY;
    public int SoftwareY
    {
      get { return m_softwareY; }
      set 
      { 
        SetProperty(ref m_softwareY, value);
        UpdateSoftwareCrosshairOverlay();
        UpdateSoftwareBoxOverlay();
      }
    }

    private int m_softwareBoxWidth;
    public int SoftwareBoxWidth
    {
      get { return m_softwareBoxWidth; }
      set 
      { 
        SetProperty(ref m_softwareBoxWidth, value);
        UpdateSoftwareBoxOverlay();
      }
    }
    private int m_softwareBoxHeight;
    public int SoftwareBoxHeight
    {
      get { return m_softwareBoxHeight; }
      set 
      { 
        SetProperty(ref m_softwareBoxHeight, value);
        UpdateSoftwareBoxOverlay();
      }
    }

    private bool m_followCentroid=false;
    public bool FollowCentroid
    {
      get { return m_followCentroid; }
      set 
      { 
        SetProperty(ref m_followCentroid, value);
        if(FollowCentroid)
        {
          ProfileCrosshairX = EDCentroidX; 
          ProfileCrosshairY = EDCentroidY; 
        }     
      }
    }

    public TextUpdateViewModel UserName { get ; }
    public LedViewModel CameraStatus { get; }

    // Main image and Profile graphs

    public IntensityMapImageViewModel        IntensityMapImage      { get ; }
    public IntensityMapProfileGraphViewModel HorizontalProfileGraph { get ; }
    public IntensityMapProfileGraphViewModel VerticalProfileGraph   { get ; }
    public IntensityMapFixedDisplayViewModel              FixedDisplay { get; }
    public IntensityMapFeaturesViewModel                  Features { get; }
    
    // Child ViewModels access these properties when creating their Channels ...

    public FilePickerService FilePickerService { get; }
    public string PvPrefix     { get ; } // eg 'SIM1:' 
    public string StreamPrefix { get ; } // eg 'cam1:' or 'image1' ... ?????????

    public ChannelAccess.ChannelsHandler ChannelsHandler { get ; }

    public ChannelRecord CreateChannelRecord ( string channelName )
    {
      return new ChannelRecord(
        channelName,
        ChannelsHandler
      ) ;
    }

    public IntensityMapViewerViewModel ( 
      string                        pvPrefix,      // eg 'SIM1:'
      string                        streamPrefix,  // eg 'cam1:'
      ChannelAccess.ChannelsHandler channelsHandler,
      FilePickerService filePicker
    ) {

      FilePickerService = filePicker;
      PvPrefix        = pvPrefix ;
      StreamPrefix    = streamPrefix ;
      ChannelsHandler = channelsHandler ;

      m_displaySize = new(
        (int)(ImageViewerStyle.DEFAULT_IMAGE_WIDTH * DisplayImageScalingFactor),
        (int)(ImageViewerStyle.DEFAULT_IMAGE_HEIGHT * DisplayImageScalingFactor)
      );

      UserName = new TextUpdateViewModel(
        width:300,
        channelRecord : CreateChannelRecord(PvPrefix+"Username")
      ) ;
      CameraStatus = new LedViewModel(
      width: 150,
      isSquare: true,
      onLabel: "Connected",
      offLabel: "Not Connected",
      ledChannelRecord: CreateChannelRecord(PvPrefix + "cam1:CameraConnected_RBV").ToLedChannelRecord("1")
      );

      IntensityMapImage = new IntensityMapImageViewModel(
        parent      : this,
        displayScalingFactor: DisplayImageScalingFactor
      ) ;

      // The 'vertical profile' graph (along the left hand side)
      // shows the values at a particular X coordinate ie along a Vertical line

      // NOTE THAT WE'RE ACCESSING 'SIZE' DEFINITIONS SET UP IN THE VIEW,
      // WHICH IS NORMALLY A BAD IDEA. BUT IN A FUTURE VERSION THE VIEW MODELS
      // WILL NOT USE THE HEIGHTS AND WIDTHS, SO LET'S LEAVE IT THIS WAY.

      VerticalProfileGraph = new IntensityMapProfileGraphViewModel(
        this,
        width  : DisplaySize.Height,   // 300,
        height : GraphHeight // 100,
      ) ;

      // The 'horizontal profile' graph (underneath the image display)
      // shows the values at a particular Y coordinate ie along a Horizontal line

      HorizontalProfileGraph = new IntensityMapProfileGraphViewModel(
        this,
        width  : DisplaySize.Width,   // 300,
        height : GraphHeight // 100,
      ) ;

      IntensityMapImage.ImageViewer.PropertyChanged += OnImageViewerPropertyChanged;

      // { Width=300,Height=100 } ;
      // SURPRISINGLY, APPLYING THE WIDTH AND HEIGHT AS *PROPERTIES* DOESN'T WORK ...

      FixedDisplay              = new IntensityMapFixedDisplayViewModel(this);
      Features                   = new IntensityMapFeaturesViewModel(this);

      IntensityMapViewerViewModel_Logic_Initiliasation();
    }

    private void OnImageViewerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(IntensityMapImage.ImageViewer.DisplayImageData))
      {
        SetHorizontalProfileGraph(ProfileCrosshairY);
        SetVerticalProfileGraph(ProfileCrosshairX);
      }
    }

    public void Dispose()
    {
      IntensityMapImage.ImageViewer.PropertyChanged -= OnImageViewerPropertyChanged;
    }
  }

}
