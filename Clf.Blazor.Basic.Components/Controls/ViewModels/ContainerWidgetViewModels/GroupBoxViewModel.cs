using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels
{
    public class GroupBoxViewModel : ContainerWidgetViewModelBase
  {

    private Colour _borderColor;
    public Colour BorderColor
    {
      get => _borderColor;
      set => SetProperty(ref _borderColor, value?? GroupBoxStyle.DEFAULT_BORDER_COLOR); 
    }

    private bool _isDisabled = false;
    public bool IsDisabled
    {
      get { return _isDisabled; }
      set { SetProperty(ref _isDisabled, value); }
    }

		public Action? OnSettingsButtonClicked { get; set; } = null;

		public GroupBoxViewModel(
      string? title = null,
      int? width = null,
      int? height = null,
      bool isVisible=true,
      bool isDisabled = false,
      Action? onSettingsButtonClicked = null,
      Colour? borderColor=null )
    : base(
        isVisible: isVisible,
        title: title,
        width: width,
        height: height
        )
    {
      _borderColor = borderColor??GroupBoxStyle.DEFAULT_BORDER_COLOR;
      _isDisabled = isDisabled;
      OnSettingsButtonClicked = onSettingsButtonClicked;
    }
  }
}
