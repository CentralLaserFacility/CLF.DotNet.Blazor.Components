using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.ViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_ROITabPluginSettingsViewModel
  {
    public IntensityMapFeatures_CommonPluginSettingsViewModel ROICommonPluginSettingsTab { get; }
    public string roiPageHeaderName;
    public string roiPluginPrefix;
    public TextEntryViewModel ROIName { get; }
    public ComboBoxViewModel ROIAutoDataTypeSet { get; }
    public TextUpdateViewModel ROIAutoDataTypeRBV { get; set; }
    public ComboBoxViewModel ROIEnableScalingSet { get; }
    public TextUpdateViewModel ROIEnableScalingRBV { get; set; }
    public TextEntryViewModel ROIScaleDivisorSet { get; }
    public TextUpdateViewModel ROIScaleDivisorRBV { get; set; }
    public ComboBoxViewModel ROICollapseDimsSet { get; }
    public TextUpdateViewModel ROICollapseDimsRBV { get; set; }
    public TextUpdateViewModel ROIInputSizeXRBV { get; set; }
    public TextUpdateViewModel ROIInputSizeYRBV { get; set; }
    public TextUpdateViewModel ROIInputSizeZRBV { get; set; }
    public ComboBoxViewModel ROIEnableXSet { get; }
    public TextUpdateViewModel ROIEnableXRBV { get; set; }
    public ComboBoxViewModel ROIEnableYSet { get; }
    public TextUpdateViewModel ROIEnableYRBV { get; set; }
    public ComboBoxViewModel ROIEnableZSet { get; }
    public TextUpdateViewModel ROIEnableZRBV { get; set; }
    public TextEntryViewModel ROIBinningXSet { get; }
    public TextUpdateViewModel ROIBinningXRBV { get; set; }
    public TextEntryViewModel ROIBinningYSet { get; }
    public TextUpdateViewModel ROIBinningYRBV { get; set; }
    public TextEntryViewModel ROIBinningZSet { get; }
    public TextUpdateViewModel ROIBinningZRBV { get; set; }
    public TextEntryViewModel ROIStartXSet { get; }
    public TextUpdateViewModel ROIStartXRBV { get; set; }
    public TextEntryViewModel ROIStartYSet { get; }
    public TextUpdateViewModel ROIStartYRBV { get; set; }
    public TextEntryViewModel ROIStartZSet { get; }
    public TextUpdateViewModel ROIStartZRBV { get; set; }
    public TextEntryViewModel ROISizeXSet { get; }
    public TextUpdateViewModel ROISizeXRBV { get; set; }
    public TextEntryViewModel ROISizeYSet { get; }
    public TextUpdateViewModel ROISizeYRBV { get; set; }
    public TextEntryViewModel ROISizeZSet { get; }
    public TextUpdateViewModel ROISizeZRBV { get; set; }
    public ComboBoxViewModel ROIAutoSizeXSet { get; }
    public TextUpdateViewModel ROIAutoSizeXRBV { get; set; }
    public ComboBoxViewModel ROIAutoSizeYSet { get; }
    public TextUpdateViewModel ROIAutoSizeYRBV { get; set; }
    public ComboBoxViewModel ROIAutoSizeZSet { get; }
    public TextUpdateViewModel ROIAutoSizeZRBV { get; set; }
    public TextUpdateViewModel ROIOutputSizeXRBV { get; set; }
    public TextUpdateViewModel ROIOutputSizeYRBV { get; set; }
    public TextUpdateViewModel ROIOutputSizeZRBV { get; set; }
    public ComboBoxViewModel ROIReverseXSet { get; }
    public TextUpdateViewModel ROIReverseXRBV { get; set; }
    public ComboBoxViewModel ROIReverseYSet { get; }
    public TextUpdateViewModel ROIReverseYRBV { get; set; }
    public ComboBoxViewModel ROIReverseZSet { get; }
    public TextUpdateViewModel ROIReverseZRBV { get; set; }

    public IntensityMapFeatures_ROITabPluginSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix )
    {
      roiPluginPrefix = "ROI1:";
      ROICommonPluginSettingsTab = new IntensityMapFeatures_CommonPluginSettingsViewModel(channelsHandler,pvPrefix, roiPluginPrefix);
      roiPageHeaderName = "ROI Plugin Settings:" + pvPrefix + roiPluginPrefix;
      ROIAutoDataTypeSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "DataTypeOut", channelsHandler)
      );
      ROIEnableScalingSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableScale", channelsHandler)
      );
      ROICollapseDimsSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "CollapseDims", channelsHandler)
      );
      ROIEnableXSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableX", channelsHandler)
      );
      ROIEnableYSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableY", channelsHandler)
      );
      ROIEnableZSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableZ", channelsHandler)
      );
      ROIAutoSizeXSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "AutoSizeX", channelsHandler)
      );
      ROIAutoSizeYSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "AutoSizeY", channelsHandler)
      );
      ROIAutoSizeZSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "AutoSizeZ", channelsHandler)
      );
      ROIReverseXSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ReverseX", channelsHandler)
      );
      ROIReverseYSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ReverseY", channelsHandler)
      );
      ROIReverseZSet = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ReverseZ", channelsHandler)
      );

      
      ROIAutoDataTypeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "DataTypeOut_RBV", channelsHandler)
      );
      ROIEnableScalingRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableScale_RBV", channelsHandler)
      );
      ROIScaleDivisorRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "Scale_RBV", channelsHandler)
      );
      ROICollapseDimsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "CollapseDims_RBV", channelsHandler)
      );
      ROIInputSizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MaxSizeX_RBV", channelsHandler)
      );
      ROIInputSizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MaxSizeY_RBV", channelsHandler)
      );
      ROIInputSizeZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MaxSizeZ_RBV", channelsHandler)
      );
      ROIEnableXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableX_RBV", channelsHandler)
      );
      ROIEnableYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableY_RBV", channelsHandler)
      );
      ROIEnableZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "EnableZ_RBV", channelsHandler)
      );
      ROIBinningXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "BinX_RBV", channelsHandler)
      );
      ROIBinningYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "BinY_RBV", channelsHandler)
      );
      ROIBinningZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "BinZ_RBV", channelsHandler)
      );
      ROIStartXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MinX_RBV", channelsHandler)
      );
      ROIStartYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MinY_RBV", channelsHandler)
      );
      ROIStartZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MinZ_RBV", channelsHandler)
      );
      ROISizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "SizeX_RBV", channelsHandler)
      );
      ROISizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "SizeY_RBV", channelsHandler)
      );
      ROISizeZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "SizeZ_RBV", channelsHandler)
      );
      ROIAutoSizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "AutoSizeX_RBV", channelsHandler)
      );
      ROIAutoSizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "AutoSizeY_RBV", channelsHandler)
      );
      ROIAutoSizeZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "AutoSizeZ_RBV", channelsHandler)
      );
      ROIOutputSizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ArraySizeX_RBV", channelsHandler)
      );
      ROIOutputSizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ArraySizeY_RBV", channelsHandler)
      );
      ROIOutputSizeZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ArraySizeZ_RBV", channelsHandler)
      );
      ROIReverseXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ReverseX_RBV", channelsHandler)
      );
      ROIReverseYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ReverseY_RBV", channelsHandler)
      );
      ROIReverseZRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "ReverseZ_RBV", channelsHandler)
      );

      ROIName = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "Name", channelsHandler)
      );
      ROIScaleDivisorSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "Scale", channelsHandler)
      );
      ROIBinningXSet = new TextEntryViewModel(
      width: 70, 
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "BinX", channelsHandler)
      );
      ROIBinningYSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "BinY", channelsHandler)
      );
      ROIBinningZSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "BinZ", channelsHandler)
      );
      ROIStartXSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MinX", channelsHandler)
      );
      ROIStartYSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MinY", channelsHandler)
      );
      ROIStartZSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "MinZ", channelsHandler)
      );
      ROISizeXSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "SizeX", channelsHandler)
      );
      ROISizeYSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "SizeY", channelsHandler)
      );
      ROISizeZSet = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + roiPluginPrefix + "SizeZ", channelsHandler)
      );
    }
    }
}
