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

  public partial class IntensityMapFeatures_AdvancedTabCameraDriverAdvancedSettingsViewModel
  {
    private string camPluginPrefix;
    public string pageFileTitle;
    public TextEntryViewModel CameraFirmwareVersionMajor { get; set; }
    public TextUpdateViewModel CameraFirmwareVersionMajorRBV { get; set; }
    public TextEntryViewModel CameraFirmwareVersionMinor { get; set; }
    public TextUpdateViewModel CameraFirmwareVersionMinorRBV { get; set; }
    public TextEntryViewModel CameraFirmwareVersionBuild { get; set; }
    public TextUpdateViewModel CameraFirmwareVersionBuildRBV { get; set; }
    public ComboBoxViewModel CameraSensorType { get; set; }
    public TextUpdateViewModel CameraSensorTypeRBV { get; set; }
    public TextEntryViewModel CameraSensorBits { get; set; }
    public TextUpdateViewModel CameraSensorBitsRBV { get; set; }
    public TextUpdateViewModel CameraDeviceVendorNameRBV { get; set; }
    public TextUpdateViewModel CameraDeviceModelNameRBV { get; set; }
    public TextUpdateViewModel CameraDeviceFirmwareVersionRBV { get; set; }
    public TextUpdateViewModel CameraDeviceIDRBV { get; set; }
    public TextUpdateViewModel CameraDeviceUserIDRBV { get; set; }
    public TextUpdateViewModel CameraDevicePartNumberRBV { get; set; }
    public ComboBoxViewModel CameraDeviceScanType { get; set; }
    public TextUpdateViewModel CameraDeviceScanTypeRBV { get; set; }
    public ComboBoxViewModel CameraDeviceTemperatureSelector { get; set; }
    public TextUpdateViewModel CameraDeviceTemperatureSelectorRBV { get; set; }
    public TextEntryViewModel CameraDeviceTemperature { get; set; }
    public TextUpdateViewModel CameraDeviceTemperatureRBV { get; set; }
    public TextEntryViewModel CameraStreamBytesPerSecond { get; set; }
    public TextUpdateViewModel CameraStreamBytesPerSecondRBV { get; set; }
    public ComboBoxViewModel CameraBandwidthControlMode { get; set; }
    public TextUpdateViewModel CameraBandwidthControlModeRBV { get; set; }
    public TextEntryViewModel CameraGevSCPSPacketSize { get; set; }
    public TextUpdateViewModel CameraGevSCPSPacketSizeRBV { get; set; }
    public ComboBoxViewModel CameraChunkModeActive { get; set; }
    public TextUpdateViewModel CameraChunkModeActiveRBV { get; set; }
    public TextEntryViewModel CameraPayloadSize { get; set; }
    public TextUpdateViewModel CameraPayloadSizeRBV { get; set; }
    public TextEntryViewModel CameraNonImagePayloadSize { get; set; }
    public TextUpdateViewModel CameraNonImagePayloadSizeRBV { get; set; }
    public ComboBoxViewModel CameraStreamFrameRate { get; set; }
    public TextUpdateViewModel CameraStreamFrameRateRBV { get; set; }
    public ComboBoxViewModel CameraStreamHoldEnable { get; set; }
    public TextUpdateViewModel CameraStreamHoldEnableRBV { get; set; }
    public TextEntryViewModel CameraStreamHoldCapacity { get; set; }
    public TextUpdateViewModel CameraStreamHoldCapacityRBV { get; set; }
    public TextEntryViewModel CameraGevTimestampTickFrequency { get; set; }
    public TextUpdateViewModel CameraGevTimestampTickFrequencyRBV { get; set; }
    public ActionButtonViewModel CameraGevTimestampControlLatch { get; set; }
    public ActionButtonViewModel CameraGevTimestampControlReset { get; set; }
    public TextEntryViewModel CameraGevTimestampValue { get; set; }
    public TextUpdateViewModel CameraGevTimestampValueRBV { get; set; }
    public ActionButtonViewModel CameraAcquisitionStart { get; set; }
    public ActionButtonViewModel CameraAcquisitionStop { get; set; }
    public ActionButtonViewModel CameraAcquisitionAbort { get; set; }
    public ComboBoxViewModel CameraAcquisitionMode { get; set; }
    public TextUpdateViewModel CameraAcquisitionModeRBV { get; set; }
    public TextEntryViewModel CameraAcquisitionFrameCount { get; set; }
    public TextUpdateViewModel CameraAcquisitionFrameCountRBV { get; set; }
    public TextEntryViewModel CameraAcquisitionFrameRateAbs { get; set; }
    public TextUpdateViewModel CameraAcquisitionFrameRateAbsRBV { get; set; }
    public TextEntryViewModel CameraAcquisitionFrameRatelimit { get; set; }
    public TextUpdateViewModel CameraAcquisitionFrameRateLimitRBV { get; set; }
    public TextEntryViewModel CameraRecorderPreEventCount { get; set; }
    public TextUpdateViewModel CameraRecorderPreEventCountRBV { get; set; }
    public ComboBoxViewModel CameraTriggerSelector { get; set; }
    public TextUpdateViewModel CameraTriggerSelectorRBV { get; set; }
    public ComboBoxViewModel CameraTriggerMode { get; set; }
    public TextUpdateViewModel CameraTriggerModeRBV { get; set; }
    public ActionButtonViewModel CameraTriggerSoftware { get; set; }
    public ComboBoxViewModel CameraTriggerSource { get; set; }
    public TextUpdateViewModel CameraTriggerSourceRBV { get; set; }
    public ComboBoxViewModel CameraTriggerActivation { get; set; }
    public TextUpdateViewModel CameraTriggerActivationRBV { get; set; }
    public ComboBoxViewModel CameraTriggerOverlap { get; set; }
    public TextUpdateViewModel CameraTriggerOverlapRBV { get; set; }
    public TextEntryViewModel CameraTriggerDelayAbs { get; set; }
    public TextUpdateViewModel CameraTriggerDelayAbsRBV { get; set; }
    public TextEntryViewModel CameraSensorWidth { get; set; }
    public TextUpdateViewModel CameraSensorWidthRBV { get; set; }
    public TextEntryViewModel CameraSensorHeight { get; set; }
    public TextUpdateViewModel CameraSensorHeightRBV { get; set; }
    public ComboBoxViewModel CameraSensorTaps { get; set; }
    public TextUpdateViewModel CameraSensorTapsRBV { get; set; }
    public ComboBoxViewModel CameraSensorDigitizationTaps { get; set; }
    public TextUpdateViewModel CameraSensorDigitizationTapsRBV { get; set; }
    public TextEntryViewModel CameraBinningHorizontal { get; set; }
    public TextUpdateViewModel CameraBinningHorizontalRBV { get; set; }
    public TextEntryViewModel CameraBinningVertical { get; set; }
    public TextUpdateViewModel CameraBinningVerticalRBV { get; set; }
    public TextEntryViewModel CameraDecimationHorizontal { get; set; }
    public TextUpdateViewModel CameraDecimationHorizontalRBV { get; set; }
    public TextEntryViewModel CameraDecimationVertical { get; set; }
    public TextUpdateViewModel CameraDecimationVerticalRBV { get; set; }
    public ComboBoxViewModel CameraReverseX { get; set; }
    public TextUpdateViewModel CameraReverseXRBV { get; set; }
    public ComboBoxViewModel CameraReverseY { get; set; }
    public TextUpdateViewModel CameraReverseYRBV { get; set; }
    public TextEntryViewModel CameraWidthMax { get; set; }
    public TextUpdateViewModel CameraWidthMaxRBV { get; set; }
    public TextEntryViewModel CameraHeightMax { get; set; }
    public TextUpdateViewModel CameraHeightMaxRBV { get; set; }
    public ComboBoxViewModel CameraPixelFormat { get; set; }
    public TextUpdateViewModel CameraPixelFormatRBV { get; set; }
    public ComboBoxViewModel CameraSensorReadoutMode { get; set; }
    public TextUpdateViewModel CameraSensorReadoutModeRBV { get; set; }
    public TextEntryViewModel CameraWidth { get; set; }
    public TextUpdateViewModel CameraWidthRBV { get; set; }
    public TextEntryViewModel CameraHeight { get; set; }
    public TextUpdateViewModel CameraHeightRBV { get; set; }
    public TextEntryViewModel CameraOffsetX { get; set; }
    public TextUpdateViewModel CameraOffsetXRBV { get; set; }
    public TextEntryViewModel CameraOffsetY { get; set; }
    public TextUpdateViewModel CameraOffsetYRBV { get; set; }
    public TextEntryViewModel CameraImageSize { get; set; }
    public TextUpdateViewModel CameraImageSizeRBV { get; set; }
    public TextEntryViewModel CameraGamma { get; set; }
    public TextUpdateViewModel CameraGammaRBV { get; set; }
    public ComboBoxViewModel CameraDefectMaskEnable { get; set; }
    public TextUpdateViewModel CameraDefectMaskEnableRBV { get; set; }
    public TextEntryViewModel CameraHue { get; set; }
    public TextUpdateViewModel CameraHueRBV { get; set; }
    public TextEntryViewModel CameraSaturation { get; set; }
    public TextUpdateViewModel CameraSaturationRBV { get; set; }
    public TextEntryViewModel CameraDSPSubregionLeft { get; set; }
    public TextUpdateViewModel CameraDSPSubregionLeftRBV { get; set; }
    public TextEntryViewModel CameraDSPSubregionTop { get; set; }
    public TextUpdateViewModel CameraDSPSubregionTopRBV { get; set; }
    public TextEntryViewModel CameraDSPSubregionRight { get; set; }
    public TextUpdateViewModel CameraDSPSubregionRightRBV { get; set; }
    public TextEntryViewModel CameraDSPSubregionBottom { get; set; }
    public TextUpdateViewModel CameraDSPSubregionBottomRBV { get; set; }
    public ComboBoxViewModel CameraExposureAuto { get; set; }
    public TextUpdateViewModel CameraExposureAutoRBV { get; set; }
    public ComboBoxViewModel CameraExposureMode { get; set; }
    public TextUpdateViewModel CameraExposureModeRBV { get; set; }
    public TextEntryViewModel CameraExposureTimeAbs { get; set; }
    public TextUpdateViewModel CameraExposureTimeAbsRBV { get; set; }
    public TextEntryViewModel CameraExposureTimeIncrement { get; set; }
    public TextUpdateViewModel CameraExposureTimeIncrementRBV { get; set; }
    public TextEntryViewModel CameraExposureAutoTarget { get; set; }
    public TextUpdateViewModel CameraExposureAutoTargetRBV { get; set; }
    public ComboBoxViewModel CameraExposureAutoAlg { get; set; }
    public TextUpdateViewModel CameraExposureAutoAlgRBV { get; set; }
    public TextEntryViewModel CameraExposureAutoMin { get; set; }
    public TextUpdateViewModel CameraExposureAutoMinRBV { get; set; }
    public TextEntryViewModel CameraExposureAutoMax { get; set; }
    public TextUpdateViewModel CameraExposureAutoMaxRBV { get; set; }
    public TextEntryViewModel CameraExposureAutoRate { get; set; }
    public TextUpdateViewModel CameraExposureAutoRateRBV { get; set; }
    public TextEntryViewModel CameraExposureAutoOutliers { get; set; }
    public TextUpdateViewModel CameraExposureAutoOutliersRBV { get; set; }
    public TextEntryViewModel CameraExposureAutoAdjust { get; set; }
    public TextUpdateViewModel CameraExposureAutoAdjustRBV { get; set; }
    public ComboBoxViewModel CameraGainSelector { get; set; }
    public TextUpdateViewModel CameraGainSelectorRBV { get; set; }
    public TextEntryViewModel CameraGain { get; set; }
    public TextUpdateViewModel CameraGainRBV { get; set; }
    public TextEntryViewModel CameraGainRaw { get; set; }
    public TextUpdateViewModel CameraGainRawRBV { get; set; }
    public ComboBoxViewModel CameraGainAuto { get; set; }
    public TextUpdateViewModel CameraGainAutoRBV { get; set; }
    public TextEntryViewModel CameraGainAutoTarget { get; set; }
    public TextUpdateViewModel CameraGainAutoTargetRBV { get; set; }
    public TextEntryViewModel CameraGainAutoMin { get; set; }
    public TextUpdateViewModel CameraGainAutoMinRBV { get; set; }
    public TextEntryViewModel CameraGainAutoMax { get; set; }
    public TextUpdateViewModel CameraGainAutoMaxRBV { get; set; }
    public TextEntryViewModel CameraGainAutoRate { get; set; }
    public TextUpdateViewModel CameraGainAutoRateRBV { get; set; }
    public TextEntryViewModel CameraGainAutoOutliers { get; set; }
    public TextUpdateViewModel CameraGainAutoOutliersRBV { get; set; }
    public TextEntryViewModel CameraGainAutoAdjust { get; set; }
    public TextUpdateViewModel CameraGainAutoAdjustRBV { get; set; }
    private ActionButtonViewModel CreateValueWriteActionButtonViewModel(string channelName)
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
            Byte.MinValue
          );
        }
      };
    }
    public IntensityMapFeatures_AdvancedTabCameraDriverAdvancedSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix)
    {
      camPluginPrefix = "cam1:";
      pageFileTitle = "Camera Driver Advanced Settings  " + pvPrefix + camPluginPrefix;
      CameraFirmwareVersionMajor = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_FirmwareVerMajor", channelsHandler)
      );
      CameraFirmwareVersionMinor = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_FirmwareVerMinor", channelsHandler)
      );
      CameraFirmwareVersionBuild = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_FirmwareVerBuild", channelsHandler)
      );
      CameraSensorBits = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorBits", channelsHandler)
      );
      CameraDeviceTemperature = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceTemperature", channelsHandler)
      );
      CameraStreamBytesPerSecond = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StrBytesPerSecond", channelsHandler)
      );
      CameraGevSCPSPacketSize = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GevSCPSPacketSize", channelsHandler)
      );
      CameraPayloadSize = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_PayloadSize", channelsHandler)
      );
      CameraNonImagePayloadSize = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_NonImaPayloadSize", channelsHandler)
      );
      CameraStreamHoldCapacity = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StrHoldCapacity", channelsHandler)
      );
      CameraGevTimestampTickFrequency = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GevTimTicFre", channelsHandler)
      );
      CameraGevTimestampValue = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GevTimestampValue", channelsHandler)
      );
      CameraAcquisitionFrameCount = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcqFrameCount", channelsHandler)
      );
      CameraAcquisitionFrameRateAbs = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcqFrameRateAbs", channelsHandler)
      );
      CameraAcquisitionFrameRatelimit = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcqFrameRateLimit", channelsHandler)
      );
      CameraRecorderPreEventCount = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_RecPreEventCount", channelsHandler)
      );
      CameraTriggerDelayAbs = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerDelayAbs", channelsHandler)
      );
      CameraSensorWidth = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorWidth", channelsHandler)
      );
      CameraSensorHeight = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorHeight", channelsHandler)
      );
      CameraBinningHorizontal = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_BinningHorizontal", channelsHandler)
      );
      CameraBinningVertical = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_BinningVertical", channelsHandler)
      );
      CameraDecimationHorizontal = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DecHorizontal", channelsHandler)
      );
      CameraDecimationVertical = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DecVertical", channelsHandler)
      );
      CameraWidthMax = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_WidthMax", channelsHandler)
      );
      CameraHeightMax = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_HeightMax", channelsHandler)
      );
      CameraWidth = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Width", channelsHandler)
      );
      CameraHeight = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Height", channelsHandler)
      );
      CameraOffsetX = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_OffsetX", channelsHandler)
      );
      CameraOffsetY = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_OffsetY", channelsHandler)
      );
      CameraImageSize = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ImageSize", channelsHandler)
      );
      CameraGamma = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Gamma", channelsHandler)
      );
      CameraHue = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Hue", channelsHandler)
      );
      CameraSaturation = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Saturation", channelsHandler)
      );
      CameraDSPSubregionLeft = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubregionLeft", channelsHandler)
      );
      CameraDSPSubregionTop = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubregionTop", channelsHandler)
      );
      CameraDSPSubregionRight = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubregionRight", channelsHandler)
      );
      CameraDSPSubregionBottom = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubBottom", channelsHandler)
      );
      CameraExposureTimeAbs = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureTimeAbs", channelsHandler)
      );
      CameraExposureTimeIncrement = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpTimeIncrement", channelsHandler)
      );
      CameraExposureAutoTarget = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpAutoTarget", channelsHandler)
      );
      CameraExposureAutoMin = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoMin", channelsHandler)
      );
      CameraExposureAutoMax = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoMax", channelsHandler)
      );
      CameraExposureAutoRate = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoRate", channelsHandler)
      );
      CameraExposureAutoOutliers = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpAutoOutliers", channelsHandler)
      );
      CameraExposureAutoAdjust = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpAutoAdjustTol", channelsHandler)
      );
      CameraGain = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Gain", channelsHandler)
      );
      CameraGainRaw = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainRaw", channelsHandler)
      );
      CameraGainAutoTarget = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoTarget", channelsHandler)
      );
      CameraGainAutoMin = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoMin", channelsHandler)
      );
      CameraGainAutoMax = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoMax", channelsHandler)
      );
      CameraGainAutoRate = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoRate", channelsHandler)
      );
      CameraGainAutoOutliers = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoOutliers", channelsHandler)
      );
      CameraGainAutoAdjust = new TextEntryViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoAdjustTol", channelsHandler)
      );
      CameraFirmwareVersionMajorRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_FirmwareVerMajor_RBV", channelsHandler)
      );
      CameraFirmwareVersionMinorRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_FirmwareVerMinor_RBV", channelsHandler)
      );
      CameraFirmwareVersionBuildRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_FirmwareVerBuild_RBV", channelsHandler)
      );
      CameraSensorTypeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorType_RBV", channelsHandler)
      );
      CameraSensorBitsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorBits_RBV", channelsHandler)
      );
      CameraDeviceVendorNameRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceVendorName_RBV", channelsHandler)
      );
      CameraDeviceModelNameRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceModelName_RBV", channelsHandler)
      );
      CameraDeviceFirmwareVersionRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DevFirVersion_RBV", channelsHandler)
      );
      CameraDeviceIDRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceID_RBV", channelsHandler)
      );
      CameraDeviceUserIDRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceUserID_RBV", channelsHandler)
      );
      CameraDevicePartNumberRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DevicePartNumber_RBV", channelsHandler)
      );
      CameraDeviceScanTypeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceScanType_RBV", channelsHandler)
      );
      CameraDeviceTemperatureSelectorRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DevTemSelector_RBV", channelsHandler)
      );
      CameraDeviceTemperatureRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceTemperature_RBV", channelsHandler)
      );
      CameraStreamBytesPerSecondRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StrBytesPerSecond_RBV", channelsHandler)
      );
      CameraBandwidthControlModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_BanControlMode_RBV", channelsHandler)
      );
      CameraGevSCPSPacketSizeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GevSCPSPacketSize_RBV", channelsHandler)
      );
      CameraChunkModeActiveRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ChunkModeActive_RBV", channelsHandler)
      );
      CameraPayloadSizeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_PayloadSize_RBV", channelsHandler)
      );
      CameraNonImagePayloadSizeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_NonImaPayloadSize_RBV", channelsHandler)
      );
      CameraStreamFrameRateRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StrFraRatCon_RBV", channelsHandler)
      );
      CameraStreamHoldEnableRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StreamHoldEnable_RBV", channelsHandler)
      );
      CameraStreamHoldCapacityRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StrHoldCapacity_RBV", channelsHandler)
      );
      CameraGevTimestampTickFrequencyRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GevTimTicFre_RBV", channelsHandler)
      );
      CameraGevTimestampValueRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GevTimestampValue_RBV", channelsHandler)
      );
      CameraAcquisitionModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcquisitionMode_RBV", channelsHandler)
      );
      CameraAcquisitionFrameCountRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcqFrameCount_RBV", channelsHandler)
      );
      CameraAcquisitionFrameRateAbsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcqFrameRateAbs_RBV", channelsHandler)
      );
      CameraAcquisitionFrameRateLimitRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcqFrameRateLimit_RBV", channelsHandler)
      );
      CameraRecorderPreEventCountRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_RecPreEventCount_RBV", channelsHandler)
      );
      CameraTriggerSelectorRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerSelector_RBV", channelsHandler)
      );
      CameraTriggerModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerMode_RBV", channelsHandler)
      );
      CameraTriggerSourceRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerSource_RBV", channelsHandler)
      );
      CameraTriggerActivationRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerActivation_RBV", channelsHandler)
      );
      CameraTriggerOverlapRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerOverlap_RBV", channelsHandler)
      );
      CameraTriggerDelayAbsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerDelayAbs_RBV", channelsHandler)
      );
      CameraSensorWidthRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorWidth_RBV", channelsHandler)
      );
      CameraSensorHeightRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorHeight_RBV", channelsHandler)
      );
      CameraSensorTapsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorTaps_RBV", channelsHandler)
      );
      CameraSensorDigitizationTapsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SenDigTaps_RBV", channelsHandler)
      );
      CameraBinningHorizontalRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_BinningHorizontal_RBV", channelsHandler)
      );
      CameraBinningVerticalRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_BinningVertical_RBV", channelsHandler)
      );
      CameraDecimationHorizontalRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DecHorizontal_RBV", channelsHandler)
      );
      CameraDecimationVerticalRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DecVertical_RBV", channelsHandler)
      );
      CameraReverseXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ReverseX_RBV", channelsHandler)
      );
      CameraReverseYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ReverseY_RBV", channelsHandler)
      );
      CameraWidthMaxRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_WidthMax_RBV", channelsHandler)
      );
      CameraHeightMaxRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_HeightMax_RBV", channelsHandler)
      );
      CameraPixelFormatRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_PixelFormat_RBV", channelsHandler)
      );
      CameraSensorReadoutModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorReadoutMode_RBV", channelsHandler)
      );
      CameraWidthRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Width_RBV", channelsHandler)
      );
      CameraHeightRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Height_RBV", channelsHandler)
      );
      CameraOffsetXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_OffsetX_RBV", channelsHandler)
      );
      CameraOffsetYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_OffsetY_RBV", channelsHandler)
      );
      CameraImageSizeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ImageSize_RBV", channelsHandler)
      );
      CameraGammaRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Gamma_RBV", channelsHandler)
      );
      CameraDefectMaskEnableRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DefectMaskEnable_RBV", channelsHandler)
      );
      CameraHueRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Hue_RBV", channelsHandler)
      );
      CameraSaturationRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Saturation_RBV", channelsHandler)
      );
      CameraDSPSubregionLeftRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubregionLeft_RBV", channelsHandler)
      );
      CameraDSPSubregionTopRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubregionTop_RBV", channelsHandler)
      );
      CameraDSPSubregionRightRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubregionRight_RBV", channelsHandler)
      );
      CameraDSPSubregionBottomRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DSPSubBottom_RBV", channelsHandler)
      );
      CameraExposureAutoRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAuto_RBV", channelsHandler)
      );
      CameraExposureModeRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureMode_RBV", channelsHandler)
      );
      CameraExposureTimeAbsRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureTimeAbs_RBV", channelsHandler)
      );
      CameraExposureTimeIncrementRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpTimeIncrement_RBV", channelsHandler)
      );
      CameraExposureAutoTargetRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpAutoTarget_RBV", channelsHandler)
      );
      CameraExposureAutoAlgRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoAlg_RBV", channelsHandler)
      );
      CameraExposureAutoMinRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoMin_RBV", channelsHandler)
      );
      CameraExposureAutoMaxRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoMax_RBV", channelsHandler)
      );
      CameraExposureAutoRateRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoRate_RBV", channelsHandler)
      );
      CameraExposureAutoOutliersRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpAutoOutliers_RBV", channelsHandler)
      );
      CameraExposureAutoAdjustRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExpAutoAdjustTol_RBV", channelsHandler)
      );
      CameraGainSelectorRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainSelector_RBV", channelsHandler)
      );
      CameraGainRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_Gain_RBV", channelsHandler)
      );
      CameraGainRawRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainRaw_RBV", channelsHandler)
      );
      CameraGainAutoRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAuto_RBV", channelsHandler)
      );
      CameraGainAutoTargetRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoTarget_RBV", channelsHandler)
      );
      CameraGainAutoMinRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoMin_RBV", channelsHandler)
      );
      CameraGainAutoMaxRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoMax_RBV", channelsHandler)
      );
      CameraGainAutoRateRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoRate_RBV", channelsHandler)
      );
      CameraGainAutoOutliersRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoOutliers_RBV", channelsHandler)
      );
      CameraGainAutoAdjustRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAutoAdjustTol_RBV", channelsHandler)
      );
      CameraSensorType = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorType", channelsHandler)
      );
      CameraDeviceScanType = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DeviceScanType", channelsHandler)
      );
      CameraDeviceTemperatureSelector = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DevTemSelector", channelsHandler)
      );
      CameraBandwidthControlMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_BanControlMode", channelsHandler)
      );
      CameraChunkModeActive = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ChunkModeActive", channelsHandler)
      );
      CameraStreamFrameRate = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StrFraRatCon", channelsHandler)
      );
      CameraStreamHoldEnable = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_StreamHoldEnable", channelsHandler)
      );
      CameraAcquisitionMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_AcquisitionMode", channelsHandler)
      );
      CameraTriggerSelector = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerSelector", channelsHandler)
      );
      CameraTriggerMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerMode", channelsHandler)
      );
      CameraTriggerSource = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerSource", channelsHandler)
      );
      CameraTriggerActivation = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerActivation", channelsHandler)
      );
      CameraTriggerOverlap = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_TriggerOverlap", channelsHandler)
      );
      CameraSensorTaps = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorTaps", channelsHandler)
      );
      CameraSensorDigitizationTaps = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SenDigTaps", channelsHandler)
      );
      CameraReverseX = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ReverseX", channelsHandler)
      );
      CameraReverseY = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ReverseY", channelsHandler)
      );
      CameraPixelFormat = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_PixelFormat", channelsHandler)
      );
      CameraSensorReadoutMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_SensorReadoutMode", channelsHandler)
      );
      CameraDefectMaskEnable = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_DefectMaskEnable", channelsHandler)
      );
      CameraExposureAuto = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAuto", channelsHandler)
      );
      CameraExposureMode = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureMode", channelsHandler)
      );
      CameraExposureAutoAlg = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_ExposureAutoAlg", channelsHandler)
      );
      CameraGainSelector = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainSelector", channelsHandler)
      );
      CameraGainAuto = new ComboBoxViewModel(
      width: 70,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + camPluginPrefix + "GC_GainAuto", channelsHandler)
      );
      CameraGevTimestampControlLatch = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "GC_GevTimConLatch.PROC");
      CameraGevTimestampControlLatch.Text = "ControlLatch";
      CameraGevTimestampControlLatch.Width = 140;
      CameraGevTimestampControlReset = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "GC_GevTimConReset.PROC");
      CameraGevTimestampControlReset.Text = "ControlReset";
      CameraGevTimestampControlReset.Width = 140;
      CameraAcquisitionStart = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "GC_AcquisitionStart.PROC");
      CameraAcquisitionStart.Text = "AcquisitionStart";
      CameraAcquisitionStart.Width = 150;
      CameraAcquisitionStop = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "GC_AcquisitionStop.PROC");
      CameraAcquisitionStop.Text = "AcquisitionStop";
      CameraAcquisitionStop.Width = 150;
      CameraAcquisitionAbort = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "GC_AcquisitionAbort.PROC");
      CameraAcquisitionAbort.Text = "AcquisitionAbort";
      CameraAcquisitionAbort.Width = 150;
      CameraTriggerSoftware = CreateValueWriteActionButtonViewModel(pvPrefix + camPluginPrefix + "GC_TriggerSoftware.PROC");
      CameraTriggerSoftware.Text = "TriggerSoftware";
      CameraTriggerSoftware.Width = 150;
    }
  }
}
