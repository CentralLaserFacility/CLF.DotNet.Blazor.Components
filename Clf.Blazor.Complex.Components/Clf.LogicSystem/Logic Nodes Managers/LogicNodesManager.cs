//
// LogicNodesManager.cs
//

namespace Clf.LogicSystem.LogicNodesManagers
{

  //
  // The primary purpose of a 'LogicNodesManager' is 
  // to (A) manage a set of Node instances,
  // and (B) manage a set of Dependency instances 
  // that represent relationships between those Nodes.
  //

  internal partial class LogicNodesManager : Clf.LogicSystem.Common.PlatformServices.HasPlatformServicesProperty
  {

    private LogicSystemBase m_logicSystem ;

    public LogicNodesManager ( LogicSystemBase logicSystem ) 
    { 
      m_logicSystem = logicSystem ;
    }

  }

}
