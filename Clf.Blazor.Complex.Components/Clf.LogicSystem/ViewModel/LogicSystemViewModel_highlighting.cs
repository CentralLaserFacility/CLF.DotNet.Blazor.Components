//
// LogicSystemViewModel_highlighting.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.Common.UI;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicSystemViewModel
  {

    private HashSet<LogicNodeViewModel> m_allHighlightedNodeViewModels = new() ;

    public IEnumerable<LogicNodeViewModel> AllHighlightedNodeViewModels => m_allHighlightedNodeViewModels ;

    public IEnumerable<LogicNode> AllHighlightedLogicNodes => AllHighlightedNodeViewModels.Select(
      logicNodeViewModel => logicNodeViewModel.LogicNode
    ) ; // .WhereNonNull() ; REMOVE

    public void RemoveAllHighlighting ( )
    {
      m_allHighlightedNodeViewModels.Clear() ;
      LogicNodeViewModels.ForEachItem(
        logicNodeViewModel => logicNodeViewModel.RemoveHighlighting()
      ) ;
    }

    public void ApplyHighlightingToSpecifiedNodes ( 
      IEnumerable<LogicNodeViewModel> nodesToHighlight
    ) {
      RemoveAllHighlighting() ;
      nodesToHighlight.ForEachItem(
        logicNodeViewModel => logicNodeViewModel.ApplyHighlighting()
      ) ;
      m_allHighlightedNodeViewModels.UnionWith(
        nodesToHighlight.WhereNonNull()
      ) ;
    }

    public void ApplyHighlightingToSpecifiedNodes ( 
      IEnumerable<LogicNode> nodesToHighlight
    ) {
      // The 'nodes-to-highlight' collection could refer to nodes
      // that are not being shown in this particular ViewModel.
      // In that case 'LocateViewModelForLogicNodeIfAvailable'
      // will return null instead of a ViewModel.
      ApplyHighlightingToSpecifiedNodes(
        nodesToHighlight.Select(
          node => LocateViewModelForLogicNodeIfAvailable(
            node
          )
        ).WhereNonNull()
      ) ;
    }

    public void ApplyHighlightingToAnchorNode ( 
      LogicNode nodeToHighlight
    ) {
      var logicNodeViewModel = LocateViewModelForLogicNode(
        nodeToHighlight
      ) ;
      logicNodeViewModel.HighlightingChoice = Clf.Common.UI.HighlightingOption.HighlightedAsAnchor ;
      m_allHighlightedNodeViewModels.Add(
        logicNodeViewModel
      ) ;
    }

    public void ApplyHighlightingToQualifyingNodes ( LogicNodeVisibilitySpecifier logicNodeVisibilitySpecifier )
    {
      ApplyHighlightingToSpecifiedNodes(
        logicNodeVisibilitySpecifier.GetQualifyingNodes(LogicSystem)
      ) ;
      if ( 
        logicNodeVisibilitySpecifier.SpecifiesAnchorNode( 
          out var anchorNode 
        ) 
      ) {
        ApplyHighlightingToAnchorNode(
          anchorNode
        ) ;
      }
    }

  }

}

