using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;

using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_BackgroundTabViewModel
  {
    private string processTiffPluginPrefix;
    private string processPluginPrefix;
    private string hdf5PluginPrefix;
    public ComboBoxViewModel BackgroundShow { get; set; }
    public ActionButtonViewModel BackgroundSetNewBackground { get; }
    public TextUpdateViewModel BackgroundSubtractionStatus { get; set; }
    public TextUpdateViewModel BackgroundSubtractionValidFile { get; set; }
    public ActionButtonViewModel BackgroundPluginSettings { get; set; }
    public TextEntryViewModel BackgroundFilePath { get; set; }
    public TextUpdateViewModel BackgroundFilePathRBV { get; set; }
    public TextEntryViewModel BackgroundFileName { get; set; }
    public TextUpdateViewModel BackgroundFileNameRBV { get; set; }
    public TextEntryViewModel BackgroundFileTemplate { get; set; }
    public TextUpdateViewModel BackgroundFileTemplateRBV { get; set; }
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
    public IntensityMapFeatures_BackgroundTabViewModel(IntensityMapViewerViewModel parent)
    {
      processPluginPrefix = ":Process1:";
      processTiffPluginPrefix = processPluginPrefix + "TIFF:";
      hdf5PluginPrefix = ":HDF1:";
      BackgroundShow = new ComboBoxViewModel(
        width: 70,
        height: 20,
        itemsFromPv: true,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":BackgroundTypeSel")
        );
      BackgroundSetNewBackground = CreateValueWriteActionButtonViewModel(parent.PvPrefix + hdf5PluginPrefix + "Capture", (short)0);
      BackgroundSetNewBackground.Text = "Set New Background";
      BackgroundSetNewBackground.Width = 170;
      BackgroundSetNewBackground.Height = 40;

      BackgroundSubtractionStatus = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processPluginPrefix + "EnableBackground_RBV")
      );

      BackgroundSubtractionValidFile = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processPluginPrefix + "ValidBackground_RBV")
      );
      BackgroundFileTemplate = new TextEntryViewModel(
      width: 170,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processTiffPluginPrefix + "FileTemplate")
      );
      BackgroundFileTemplateRBV = new TextUpdateViewModel(
      width: 170,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processTiffPluginPrefix + "FileTemplate_RBV")
      );
      BackgroundFileName = new TextEntryViewModel(
      width: 170,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processTiffPluginPrefix + "FileName")
      );
      BackgroundFileNameRBV = new TextUpdateViewModel(
      width: 170,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processTiffPluginPrefix + "FileName_RBV")
      );
      BackgroundFilePath = new TextEntryViewModel(
      width: 170,
      isVisible: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processTiffPluginPrefix + "FilePath")
      );
      BackgroundFilePathRBV = new TextUpdateViewModel(
      width: 170,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + processTiffPluginPrefix + "FilePath_RBV")
      );
      BackgroundPluginSettings = new ActionButtonViewModel(
        text: "Background Subtraction Plugin Settings",
        width: 300,
        height: 50
        );      
    }
  }
}