//
// LogicSystemViewModel_configuration_methods.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem.ViewModel
{

  // Methods to configure the ViewModel, eg to install a different Clf.LogicSystem
  // or change the criteria that determine which Nodes are to be displayed.

  partial class LogicSystemViewModel 
  { 
  
    public void ConfigureLogicSystemAndWhichNodesToDisplay ( 
      Clf.LogicSystem.LogicSystemBase               logicSystem,
      Clf.LogicSystem.LogicNodeVisibilitySpecifier? logicNodeVisibilitySpecifier = null
    ) {
    
      m_logOfInterestingEvents?.Clear() ;
      m_logOfInterestingEvents?.AddLogEntry(
        $"Configuring logic system '{logicSystem.Name}'"
      ) ;

      // LoadLogicNetworkViewModelFromJsonFile(@"C:\temp\MachineSafety\LogicNetworkViewModel.json") ;
      // System.Console.WriteLine("InstallLogicSystem ...") ;

      InstallDifferentLogicSystem(
        logicSystem,
        logicNodeVisibilitySpecifier
      ) ;

      //
      // By setting the 'Clf.LogicSystem' property to null, we can simulate
      // what would happen if we'd restored a LogicNetworkViewModel from a JSON file.
      // LogicNetworkViewModel.LogicSystemFromWhichThisViewModelWasGenerated = new MachineSafety.EmptyLogicSystemWithNoNodes() ;
      //

      System.Console.WriteLine("ScanLogicSystem_BuildingAllScenarioMenuItems ...") ;
      // FIX_THIS_STEVE - do it differently, don't pass in a menu ??
      ScanLogicSystem_BuildingAllScenarioMenuItems(
        out var menuCreated
      ) ;

    }

    // 
    // The Clf.LogicSystem is primarily a property of the LogicSystemViewModel,
    // and whenever it changes to a different instance, we refresh the LogicSystemViewModel
    // configured to show the desired nodes. 
    //

    private void InstallDifferentLogicSystem ( 
      Clf.LogicSystem.LogicSystemBase               logicSystem, 
      Clf.LogicSystem.LogicNodeVisibilitySpecifier? logicNodeVisibilitySpecifier = null
    ) {
      m_logicSystem             = logicSystem ;
      m_networkLayoutDescriptor = null ;
      PopulateNodesAndLinks(
        ref m_networkLayoutDescriptor,
        logicNodeVisibilitySpecifier
      ) ;
      LogicSystemInstanceChanged?.Invoke() ;
      MainDescriptionTextLine = $"Logic System is a {LogicSystem.GetType().FullName!}" ;
    }

    public void RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded ( 
      Clf.LogicSystem.LogicNodeVisibilitySpecifier? logicNodeVisibilitySpecifier = null
    ) {
      m_networkLayoutDescriptor = null ;
      PopulateNodesAndLinks(
        ref m_networkLayoutDescriptor,
        logicNodeVisibilitySpecifier
      ) ;
      if ( 
        LogicNodeVisibilitySpecifier.SpecifiesAnchorNode( 
          out var anchorNode 
        )
      ) {
        this.ApplyHighlightingToAnchorNode(
          anchorNode
        ) ;
      }
      // Provoke the UI into doing a complete repaint
      this.RaisePropertyChangedEvent("REBUILD") ; 
    }

    public void RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded ( 
      IEnumerable<Clf.LogicSystem.LogicNodes.LogicNode> nodesToInclude, 
      Clf.LogicSystem.LogicNodes.LogicNode?             anchorNodeToAlsoInclude = null 
    ) {
      RepopulateNodesAndLinksFromSameLogicSystemButWithDifferentNodesIncluded(
        new LogicNodeVisibilitySpecifier.SpecifyingNodesToIncludeAndOptionalAnchorNode(
          nodesToInclude, 
          anchorNodeToAlsoInclude
        )
      ) ;
    }

  }

}

