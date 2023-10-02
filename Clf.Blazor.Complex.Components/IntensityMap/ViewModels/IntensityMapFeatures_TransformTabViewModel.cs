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

  public partial class IntensityMapFeatures_TransformTabViewModel
  {
    private string transPluginPrefix;
    public ComboBoxViewModel TransformTypeSet { get; set; }
    public TextUpdateViewModel TransformTypeRBV { get; set; }
    public TextUpdateViewModel TransformOutputArraySizeX { get; set; }
    public TextUpdateViewModel TransformOutputArraySizeY { get; set; }
    public ActionButtonViewModel TransformPluginSettings { get; }
    public IntensityMapFeatures_TransformTabViewModel(IntensityMapViewerViewModel parent)
    {
      transPluginPrefix = "Trans1:";
      TransformTypeSet = new ComboBoxViewModel(
      width: 100,
      height: 20,
      itemsFromPv: true,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + transPluginPrefix + "Type")
      );
      TransformTypeRBV = new TextUpdateViewModel(
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + transPluginPrefix + "Type")
      );
      TransformOutputArraySizeX = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + transPluginPrefix + "ArraySizeX_RBV")
      );
      TransformOutputArraySizeY = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + transPluginPrefix + "ArraySizeY_RBV")
      );
      TransformPluginSettings = new ActionButtonViewModel(
        text: "Transform Plugin Settings",
        height: 50,
        width: 150
        );
    }
    }
}
