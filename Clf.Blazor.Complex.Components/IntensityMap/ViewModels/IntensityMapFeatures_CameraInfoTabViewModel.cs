using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_CameraInfoTabViewModel
  {
    private string camPluginPrefix;
    public TextUpdateViewModel ManufacturerRBV { get; set; }
    public TextUpdateViewModel ModelNumberRBV { get; set; }
    public TextUpdateViewModel IdentificationRBV { get; set; }
    public TextUpdateViewModel IPAddressRBV { get; set; }
    public TextUpdateViewModel SizeXRBV { get; set; }
    public TextUpdateViewModel SizeYRBV { get; set; }
    public TextUpdateViewModel TemperatureRBV { get; set; }

    public IntensityMapFeatures_CameraInfoTabViewModel(IntensityMapViewerViewModel parent)
    {
      camPluginPrefix = ":cam1:";
      ManufacturerRBV = new TextUpdateViewModel(
      width: 150,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_DeviceVendorName_RBV")
      );

      ModelNumberRBV = new TextUpdateViewModel(
      width: 150,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_DeviceModelName_RBV")
      );

      IdentificationRBV = new TextUpdateViewModel(
      width: 150,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "GC_DeviceID_RBV")
      );

      IPAddressRBV = new TextUpdateViewModel(
      width: 150,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":DeviceIP_RBV")
      );

      SizeXRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "MaxSizeX_RBV")
      );

      SizeYRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "MaxSizeY_RBV")
      );

      TemperatureRBV = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + camPluginPrefix + "Temperature_RBV")
      );
    }

    }
}
