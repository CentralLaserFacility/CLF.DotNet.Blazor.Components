//
// LogicNodesManager_nodes.cs
//

using Clf.LogicSystem.LogicNodes;
using FluentAssertions;
using System.Collections.Generic;

namespace Clf.LogicSystem.LogicNodesManagers
{

  internal partial class LogicNodesManager
  {

    internal readonly Dictionary<
      string,   // logicNodeName 
      LogicNode
    > LogicNodesDictionary = new() ;

    internal int RegisterNewlyCreatedNode (
      LogicNode newlyCreatedNode,
      string    propertyName
    ) {
      LogicNodesDictionary.Add(
        key   : propertyName,
        value : newlyCreatedNode
      ) ;
      newlyCreatedNode.IsRegisteredWithLogicNodesManager().Should().BeTrue() ;
      return m_nextAvailableUniqueId++ ;
    }

    //
    // By starting the id's at 100 we make them all have at least 3 digits
    // and this makes them more legible on a textual display.
    //

    private int m_nextAvailableUniqueId = 100 ;
    
  }

}
