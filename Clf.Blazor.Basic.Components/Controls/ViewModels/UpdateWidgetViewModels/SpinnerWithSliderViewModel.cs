//
// SpinnerWithSliderViewModel.cs
//

using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using System;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  [Obsolete]
  public class SpinnerWithSliderViewModel
  {
    public SliderViewModel Slider { get; }
    public SpinnerViewModel Spinner { get; }

    public SpinnerWithSliderViewModel(
      ChannelRecord? channelRecord = null,
      double? minimum = null,
      double? maximum = null,
      double? value = null,
      double? increment = 1,
      bool limitsFromPv = false,
      int sliderWidth = 100,
      int spinnerWidth = 70
    )
    {
      Slider = new SliderViewModel(
        limitsFromPv: limitsFromPv,
        minimum: minimum,
        maximum: maximum,
        value: value,
        increment: increment,
        width: sliderWidth,
        channelRecord: channelRecord
      )
      {
        //ShowValue = false
      };
      Spinner = new SpinnerViewModel(
        limitsFromPv: limitsFromPv,
        value: value,
        width: spinnerWidth,
        channelRecord: channelRecord,
        minimum:minimum,
        maximum:maximum,
        increment: increment
      );
    }

  }

}
