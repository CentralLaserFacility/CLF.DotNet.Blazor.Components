using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Clf.ChannelAccess;
using Clf.Common.Drawing;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels
{
    public class WidgetViewModelBase : ObservableObject, IWidgetViewModelBase
  {
    /// <value>Property <c>Width</c> represents the width of the Widget.</value>
    private int? _width;
    public int? Width
    {
      get { return _width; }
      set { SetProperty(ref _width, value); }
    }

        /// <value>Property <c>Height</c> represents the height of the Widget.</value>
    private int? _height;
    public int? Height
    {
      get { return _height; }
      set { SetProperty(ref _height, value); }
    }

    private bool _isVisible = true;
    public bool IsVisible
    {
      get { return _isVisible; }
      set { SetProperty(ref _isVisible, value); }
    }
    [Obsolete]
    private string _htmlFontString = Models.FontStyle.DEFAULT_FONT_STYLE;
    [Obsolete]
    public string HtmlFontString
    {
      get { return _htmlFontString; }
      set { SetProperty(ref _htmlFontString, value ?? Models.FontStyle.DEFAULT_FONT_STYLE); }
    }

    private string _tooltipText = string.Empty;
    public string TooltipText
    {
      get { return _tooltipText; }
      set { SetProperty(ref _tooltipText, value??string.Empty); }
    }

    private bool _showTooltip = true;
    public bool ShowTooltip
    {
      get { return _showTooltip; }
      set { SetProperty(ref _showTooltip, value); }
    }

    public WidgetViewModelBase(
      int? width=null,
      int? height=null,
      bool isVisible=true,
      bool showTooltip = true,
      string? fontStyle = null,
      string? tooltipText = null
      )
    {      
      _isVisible = isVisible;
      _tooltipText = tooltipText ?? string.Empty;
      _showTooltip = showTooltip;
      _width = width;
      _height = height;
    }
  }

  public record ChannelRecord (string ChannelName, ChannelsHandler ChannelsHandler)
  {
    public IChannel? Channel { get; private set; }
    public void InitialiseChannel(Action<bool, ChannelState> connectionStatusChangedHandler, Action<ValueInfo, ChannelState> valueChangedHandler)
    {
      if (!string.IsNullOrWhiteSpace(ChannelName))
        ChannelsHandler.InstallChannel(
        Channel = Hub.GetOrCreateChannel(ChannelName), 
        connectionStatusChangedHandler, valueChangedHandler
        );
    }
    public LedChannelRecord ToLedChannelRecord ( string LedOnValue )
    => new(this.ChannelName,this.ChannelsHandler,LedOnValue) ;

  }
  
  //Creating a record that will contain PvField (Channel Object)
  //and LedOnValue (a string for the "ON" value)
  public record LedChannelRecord (string ChannelName, ChannelsHandler ChannelsHandler, string LedOnValue): ChannelRecord(ChannelName,ChannelsHandler);
}
