//
// ImageViewerViewModelEx.cs
//
// Cloned from ImageViewerViewModel.
// Enhanced to provide mappings from intensity values to colours,
// and use the ChannelsHandler instead of the 'StateChanged' event.
//



using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;
using System.Linq;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{

    public class ImageViewerViewModel : MonitorWidgetViewModelBase
  {

    private ColourMapOption m_colourMapOption = ColourMapOption.Jet;
    public ColourMapOption ColourMapOption
    {
      get { return m_colourMapOption; }
      set { SetProperty(ref m_colourMapOption, value); }
    }

    private bool m_autoNormalise = true;
    public bool AutoNormalise
    {
      get { return m_autoNormalise; }
      set
      {
        if (SetProperty(ref m_autoNormalise, value))
          ComputeDisplayImageData();
      }
    }

    private byte m_normalisationValue = 255;
    public byte NormalisationValue
    {
      get { return m_normalisationValue; }
      set
      {
        if (SetProperty(ref m_normalisationValue, value))
          ComputeDisplayImageData();
      }
    }

    private BorderStatus m_borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get { return m_borderStatus; }
      set { SetProperty(ref m_borderStatus, value); }
    }

    private int m_imageWidth = ImageViewerStyle.DEFAULT_IMAGE_WIDTH;
    public int ImageWidth
    {
      get { return m_imageWidth; }
      set
      {
        if (SetProperty(ref m_imageWidth, value))
          SetOriginalImageData();
      }
    }

    private int m_imageHeight = ImageViewerStyle.DEFAULT_IMAGE_HEIGHT;
    public int ImageHeight
    {
      get { return m_imageHeight; }
      set
      {
        if (SetProperty(ref m_imageHeight, value))
          SetOriginalImageData();
      }
    }

    private byte[] m_rawImageData = ImageViewerStyle.DEFAULT_RAW_IMAGE_DATA;

    public byte[] RawImageData
    {
      get { return m_rawImageData; }
      set
      {
        if (SetProperty(ref m_rawImageData, value))
          SetOriginalImageData();
      }
    }

    public DisplaySize DisplaySize { get; private set; }


    public int DisplayWidth => DisplayImageData?.Width ?? 0;
    public int DisplayHeight => DisplayImageData?.Height ?? 0;

    private LinearArrayHoldingPixelBytes m_originalImageData;
    //= new LinearArrayHoldingPixelBytes(
    //  width: ImageViewerStyle.DEFAULT_IMAGE_WIDTH,
    //  height: ImageViewerStyle.DEFAULT_IMAGE_HEIGHT,
    //  // We create a clone here, because when the image data array is passed into
    //  // the constructor, ownership is assumed to be transferred to the
    //  // new 'LinearArrayHoldingPixelBytes' instance ; that is, the class is free
    //  // to modify the data in place. If we don't create a clone, any such
    //  // modifications will affect the 'DEFAULT_RAW_IMAGE_DATA' values
    //  // since that array is being passed by reference.
    //  linearArrayThatWeWillTakeOwnershipOf: IntensityMapLib.Helpers.CreateCloneOfByteArray(
    //    ImageViewerStyle.DEFAULT_RAW_IMAGE_DATA
    //  )
    //);
    public LinearArrayHoldingPixelBytes OriginalImageData
    {
      get { return m_originalImageData; }
      set { SetProperty(ref m_originalImageData, value); }
    }

    private LinearArrayHoldingPixelBytes m_displayImageData;
    public LinearArrayHoldingPixelBytes DisplayImageData
    {
      get { return m_displayImageData; }
      set { SetProperty(ref m_displayImageData, value); }
    }

    private OverlaysDescriptor m_overlaysDescriptor = new OverlaysDescriptor();
    public OverlaysDescriptor OverlaysDescriptor
    {
      get { return m_overlaysDescriptor; }
      set { SetProperty(ref m_overlaysDescriptor, value ?? new OverlaysDescriptor()); }
    }

    //Channel PV Object
    private ChannelRecord? ImageDataRecord { get; set; }
    private ChannelRecord? ImageWidthRecord { get; set; }
    private ChannelRecord? ImageHeightRecord { get; set; }

    private bool m_imageDataIsConnected = false;
    private bool m_imageWidthIsConnected = false;
    private bool m_imageHeightIsConnected = false;

    public ImageViewerViewModel(
      int? imageWidth = null,
      int? imageHeight = null,
      double? displayImageScalingFactor = null, // Needs to be removed
      DisplaySize? displaySize = null,
      bool isVisible = true,
      bool showTooltip = true,
      string? fontStyle = null,
      string? tooltipText = null,
      BorderStatus borderStatus = BorderStatus.NotConnected,
      ChannelRecord? imageDataRecord = null,
      ChannelRecord? imageWidthRecord = null,
      ChannelRecord? imageHeightRecord = null
    ) :
    base(
      isVisible: isVisible,
      fontStyle: fontStyle,
      tooltipText: tooltipText,
      showTooltip: showTooltip
    )
    {
      DisplaySize = displaySize ?? new DisplaySize(400,400);

      m_imageWidth = DisplaySize.Width;
      m_imageHeight = DisplaySize.Height;


      m_originalImageData = new LinearArrayHoldingPixelBytes(m_imageWidth, m_imageHeight);

      m_displayImageData = m_originalImageData.CreateResizedClone_NearestNeighbour(DisplaySize.Width, DisplaySize.Height);

      m_borderStatus = borderStatus;


      //ChannelAccess implementation
      m_imageDataIsConnected = imageDataRecord == null;
      m_imageWidthIsConnected = imageWidthRecord == null;
      m_imageHeightIsConnected = imageHeightRecord == null;
      
      // In Border status AlarmStatusAndSeverity is not considered for ImageViewer
      ImageDataRecord = imageDataRecord;
      ImageDataRecord?.InitialiseChannel(
        (isConnected, currentState) => {
          m_imageDataIsConnected = isConnected;
          BorderStatus = (m_imageDataIsConnected && m_imageWidthIsConnected && m_imageHeightIsConnected) ? BorderStatus.Connected : BorderStatus.NotConnected;
        },
        valueChangedHandler: (valueInfo, state) =>
        {
            object? imageData = valueInfo.Value;
            if (imageData is byte[] eightBitImageData)
            {
                RawImageData = eightBitImageData;
            }
           /* else if (imageData is ushort[] sixteenBitImageData)
            {
                RawImageData = sixteenBitImageData.Select(
                   pixel => (byte)(pixel >> 8)
                ).ToArray();
            }
            else
            {
                // Unexpected value !
            }*/
        }
        );

      ImageWidthRecord = imageWidthRecord;
      ImageWidthRecord?.InitialiseChannel(
        (isConnected, currentState) => {
          m_imageWidthIsConnected = isConnected;
          BorderStatus = (m_imageDataIsConnected && m_imageWidthIsConnected && m_imageHeightIsConnected) ? BorderStatus.Connected : BorderStatus.NotConnected;
        },
        valueChangedHandler: (valueInfo, state) =>
        {
          ImageWidth = (int)valueInfo.Value;
        }
        );

      ImageHeightRecord = imageHeightRecord;
      ImageHeightRecord?.InitialiseChannel(
        (isConnected, currentState) => {
          m_imageHeightIsConnected = isConnected;
          BorderStatus = (m_imageDataIsConnected && m_imageWidthIsConnected && m_imageHeightIsConnected) ? BorderStatus.Connected : BorderStatus.NotConnected;
        },
        valueChangedHandler: (valueInfo, state) =>
        {
          ImageHeight = (int)valueInfo.Value;

        }
        );
    }

    public void SetImageData(int width, int height, byte[] imageData)
    {
      m_imageWidth = width;
      m_imageHeight = height;
      m_rawImageData = imageData;
      SetOriginalImageData();
    }
    private void SetOriginalImageData()
    {
      if (RawImageData.Length == ImageWidth * ImageHeight)
      {
        OriginalImageData = new LinearArrayHoldingPixelBytes(
          ImageWidth,
          ImageHeight,
          RawImageData
        );
        ComputeDisplayImageData();
      }
    }

    private void ComputeDisplayImageData()
    {
      // We're creating clones here, but they are only used as intermediate variables
      // that will immediately go out of scope and be garbage collected,
      // so it'll be more efficient to apply all the operations in one go ...
      var resizedImage = OriginalImageData.CreateResizedClone_NearestNeighbour(DisplaySize.Width, DisplaySize.Height);
      //var rotatedResizedImage = resizedImage.CreateRotatedClone(RotationFactor) ;
      var normalisedRotatedResizedImage = resizedImage.CreateNormalisedClone(
        AutoNormalise
        ? resizedImage.MaxValue
        : NormalisationValue
      );
      DisplayImageData = normalisedRotatedResizedImage;
    }
  }

}
