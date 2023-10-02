using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.ChannelAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;
using Clf.Common.Arithmatic;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
  public partial class IntensityMapFeatures_DisplayTabViewModel
  {
    private void IntensityMapFeatures_DisplayTabViewModel_Logic_Initiliasation()
    {
        m_parent.ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(m_parent.PvPrefix + LOCALPV_AUTO_NORMALIZE),
        valueChangedHandler: (valueInfo, _) =>
        {
          var isAuto = Converters.GetDoubleFromObject(valueInfo.Value) > 0;
          m_parent.IntensityMapImage.ImageViewer.AutoNormalise = isAuto;
          if (isAuto)
          {
            ManualNormalisationValue.IsDisabled = true;
            ManualNormalisationValue.IsDisabled = true;
          }
          else
          {
            ManualNormalisationValue.IsDisabled = false;
            ManualNormalisationValue.IsDisabled = false;
          }
        }
        );

        m_parent.ChannelsHandler.InstallChannel(
        Hub.GetOrCreateChannel(m_parent.PvPrefix + LOCALPV_MANUAL_NORMALIZE_VALUE),
        valueChangedHandler: (valueInfo, _) =>
        {
          m_parent.IntensityMapImage.ImageViewer.NormalisationValue = (byte)Converters.GetDoubleFromObject(valueInfo.Value);
        }
        );
        m_parent.ChannelsHandler.InstallChannel(
            Hub.GetOrCreateChannel(m_parent.PvPrefix + "ColourMap"),
            valueChangedHandler: (valueInfo, _) =>
            {
                m_parent.IntensityMapImage.ImageViewer.ColourMapOption = GetColourMapFromString(valueInfo.ValueAsString());
            }
        );
        }

        public static ColourMapOption GetColourMapFromString(string colourMap)
        {
            var result = colourMap switch
            {
                "Gray" => ColourMapOption.GreyScale,
                "Jet" => ColourMapOption.Jet,
                "Cool" => ColourMapOption.Cool,
                "Hot" => ColourMapOption.Hot,
                "Spectrum" => ColourMapOption.Spectrum,
                "Viridis" => ColourMapOption.Viridis,
                "Magma" => ColourMapOption.Magma,
                "Red" => ColourMapOption.Red,
                "Green" => ColourMapOption.Green,
                "Blue" => ColourMapOption.Blue,
                _ => ColourMapOption.GreyScale
            };
            return result;
        }

    }
}
