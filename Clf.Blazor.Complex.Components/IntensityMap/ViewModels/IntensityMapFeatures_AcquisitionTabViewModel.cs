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
    private string camPluginPrefix;
    public ActionButtonViewModel StartAcquisition { get; }
    public ActionButtonViewModel StopAcquisition { get; }
    public TextUpdateViewModel AcquisitionStatusRBV { get; }
    public ComboBoxViewModel TriggerSource { get; }
    public ActionButtonViewModel SoftwareTrigger { get; }
    public TextEntryViewModel ExposureTimeSet { get; }
    public TextUpdateViewModel ExposureTimeRBV { get; set; }
    public TextEntryViewModel GainSet { get; }
    public TextUpdateViewModel GainRBV { get; set; }

    private ActionButtonViewModel CreateValueWriteActionButtonViewModel(string channelName, object valueToWrite)
    {
      return new ActionButtonViewModel(
        width: 45,
        height: 20
        )
      {
        OnActionButtonClicked = async () => {
          await ChannelAccess.Hub.PutValueAsync(
            channelName,
            valueToWrite
          );
        }
      };
    }
    public IntensityMapFeatures_AcquisitionTabViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;
      camPluginPrefix = "cam1:";
      StartAcquisition = CreateValueWriteActionButtonViewModel(parent.PvPrefix + camPluginPrefix + "Acquire", (short)1);
      StartAcquisition.Text = "Start";

      StopAcquisition = new ActionButtonViewModel(
        text: "Stop",
        width: 45,
        height: 20
        );

      TriggerSource = new ComboBoxViewModel(
      width: 100,
      height: 20,
      itemsFromPv: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "TriggerSource")
      );

      SoftwareTrigger = CreateValueWriteActionButtonViewModel(parent.PvPrefix + camPluginPrefix + "TriggerSoftware", (short)1);
      SoftwareTrigger.Text = "Software Trigger";
      SoftwareTrigger.Width = 120;

      ExposureTimeSet = new TextEntryViewModel(
      width: 100,
      height: 20,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ExposureTime")
      );

      ExposureTimeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ExposureTime_RBV")
      );

      GainSet = new TextEntryViewModel(
      showUnits: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "Gain")
      );

      GainRBV = new TextUpdateViewModel(
      width: 70,
      showUnits: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "Gain_RBV")
      );

      AcquisitionStatusRBV = new TextUpdateViewModel(
      width: 120,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "AcquireBusy")
      );
      IntensityMapFeatures_AcquisitionTabViewModel_Logic_Initiliasation();
    }
  }
}
