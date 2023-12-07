//
// LogicSystemViewModel_nodes_and_links.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem.ViewModel
{

  // The lists oof Nodes and Links are populated
  // by scanning the Logic System and the 'plain' file.

  public partial class LogicSystemViewModel
  {

    // Diagram body

    public LogicSystemCanvasViewModel BackgroundCanvasViewModel { get ; set ; }

    // Nodes

    private List<LogicNodeViewModel> m_logicNodeViewModelsList = new() ;

    public IEnumerable<LogicNodeViewModel> LogicNodeViewModels => m_logicNodeViewModelsList ;

    internal void AddLogicNodeViewModel ( LogicNodeViewModel logicNodeViewModel )
    {
      m_logicNodeViewModelsList.Add(logicNodeViewModel) ;
    }

    // Links

    private List<DependencyLinkViewModel> m_dependencyLinkViewModelsList = new() ;

    public IEnumerable<DependencyLinkViewModel> DependencyLinkViewModels => m_dependencyLinkViewModelsList ;

    internal void AddDependencyLinkViewModel ( DependencyLinkViewModel dependencyLinkViewModel )
    {
      m_dependencyLinkViewModelsList.Add(dependencyLinkViewModel) ;
    }

  }

}

