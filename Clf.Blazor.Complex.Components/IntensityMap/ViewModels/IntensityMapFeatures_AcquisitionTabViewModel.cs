using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.ViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_AcquisitionTabViewModel
  {
    private IntensityMapViewerViewModel m_parent;
    public ActionButtonViewModel StartAcquisition { get; }
    public ActionButtonViewModel StopAcquisition { get; }
    public TextUpdateViewModel DetectorStateRBV { get;}
    public TextUpdateViewModel AcquisitionStatusRBV { get; }
    public ComboBoxViewModel TriggerSource{ get; }
    public TextUpdateViewModel TriggerSourceRBV { get;}
    public ComboBoxViewModel ImageMode { get;}
    public TextUpdateViewModel ImageModeRBV { get;}
    public TextEntryViewModel NumImages { get; }
    public TextUpdateViewModel NumImagesRBV { get; }
    public TextUpdateViewModel NumImagesCounterRBV { get; }
    //public ActionButtonViewModel SoftwareTrigger { get; }
    public TextEntryViewModel ExposureTime { get; }
    public TextUpdateViewModel ExposureTimeRBV { get; set; }
    public TextEntryViewModel AcquirePeriod { get; }
    public TextUpdateViewModel AcquirePeriodRBV { get; }
    public TextEntryViewModel Gain { get; }
    public TextUpdateViewModel GainRBV { get;}
    public TextUpdateViewModel ImageRateRBV { get; }
    public IntensityMapFeatures_AcquisitionTabViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;

      AcquisitionStatusRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "AcquireBusy")
      );

      StartAcquisition = new ActionButtonViewModel(
        text: "Start"
        )
      {
        OnActionButtonClicked = async () =>
        {
          await ChannelAccess.Hub.PutValueAsync(parent.PvPrefix + parent.StreamPrefix + "Acquire", (short)1);
        }
      };

      StopAcquisition = new ActionButtonViewModel(
        text: "Stop"
        )
      {
        OnActionButtonClicked = async () =>
        {
          await ChannelAccess.Hub.PutValueAsync(parent.PvPrefix + parent.StreamPrefix + "Acquire",(short)0);
        }
      };

      DetectorStateRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "DetectorState_RBV")
      );

      //SoftwareTrigger = CreateValueWriteActionButtonViewModel(parent.PvPrefix + camPluginPrefix + "TriggerSoftware", (short)1);
      //SoftwareTrigger.Text = "Software Trigger";

      ExposureTime = new TextEntryViewModel(
      units: "s",
      precision: 3,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "AcquireTime")
      );

      ExposureTimeRBV = new TextUpdateViewModel(
        units: "s",
        precision: 3,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "AcquireTime_RBV")
      );

      AcquirePeriod = new TextEntryViewModel(
        units: "s",
        precision: 3,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "AcquirePeriod")
      );

      AcquirePeriodRBV = new TextUpdateViewModel(
        units: "s",
        precision: 3,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "AcquirePeriod_RBV")
      );

      Gain = new TextEntryViewModel(
        precision: 3,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "Gain")
      );

      GainRBV = new TextUpdateViewModel(
        precision: 3,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "Gain_RBV")
      );

      TriggerSource = new ComboBoxViewModel(
      itemsFromPv: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "TriggerSource")
      );

      TriggerSourceRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "TriggerSource_RBV")
      );

      ImageMode = new ComboBoxViewModel(
      itemsFromPv: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "ImageMode")
      );

      ImageModeRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "ImageMode_RBV")
      );

      NumImages = new TextEntryViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "NumImages")
      );

      NumImagesRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "NumImages_RBV")
      );

      NumImagesCounterRBV = new TextUpdateViewModel(
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "NumImagesCounter_RBV")
      );

      ImageRateRBV = new TextUpdateViewModel(
      precision: 2,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + parent.StreamPrefix + "ArrayRate_RBV")
      );

      IntensityMapFeatures_AcquisitionTabViewModel_Logic_Initialisation();
    }
  }
}
