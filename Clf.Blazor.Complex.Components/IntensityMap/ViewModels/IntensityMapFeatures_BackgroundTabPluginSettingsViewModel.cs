using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;

using Clf.Blazor.Basic.Components.Controls.Widgets.Monitors;
using Clf.Blazor.Basic.Components.Controls.ViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_BackgroundTabPluginSettingsViewModel
  {
    public IntensityMapFeatures_CommonPluginSettingsViewModel BackgroundCommonPluginSettingsTab { get; }
    public string pageFileTitle;
    private string processPluginPrefix;
    public ActionButtonViewModel BackgroundSubtractionSaveFile { get; set; }
    public TextUpdateViewModel BackgroundSubtractionSaveFileRBV { get; set; }
    public ComboBoxViewModel BackgroundSubtractionEnable { get; set; }
    public TextUpdateViewModel BackgroundSubtractionEnableRBV { get; set; }
    public ActionButtonViewModel BackgroundSubtractionReadTIFF { get; set; }
    public ActionButtonViewModel BackgroundFlatFieldSave { get; set; }
    public TextUpdateViewModel BackgroundFlatFieldSaveRBV { get; set; }
    public ComboBoxViewModel BackgroundFlatFieldEnable { get; set; }
    public TextUpdateViewModel BackgroundFlatFieldEnableRBV { get; set; }
    public TextEntryViewModel BackgroundFlatFieldScale { get; set; }
    public TextUpdateViewModel BackgroundFlatFieldScaleRBV { get; set; }
    public ActionButtonViewModel BackgroundFlatFieldReadTIFF { get; set; }
    public ComboBoxViewModel BackgroundScaleEnable { get; set; }
    public TextUpdateViewModel BackgroundScaleEnableRBV { get; set; }
    public ActionButtonViewModel BackgroundScaleAuto { get; set; }
    public TextEntryViewModel BackgroundScaleValue { get; set; }
    public TextUpdateViewModel BackgroundScaleValueRBV { get; set; }
    public TextEntryViewModel BackgroundScaleOffsetValue { get; set; }
    public TextUpdateViewModel BackgroundScaleOffsetValueRBV { get; set; }
    public ComboBoxViewModel BackgroundClipEnableLow { get; set; }
    public TextUpdateViewModel BackgroundClipEnableLowRBV { get; set; }
    public TextEntryViewModel BackgroundClipLowValue { get; set; }
    public TextUpdateViewModel BackgroundClipLowValueRBV { get; set; }
    public ComboBoxViewModel BackgroundClipEnableHigh { get; set; }
    public TextUpdateViewModel BackgroundClipEnableHighRBV { get; set; }
    public TextEntryViewModel BackgroundClipHighValue { get; set; }
    public TextUpdateViewModel BackgroundClipHighValueRBV { get; set; }
    public ComboBoxViewModel BackgroundOutputDataType { get; set; }
    public TextUpdateViewModel BackgroundOutputDataTypeRBV { get; set; }
    public ComboBoxViewModel BackgroundFilterEnable { get; set; }
    public TextUpdateViewModel BackgroundFilterEnableRBV { get; set; }
    public TextEntryViewModel BackgroundFilterOrder { get; set; }
    public TextUpdateViewModel BackgroundFilterOrderRBV { get; set; }
    public TextUpdateViewModel BackgroundFilterRBV { get; set; }
    public ComboBoxViewModel BackgroundFilterType { get; set; }
    public ActionButtonViewModel BackgroundFilterReset { get; set; }
    public ComboBoxViewModel BackgroundFilterAutoReset { get; set; }
    public ComboBoxViewModel BackgroundFilterCallbacks { get; set; }
    public TextEntryViewModel BackgroundFilterOOffset { get; set; }
    public TextUpdateViewModel BackgroundFilterOOffsetRBV { get; set; }
    public TextEntryViewModel BackgroundFilterOScale { get; set; }
    public TextUpdateViewModel BackgroundFilterOScaleRBV { get; set; }
    public TextEntryViewModel BackgroundFilterOC1 { get; set; }
    public TextUpdateViewModel BackgroundFilterOC1RBV { get; set; }
    public TextEntryViewModel BackgroundFilterOC2 { get; set; }
    public TextUpdateViewModel BackgroundFilterOC2RBV { get; set; }
    public TextEntryViewModel BackgroundFilterOC3 { get; set; }
    public TextUpdateViewModel BackgroundFilterOC3RBV { get; set; }
    public TextEntryViewModel BackgroundFilterOC4 { get; set; }
    public TextUpdateViewModel BackgroundFilterOC4RBV { get; set; }
    public TextEntryViewModel BackgroundFilterFOffset { get; set; }
    public TextUpdateViewModel BackgroundFilterFOffsetRBV { get; set; }
    public TextEntryViewModel BackgroundFilterFScale { get; set; }
    public TextUpdateViewModel BackgroundFilterFScaleRBV { get; set; }
    public TextEntryViewModel BackgroundFilterFC1 { get; set; }
    public TextUpdateViewModel BackgroundFilterFC1RBV { get; set; }
    public TextEntryViewModel BackgroundFilterFC2 { get; set; }
    public TextUpdateViewModel BackgroundFilterFC2RBV { get; set; }
    public TextEntryViewModel BackgroundFilterFC3 { get; set; }
    public TextUpdateViewModel BackgroundFilterFC3RBV { get; set; }
    public TextEntryViewModel BackgroundFilterFC4 { get; set; }
    public TextUpdateViewModel BackgroundFilterFC4RBV { get; set; }
    public TextEntryViewModel BackgroundFilterROffset { get; set; }
    public TextUpdateViewModel BackgroundFilterROffsetRBV { get; set; }
    public TextEntryViewModel BackgroundFilterRC1 { get; set; }
    public TextUpdateViewModel BackgroundFilterRC1RBV { get; set; }
    public TextEntryViewModel BackgroundFilterRC2 { get; set; }
    public TextUpdateViewModel BackgroundFilterRC2RBV { get; set; }
    private ActionButtonViewModel CreateValueWriteActionButtonViewModel(string channelName, object valueToWrite)
    {
      return new ActionButtonViewModel(
        width: 45,
        height: 20
        )
      {
        OnActionButtonClicked = async () =>
        {
          await ChannelAccess.Hub.PutValueAsync(
            channelName,
            valueToWrite
          );
        }
      };
    }
    public IntensityMapFeatures_BackgroundTabPluginSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix)
    {
      processPluginPrefix = "Process1:";
      BackgroundCommonPluginSettingsTab = new IntensityMapFeatures_CommonPluginSettingsViewModel(channelsHandler, pvPrefix, processPluginPrefix);
      pageFileTitle = "Background Subtraction Plugin Settings  " + pvPrefix + processPluginPrefix;

      BackgroundSubtractionSaveFile = CreateValueWriteActionButtonViewModel(pvPrefix + processPluginPrefix + "SaveBackground", (short)1);
      BackgroundSubtractionSaveFile.Text = "Save";
      BackgroundSubtractionReadTIFF = CreateValueWriteActionButtonViewModel(pvPrefix + processPluginPrefix + "ReadBackgroundTIFFSeq.PROC", (short)1);
      BackgroundSubtractionReadTIFF.Text = "Read";
      BackgroundFlatFieldSave = CreateValueWriteActionButtonViewModel(pvPrefix + processPluginPrefix + "SaveFlatField", (short)1);
      BackgroundFlatFieldSave.Text = "Save";
      BackgroundFlatFieldReadTIFF = CreateValueWriteActionButtonViewModel(pvPrefix + processPluginPrefix + "ReadFlatFieldTIFFSeq", (short)1);
      BackgroundFlatFieldReadTIFF.Text = "Read";
      BackgroundScaleAuto = CreateValueWriteActionButtonViewModel(pvPrefix + processPluginPrefix + "AutoOffsetScale", (short)1);
      BackgroundScaleAuto.Text = "Auto";
      BackgroundFilterReset = CreateValueWriteActionButtonViewModel(pvPrefix + processPluginPrefix + "ResetFilter", (short)1);
      BackgroundFilterReset.Text = "Reset";

      BackgroundSubtractionEnable = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableBackground", channelsHandler)
      );
      BackgroundFlatFieldEnable = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableFlatField", channelsHandler)
      );
      BackgroundScaleEnable = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableOffsetScale", channelsHandler)
      );
      BackgroundClipEnableLow = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableLowClip", channelsHandler)
      );
      BackgroundClipEnableHigh = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableHighClip", channelsHandler)
      );
      BackgroundOutputDataType = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "DataTypeOut", channelsHandler)
      );
      BackgroundFilterEnable = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableFilter", channelsHandler)
      );
      BackgroundFilterType = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FilterType", channelsHandler)
      );
      BackgroundFilterAutoReset = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "AutoResetFilter", channelsHandler)
      );
      BackgroundFilterCallbacks = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FilterCallbacks", channelsHandler)
      );

      BackgroundSubtractionSaveFileRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "ValidBackground_RBV", channelsHandler)
      );
      BackgroundSubtractionEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableBackground_RBV", channelsHandler)
      );
      BackgroundFlatFieldSaveRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "ValidFlatField_RBV", channelsHandler)
      );
      BackgroundFlatFieldEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableFlatField_RBV", channelsHandler)
      );
      BackgroundFlatFieldScaleRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "ScaleFlatField_RBV", channelsHandler)
      );
      BackgroundScaleEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableOffsetScale_RBV", channelsHandler)
      );
      BackgroundScaleValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "Scale_RBV", channelsHandler)
      );
      BackgroundScaleOffsetValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "Offset_RBV", channelsHandler)
      );
      BackgroundClipEnableLowRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableLowClip_RBV", channelsHandler)
      );
      BackgroundClipLowValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "LowClip_RBV", channelsHandler)
      );
      BackgroundClipEnableHighRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableHighClip_RBV", channelsHandler)
      );
      BackgroundClipHighValueRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "HighClip_RBV", channelsHandler)
      );
      BackgroundOutputDataTypeRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "DataTypeOut_RBV", channelsHandler)
      );
      BackgroundFilterEnableRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "EnableFilter_RBV", channelsHandler)
      );
      BackgroundFilterOrderRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "NumFilter_RBV", channelsHandler)
      );
      BackgroundFilterRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "NumFiltered_RBV", channelsHandler)
      );
      BackgroundFilterOOffsetRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OOffset_RBV", channelsHandler)
      );                                                                                                                                                            
      BackgroundFilterOScaleRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OScale_RBV", channelsHandler)
      );
      BackgroundFilterOC1RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC1_RBV", channelsHandler)
      );
      BackgroundFilterOC2RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC2_RBV", channelsHandler)
      );
      BackgroundFilterOC3RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC3_RBV", channelsHandler)
      );
      BackgroundFilterOC4RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC4_RBV", channelsHandler)
      );
      BackgroundFilterFOffsetRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FOffset_RBV", channelsHandler)
      );
      BackgroundFilterFScaleRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FScale_RBV", channelsHandler)
      );
      BackgroundFilterFC1RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC1_RBV", channelsHandler)
      );
      BackgroundFilterFC2RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC2_RBV", channelsHandler)
      );
      BackgroundFilterFC3RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC3_RBV", channelsHandler)
      );
      BackgroundFilterFC4RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC4_RBV", channelsHandler)
      );
      BackgroundFilterROffsetRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "ROffset_RBV", channelsHandler)
      );
      BackgroundFilterRC1RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "RC1_RBV", channelsHandler)
      );
      BackgroundFilterRC2RBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "RC2_RBV", channelsHandler)
      );

      BackgroundFlatFieldScale = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "ScaleFlatField", channelsHandler)
      );
      BackgroundScaleValue = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "Scale", channelsHandler)
      );
      BackgroundScaleOffsetValue = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "Offset", channelsHandler)
      );
      BackgroundClipLowValue = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "LowClip", channelsHandler)
      );
      BackgroundClipHighValue = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "HighClip", channelsHandler)
      );
      BackgroundFilterOrder = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "NumFilter", channelsHandler)
      );
      BackgroundFilterOOffset = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OOffset", channelsHandler)
      );
      BackgroundFilterOScale = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OScale", channelsHandler)
      );
      BackgroundFilterOC1 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC1", channelsHandler)
      );
      BackgroundFilterOC2 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC2", channelsHandler)
      );
      BackgroundFilterOC3 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC3", channelsHandler)
      );
      BackgroundFilterOC4 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "OC4", channelsHandler)
      );
      BackgroundFilterFOffset = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FOffset", channelsHandler)
      );
      BackgroundFilterFScale = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FScale", channelsHandler)
      );
      BackgroundFilterFC1 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC1", channelsHandler)
      );
      BackgroundFilterFC2 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC2", channelsHandler)
      );
      BackgroundFilterFC3 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC3", channelsHandler)
      );
      BackgroundFilterFC4 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "FC4", channelsHandler)
      );
      BackgroundFilterROffset = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "ROffset", channelsHandler)
      );
      BackgroundFilterRC1 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "RC1", channelsHandler)
      );
      BackgroundFilterRC2 = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + processPluginPrefix + "RC2", channelsHandler)
      );                              
    
    
    
    
    }
  }
}
