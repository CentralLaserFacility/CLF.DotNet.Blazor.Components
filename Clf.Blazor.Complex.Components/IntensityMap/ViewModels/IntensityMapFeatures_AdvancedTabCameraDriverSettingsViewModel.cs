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

  public partial class IntensityMapFeatures_AdvancedTabCameraDriverSettingsViewModel
  {
    private string camPluginPrefix;
    public string pageFileTitle;
    public TextEntryViewModel CameraExposureTime { get; set; }
    public TextUpdateViewModel CameraExposureTimeRBV { get; set; }
    public ComboBoxViewModel CameraExposureAuto { get; set; }
    public TextUpdateViewModel CameraExposureAutoRBV { get; set; }
    public TextEntryViewModel CameraAcquirePeriod { get; set; }
    public TextUpdateViewModel CameraAcquirePeriodRBV { get; set; }
    public TextEntryViewModel CameraFrameRate { get; set; }
    public TextUpdateViewModel CameraFrameRateRBV { get; set; }
    public TextEntryViewModel CameraNumImages { get; set; }
    public TextUpdateViewModel CameraNumImagesRBV { get; set; }
    public TextUpdateViewModel CameraImagesCompleteRBV { get; set; }
    public ComboBoxViewModel CameraImageMode { get; set; }
    public TextUpdateViewModel CameraImageModeRBV { get; set; }
    public ActionButtonViewModel CameraAcquireStart { get; set; }
    public ActionButtonViewModel CameraAcquireStop { get; set; }
    public TextUpdateViewModel CameraQueuedArraysRBV { get; set; }
    public ComboBoxViewModel CameraWaitForPlugins { get; set; }
    public TextUpdateViewModel CameraAcquireBusyRBV { get; set; }
    public TextUpdateViewModel CameraDetectorStateRBV { get; set; }
    public TextUpdateViewModel CameraStatusRBV { get; set; }
    public TextEntryViewModel CameraImageCounter { get; set; }
    public TextUpdateViewModel CameraImageCounterRBV { get; set; }
    public TextUpdateViewModel CameraImageRateRBV { get; set; }
    public ComboBoxViewModel CameraArrayCallbacks { get; set; }
    public TextUpdateViewModel CameraArrayCallbacksRBV { get; set; }
    public ComboBoxViewModel CameraStatusRate { get; set; }
    public TextUpdateViewModel CameraFramesDeliveredRBV { get; set; }
    public TextUpdateViewModel CameraFramesDroppedRBV { get; set; }
    public TextUpdateViewModel CameraFramesUnderunsRBV { get; set; }
    public TextUpdateViewModel CameraPacketsReceivedRBV { get; set; }
    public TextUpdateViewModel CameraPacketsMissedRBV { get; set; }
    public TextUpdateViewModel CameraPacketsErrorsRBV { get; set; }
    public TextUpdateViewModel CameraPacketsRequestedRBV { get; set; }
    public TextUpdateViewModel CameraPacketsResentRBV { get; set; }
    public TextUpdateViewModel CameraTemperatureRBV { get; set; }
    public TextUpdateViewModel CameraSensorSizeXRBV { get; set; }
    public TextUpdateViewModel CameraSensorSizeYRBV { get; set; }
    public TextEntryViewModel CameraROIStartX { get; set; }
    public TextUpdateViewModel CameraROIStartXRBV { get; set; }
    public TextEntryViewModel CameraROIStartY { get; set; }
    public TextUpdateViewModel CameraROIStartYRBV { get; set; }
    public TextEntryViewModel CameraROISizeX { get; set; }
    public TextUpdateViewModel CameraROISizeXRBV { get; set; }
    public TextEntryViewModel CameraROISizeY { get; set; }
    public TextUpdateViewModel CameraROISizeYRBV { get; set; }
    public TextEntryViewModel CameraBinningX { get; set; }
    public TextUpdateViewModel CameraBinningXRBV { get; set; }
    public TextEntryViewModel CameraBinningY { get; set; }
    public TextUpdateViewModel CameraBinningYRBV { get; set; }
    public TextUpdateViewModel CameraImageSizeXRBV { get; set; }
    public TextUpdateViewModel CameraImageSizeYRBV { get; set; }
    public TextUpdateViewModel CameraImageBytesRBV { get; set; }
    public TextEntryViewModel CameraGain { get; set; }
    public TextUpdateViewModel CameraGainRBV { get; set; }
    public ComboBoxViewModel CameraGainAuto { get; set; }
    public TextUpdateViewModel CameraGainAutoRBV { get; set; }
    public TextUpdateViewModel CameraDataTypeRBV { get; set; }
    public TextUpdateViewModel CameraColourModeRBV { get; set; }
    public ComboBoxViewModel CameraPixelFormat { get; set; }
    public TextUpdateViewModel CameraPixelFormatRBV { get; set; }
    public ComboBoxViewModel CameraConvertFormat { get; set; }
    public TextUpdateViewModel CameraConvertFormatRBV { get; set; }
    public ComboBoxViewModel CameraTimestampMode { get; set; }
    public TextUpdateViewModel CameraTimestampModeRBV { get; set; }
    public ComboBoxViewModel CameraUniqueIDMode { get; set; }
    public TextUpdateViewModel CameraUniqueIDModeRBV { get; set; }
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
    public IntensityMapFeatures_AdvancedTabCameraDriverSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix)
    {
      camPluginPrefix = "cam1:";
      pageFileTitle = "Camera Driver Settings  " + pvPrefix + camPluginPrefix;
      CameraExposureAuto = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ExposureAuto", channelsHandler)
      );
      CameraImageMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ImageMode", channelsHandler)
      );
      CameraWaitForPlugins = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "WaitForPlugins", channelsHandler)
      );
      CameraArrayCallbacks = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArrayCallbacks", channelsHandler)
      );
      CameraStatusRate = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ReadStatus.SCAN", channelsHandler)
      );
      CameraGainAuto = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GainAuto", channelsHandler)
      );
      CameraPixelFormat = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "PixelFormat", channelsHandler)
      );
      CameraConvertFormat = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ConvertPixelFormat", channelsHandler)
      );
      CameraTimestampMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "TimeStampMode", channelsHandler)
      );
      CameraUniqueIDMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "UniqueIdMode", channelsHandler)
      );

      CameraExposureTime = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "AcquireTime", channelsHandler)
      );
      CameraAcquirePeriod = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "AcquirePeriod", channelsHandler)
      );
      CameraFrameRate = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "FrameRate", channelsHandler)
      );
      CameraNumImages = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "NumImages", channelsHandler)
      );
      CameraImageCounter = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArrayCounter", channelsHandler)
      );
      CameraROIStartX = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "MinX", channelsHandler)
      );
      CameraROIStartY = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "MinY", channelsHandler)
      );
      CameraROISizeX = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "SizeX", channelsHandler)
      );
      CameraROISizeY = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "SizeY", channelsHandler)
      );
      CameraBinningX = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "BinX", channelsHandler)
      );
      CameraBinningY = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "BinY", channelsHandler)
      );
      CameraGain = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "Gain", channelsHandler)
      );

      CameraExposureTimeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "AcquireTime_RBV", channelsHandler)
      );
      CameraExposureAutoRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ExposureAuto_RBV", channelsHandler)
      );
      CameraAcquirePeriodRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "AcquirePeriod_RBV", channelsHandler)
      );
      CameraFrameRateRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "FrameRate_RBV", channelsHandler)
      );
      CameraNumImagesRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "NumImages_RBV", channelsHandler)
      );
      CameraImagesCompleteRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "NumImagesCounter_RBV", channelsHandler)
      );
      CameraImageModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ImageMode_RBV", channelsHandler)
      );
      CameraQueuedArraysRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "NumQueuedArrays", channelsHandler)
      );
      CameraAcquireBusyRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "AcquireBusy", channelsHandler)
      );
      CameraDetectorStateRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "DetectorState_RBV", channelsHandler)
      );
      CameraStatusRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "StatusMessage_RBV", channelsHandler)
      );
      CameraImageCounterRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArrayCounter_RBV", channelsHandler)
      );
      CameraImageRateRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArrayRate_RBV", channelsHandler)
      );
      CameraArrayCallbacksRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArrayCallbacks_RBV", channelsHandler)
      );
      CameraFramesDeliveredRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatFrameDelivered_RBV", channelsHandler)
      );
      CameraFramesDroppedRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatFrameDropped_RBV", channelsHandler)
      );
      CameraFramesUnderunsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatFrameUnderrun_RBV", channelsHandler)
      );
      CameraPacketsReceivedRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatPacketReceived_RBV", channelsHandler)
      );
      CameraPacketsMissedRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatPacketMissed_RBV", channelsHandler)
      );
      CameraPacketsErrorsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatPacketErrors_RBV", channelsHandler)
      );
      CameraPacketsRequestedRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatPacketRequested_RBV", channelsHandler)
      );
      CameraPacketsResentRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StatPacketResent_RBV", channelsHandler)
      );
      CameraTemperatureRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "TemperatureActual", channelsHandler)
      );
      CameraSensorSizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "MaxSizeX_RBV", channelsHandler)
      );
      CameraSensorSizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "MaxSizeY_RBV", channelsHandler)
      );
      CameraROIStartXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "MinX_RBV", channelsHandler)
      );
      CameraROIStartYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "MinY_RBV", channelsHandler)
      );
      CameraROISizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "SizeX_RBV", channelsHandler)
      );
      CameraROISizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "SizeY_RBV", channelsHandler)
      );
      CameraBinningXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "BinX_RBV", channelsHandler)
      );
      CameraBinningYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "BinY_RBV", channelsHandler)
      );
      CameraImageSizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArraySizeX_RBV", channelsHandler)
      );
      CameraImageSizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArraySizeY_RBV", channelsHandler)
      );
      CameraImageBytesRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ArraySize_RBV", channelsHandler)
      );
      CameraGainRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "Gain_RBV", channelsHandler)
      );
      CameraGainAutoRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GainAuto_RBV", channelsHandler)
      );
      CameraDataTypeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "DataType_RBV", channelsHandler)
      );
      CameraColourModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ColorMode_RBV", channelsHandler)
      );
      CameraPixelFormatRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "PixelFormat_RBV", channelsHandler)
      );
      CameraConvertFormatRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "ConvertPixelFormat_RBV", channelsHandler)
      );
      CameraTimestampModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "TimeStampMode_RBV", channelsHandler)
      );
      CameraUniqueIDModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "UniqueIdMode_RBV", channelsHandler)
      );
      CameraAcquireStart = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "Acquire", (short)1);
      CameraAcquireStart.Text = "Start";
      CameraAcquireStop = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "Acquire", (short)0);
      CameraAcquireStop.Text = "Stop";
    }
  }
}
