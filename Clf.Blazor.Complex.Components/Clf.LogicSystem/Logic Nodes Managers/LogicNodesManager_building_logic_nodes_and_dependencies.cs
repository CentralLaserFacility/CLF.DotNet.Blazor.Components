//
// LogicNodesManager_buildingDependencyLinks_topLevelValues.cs
//

using Clf.ChannelAccess;
using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.LogicNodesManagers
{

  internal partial class LogicNodesManager
  {

    internal (
      IEnumerable<string> warnings, 
      IEnumerable<string> errors
    ) ValidateLogicSystem ( 
      // System.Action<string> handleWarningMessage,
      // System.Action<string> handleErrorMessage
    ) {
      List<string> warnings = new() ;
      List<string> errors = new() ;
      this.AllLogicNodes.ForEachItem(
        logicNode => {
          if ( logicNode.DefinesAnActualChannelType)
          {
            if (logicNode.ChannelType == ChannelType.Undefined) 
            {
              warnings.Add(
                $"Node {logicNode.PropertyName} defines a Channel that doesn't have an explicit channel type."
              ) ;
            }
          }
          else
          {
            warnings.Add(
                $"Node {logicNode.PropertyName} does not define a Channel, default behavior will be used."
              );
          }

          if ( logicNode.DefinesAnActualChannelName )
          {
            if ( ! logicNode.ChannelName.Name.Contains(':') ) 
            {
              warnings.Add(
                $"Node {logicNode.PropertyName} defines a ChannelName that doesn't contain a ':' prefix"
              ) ;
            }
            // if ( logicNode.ChannelName.Length > 28 ) 
            // {
            //   warnings.Add(
            //     $"Node {logicNode.PropertyName} defines a ChannelName of length > 28"
            //   ) ;
            // }
          }
          if ( 
            logicNode.LogicNodesManager.Dependencies.Where(
              link => link.Involves(logicNode)
            ).Any() is false
          ) {
            warnings.Add(
              $"Node {logicNode.PropertyName} does not link to any other nodes"
            ) ;
          }
        }
      ) ;
      return (warnings,errors) ;
    }

    public void BuildDependencies_Begin()
    {
        IsBuildingDependencies = true;
    }

    public void BuildDependencies_End()
    {
      IsBuildingDependencies = false;
        LogicNode.ParentLogicSystemBeingConfigured = null;
        AllNodesShouldHaveBeenCreated = true;
        /* PlatformServices.WriteMessageLogLine(
            $"Visiting the Properties of '{logicSystemTypeName}' - FINISHED"
            ) ;*/

        // Now that we've visited all the properties,
        // all the 'computed' nodes will have been evaluated.
        ComputedLogicNodes().ForEachItem(
            computedNode =>
            {
                computedNode.CachedComputedValueIsBelievedToBeCorrect.Should().BeTrue();
                computedNode.CachedComputedValueIsDefinitelyCorrect.Should().BeTrue();
            }
            );
    }

  }

}

