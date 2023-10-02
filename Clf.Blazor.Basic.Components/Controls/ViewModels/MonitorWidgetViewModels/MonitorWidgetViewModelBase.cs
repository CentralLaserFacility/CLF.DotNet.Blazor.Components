using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Common.Drawing;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels
{
    public class MonitorWidgetViewModelBase : WidgetViewModelBase
    {
        public MonitorWidgetViewModelBase(
          int? width = null,
          int? height = null,
          bool isVisible = true,
          bool showTooltip = true,
          string? fontStyle = null,
          string? tooltipText = null
          )
         : base(
             isVisible: isVisible,
             fontStyle: fontStyle,
             tooltipText: tooltipText,
             showTooltip: showTooltip,
             width: width,
             height: height
             )
        {

        }
    }
}
