using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeaturesViewModel
  {
    private IntensityMapViewerViewModel parent;
    public IntensityMapFeatures_DisplayTabViewModel DisplayTab { get; }

    public IntensityMapFeatures_CentroidTabViewModel CentroidTab { get; }
    public IntensityMapFeatures_AcquisitionTabViewModel AcquisitionTab { get; }
    public IntensityMapFeatures_ROITabViewModel ROITab { get; }
    public IntensityMapFeatures_CameraInfoTabViewModel CameraInfoTab { get; }
    public IntensityMapFeatures_HDF5TabViewModel HDF5Tab { get; }
    public IntensityMapFeatures_BinningTabViewModel BinningTab { get; }
    public IntensityMapFeatures_TransformTabViewModel TransformTab { get; }
    public IntensityMapFeatures_StatisticsTabViewModel StatisticsTab { get; }
    public IntensityMapFeatures_KafkaTabViewModel KafkaTab { get; }
    public IntensityMapFeatures_BackgroundTabViewModel BackgroundTab { get; }
    public IntensityMapFeatures_AdvancedTabViewModel AdvancedTab { get; }
    public IntensityMapFeatures_OverlayTabViewModel OverlayTab { get; }


    public IntensityMapFeaturesViewModel(IntensityMapViewerViewModel intensityMapViewerViewModel)
    {
      this.parent = intensityMapViewerViewModel;
      DisplayTab = new IntensityMapFeatures_DisplayTabViewModel(parent);
      CentroidTab = new IntensityMapFeatures_CentroidTabViewModel(parent);
      AcquisitionTab = new IntensityMapFeatures_AcquisitionTabViewModel(parent);
      ROITab = new IntensityMapFeatures_ROITabViewModel(parent);
      CameraInfoTab = new IntensityMapFeatures_CameraInfoTabViewModel(parent);
      //HDF5Tab = new IntensityMapFeatures_HDF5TabViewModel(parent);
      BinningTab = new IntensityMapFeatures_BinningTabViewModel(parent);
      TransformTab = new IntensityMapFeatures_TransformTabViewModel(parent);
      StatisticsTab = new IntensityMapFeatures_StatisticsTabViewModel(parent);
      //KafkaTab = new IntensityMapFeatures_KafkaTabViewModel(parent);
      BackgroundTab = new IntensityMapFeatures_BackgroundTabViewModel(parent);
      AdvancedTab = new IntensityMapFeatures_AdvancedTabViewModel(parent);
      OverlayTab = new IntensityMapFeatures_OverlayTabViewModel(parent);
    }
  }
}
