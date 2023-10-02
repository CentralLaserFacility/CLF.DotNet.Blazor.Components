using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_DisplayTabViewModel
  {
    private string camPluginPrefix;
    private string statsPluginPrefix;
    private string ndStdArrayPluginPrefix;

    private IntensityMapViewerViewModel m_parent;

    public const string LOCALPV_AUTO_NORMALIZE = "Loc:AutoNormalize";
    public const string LOCALPV_MANUAL_NORMALIZE_VALUE = "Loc:ManualNormalizeValue";
    public TextUpdateViewModel ImageCounterRBV { get; set; }
    public TextUpdateViewModel MaxPixelValueRBV { get; set; }
    public TextUpdateViewModel PixelBitDepthRBV { get; set; }
    public ComboBoxViewModel ColourMap { get; }
    public SliderViewModel BrightnessSliderSet { get; set; }
    public SliderViewModel ContrastSliderSet { get; set; }
    public CheckboxViewModel AutoNormalise { get; }
    public SliderViewModel ManualNormalisationValue { get; }
    public IntensityMapFeatures_DisplayTabViewModel(IntensityMapViewerViewModel parent)
    {
      camPluginPrefix = "cam1:";
      statsPluginPrefix = "Stats1:";
      ndStdArrayPluginPrefix = "image1:";
      m_parent = parent;

      ImageCounterRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ndStdArrayPluginPrefix + "ArrayCounter_RBV")
      );
      MaxPixelValueRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + statsPluginPrefix + "MaxValue_RBV")
      );
      PixelBitDepthRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "PixelFormat_RBV")
      );
      ColourMap = new ComboBoxViewModel(
        width: 80,
        height: 20,
        items: new System.Collections.ObjectModel.ObservableCollection<string>(
          new[] {
            "Gray"
          , "Jet"
          , "Cool"
          , "Hot"
          , "Viridis"
          , "Magma"
          , "Spectrum"
          , "Red"
          , "Green"
          , "Blue"
          // , "SHADED"
          }
        ),
        itemsFromPv: false,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ColourMap")
      );
      BrightnessSliderSet = new SliderViewModel(
      limitsFromPv: true,
      showSpinner: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Brightness")
      );
      ContrastSliderSet = new SliderViewModel(
      limitsFromPv: true,
      showSpinner: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "Contrast")
      );

      Hub.GetOrCreateLocalChannel(ChannelDescriptor.FromEncodedString($"{parent.PvPrefix + LOCALPV_AUTO_NORMALIZE}|i16|1"), true);
      Hub.GetOrCreateLocalChannel(ChannelDescriptor.FromEncodedString($"{parent.PvPrefix + LOCALPV_MANUAL_NORMALIZE_VALUE}|i32|255"), true);

      AutoNormalise = new CheckboxViewModel(
         label: "Auto",
         width: 60,
         channelRecord: parent.CreateChannelRecord(parent.PvPrefix + LOCALPV_AUTO_NORMALIZE)
      );

      ManualNormalisationValue = new SliderViewModel(
        //width: 70,
        minimum: 0.0,
        maximum: 255.0,
        showSpinner: true,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + LOCALPV_MANUAL_NORMALIZE_VALUE)
      );
      IntensityMapFeatures_DisplayTabViewModel_Logic_Initiliasation();
    }
  }
}
