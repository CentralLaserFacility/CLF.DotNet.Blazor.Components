//
// LogicSystemViewModel_helper_methods.cs
//

using Clf.LogicSystem.LogicNodes;
using Clf.LogicSystem.Miscellaneous;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Clf.LogicSystem.ViewModel
{

  // Methods to get from ViewModel instance to Clf.LogicSystem instance, etc.

  partial class LogicSystemViewModel 
  { 
  
    internal LogicNodeViewModel LocateViewModelForLogicNode ( LogicNode logicNode ) 
    => LocateViewModelForNodeId(logicNode.UniqueIntegerIdentifier) ;
    
    internal LogicNodeViewModel LocateViewModelForNodeId ( int id ) => LogicNodeViewModels.Single(
      logicNodeViewModel => logicNodeViewModel.UniqueIntegerIdentifier == id
    ) ;
    
    internal LogicNodeViewModel? LocateViewModelForLogicNodeIfAvailable ( LogicNode logicNode ) 
    => LocateViewModelForNodeIdIfAvailable(logicNode.UniqueIntegerIdentifier) ;
    
    internal LogicNodeViewModel? LocateViewModelForNodeIdIfAvailable ( int id ) => LogicNodeViewModels.SingleOrDefault(
      logicNodeViewModel => logicNodeViewModel.UniqueIntegerIdentifier == id
    ) ;
    
    internal bool CanLocateViewModelForLogicNode ( 
      LogicNode                                   logicNode, 
      [NotNullWhen(true)] out LogicNodeViewModel? viewModel 
    ) => CanLocateViewModelForLogicNodeWithId(
      logicNode.UniqueIntegerIdentifier,
      out viewModel
    ) ;
    
    internal bool CanLocateViewModelForLogicNodeWithId ( 
      int                                         id,  
      [NotNullWhen(true)] out LogicNodeViewModel? viewModel 
    ) => (
      viewModel = LogicNodeViewModels.SingleOrDefault(
        logicNodeViewModel => logicNodeViewModel.UniqueIntegerIdentifier == id
      ) 
    ) != null ;
    
    internal LogicNode? MaybeGetLogicNodeForViewModel ( 
      LogicNodeViewModel viewModel
    ) => (
      LogicSystem.AllLogicNodes.SingleOrDefault(
        logicNode => logicNode.UniqueIntegerIdentifier == viewModel.UniqueIntegerIdentifier
      ) 
    ) ;
    
    internal LogicNode GetLogicNodeForViewModel ( 
      LogicNodeViewModel viewModel
    ) => (
      LogicSystem.AllLogicNodes.Single(
        logicNode => logicNode.UniqueIntegerIdentifier == viewModel.UniqueIntegerIdentifier
      ) 
    ) ;
    
    internal bool CanGetLogicNodeForViewModel ( 
      LogicNodeViewModel                 viewModel,
      [NotNullWhen(true)] out LogicNode? logicNode
    ) => (
      logicNode = LogicSystem.AllLogicNodes.SingleOrDefault(
        logicNode => logicNode.UniqueIntegerIdentifier == viewModel.UniqueIntegerIdentifier
      ) 
    ) != null ;
    
    // ---------------------------------

    internal void RebuildEntireDiagram ( System.Action<string>? valueWasChangedLoggingAction = null )
    { 
      RaisePropertyChangedEvent(MagicStrings.REBUILD) ; 
    }

  }

}

