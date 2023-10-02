using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clf.Common.Drawing;
using Clf.Blazor.Basic.Components.Controls.Helpers;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels
{
  public class UpdateWidgetViewModelBase : WidgetViewModelBase
  {
    private bool _isDisabled = false;
    public bool IsDisabled
    {
      get { return _isDisabled; }
      set { SetProperty(ref _isDisabled, value); }
    }

    private BorderStatus _borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get { return _borderStatus; }
      set { SetProperty(ref _borderStatus, value); }
    }

    public UpdateWidgetViewModelBase(
      int? width = null,
      int? height = null,
      bool isVisible = true,
      bool isDisabled = false,
      BorderStatus borderStatus = BorderStatus.NotConnected,
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
      _isDisabled = isDisabled;
      _borderStatus = borderStatus;
    }

    public bool GetDisableStatus()
    {
      return Utilities.GetBorderStatusDisable(BorderStatus) || IsDisabled;
    }
  }
}
