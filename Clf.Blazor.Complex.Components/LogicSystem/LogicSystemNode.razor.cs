using Clf.LogicSystem.ViewModel;
using Microsoft.AspNetCore.Components ;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Clf.Blazor.Complex.LogicSystem
{

  public partial class LogicSystemNode : ComponentBase, System.IDisposable
  {

    [Parameter]
    [EditorRequired]
    public LogicNodeViewModel LogicNodeViewModel { get ; set ; } = null! ;

    [CascadingParameter]
    LogicSystemDisplayPanel ParentDisplayPanel { get ; set ; } 

    public float TextHeight  = 0.0f ;
    public float TextOffsetX = 0.0f ;
    public float TextOffsetY = 0.0f ;

    public LogicSystemNode( )
    {
    }

    protected override void OnParametersSet ( )
    {
      base.OnParametersSet() ;
      var layoutSizingParameters = LogicNodeViewModel.LayoutSizingParameters ;
      TextHeight  = layoutSizingParameters.CharacterCellSize.Height ;
      TextOffsetX = layoutSizingParameters.OffsetFromTopLeftPointToFirstCharacterBaselinePosition.Width ;
      TextOffsetY = layoutSizingParameters.OffsetFromTopLeftPointToFirstCharacterBaselinePosition.Height ;
    }

    //
    // We get an 'OnClick' for each and every element,
    // with the 'top' element event raised first.
    //
    // The 'MouseEventArgs' tells us the pixel coordinates of the click
    // but doesn't tell us anything about the id of the DOM node.
    //

    private int n = 0 ;

    // This is all we need, because it'll be always be invoked.

    public void OnGroupRightClicked ( MouseEventArgs mouseEventArgs ) 
    {
      // Client coordinates, which are the same as Page coordinates, are relative to
      // the top left hand corner of the entire page being shown in the browser window.
      System.Diagnostics.Debug.WriteLine(
        $"Node : group right-clicked (#{n++}) at {mouseEventArgs.ClientX:F3},{mouseEventArgs.ClientY:F3}"
      ) ;
      ParentDisplayPanel.ContextMenuDescriptor.ClearAllChildItems() ;
      LogicNodeViewModel.HandleMouseRightButtonEvent_PopulatingContextMenu(
        ParentDisplayPanel.ContextMenuDescriptor
      ) ;
      // FIX_THIS - invoke a method in the parent display panel,
      // which already contains code very similar to this ...
      if ( ParentDisplayPanel.ContextMenuDescriptor.ItemsCount > 0 )
      {
        if ( ParentDisplayPanel.ContextMenuService != null )
        {
          ParentDisplayPanel.ContextMenuService.OpenContextMenu(
            ParentDisplayPanel.ContextMenuDescriptor
          ) ;
        }
        else
        {
          ParentDisplayPanel.ContextMenuDescriptor.AddCancelAction() ;
          // ContextMenuDescriptor.ActionItemClickHandler = (menuItem) => OnContextMenuItemClicked() ;
          ParentDisplayPanel.ContextMenuDescriptor.ActionItemClickHandler = (menuItem) => {
            ParentDisplayPanel.ContextMenuDescriptor.ActionItemClickHandler = null ;
            ParentDisplayPanel.RaiseStateHasChanged() ;
          } ;
          ParentDisplayPanel.ContextMenuDescriptor.IsVisible = true ;
          ParentDisplayPanel.RaiseStateHasChanged() ;
        }
      }
    }

    public void OnGroupLeftClicked ( 
      // MouseEventArgs e_optional
    ) {
      // Hmm, suppose we wanted the id of the DOM node that was clicked on ??
      // https://stackoverflow.com/questions/59213382/how-to-obtain-the-target-element-on-click-event-in-blazor
      // https://www.syncfusion.com/faq/blazor/event-handling/how-do-you-get-the-target-element-onclick-event-in-blazor
      System.Diagnostics.Debug.WriteLine(
        $"Node : group left-clicked !! {n++}"
      ) ;
      LogicNodeViewModel.HandleMouseLeftButtonEvent() ;
      InvokeAsync(StateHasChanged) ;
      ParentDisplayPanel?.RaiseStateHasChanged() ;
    }

    public void OnTextLineClicked ( 
      // MouseEventArgs e_optional
    ) {
      // Hmm, suppose we want the id of the DOM node that was clicked on ??
      // https://stackoverflow.com/questions/59213382/how-to-obtain-the-target-element-on-click-event-in-blazor
      // https://www.syncfusion.com/faq/blazor/event-handling/how-do-you-get-the-target-element-onclick-event-in-blazor
      System.Diagnostics.Debug.WriteLine(
        $"Text line clicked !! {n++}"
      ) ;
    }

    public void OnBackgroundRectangleClicked ( 
      // MouseEventArgs e_optional
    ) {
      // Hmm, suppose we want the id of the DOM node that was clicked on ??
      // https://stackoverflow.com/questions/59213382/how-to-obtain-the-target-element-on-click-event-in-blazor
      // https://www.syncfusion.com/faq/blazor/event-handling/how-do-you-get-the-target-element-onclick-event-in-blazor
      System.Diagnostics.Debug.WriteLine(
        $"Background rectangle clicked !! {n++}"
      ) ;
    }

    protected override void OnInitialized ( )
    {
      base.OnInitialized() ;
      LogicNodeViewModel.PropertyChanged += OnPropertyChanged ;
    }

    public void OnPropertyChanged ( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
      InvokeAsync(StateHasChanged) ;
    }

    private int RenderNumber = 0 ;

    protected override void OnAfterRender ( bool isFirstRender )
    {
      RenderNumber++ ;
    }

    void IDisposable.Dispose()
    {
      LogicNodeViewModel.PropertyChanged -= OnPropertyChanged;
    }
  }

}