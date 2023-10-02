//
// SpinnerWithSlider.razor.css
//

using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using System;

namespace Clf.Blazor.Basic.Components.Controls.Widgets.Updates
{
  [Obsolete]
  public partial class SpinnerWithSlider
  {

	  [Microsoft.AspNetCore.Components.Parameter]
    public SpinnerWithSliderViewModel ViewModel { get ; set ; } = new() ;

  }

}