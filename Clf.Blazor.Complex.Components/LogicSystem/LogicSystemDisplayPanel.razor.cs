//
// DisplayPanel.razor.cs
//

using Clf.LogicSystem ;
using Clf.Common ;
using Clf.LogicSystem.Common.ExtensionMethods;
using Microsoft.AspNetCore.Components; 
using Microsoft.AspNetCore.Components.Web ;
using Clf.LogicSystem.ViewModel;
using Clf.Common.MenuHandling;

namespace Clf.Blazor.Complex.LogicSystem
{
  public partial class LogicSystemDisplayPanel : ComponentBase, System.IDisposable
  {

    [Parameter]
    [EditorRequired]
    public LogicSystemBase LogicSystem { get ; set ; } = null! ;
    
    [Parameter]
    public LogicNodeVisibilitySpecifier? LogicNodeVisibilitySpecifier { get ; set ; } = null ;

    [Parameter] // CASCADING ??? or inject this service ???
    public Clf.Common.MenuHandling.IContextMenuService? ContextMenuService { get ; set ; } = null ;

    // We should make this a property, with code in the setter to register/deregister.

    private LogicSystemViewModel LogicSystemViewModel = null! ;

    private LogicSystemCanvasViewModel DiagramCanvasViewModel = null! ;

    public ContextMenuDescriptor ContextMenuDescriptor { get ; set ; } = new ContextMenuDescriptor() ;

    public LogicSystemDisplayPanel ( )
    {
      RightClickPos_X_InPageCoordinates = "0px" ;
      RightClickPos_Y_InPageCoordinates = "0px" ;
      ContextMenuDescriptor.ActionItemClickHandler = (menuItem) => this.StateHasChanged() ;
    }

    protected override void OnParametersSet ( )
    {
      base.OnParametersSet() ;
      LogicSystemViewModel = new LogicSystemViewModel(
        LogicSystem,
        LogicNodeVisibilitySpecifier ?? new LogicNodeVisibilitySpecifier.SpecifyingAllNodes(),
        new Clf.LogicSystem.Common.UI.UiServices(
          LogOfInterestingEvents   : new Clf.Blazor.Common.Logging.LogEntriesCollection(),
          MenuDescriptorsProvider  : null!,
          ContextMenuService       : ContextMenuService,
          DialogHandler            : new Clf.Blazor.Common.Dialogs.DialogHandler()
        )
      ) ;
      LogicSystemViewModel.PropertyChanged += OnPropertyChanged ;
      DiagramCanvasViewModel = LogicSystemViewModel.BackgroundCanvasViewModel ;
    }

    private int RenderNumber = 0 ;

    protected override void OnAfterRender ( bool firstRender )
    {
      RenderNumber++ ;
    }

    public void OnLeftClick ( MouseEventArgs mouseEventArgs )
    {
      System.Diagnostics.Debug.WriteLine(
        $"Left click on DisplayPanel"
      ) ;
      System.Diagnostics.Debug.WriteLine(
        // Page coordinates are relative to the top left hand corner
        // of the entire page being shown in the browser window.
        $"  Page   : {mouseEventArgs.PageX:F3},{mouseEventArgs.PageY:F3}"
      ) ;
      System.Diagnostics.Debug.WriteLine(
        // Client coordinates are the same as Page coordinates, ie relative to
        // the top left hand corner of the entire page being shown in the browser window.
        $"  Client : {mouseEventArgs.ClientX:F3},{mouseEventArgs.ClientY:F3}"
      ) ;
      System.Diagnostics.Debug.WriteLine(
        // Offset coordinates are relative to the top left corner
        // of the div that raised the event
        $"  Offset : {mouseEventArgs.OffsetX:F3},{mouseEventArgs.OffsetY:F3}"
      ) ;
      // Screen coordinates are relative to the entire screen.
      // If the page is being displayed on the 'primary' monitor, the origin
      // corresponds to the top left corner of that monitor. However if the
      // page is being displayed on a second monitor, the screen coordinates
      // are nevertheless given in terms of the primary monitor !
      System.Diagnostics.Debug.WriteLine(
        $"  Screen : {mouseEventArgs.ScreenX:F3},{mouseEventArgs.ScreenY:F3}"
      ) ;
      LogicSystemViewModel.BackgroundCanvasViewModel.HandleMouseLeftButtonEvent() ;
    }

    // We use the coordinates of the right-button mouse click to 
    // assign the absolute position of the 'div' that holds our popup ContextMenu.
    // The coordinates are in terms of the Page coordinate system, with [0,0]
    // at the top left corner of the window hosted by the web browser.

    private string RightClickPos_X_InPageCoordinates { get ; set ; }
    private string RightClickPos_Y_InPageCoordinates { get ; set ; }

    private string ContextMenuTopLeftPos_X_InPageCoordinates { get ; set ; } = "30px" ;
    private string ContextMenuTopLeftPos_Y_InPageCoordinates { get ; set ; } = "80px" ;

    public void OnRightClick ( MouseEventArgs mouseEventArgs )
    {

      RightClickPos_X_InPageCoordinates = $"{mouseEventArgs.PageX:F3}px" ;
      RightClickPos_Y_InPageCoordinates = $"{mouseEventArgs.PageY:F3}px" ;

      System.Diagnostics.Debug.WriteLine(
        $"Right click on DisplayPanel at {RightClickPos_X_InPageCoordinates},{RightClickPos_Y_InPageCoordinates}"
      ) ;
      ContextMenuDescriptor.ClearAllChildItems() ;
      LogicSystemViewModel.BackgroundCanvasViewModel.HandleMouseRightButtonEvent_PopulatingContextMenu(
        ContextMenuDescriptor
      ) ;
      if ( ContextMenuDescriptor.ItemsCount > 0 )
      {
        if ( ContextMenuService != null )
        {
          ContextMenuService.OpenContextMenu(
            ContextMenuDescriptor
          ) ;
        }
        else
        {
          ContextMenuDescriptor.AddCancelAction() ;
          // ContextMenuDescriptor.ActionItemClickHandler = (menuItem) => OnContextMenuItemClicked() ;
          ContextMenuDescriptor.ActionItemClickHandler = (menuItem) => {
            ContextMenuDescriptor.ActionItemClickHandler = null ;
            this.StateHasChanged() ;
          } ;
          ContextMenuDescriptor.IsVisible = true ;
          // NOT NECESSARY TO CALL STATE_HAS_CHANGED HERE BECAUSE
          // WE'RE RESPONDING TO A UI EVENT ON THE COMPONENT ITSELF
          // StateHasChanged() ;
        }
      }
    }

    public void OnPropertyChanged ( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
      InvokeAsync(StateHasChanged) ;
    }

    // Schedule a complete repaint of this node and all its children

    public void RaiseStateHasChanged ( )
    {
      // Always better to do this asynchronously,
      // although a plain call to 'StateHasChanged()' also works ...
      InvokeAsync(StateHasChanged) ;
    }

    public void Dispose ( )
    {
      LogicSystemViewModel.PropertyChanged -= OnPropertyChanged ;
      // Deregister our interest in node changes
      LogicSystemViewModel.DeregisterFromLogicNodeValueChangedEvents() ;
    }

  }

}