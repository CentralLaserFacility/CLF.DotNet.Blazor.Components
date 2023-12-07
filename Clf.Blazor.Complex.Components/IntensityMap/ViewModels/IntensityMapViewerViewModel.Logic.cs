using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using Clf.Common.Arithmatic;
using Clf.Common.Drawing;
using Clf.Common.ExtensionMethods;
using Clf.Common.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
  public partial class IntensityMapViewerViewModel
  {
    IChannel? m_profXhairXHighChannel = null;
    IChannel? m_profXhairYHighChannel = null;
    IChannel? m_softXHighChannel = null;
    IChannel? m_softYHighChannel = null;
    IChannel? m_showProfileGraphChannel = null;

    int _imageWidth = 0;
    int _imageHeight = 0;

    public int ImageWidth { get => _imageWidth<1?DisplaySize.Width:_imageWidth; set => _imageWidth = value; }
    public int ImageHeight { get => _imageHeight < 1 ? DisplaySize.Height : _imageHeight; set => _imageHeight = value; }

    private void IntensityMapViewerViewModel_Logic_Initialisation()
    {
      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":Username"),
        valueChangedHandler: (valueInfo, _) =>
        {
          var userName = valueInfo.ValueAsString();
          if(!string.IsNullOrEmpty(userName))
          {
            FriendlyName = userName;
          }
        }
      );
      ChannelsHandler.InstallChannel(
        m_profXhairXHighChannel = Hub.GetOrCreateChannel(PvPrefix + ":ProfXhairX.HOPR")
      );
      ChannelsHandler.InstallChannel(
        m_profXhairYHighChannel = Hub.GetOrCreateChannel(PvPrefix + ":ProfXhairY.HOPR")
      );
      ChannelsHandler.InstallChannel(
        m_softXHighChannel = Hub.GetOrCreateChannel(PvPrefix + ":SoftX.HOPR")
      );
      ChannelsHandler.InstallChannel(
        m_softYHighChannel = Hub.GetOrCreateChannel(PvPrefix + ":SoftY.HOPR")
      );
      ChannelsHandler.InstallChannel(
        m_showProfileGraphChannel = Hub.GetOrCreateChannel(PvPrefix + ":ShowProfileGraph"),
      (valueInfo, _) => OnShowProfileGraphChange((System.Int16)valueInfo.Value)
      );
      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":image1:ArraySize0_RBV"),
        valueChangedHandler: (valueInfo, _) =>
        {
          m_profXhairXHighChannel.PutValue(Convert.ToDouble(valueInfo.Value));
          m_softXHighChannel.PutValue(Convert.ToDouble(valueInfo.Value));
          ImageWidth = (int)valueInfo.Value;
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":image1:ArraySize1_RBV"),
        valueChangedHandler: (valueInfo, _) =>
        {
          m_profXhairYHighChannel.PutValue(Convert.ToDouble(valueInfo.Value));
          m_softYHighChannel.PutValue(Convert.ToDouble(valueInfo.Value));
          ImageHeight = (int)valueInfo.Value;
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":image1:ArrayData"),
        valueChangedHandler: (valueInfo, _) =>
        {
          var fullArrayData = (byte[])valueInfo.Value;
          int correctDataCount = ImageWidth * ImageHeight;
          if (correctDataCount <= fullArrayData.Length)
          {
            byte[] correctData = new byte[correctDataCount];
            Array.Copy(fullArrayData, correctData, correctDataCount);
            IntensityMapImage.ImageViewer.SetImageData(ImageWidth, ImageHeight, correctData);
          }
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ProfXhairX"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ProfileCrosshairX = Convert.ToInt32((double)valueInfo.Value * XDisplayScalingFactor);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ProfXhairY"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ProfileCrosshairY = Convert.ToInt32((double)valueInfo.Value * YDisplayScalingFactor);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ShowProfXhair"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ShowProfileCrosshair = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":SoftX"),
        valueChangedHandler: (valueInfo, _) =>
        {
          SoftwareX = Convert.ToInt32((double)valueInfo.Value * XDisplayScalingFactor);
        }
      );
        
        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:RectangleBeamHeight"),
            valueChangedHandler: (valueInfo, _) =>
            {
                EDRectangleHeight = Convert.ToInt32((int)valueInfo.Value * YDisplayScalingFactor);
            }
        );

        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:RectangleBeamWidth"),
            valueChangedHandler: (valueInfo, _) =>
            {
                EDRectangleWidth = Convert.ToInt32((int)valueInfo.Value * XDisplayScalingFactor);
            }
        );

        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:RectangleBeamTiltAngle"),
            valueChangedHandler: (valueInfo, _) =>
            {
                EDRectangleTilt = Convert.ToInt32((int)valueInfo.Value);
            }
        );

        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:CircleDiameter"),
            valueChangedHandler: (valueInfo, _) =>
            {
                EDCircleRadius = (Convert.ToInt32((int)valueInfo.Value * XDisplayScalingFactor))/2;
            }
        );
        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":ShowEDBeamShapeXhair"),
            valueChangedHandler: (valueInfo, _) =>
            {
                ShowEDBeamShape = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
            }
        );

        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":BeamShape"),
            valueChangedHandler: (valueInfo, _) =>
            {
                EDBeamShape = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
            }
        );

        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:CentroidAlgorithm"),
            valueChangedHandler: (valueInfo, _) =>
            {
                CentroidAlgorithm = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
            }
        );

        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":EDBeamShapeXhairThick"),
            valueChangedHandler: (valueInfo, _) =>
            {
                EDBeamShapeThick = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
            }
        );
        ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(PvPrefix + ":EDBeamShapeXhairColour"),
            valueChangedHandler: (valueInfo, _) =>
            {
                var colour = System.Drawing.ColorTranslator.FromHtml(valueInfo.ValueAsString());
                if (colour != System.Drawing.Color.Empty)
                    EDBeamShapeColour = Colour.SystemDrawingColorToColour(colour);
            }
        );

        ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:EdgeDetectionContour"),
        valueChangedHandler: (valueInfo, _) =>
        {
          if ( valueInfo.Value is int[] intArray )
          {
            ContourDataSet = intArray.Select(
              i => (int) ( i * XDisplayScalingFactor )
            ).ToArray() ;
          }
          else
          {
            throw new System.ApplicationException(
              "Centroid1:EdgeDetectionContour is not of type int[]"
            );
          }
        }
      );
      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":SoftY"),
        valueChangedHandler: (valueInfo, _) =>
        {
          SoftwareY = Convert.ToInt32((double)valueInfo.Value * YDisplayScalingFactor);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ShowSoftXhair"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ShowSoftwareCrosshair = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":SoftWidth"),
        valueChangedHandler: (valueInfo, _) =>
        {
          SoftwareBoxWidth = Convert.ToInt32((double)valueInfo.Value);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":SoftHeight"),
        valueChangedHandler: (valueInfo, _) =>
        {
          SoftwareBoxHeight = Convert.ToInt32((double)valueInfo.Value);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ShowSoftBox"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ShowSoftwareBox = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ProfColour"),
        valueChangedHandler: (valueInfo, _) =>
        {
          var colour = System.Drawing.ColorTranslator.FromHtml(valueInfo.ValueAsString());
          if (colour != System.Drawing.Color.Empty)
            ProfileColour = Colour.SystemDrawingColorToColour(colour);
        }
      );
      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ContourXhairColour"),
        valueChangedHandler: (valueInfo, _) =>
        {
          var colour = System.Drawing.ColorTranslator.FromHtml(valueInfo.ValueAsString());
          if (colour != System.Drawing.Color.Empty)
            ContourColour = Colour.SystemDrawingColorToColour(colour);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":SoftColour"),
        valueChangedHandler: (valueInfo, _) =>
        {
          var colour = System.Drawing.ColorTranslator.FromHtml(valueInfo.ValueAsString());
          if (colour != System.Drawing.Color.Empty)
            SoftwareColour = Colour.SystemDrawingColorToColour(colour);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ProfXhairSize"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ProfileCrosshairSize = Convert.ToInt32((double)valueInfo.Value);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ShowContourXhair"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ShowContourCrosshair = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":ContourXhairThick"),
        valueChangedHandler: (valueInfo, _) =>
        {
          ContourCrosshairThick = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
        }
      );
      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":SoftXhairSize"),
        valueChangedHandler: (valueInfo, _) =>
        {
          SoftwareCrosshairSize = Convert.ToInt32((double)valueInfo.Value);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:FollowCentroid"),
        valueChangedHandler: (valueInfo, _) =>
        {
          FollowCentroid = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:CentroidX"),
        valueChangedHandler: (valueInfo, _) =>
        {
          EDCentroidX = Convert.ToInt32((int)valueInfo.Value * XDisplayScalingFactor);
          if (FollowCentroid)
            ProfileCrosshairX = Convert.ToInt32((int)valueInfo.Value * XDisplayScalingFactor);
        }
      );

      ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(PvPrefix + ":Centroid1:CentroidY"),
        valueChangedHandler: (valueInfo, _) =>
        {
          EDCentroidY = Convert.ToInt32((int)valueInfo.Value * YDisplayScalingFactor);
          if (FollowCentroid)
            ProfileCrosshairY = Convert.ToInt32((int)valueInfo.Value * YDisplayScalingFactor);
        }
      );
      
    }
      private void OnShowProfileGraphChange(int value)
      {
        if(value == 0)
        {
          VerticalProfileGraph.IntensityProfileGraph.IsVisible = false;
          HorizontalProfileGraph.IntensityProfileGraph.IsVisible = false; 
        }
        else
        {
          VerticalProfileGraph.IntensityProfileGraph.IsVisible = true;
          HorizontalProfileGraph.IntensityProfileGraph.IsVisible = true;
        }
    }

  }
}
