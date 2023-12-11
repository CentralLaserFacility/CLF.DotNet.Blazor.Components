//
// LogicSystemViewModel.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common;
using Clf.LogicSystem.Common.ExtensionMethods;

namespace Clf.LogicSystem.ViewModel
{

  //
  // The coordinate system used to specify positions and sizes
  // is pixel-oriented, ie a length of 10.0 refers to 10 pixels on screen.
  // That is largely driven by the way font sizes are represented,
  // and all our dimensions are ultimately tied to the sizes of
  // the textual lines that are displayed within the Box that
  // represents a LogicNode.
  //
  // The coordinate system has [0,0] at the top left, and Y increases downwards.
  // 
  // Note that by the time we actually display graphics on the screen,
  // a zoom factor will have been applied, magnifying the sizes and so on,
  // and panning may have moved the [0,0] reference point elsewhere.
  // Panning and zooming are however applied in the rendering code,
  // the ViewModel always deals in 'nominal pixels'.
  // 

  //
  // A LogicSystemViewModel is built by a LogicSystemDiagramViewModelBuilder
  // as a 'view' of a particular set of nodes in a particular Clf.Clf.LogicSystem.
  //
  // Depending on how the Builder was configured, the Diagram will show 
  // either the entire set of LogicNodes or just a filtered subset of them.
  //

  //
  // Mouse action handlers can be invoked from a View.
  //
  // On a left click on an 'Input' node, we might want to cycle the Value
  // of the Node eg from null->false->true. This involves several steps :
  //   1. Ask the LogicNodeViewModel for the ID of 
  //      the InputLogicNode it's associated with.
  //   2. Submit an InputValueChange based on the current Value,
  //      getting back a list of the LogicNodes that changed as a consequence.
  //   3. For each of the changed Nodes, apply the changed properties
  //      to the ViewModel that is associated with that Node.
  // Property-Changed notifications update the display.
  //
  // On a right-click, we might want to change the value as above,
  // or do something a little more ambitious, eg create a Scenario.
  //
  // When the mouse pointer enters or leaves the displayed Node, 
  // we could in principle change the outer border colour
  // to indicate that a 'click' would perform an Action on the Node.
  //
  // BUT, it's better to use border colour to indicate other 'status' aspects
  // such as whether there's a Value discrepancy for Computed nodes ...
  //

  public sealed partial class LogicSystemViewModel
  : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
  {

    public int InstanceNumber { get ; }

    private static int HowManyInstancesCreated = 0 ;

    private static int HowManyInstancesExisting = 0 ;

    private VisualisationOptions m_visualisationOptions = VisualisationOptions.Default ;

    public VisualisationOptions VisualisationOptions 
    { 
      get => m_visualisationOptions ; 
      set {
        if ( m_visualisationOptions != value )
        { 
          m_visualisationOptions = value;
          RebuildEntireDiagram() ;
        }
      }
    }

    public LogicSystemViewModel ( 
      LogicSystemBase                           logicSystem,
      LogicNodeVisibilitySpecifier?             logicNodeVisibilitySpecifier,
      Clf.LogicSystem.Common.UI.IProvidesUiServices uiServices,
      VisualisationOptions?                     visualisationOptions = null
    ) {
      InstanceNumber = HowManyInstancesCreated++ ;
      HowManyInstancesExisting++ ;
      m_logicSystem = logicSystem ;
      UiServices    = uiServices ;
      NetworkLayoutDescriptor? networkLayoutDescriptor = null ;
      if ( visualisationOptions != null )
      {
        VisualisationOptions = visualisationOptions ;
      }

#if OFFLINE_SIM && ENABLE_INPUT_CHANGES
      m_inputChangesAreEnabled = true;
#endif

      BackgroundCanvasViewModel = new LogicSystemCanvasViewModel(this) ;
      PopulateNodesAndLinks(
        ref networkLayoutDescriptor,
        logicNodeVisibilitySpecifier
      ) ;
    }

    public void DeregisterFromLogicNodeValueChangedEvents ( )
    {
      LogicNodeViewModels.ForEachItem(
        logicNodeViewModel => logicNodeViewModel.DeregisterFromLogicNodeValueChangedEvent()
      ) ;
    }

    internal void RaisePropertyChangedEvent ( string propertyName )
    {
      OnPropertyChanged(propertyName) ;
    }

    public override string ToString ( ) => $"{LogicSystem.ClassName()} #{InstanceNumber} ; {HowManyInstancesCreated} {HowManyInstancesExisting}" ;

  }

}

