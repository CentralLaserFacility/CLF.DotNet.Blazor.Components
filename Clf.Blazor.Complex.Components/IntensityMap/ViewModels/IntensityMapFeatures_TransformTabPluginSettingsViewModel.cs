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

  public partial class IntensityMapFeatures_TransformTabPluginSettingsViewModel
  {
    public IntensityMapFeatures_CommonPluginSettingsViewModel TransformCommonPluginSettingsTab { get; }
    public string pageFileTitle;
    private string transPluginPrefix;
    public ComboBoxViewModel TransformTypeSet { get; set; }
    public TextUpdateViewModel TransformTypeRBV { get; set; }
    public TextUpdateViewModel TransformOutputArraySizeX { get; set; }
    public TextUpdateViewModel TransformOutputArraySizeY { get; set; }
    public IntensityMapFeatures_TransformTabPluginSettingsViewModel(ChannelsHandler channelsHandler, string pvPrefix)
    {
      transPluginPrefix = "Trans1:";
      TransformCommonPluginSettingsTab = new IntensityMapFeatures_CommonPluginSettingsViewModel(channelsHandler, pvPrefix, transPluginPrefix);
      pageFileTitle = "Transform Plugin Settings  " + pvPrefix + transPluginPrefix;
      TransformTypeRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + transPluginPrefix + "Type", channelsHandler)
      );
      TransformOutputArraySizeX = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + transPluginPrefix + "ArraySizeX_RBV", channelsHandler)
      );
      TransformOutputArraySizeY = new TextUpdateViewModel(
        width: 50,
        channelRecord: new ChannelRecord(pvPrefix + transPluginPrefix + "ArraySizeY_RBV", channelsHandler)
      );
      TransformTypeSet = new ComboBoxViewModel(
      width: 50,
      height: 20,
      itemsFromPv: true,
      channelRecord: new ChannelRecord(pvPrefix + transPluginPrefix + "Type", channelsHandler)
      );

    }
    }
}
