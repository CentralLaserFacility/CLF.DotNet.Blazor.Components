//
// LogicNodesManager_links.cs
//

using Clf.LogicSystem.Common.ExtensionMethods ;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using Clf.LogicSystem.Dependencies;

namespace Clf.LogicSystem.LogicNodesManagers
{
  internal partial class LogicNodesManager
  {

    private HashSet<Dependency> m_dependencies = new() ;

    public bool LinkIsPresent ( LogicNode a, ComputedNodeBase b )
    => m_dependencies.Contains(
      new Dependency(a,b)
    ) ;

    public IEnumerable<Dependency> Dependencies => m_dependencies ;

    internal void EnsureDependencyExists (
      LogicNode        fromSource,
      ComputedNodeBase toTarget
    ) {
      bool added = m_dependencies.Add(
        new Dependency(fromSource,toTarget)
      ) ;
      if ( added )
      {
        /*PlatformServices.WriteMessageLogLine(
          $"Added dependency from '{fromSource.PropertyName}' to '{toTarget.PropertyName}'"
        ) ;*/
      }
    }
  }

}
