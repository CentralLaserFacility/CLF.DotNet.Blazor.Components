//
// ILogicSystemViewModelElement.cs
//

using Clf.LogicSystem.Common;
using Clf.Common.MenuHandling;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace Clf.LogicSystem.ViewModel
{

  //
  // This interface is used to 'tag' the various elements
  // that make up a LogicSystemViewModel.
  //

  public interface ILogicSystemViewModelElement 
  : Clf.Common.UI.IProvidesTextualToolTipContent
  , Clf.Common.UI.IProvidesStackingOrderIndicatorZ
  , Clf.Common.UI.IHandlesMouseButtonEvents
  , System.ComponentModel.INotifyPropertyChanged
  { 
    LogicSystemViewModel Parent { get ; }
  }
  
  public abstract class LogicSystemViewModelElementBase 
  : ObservableObject
  , ILogicSystemViewModelElement
  {

    protected LogicSystemViewModelElementBase ( LogicSystemViewModel parent )
    {
      Parent = parent ;
      LayoutSizingParameters = Parent.LogicSystem.LayoutSizingParameters ;
    }

    public LogicSystemViewModel Parent { get ; }

    public LayoutSizingParameters LayoutSizingParameters { get ; }

    public virtual IEnumerable<string> ToolTipTextLines 
    => (
      new []{
        "Tooltip info is not available",
        $"for this '{this.GetType().Name}'"
      }
    ) ;

    public abstract int StackingOrderZ { get ; }

    protected void WriteLogMessage ( params string[] messages )
    {
      foreach ( string message in messages )
      {
        System.Diagnostics.Debug.WriteLine(message) ;
      }
    }

    public abstract void HandleMouseRightButtonEvent_PopulatingContextMenu ( MenuDescriptor contextMenu ) ;

    public abstract void HandleMouseLeftButtonEvent ( ) ;

  }
  
}

