//
// ILogicSystemRenderer.cs
//

using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;

namespace Clf.LogicSystem
{

  //
  // Renders a network of Nodes and Links from a Clf.LogicSystem, as a NetworkLayoutDescriptor
  // that describes the geometrical aspects of a layout that provides a reasonableas
  // visualisation of the ndes and links.
  //
  //
  // This is enough information for a tool such as GraphViz to generate a layout
  // which we can use to assign positional attributes to the Nodes and Links.
  // The layout algorithm just needs to know (A) the nodes' dimensions,
  // and (B) the links that connect them.
  //
  // Initially we use GraphViz to perform the layout, but in a future version
  // we'll support an alternative algorithm such as Dagre or GraphRe.
  //

  public interface ILogicSystemRenderer
  {

    static ILogicSystemRenderer Instance => (
      // Hmm, we could access this via Dependency Injection ..
      LogicSystemRenderer_UsingGraphViz.Instance 
    ) ;

    Clf.LogicSystem.Common.NetworkLayoutDescriptor BuildNetworkLayoutDescriptor (
      LogicSystemBase        logicSystem,
      IEnumerable<LogicNode> nodesToInclude
    ) ;

  }

}
