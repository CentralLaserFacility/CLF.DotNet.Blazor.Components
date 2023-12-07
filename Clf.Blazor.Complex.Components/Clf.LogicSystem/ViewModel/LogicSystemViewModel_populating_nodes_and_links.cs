//
// LogicSystemViewModel_populating_nodes_and_links.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;
using static Clf.Common.Utils.DebugHelpers;

namespace Clf.LogicSystem.ViewModel
{

  partial class LogicSystemViewModel
  {

    //
    // This builds a ViewModel from
    //
    //   (A) a LogicNetwork holding a complete set of Nodes and Links,
    //
    //   (B) a LogicNodeVisibilityRuleDescriptor that describes the criteria
    //       that determine whether a particular Node is to be rendered in the ViewModel      
    //
    //   (C) an associated 'NetworkLayoutDescriptor'.
    //
    // The NetworkLayoutDescriptor tells us about the locations and sizes of the boxes
    // that have been assigned to the Nodes, and the Paths to be followed
    // by the connecting lines that represent Links.
    //
    // The 'NetworkLayoutDescriptor' is generated from a Clf.Clf.LogicSystem according to the
    // particular set of Nodes that are to be included, specified by their integer ID's,
    // eg '102,124,134,165,143'.
    //
    //   TODO :
    //   Building the layout is a compute-intensive process that involves invoking GraphViz or Dagre.
    //   So we should implement a cache, whereby prior to building the Descriptor we create
    //   a key that represents the nodes that are to be included, and use that key to
    //   either (a) retrieve the Descriptor that was previously built for those specific Nodes,
    //   or (b) build a fresh Descriptor, and insert it into the cache for next time.
    //
    // If we've already computed a valid 'NetworkLayoutDescriptor', use that.
    // Otherwise, we build one from the Clf.Clf.LogicSystem :
    // - scan the Clf.Clf.LogicSystem nodes to build a 'dot' string for GraphViz
    // - run GraphViz to produce a 'plain' string that describes the layout
    // - parse that 'plain' text to build a NetworkLayoutDescriptor
    //

    public static bool UseCachedNetworkLayoutDescriptorWhenAvailable = false ;

    public void PopulateNodesAndLinks ( 
      ref Clf.LogicSystem.Common.NetworkLayoutDescriptor? networkLayoutDescriptor,
      LogicNodeVisibilitySpecifier?                      logicNodeVisibilitySpecifier = null
    ) {
      logicNodeVisibilitySpecifier ??= new LogicNodeVisibilitySpecifier.SpecifyingAllNodes() ;
      LogicNodeVisibilitySpecifier = logicNodeVisibilitySpecifier ;
      IEnumerable<LogicNode> logicNodesToInclude = logicNodeVisibilitySpecifier.GetQualifyingNodes(LogicSystem) ;
      // We should implement a proper timing scheme ???
      // using var timer = new LogicSystemUtilities.Stopwatch() ;
      networkLayoutDescriptor ??= ILogicSystemRenderer.Instance.BuildNetworkLayoutDescriptor(
        LogicSystem,
        logicNodesToInclude
      ) ;
      using Clf.Common.Utils.Stopwatch timer = new(
        "Populating ViewModel",
        (elapsedMillseconds) => LogicSystem.MillisecsTakenPopulatingViewModel = elapsedMillseconds
      ) ;
      PopulateNodesAndLinksFromNetworkLayoutDescriptor(
        networkLayoutDescriptor
      ) ;
      if ( UseCachedNetworkLayoutDescriptorWhenAvailable is false )
      {
        networkLayoutDescriptor = null ;
      }
      // Hmm, dangerous to do this here - it should already have been called
      // when the Clf.Clf.LogicSystem was created, if we're interested in connecting
      // to live Channels via ChannelAccess and ThinIoc. Thanks Priya !!
      // Clf.Clf.LogicSystem.EnsureMonitoredChannelsConfigured() ;
    }

    //
    // Given an existing 'LogicNodeViewModel', which holds the ID of 
    // a LogicNode in a particular Clf.Clf.LogicSystem, this method accesses the
    // current Properties of that LogicNode and applies them to the 
    // LogicNodeViewModel, thereby ensuring that the two are in sync.
    //
    // This method is invoked (a) when initially creating the LogicNodeViewModel,
    // and also (b) when a change to a LogicNode has occurred, and we need to
    // update the LogicNodeViewModel.
    //

