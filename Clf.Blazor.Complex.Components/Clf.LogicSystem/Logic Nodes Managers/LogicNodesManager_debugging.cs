//
// LogicNodesManager_debugging.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;

namespace Clf.LogicSystem.LogicNodesManagers
{

  internal partial class LogicNodesManager
  {

    public string LatestDebugInfo = "" ;

    // Hmm, this causes a problem if called during tree construction ...
    // For example, invoking 'ToString()' on a computed Value node causes havoc

    public string RefreshLatestDebugInfo ( )
    => LatestDebugInfo = (
      // false //
      IsBuildingDependencies
      ? "Debug info is not available (dependencies are being built)"
      : GetDebugInfo()
    ) ;

    private string GetDebugInfo ( )
    {
      var text = new System.Text.StringBuilder() ;
      try
      {
        text.AppendLine("Nodes :") ;
        var allKnownNodes = new HashSet<LogicNode>() ;
        this.LogicNodesDictionary.Values.ForEachItem(
          node => {
            allKnownNodes.Add(node) ;
            text.AppendLine(
              $"  {node.UniqueIntegerIdentifierAsString} {node.NameAndClass()}"
            ) ;
          }
        ) ;
        Dependencies.ForEachItem(
          link => {
            allKnownNodes.Add(link.FromSource) ;
            allKnownNodes.Add(link.ToTarget) ;
          }
        ) ;
        allKnownNodes.ForEachItem(
          node => {
            text.AppendLine(
              $"  {node.UniqueIntegerIdentifierAsString} {node.NameAndClass()}"
            ) ;
          }
        ) ;
        text.AppendLine("Links :") ;
        Dependencies.ForEachItem(
          link => text.AppendLine(
            $"  {link.ToString()}"
          )
        ) ;
      }
      catch ( System.Exception x )
      {
        text.AppendLine(
          "EXCEPTION !! : "
        + x.Message
        ) ;
      }
      return text.ToString() ;
    }

  }

}
