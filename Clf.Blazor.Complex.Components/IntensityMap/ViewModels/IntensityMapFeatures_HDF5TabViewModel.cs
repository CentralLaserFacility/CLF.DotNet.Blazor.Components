using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;

using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_HDF5TabViewModel
  {
    private string hdf5PluginPrefix;
    public ActionButtonViewModel StartHdf5Capture { get; }
    public ActionButtonViewModel StopHdf5Capture { get; }
    public TextUpdateViewModel Hdf5CaptureRBV { get; set; }
    public ActionButtonViewModel Hdf5PluginSettings { get; }
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
    public IntensityMapFeatures_HDF5TabViewModel(IntensityMapViewerViewModel parent)
    {
      hdf5PluginPrefix = ":HDF1:";
      StartHdf5Capture = CreateValueWriteActionButtonViewModel(parent.PvPrefix + hdf5PluginPrefix + "Capture", (short)1);
      StartHdf5Capture.Text = "Start";
      StartHdf5Capture.Width = 60;

      StopHdf5Capture = CreateValueWriteActionButtonViewModel(parent.PvPrefix + hdf5PluginPrefix + "Capture", (short)0);
      StopHdf5Capture.Text = "Stop";
      StopHdf5Capture.Width = 60;

      Hdf5CaptureRBV = new TextUpdateViewModel(
      width: 60,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + hdf5PluginPrefix + "Capture_RBV")
      );

      Hdf5PluginSettings = new ActionButtonViewModel(
        text: "HDF5 Plugin Settings",
        width: 150
        );


    }
  }
}
