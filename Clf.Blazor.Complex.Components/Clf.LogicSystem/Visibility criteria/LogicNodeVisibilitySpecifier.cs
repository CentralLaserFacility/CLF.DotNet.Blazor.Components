//
// LogicNodeVisibilitySpecifier.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using FluentAssertions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Clf.LogicSystem
{

  //
  // The primary mechanism to identify a node is the actual 'LogicNode'.
  //
  // If we wanted to use Json Persistence to save and restore the descriptor,
  // we'd use a VisibilityRuleJsonDescriptor that would encode the type name
  // together with either the anchor node id or the explicit list of node id's.
  //
  // Also :
  //
  //   1. Could represent with Unique Integer Identifiers instead of node references.
  //   2. The anchor, if specified, should always be part of the set ; we just need a way
  //      to identify which item in the set is the anchor.
  //

  public abstract record LogicNodeVisibilitySpecifier 
  {

    // Resolve the nodes that are being referred to ...

    // Hmm, maybe we don't need to pass in the Clf.Clf.LogicSystem ?
    // But actually - we really do, for some cases ...

    public abstract IEnumerable<LogicNode> GetQualifyingNodes ( 
      LogicSystemBase logicSystem 
    ) ;

    public virtual bool SpecifiesAnchorNode ( [NotNullWhen(true)] out LogicNode? anchorNode )
    {
      anchorNode = (
        this is VisibilityRuleSpecifier_SpecifyingAnchorNode ruleSpecifyingAnchorNode 
        ? ruleSpecifyingAnchorNode.AnchorNode
        : null
      ) ;
      return anchorNode != null ;
    }

    public record SpecifyingNodesToIncludeAndOptionalAnchorNode ( 
      IEnumerable<LogicNode> NodesToInclude, 
      LogicNode?             AnchorNode = null
    ) 
    : LogicNodeVisibilitySpecifier  
    {
      public override string ToString ( ) => $"Specified nodes ({NodesToInclude.Count()})" ;
      public override IEnumerable<LogicNode> GetQualifyingNodes ( LogicSystemBase logicSystem ) 
      {
        return NodesToInclude ;
      }
      public override bool SpecifiesAnchorNode ( [NotNullWhen(true)] out LogicNode? anchorNode )
      {
        anchorNode = AnchorNode ;
        return anchorNode != null ;
      }
    }

    public record SpecifyingNodeInstances ( IEnumerable<LogicNode> LogicNodes ) : LogicNodeVisibilitySpecifier 
    {
      public override string ToString ( ) => $"Specific nodes ({LogicNodes.Count()})" ;
      public override IEnumerable<LogicNode> GetQualifyingNodes ( LogicSystemBase logicSystem ) 
      {
        return LogicNodes ;
      }
    }

    public record SpecifyingAllNodes ( ) : LogicNodeVisibilitySpecifier 
    {
      public override string ToString ( ) => $"All nodes" ;
      public override IEnumerable<LogicNode> GetQualifyingNodes ( LogicSystemBase logicSystem ) 
      {
        return logicSystem.AllLogicNodes ;
      }
    }

    // Specification involving a predicate

    public record SpecifyingAllNodesMatchingPredicate ( System.Func<LogicNode,bool> predicate ) : LogicNodeVisibilitySpecifier 
    {
      public override string ToString ( ) => $"All nodes matching predicate" ;
      public override IEnumerable<LogicNode> GetQualifyingNodes ( LogicSystemBase logicSystem ) 
      {
        return logicSystem.AllLogicNodes.Where(predicate) ;
      }
    }

    public record SpecifyingNoNodes ( ) : LogicNodeVisibilitySpecifier 
    {
      public override string ToString ( ) => $"No nodes" ;
      public override IEnumerable<LogicNode> GetQualifyingNodes ( LogicSystemBase logicSystem ) 
      {
        return Enumerable.Empty<LogicNode>() ;
      }
    }

    // Specifications involving a single Anchor node

    public abstract record VisibilityRuleSpecifier_SpecifyingAnchorNode ( 
      LogicNode AnchorNode
    ) 
    : LogicNodeVisibilitySpecifier
    {
      public override IEnumerable<LogicNode> GetQualifyingNodes ( 
        LogicSystemBase logicSystem 
      ) {
        return AnchorNode.WithAdditionalItems(
          GetAnchoredNodes(logicSystem)
        ) ;
      }
      public abstract IEnumerable<LogicNode> GetAnchoredNodes ( 
        LogicSystemBase logicSystem 
      ) ;
    }

    public record SpecifyingAllNodesContributingToAnchorNode ( 
      ComputedNodeBase ComputedNode 
    ) 
    : VisibilityRuleSpecifier_SpecifyingAnchorNode(ComputedNode) 
    {
      public override string ToString ( ) => $"Nodes contributing to '{AnchorNode.PropertyName}'" ;
      public override IEnumerable<LogicNode> GetAnchoredNodes ( LogicSystemBase logicSystem ) 
      {
        return logicSystem.QuerySourceNodesDirectlyOrIndirectlyContributingTo(
          ComputedNode
        ) ;
      }
    }

    public record SpecifyingAllNodesInfluencedByAnchorNode ( 
      LogicNode AnchorNode
    ) 
    : VisibilityRuleSpecifier_SpecifyingAnchorNode(AnchorNode)  
    {
      public override string ToString ( ) => $"Nodes influenced by '{AnchorNode.PropertyName}'" ;
      public override IEnumerable<LogicNode> GetAnchoredNodes ( LogicSystemBase logicSystem ) 
      {
        return logicSystem.QueryComputedNodesDirectlyOrIndirectlyInfluencedBy(AnchorNode) ;
      }
    }

    public record SpecifyingAllNodesInfluencedByOrContributingToAnchorNode (
      LogicNode AnchorNode
    ) 
    : VisibilityRuleSpecifier_SpecifyingAnchorNode(AnchorNode)  
    {
      public override string ToString ( ) => $"Nodes influenced by or contributing to '{AnchorNode.PropertyName}'" ;
      public override IEnumerable<LogicNode> GetAnchoredNodes ( LogicSystemBase logicSystem ) 
      {
        return logicSystem.QueryAllNodesDirectlyOrIndirectlyContributingToOrInfluencedBy(AnchorNode) ;
      }
    }

    #if false

    // Specifications involving multiple Anchor nodes : NOT YET IMPLEMENTED ...
    // WOULD THIS BE USEFUL ? PROBABLY NOT ...
    // WOULD NEED MULTIPLE SELECTION CAPABILITY TO PERMIT MULTIPLE ANCHOR NODES TO BE IDENTIFIED
    // ALSO : POTENTIAL CONFUSION IF THE ANCHOR NODES ARE ALSO CONTAINED IN THE 'DEPENDENT' SETS

    // This is the kind of code we used in a previous version ... ???

    QueryAllNodesDirectlyOrIndirectlyContributingToOrInfluencedBy() ;

    // private IEnumerable<LogicNode> GetAllNodesInfluencedByOrContributingToSpecifiedNodes ( 
    //   LogicNodeVisibilityRuleDescriptor logicNodeVisibilityRuleDescriptor 
    // ) {
    //   return QueryAllNodesDirectlyOrIndirectlyContributingToOrInfluencedBy(
    //     logicNodeVisibilityRuleDescriptor.SpecifiedNodeNames.VerifiedAsNonNullInstance().Split(',').Select(
    //       nodeName => LookupLogicNode(nodeName)
    //     )
    //   ) ;
    // }
    
    private IEnumerable<LogicNode> GetAllNodesInfluencedByOrContributingToSpecifiedNodes ( 
      LogicNodeVisibilityRuleDescriptor logicNodeVisibilityRuleDescriptor 
    ) => QueryAllNodesDirectlyOrIndirectlyContributingToOrInfluencedBy(
      logicNodeVisibilityRuleDescriptor.SpecifiedNodes.VerifiedAsNonNullInstance()
    ).Concat(
      logicNodeVisibilityRuleDescriptor.SpecifiedNodes.VerifiedAsNonNullInstance()
    ) ;
    
    #endif

  }

}
