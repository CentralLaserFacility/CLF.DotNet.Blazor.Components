//
// LogicNodeVisibilityRule.cs
//

using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;

namespace Clf.LogicSystem
{

  public record LogicNodeVisibilityRule (
    string Name,
    string Description,
    System.Func<
      LogicSystemBase,
      IEnumerable<LogicNode>,
      IEnumerable<LogicNode>
    > NodesToIncludeFunc
  ) {
  }

}
