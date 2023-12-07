//
// LogicSystemBase_lookup_nodes.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.Dependencies;
using Clf.LogicSystem.LogicNodes;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Clf.LogicSystem
{

  public partial class LogicSystemBase
  { 
  
    public T LookupNode<T> ( string propertyNameOrChannelName ) 
    where T : LogicNode
    => (T) LogicNodesManager.LookupLogicNode(propertyNameOrChannelName) ;

    public ComputedNodeBase LookupComputedLogicNode ( string propertyNameOrChannelName ) 
    => ComputedLogicNodes.Where(
      node => node.HasPropertyNameOrChannelNameMatching(propertyNameOrChannelName)
    ).Single() ;

    public LogicNode LookupLogicNode ( string propertyNameOrChannelName ) 
    => AllLogicNodes.Where(
      node => node.HasPropertyNameOrChannelNameMatching(propertyNameOrChannelName)
    ).Single() ;

    public LogicNode? LookupLogicNodeIfKnown ( string? propertyNameOrChannelName ) 
    => (
      propertyNameOrChannelName is null
      ? null
      : AllLogicNodes.Where(
          node => node.HasPropertyNameOrChannelNameMatching(propertyNameOrChannelName)
        ).SingleOrDefault() 
    ) ;

    public bool CanLookupLogicNode<T> ( 
      string?                    propertyNameOrChannelName, 
      [NotNullWhen(true)] out T? node 
    ) where T : LogicNode
    => (
      node = (
        propertyNameOrChannelName is null
        ? null
        : AllLogicNodes.GetSingleMatchingItem(
            node => node.HasPropertyNameOrChannelNameMatching(propertyNameOrChannelName)
          )
      ) as T
    ) != null ;

    public InputNode<bool> LookupInputLogicNode_Boolean ( string propertyName )
    => (
      InputNodeExists(
        propertyName,
        out InputNode<bool>? node
      )
      ? node
      : throw new System.ApplicationException(
          $"InputLogicNode '{propertyName}' not found"
        )
    ) ;

    public LogicNode LookupLogicNode_FromUniqueIntegerIdentifier ( int uniqueIdentifier ) 
    => AllLogicNodes.Where(
      node => node.UniqueIntegerIdentifier == uniqueIdentifier
    ).Single() ;

    public bool CanLookupLogicNode_FromUniqueIntegerIdentifier ( 
      int                                uniqueIdentifier, 
      [NotNullWhen(true)] out LogicNode? logicNode 
    ) => (
     logicNode = AllLogicNodes.Where(
        node => node.UniqueIntegerIdentifier == uniqueIdentifier
      ).SingleOrDefault() 
    ) is not null ;

    public Dependency LookupDependency_FromSourceAndTargetNodeUniqueIntegerIdentifiers ( 
      int from_uniqueIdentifier,
      int to_uniqueIdentifier
    ) 
    => Dependencies.Where(
      dependency => (
         dependency.FromSource.UniqueIntegerIdentifier == from_uniqueIdentifier
      && dependency.ToTarget  .UniqueIntegerIdentifier == to_uniqueIdentifier
      )
    ).Single() ;

    public bool CanLookupDependency_FromSourceAndTargetNodeUniqueIntegerIdentifiers ( 
      int                                 from_uniqueIdentifier,
      int                                 to_uniqueIdentifier,
      [NotNullWhen(true)] out Dependency? dependency 
    ) => (
      dependency = Dependencies.Where(
        dependency => (
           dependency.FromSource.UniqueIntegerIdentifier == from_uniqueIdentifier
        && dependency.ToTarget  .UniqueIntegerIdentifier == to_uniqueIdentifier
        )
      ).SingleOrDefault() 
    ) is not null ;

  }

}
