using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Globalization;
using Clf.ChannelAccess.ExtensionMethods;
using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFixedDisplayViewModel
  {
    private string camPluginPrefix;
    private string imagePluginPrefix;
    private IntensityMapViewerViewModel m_parent;
    public TextUpdateViewModel TriggerModeRBV { get; set; }
    public TextUpdateViewModel NumFramesRBV { get; set; }
    public TextUpdateViewModel ArrayRateRBV { get; set; }
    public CheckboxViewModel ShowProfileGraph { get; }
    public ActionButtonViewModel ExportProfileGraph { get; }
    public ActionButtonViewModel ExportImage { get; }
    public ActionButtonViewModel ExportProfileGraphXViewModel { get; private set; }
    public ActionButtonViewModel ExportProfileGraphYViewModel { get; private set; }
    public ModalDialogViewModel ExportProfileGraphModal { get; }
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
    public IntensityMapFixedDisplayViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;
      camPluginPrefix = "cam1:";
      imagePluginPrefix = "image1:";
      TriggerModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "TriggerSource_RBV")
      );
      NumFramesRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + imagePluginPrefix + "ArrayCounter_RBV")
      );
      ArrayRateRBV = new TextUpdateViewModel(
      width: 70,
      precision: 2,
      showUnits: true,
      units: "Hz",
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "ArrayRate_RBV")
      );
      ShowProfileGraph = new CheckboxViewModel(
      label: "Show Profile Graph",
      width: 150,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + "ShowProfileGraph")
      );

      ExportImage = new ActionButtonViewModel();
      ExportImage.Text = "Export Image";

      ExportProfileGraphModal = new ModalDialogViewModel(
        title: "Export Profile Data",
        type: Basic.Components.Controls.Models.ModalTypes.OneButton,
        button1Text: "OK"
      );
      ExportProfileGraphModal.OnModalDialogButton1Clicked = () => { ExportProfileGraphModal.Close(); };

      ExportProfileGraph = new ActionButtonViewModel(
        text: "Export Profile"
        )
      {

        OnActionButtonClicked = () => { ExportProfileGraphModal.Open(); }
      };
      ExportProfileGraphXViewModel = new ActionButtonViewModel();
      ExportProfileGraphXViewModel.Text = "Export Profile X";
      ExportProfileGraphXViewModel.Height = 50;
      ExportProfileGraphXViewModel.Width = 110;
      ExportProfileGraphYViewModel = new ActionButtonViewModel();
      ExportProfileGraphYViewModel.Text = "Export Profile Y";
      ExportProfileGraphYViewModel.Height = 50;
      ExportProfileGraphYViewModel.Width = 110;
      
      IntensityMapFixedDisplayViewModel_Logic_Initiliasation();

    }
  }
}

