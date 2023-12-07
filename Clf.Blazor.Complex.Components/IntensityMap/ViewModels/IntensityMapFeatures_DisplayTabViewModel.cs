using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Models;
using System.ComponentModel;
using Clf.Common.ImageProcessing;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_DisplayTabViewModel: IDisposable
  {
    private IntensityMapViewerViewModel m_parent;

    public const string LOCALPV_AUTO_NORMALIZE = "Loc:AutoNormalize";
    public const string LOCALPV_MANUAL_NORMALIZE_VALUE = "Loc:ManualNormalizeValue";

    public TextUpdateViewModel EPICSName { get; set; }
    public TextUpdateViewModel ImageRateRBV { get; set; }
    public TextEntryViewModel ImageCounter { get; set; }
    public TextUpdateViewModel ImageCounterRBV { get; set; }
    public TextUpdateViewModel MaxPixelValueRBV { get; set; }
    public TextUpdateViewModel PixelBitDepthRBV { get; set; }
    public ComboBoxViewModel ColourMap { get; }
    public CheckboxViewModel AutoNormalise { get; }
    public SliderViewModel ManualNormalisationValue { get; }
    public CheckboxViewModel ShowProfileGraph { get; }
    public ActionButtonViewModel ExportProfileGraphX { get; }
    public ActionButtonViewModel ExportProfileGraphY { get; }
    public ActionButtonViewModel ExportImage { get; }
    public IntensityMapFeatures_DisplayTabViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;
      Hub.GetOrCreateLocalChannel(ChannelDescriptor.FromEncodedString($"{parent.PvPrefix + LOCALPV_AUTO_NORMALIZE}|i16|1"), true);
      Hub.GetOrCreateLocalChannel(ChannelDescriptor.FromEncodedString($"{parent.PvPrefix + LOCALPV_MANUAL_NORMALIZE_VALUE}|i32|255"), true);
      Hub.GetOrCreateLocalChannel(ChannelDescriptor.FromEncodedString($"{parent.PvPrefix + ":ShowProfileGraph"}|i16|1"), true);
      Hub.GetOrCreateLocalChannel(ChannelDescriptor.FromEncodedString($"{parent.PvPrefix+ ":ColourMap"}|enum:{string.Join(",", Enum.GetNames(typeof(ColourMapOption)).Select(name => name == "GreyScale" ? "Grey" : name))}|0"), true);


      EPICSName = new TextUpdateViewModel(
        text: $"{parent.PvPrefix}{parent.StreamPrefix}",
        borderStatus: BorderStatus.Connected
      );

      ImageRateRBV = new TextUpdateViewModel(
      precision: 2,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "ArrayRate_RBV")
      );

      ImageCounter = new TextEntryViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "ArrayCounter")
      );

      ImageCounterRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "ArrayCounter_RBV")
      );
      MaxPixelValueRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StatsPrefix + "MaxValue_RBV")
      );
      PixelBitDepthRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "PixelFormat_RBV")
      );
      ColourMap = new ComboBoxViewModel(
        itemsFromPv: true,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":ColourMap")
      );

      AutoNormalise = new CheckboxViewModel(
         label: "Auto",
         channelRecord: parent.CreateChannelRecord(parent.PvPrefix + LOCALPV_AUTO_NORMALIZE)
      );

      ManualNormalisationValue = new SliderViewModel(
        minimum: 0.0,
        maximum: 255.0,
        showSpinner: true,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + LOCALPV_MANUAL_NORMALIZE_VALUE)
      ) ;

      ShowProfileGraph = new CheckboxViewModel(
        label: "",
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":ShowProfileGraph")
      );

      ExportProfileGraphX = new ActionButtonViewModel(
        text: "X Profile"
        )
      {
        OnActionButtonClicked = OnExportProfileGraphXClicked
      };

      ExportProfileGraphY = new ActionButtonViewModel(
        text: "Y Profile"
        )
      {
        OnActionButtonClicked = OnExportProfileGraphYClicked
      };

      ExportImage = new ActionButtonViewModel(
        text: "Save Image"
        )
      {
        OnActionButtonClicked = OnExportImageClicked
      };

      parent.IntensityMapImage.ImageViewer.PropertyChanged += OnImageViewerPropertyChanged;
      IntensityMapFeatures_DisplayTabViewModel_Logic_Initialisation();
    }

    private void OnImageViewerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(m_parent.IntensityMapImage.ImageViewer.DisplayImageData))
      {
        if (m_parent.IntensityMapImage.ImageViewer.AutoNormalise)
        {
          ManualNormalisationValue.Value = Convert.ToDouble(m_parent.IntensityMapImage.ImageViewer.OriginalImageData.MaxValue);
          m_parent.IntensityMapImage.ImageViewer.NormalisationValue = m_parent.IntensityMapImage.ImageViewer.OriginalImageData.MaxValue;
        }
      }
    }

    public void Dispose()
    {
      m_parent.IntensityMapImage.ImageViewer.PropertyChanged -= OnImageViewerPropertyChanged;
    }
  }
}