    private void PopulateNodesAndLinksFromNetworkLayoutDescriptor ( 
      Clf.LogicSystem.Common.NetworkLayoutDescriptor networkLayoutDescriptor
    ) {
      using var timer = new Clf.Common.Utils.Stopwatch() ;
      DeregisterFromLogicNodeValueChangedEvents() ;
      m_logicNodeViewModelsList.Clear() ;
      m_dependencyLinkViewModelsList.Clear() ;
      networkLayoutDescriptor.NodeLayoutDescriptors.ForEachItem(
        nodeLayoutDescriptor => {
          if (
            LogicSystem.CanLookupLogicNode_FromUniqueIntegerIdentifier(
              nodeLayoutDescriptor.NodeId.ParsedAs<int>(),
              out LogicNode? logicNode
            )           
          ) {
            // We've found a node whose 'id' is an integer corresponding to 
            // the unique identifier of a Clf.Clf.LogicSystem node.
            DontWriteDebugLines(
              $"Node '{logicNode.UniqueIntegerIdentifierAsString}' is at ({nodeLayoutDescriptor.CentrePosition})"
            ) ;
            // Create a ViewModel element with all the 'immutable' properties assigned,
            // but for the time don't set the 'mutable' properties (the ones pertaining to 
            // the Value). They will be assigned just prior to returning from the current method.
            var logicNodeViewModel = new LogicNodeViewModel(
              parent                  : this,
              uniqueIntegerIdentifier : nodeLayoutDescriptor.NodeId.ParsedAs<int>(),
              boundingRectangle       : nodeLayoutDescriptor.NodeOutlineRectangle
            ) ;
            // We could at this point set the 'mutable' properties for the
            // specific Node we're dealing with ; but by performing that operation 
            // later on, for all the LogicNodes at once, we'll be exercising the
            // same function that will be utilised in future when we need to keep
            // the ViewModel in sync with the Clf.Clf.LogicSystem.
            AddLogicNodeViewModel(
              logicNodeViewModel
            ) ;
          }
          else
          {
            throw new System.Diagnostics.UnreachableException(
              $"Node '{nodeLayoutDescriptor.NodeId}' NOT FOUND"
            ) ;
          }
        }
      ) ;
      networkLayoutDescriptor.EdgeLayoutDescriptors.ForEachItem(
        edgeFromPlainFile => {
          if ( 
            LogicSystem.CanLookupDependency_FromSourceAndTargetNodeUniqueIntegerIdentifiers(
              edgeFromPlainFile.SourceNodeId.ParsedAs<int>(),
              edgeFromPlainFile.TargetNodeId.ParsedAs<int>(),
              out LogicSystem.Dependencies.Dependency? link
            )
          ) {
            DontWriteDebugLines(
              $"Link {link.FromSource.UniqueIntegerIdentifierAsString} -> {link.ToTarget.UniqueIntegerIdentifierAsString}"
            ) ;
            AddDependencyLinkViewModel(
              new DependencyLinkViewModel(
                parent       : this,
                sourceNodeId : link.FromSource.UniqueIntegerIdentifier,
                targetNodeId : link.ToTarget.UniqueIntegerIdentifier,
                pointsOnPath : edgeFromPlainFile.Points.ToList()
              ){
                ToolTipTextLine = $"{link.FromSource.ChannelName} influences {link.ToTarget.ChannelName}"
              }
            ) ;
          }
        }
      ) ;

      // Set up the overall 'diagram' properties relating to
      // the background canvas that surrounds the diagram elements.

      BackgroundCanvasViewModel.LoadPropertiesFrom(networkLayoutDescriptor) ;

      // Now that we've installed LogicNodeViewModels for all the LogicNodes that are of interest,
      // register each one with the 'value-changed' event published by its LogicNode.  

      LogicNodeViewModels.ForEachItem(
        logicNodeViewModel => logicNodeViewModel.RegisterWithLogicNodeVisualisedPropertyChangedEvent()
      ) ;

    }

  }

}

