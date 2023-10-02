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

  public partial class IntensityMapFeatures_Hdf5TabPluginSettingsViewModel
  {
    public IntensityMapFeatures_CommonPluginSettingsViewModel HDF5CommonPluginSettingsTab { get; }
    public string pageFileTitle;
    private string hdf5PluginPrefix;
    public TextUpdateViewModel HDF5FilePathExistRBV { get; set; }
    public TextEntryViewModel HDF5FilePathSet { get; set; }
    public TextUpdateViewModel HDF5FilePathRBV { get; set; }
    public TextEntryViewModel HDF5DirDepthSet { get; set; }
    public TextUpdateViewModel HDF5DirDepthRBV { get; set; }
    public TextEntryViewModel HDF5FileNameSet { get; set; }
    public TextUpdateViewModel HDF5FileNameRBV { get; set; }
    public TextEntryViewModel HDF5NextFileSet { get; set; }
    public TextUpdateViewModel HDF5NextFileRBV { get; set; }
    public TextEntryViewModel HDF5TempSuffixSet { get; set; }
    public TextUpdateViewModel HDF5TempSuffixRBV { get; set; }
    public ComboBoxViewModel HDF5AutoIncrementSet { get; set; }
    public TextUpdateViewModel HDF5AutoIncrementRBV { get; set; }
    public ComboBoxViewModel HDF5LazyOpenSet { get; set; }
    public TextUpdateViewModel HDF5LazyOpenRBV { get; set; }
    public TextEntryViewModel HDF5FileFormatSet { get; set; }
    public TextUpdateViewModel HDF5FileFormatRBV { get; set; }
    public TextUpdateViewModel HDF5LastFileNameRBV { get; set; }
    public ActionButtonViewModel HDF5SaveFileSet { get; set; }
    public TextUpdateViewModel HDF5SaveFileRBV { get; set; }
    public ActionButtonViewModel HDF5ReadFileSet { get; set; }
    public TextUpdateViewModel HDF5ReadFileRBV { get; set; }
    public ComboBoxViewModel HDF5AutoSaveSet { get; set; }
    public TextUpdateViewModel HDF5AutoSaveRBV { get; set; }
    public ComboBoxViewModel HDF5WriteModeSet { get; set; }
    public TextUpdateViewModel HDF5WriteModeRBV { get; set; }
    public TextEntryViewModel HDF5CaptureSet { get; set; }
    public TextUpdateViewModel HDF5CaptureRBV { get; set; }
    public TextUpdateViewModel HDF5CapturedRBV { get; set; }
    public ActionButtonViewModel HDF5CaptureStart { get; set; }
    public ActionButtonViewModel HDF5CaptureStop { get; set; }
    public ComboBoxViewModel HDF5DeleteDriverFile { get; set; }
    public TextUpdateViewModel HDF5DeleteDriverFileRBV { get; set; }
    public TextUpdateViewModel HDF5WriteStatusRBV { get; set; }
    public TextUpdateViewModel HDF5WriteMessageRBV { get; set; }
    public ComboBoxViewModel HDF5CompressionSet { get; set; }
    public TextUpdateViewModel HDF5CompressionRBV { get; set; }
    public TextEntryViewModel HDF5DatabitsSet { get; set; }
    public TextUpdateViewModel HDF5DatabitsRBV { get; set; }
    public TextEntryViewModel HDF5OffsetbitsSet { get; set; }
    public TextUpdateViewModel HDF5OffsetbitsRBV { get; set; }
    public TextEntryViewModel HDF5SzipPixelSet { get; set; }
    public TextUpdateViewModel HDF5SzipPixelRBV { get; set; }
    public TextEntryViewModel HDF5ZlibLevelSet { get; set; }
    public TextUpdateViewModel HDF5ZlibLevelRBV { get; set; }
    public TextEntryViewModel HDF5JPEGQualitySet { get; set; }
    public TextUpdateViewModel HDF5JPEGQualityRBV { get; set; }
    public ComboBoxViewModel HDF5StorePerformance { get; set; }
    public TextUpdateViewModel HDF5StorePerformanceRBV { get; set; }
    public ComboBoxViewModel HDF5StoreAttributes { get; set; }
    public TextUpdateViewModel HDF5StoreAttributesRBV { get; set; }
    public TextUpdateViewModel HDF5RunTimeRBV { get; set; }
    public TextUpdateViewModel HDF5IOSpeedRBV { get; set; }
    public TextUpdateViewModel HDF5XMLErrorMessage { get; set; }
    public TextUpdateViewModel HDF5XMLFileExist { get; set; }
    public TextEntryViewModel HDF5XMLFileNameSet { get; set; }
    public TextUpdateViewModel HDF5XMLFileNameRBV { get; set; }
    public TextUpdateViewModel HDF5SWMRSupportedRBV { get; set; }
    public ComboBoxViewModel HDF5SWMRMode { get; set; }
    public TextUpdateViewModel HDF5SWMRModeRBV { get; set; }
    public TextUpdateViewModel HDF5SWMRActiveRBV { get; set; }
    public TextUpdateViewModel HDF5SWMRCallbacksRBV { get; set; }
    public ActionButtonViewModel HDF5SWMRFlushDisk { get; set; } 
    public ComboBoxViewModel  HDF5BloscShuffle { get; set; }
    public TextUpdateViewModel HDF5BloscShuffleRBV { get; set; }
    public ComboBoxViewModel HDF5BloscCompressor { get; set; }
    public TextUpdateViewModel HDF5BloscCompressorRBV { get; set; }
    public TextEntryViewModel HDF5BloscLevel { get; set; } 
    public TextUpdateViewModel HDF5BloscLevelRBV { get; set; }
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
    public IntensityMapFeatures_Hdf5TabPluginSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix)
    {
      hdf5PluginPrefix = "HDF1:";
      HDF5CommonPluginSettingsTab = new IntensityMapFeatures_CommonPluginSettingsViewModel(channelsHandler,pvPrefix, hdf5PluginPrefix);
      pageFileTitle = "HDF5 Plugin Settings  " + pvPrefix + hdf5PluginPrefix;
      HDF5FilePathSet = new TextEntryViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FilePath", channelsHandler)
      );
      HDF5DirDepthSet = new TextEntryViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "CreateDirectory", channelsHandler)
      );
      HDF5FileNameSet = new TextEntryViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileName", channelsHandler)
      );
      HDF5NextFileSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileNumber", channelsHandler)
      );
      HDF5TempSuffixSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "TempSuffix", channelsHandler)
      );
      HDF5FileFormatSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileTemplate", channelsHandler)
      );
      HDF5CaptureSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "NumCapture", channelsHandler)
      );
      HDF5DatabitsSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "NumDataBits", channelsHandler)
      );
      HDF5OffsetbitsSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "DataBitsOffset", channelsHandler)
      );
      HDF5SzipPixelSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "SZipNumPixels", channelsHandler)
      );
      HDF5ZlibLevelSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "ZLevel", channelsHandler)
      );
      HDF5JPEGQualitySet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "JPEGQuality", channelsHandler)
      );
      HDF5XMLFileNameSet = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "XMLFileName", channelsHandler)
      );
      HDF5BloscLevel = new TextEntryViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "BloscLevel", channelsHandler)
      );                                                                              


      HDF5FilePathExistRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FilePathExists_RBV", channelsHandler)
      );
      HDF5FilePathRBV = new TextUpdateViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FilePath_RBV", channelsHandler)
      );
      HDF5DirDepthRBV = new TextUpdateViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "CreateDirectory_RBV", channelsHandler)
      );
      HDF5FileNameRBV = new TextUpdateViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileName_RBV", channelsHandler)
      );
      HDF5NextFileRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileNumber_RBV", channelsHandler)
      );
      HDF5TempSuffixRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "TempSuffix_RBV", channelsHandler)
      );
      HDF5AutoIncrementRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "AutoIncrement_RBV", channelsHandler)
      );
      HDF5LazyOpenRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "LazyOpen_RBV", channelsHandler)
      );
      HDF5FileFormatRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileTemplate_RBV", channelsHandler)
      );
      HDF5LastFileNameRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FullFileName_RBV", channelsHandler)
      );
      HDF5SaveFileRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "WriteFile_RBV", channelsHandler)
      );
      HDF5ReadFileRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "ReadFile_RBV", channelsHandler)
      );
      HDF5AutoSaveRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "AutoSave_RBV", channelsHandler)
      );
      HDF5WriteModeRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileWriteMode_RBV", channelsHandler)
      );
      HDF5CaptureRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "NumCapture_RBV", channelsHandler)
      );
      HDF5CapturedRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "NumCaptured_RBV", channelsHandler)
      );      
      HDF5DeleteDriverFileRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "DeleteDriverFile_RBV", channelsHandler)
      );
      HDF5WriteStatusRBV = new TextUpdateViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "WriteStatus", channelsHandler)
      );
      HDF5WriteMessageRBV = new TextUpdateViewModel(
        width: 150,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "WriteMessage", channelsHandler)
      );
      HDF5CompressionRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "Compression_RBV", channelsHandler)
      );
      HDF5DatabitsRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "NumDataBits_RBV", channelsHandler)
      );
      HDF5OffsetbitsRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "DataBitsOffset_RBV", channelsHandler)
      );
      HDF5SzipPixelRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "SZipNumPixels_RBV", channelsHandler)
      );
      HDF5ZlibLevelRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "ZLevel_RBV", channelsHandler)
      );
      HDF5JPEGQualityRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "JPEGQuality_RBV", channelsHandler)
      );
      HDF5StorePerformanceRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "StorePerform_RBV", channelsHandler)
      );
      HDF5StoreAttributesRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "StoreAttr_RBV", channelsHandler)
      );
      HDF5RunTimeRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "RunTime", channelsHandler)
      );
      HDF5IOSpeedRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "IOSpeed", channelsHandler)
      );
      HDF5XMLErrorMessage = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "XMLErrorMsg_RBV", channelsHandler)
      );
      HDF5XMLFileExist = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "XMLValid_RBV", channelsHandler)
      );
      HDF5XMLFileNameRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "XMLFileName_RBV", channelsHandler)
      );
      HDF5SWMRSupportedRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "SWMRSupported_RBV", channelsHandler)
      );
      HDF5SWMRModeRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "SWMRMode_RBV", channelsHandler)
      );
      HDF5SWMRActiveRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "SWMRActive_RBV", channelsHandler)
      );
      HDF5SWMRCallbacksRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "SWMRCbCounter_RBV", channelsHandler)
      );
      HDF5BloscShuffleRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "BloscShuffle_RBV", channelsHandler)
      );
      HDF5BloscCompressorRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "BloscCompressor_RBV", channelsHandler)
      );
      HDF5BloscLevelRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "BloscLevel_RBV", channelsHandler)
      );

      HDF5AutoIncrementSet = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "AutoIncrement", channelsHandler)
      );
      HDF5LazyOpenSet = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "LazyOpen", channelsHandler)
      );
      HDF5WriteModeSet = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "FileWriteMode", channelsHandler)
      );
      HDF5AutoSaveSet = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "AutoSave", channelsHandler)
      );
      HDF5DeleteDriverFile = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "DeleteDriverFile", channelsHandler)
      );
      HDF5CompressionSet = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "Compression", channelsHandler)
      );
      HDF5StorePerformance = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "StorePerform", channelsHandler)
      );
      HDF5StoreAttributes = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "StoreAttr", channelsHandler)
      );
      HDF5SWMRMode = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "SWMRMode", channelsHandler)
      );
      HDF5BloscShuffle = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "BloscShuffle", channelsHandler)
      );
      HDF5BloscCompressor = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + hdf5PluginPrefix + "BloscCompressor", channelsHandler)
      );

      HDF5SaveFileSet = CreateValueWriteActionButtonViewModel(pvPrefix + hdf5PluginPrefix + "WriteFile", (short)1);
      HDF5SaveFileSet.Text = "Save";
      HDF5ReadFileSet = CreateValueWriteActionButtonViewModel(pvPrefix + hdf5PluginPrefix + "ReadFile", (short)1);
      HDF5ReadFileSet.Text = "Read";
      HDF5CaptureStart = CreateValueWriteActionButtonViewModel(pvPrefix + hdf5PluginPrefix + "Capture", (short)1);
      HDF5CaptureStart.Text = "Start";
      HDF5CaptureStop = CreateValueWriteActionButtonViewModel(pvPrefix + hdf5PluginPrefix + "Capture", (short)0);
      HDF5CaptureStop.Text = "Stop";
      HDF5SWMRFlushDisk = CreateValueWriteActionButtonViewModel(pvPrefix + hdf5PluginPrefix + "FlushNow", (short)1);
      HDF5SWMRFlushDisk.Text = "Start";                        
    }


    }
}

