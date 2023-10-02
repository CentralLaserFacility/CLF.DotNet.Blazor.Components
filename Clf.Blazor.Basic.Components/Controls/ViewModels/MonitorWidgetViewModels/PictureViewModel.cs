using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{

    public class PictureViewModel : MonitorWidgetViewModelBase
  {
    private string _imagePath = string.Empty;
    public string ImagePath
    {
      get => _imagePath;
      set => SetProperty(ref _imagePath, value ?? string.Empty);
    }

    public PictureViewModel(
      string? imagePath=null,
      string? fontStyle = null,
      string? tooltipText = null,
      int width = 0,
      int height = 0,
      bool isVisible = true,
      bool showTooltip = true
      )
    : base(
        isVisible: isVisible,
        fontStyle: fontStyle,
        tooltipText: tooltipText,
        showTooltip: showTooltip,
        width: width <= 0 ? PictureStyle.DEFAULT_WIDTH : width,
        height: height <= 0 ? PictureStyle.DEFAULT_HEIGHT : height
        )
    {
      _imagePath = imagePath ?? string.Empty;
      TooltipText = TooltipText == string.Empty ? Path.GetFileName(_imagePath)??string.Empty : TooltipText;
    }
  }
}
