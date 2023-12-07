//
// LogicNode_ExtensionMethods.cs
//

using Clf.Common;
using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Attributes;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes.ExtensionMethods;
using Clf.LogicSystem.Miscellaneous;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.LogicNodes
{

  //
  // Being defined as methods, these can potentially be redefined to use generics
  // where we can define the exact type of the expected return result ...
  //
  // Hmm, we're returning IEnumerable<> sequences which are often lazily evaluated.
  // It's up to the client to resolve these into a snapshotted IList<> where necessary.
  //

  //
  // Note that when we define an Extension Method (instead of defining an instance method)
  // we *must* access it via an object instance. That's fine, but note that when you need
  // to invoke an Extension Method inside a method of a class, you need to explicitly
  // supply 'this' :
  //
  //   void MyInstanceMethodA ( )
  //   {
  //     this.MyExtensionMethodB() ;
  //   }
  //
  //   void MyInstanceMethodA ( )
  //   {
  //     MyInstanceMethodB() ; // <== 'this' is not required here !!!
  //   }
  //

  //
  // Fundamental methods that we delegate to the LogicNodesManager.
  //

  public static partial class LogicNode_ExtensionMethods
  {

    // We can quickly check whether we have a 'top level' node
    // by seeing whether the node is present in the dictionary.

    public static bool IsRegisteredInNodesDictionary  ( this LogicNode node )
    => node.LogicNodesManager.LogicNodesDictionary.Values.Contains(node) ;

    // The reported name is 'locally unique', by which we mean that other
    // instances in the same locality, eg instances which are directly nested
    // children of the same parent, do not have the same name.
    // This will be the case if the child nodes have been created
    // to represent Properties of a parent object, eg using 'GetOrCreateXX()',
    // which are guaranteed to all have distinct names.

    // public static string Name ( this LogicNode node )
    // => (
    //   node.IsRegisteredInNodesDictionary()
    //   ? node.Name_FromKeyInNodesDictionary()
    //   : throw null!
    // ) ;

    // public static NodeName_WithOptionalAliases Name_WithOptionalAliases ( this LogicNode node )
    // => (
    //   node.IsTopLevelNodeWithNoParent()
    //   ? new NodeName_WithOptionalAliases(
    //       node.Name_AssumingIsTopLevelNode()
    //     )
    //   : throw new System.ApplicationException("Expected a top level node")
    // ) ;

    public static string GetPropertyName_FromKeyInLogicNodesDictionary ( this LogicNode node )
    => node.LogicNodesManager.LogicNodesDictionary.Where(
      keyValuePair => keyValuePair.Value == node
    ).Single(
    ).Key ;

    public static int IndexInNodesDictionary ( this LogicNode node )
    => node.LogicNodesManager.LogicNodesDictionary.Values.IndexOfItem(node).VerifiedAsNonNullValue() ;

  }

  public static partial class ILogicNode_ExtensionMethods
  {

    public static bool HasPropertyNameOrChannelNameMatching ( this LogicNode instance, string name )
    => (
       instance.PropertyName.ToLower() == name.ToLower()
    || instance.ChannelName.Name.ToLower()       == name.ToLower()
    ) ;

    public static IEnumerable<LogicNode> OrderedByPropertyName (
      this IEnumerable<LogicNode> nodes
    ) => nodes.OrderBy(
      node => node.PropertyName
    ) ;

    public static string NodeNames ( this IEnumerable<LogicNode> source )
    => source.Select(
      node => node.PropertyName
    ).ToDelimitedList() ;

    public static string NameAndClass ( this LogicNode node )
    => $"'{node.PropertyName}' class=({node.ClassName()})" ;

    public static string NameAndClassWithUniqueIdentifier ( this LogicNode node )
    => $"'{node.PropertyName}' class=({node.ClassName()}) id={node.UniqueIntegerIdentifierAsString}" ;

    public static int ComputationalDepth_Max ( this LogicNode node )
    => ( 
      node is ComputedNodeBase computedNode
      ? computedNode.DirectlyContributingNodes().Max(
          contributingNode => contributingNode.ComputationalDepth_Max()
        ) + 1 
      : 0
    ) ;

    public static int ComputationalDepth_Min ( this LogicNode node )
    => ( 
      node is ComputedNodeBase computedNode
      ? computedNode.DirectlyContributingNodes().Min(
          contributingNode => contributingNode.ComputationalDepth_Min()
        ) + 1
      : 0
    ) ;

    public static bool IsRegisteredWithLogicNodesManager ( this LogicNode node )
    => node.LogicNodesManager.LogicNodesDictionary.Values.Contains(node) ;

  }

  public static partial class LogicNode_ExtensionMethods
  {

    public static bool ValueIsNull (
      this LogicNode node
    ) => (
      node.ValueAsObject is null
    ) ;

    public static string GetSummaryInfo (
      this LogicNode node
    ) => (
      $"{node.PropertyName} : {node.ValueAsString}"
    ) ;

    public static bool IsMutableInputNode ( this LogicNode node )
    => (
      node is InputNodeBase
    ) ;

    public static bool IsComputedNode ( this LogicNode node )
    => (
      node is ComputedNodeBase
    ) ;

    public static bool ValueIsNonNull ( this LogicNode node )
    => (
      node.ValueAsObject is not null
    ) ;

    public static bool FeedsIntoOneOrMoreComputedNodesThatAreDirectDependents ( this LogicNode valueNode )
    => (
      Helpers.ComputedTargetNodesDirectlyContributedToBy(valueNode).Any()
    ) ;

    public static bool IsOutputNodeWithNoDirectDependents ( this LogicNode valueNode )
    => (
      Helpers.ComputedTargetNodesDirectlyContributedToBy(valueNode).Any() is false
    ) ;

    public static bool FeedsIntoOneOrMoreComputedNodesThatAreDirectDependents (
      this LogicNode                     valueNode,
      out IEnumerable<ComputedNodeBase> computedValueNodesDirectlyContributedToByThisNode
    ) => (
      computedValueNodesDirectlyContributedToByThisNode
      = Helpers.ComputedTargetNodesDirectlyContributedToBy(valueNode)
    ).Any() ;

    public static bool FeedsIntoExactlyTheseComputedNodesThatAreDirectDependents (
      this LogicNode             valueNode,
      params ComputedNodeBase[] computedValueNodesDirectlyContributedToByThisNode_expected
    ) => (
      // Hmm, are we sure that it's OK to put these in a HashSet ?
      // Have we correctly overridden GetHashCode() ???
      Helpers.ComputedTargetNodesDirectlyContributedToBy(valueNode).ToHashSet().SetEquals(
        computedValueNodesDirectlyContributedToByThisNode_expected
      )
    ) ;

    //
    // This is a 'template' in that some of the content, eg a '$VAL$' string,
    // might be replaced by a different value when the visual is rendered.
    // The template text is used to determine the size of the box.
    //
    // The 'label text lines' that are generated via this template
    // determine the size of the box that we'll be using to represent the node.
    // Since we don't know the 'Value' ahead of time, we supply a 'typical'
    // string to replace the '$VAL$' when we compute the size.
    //

    public static IEnumerable<string> LabelTextLinesTemplate ( 
      this LogicNode logicNode 
    ) => (
      // This template definition produces the following text lines :
      //  1. The display-name,
      //     or if that's not available, the PV name, 
      //     or if that's not available, the Property name 
      //  2. A summary of the Value, if available
      //  3. The 'caption', if available
      new List<string?>(){
           logicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.DisplayNameAttribute>() 
        ?? logicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.ChannelNameAttribute>() 
        ?? logicNode.PropertyName,
        logicNode.ValueSummaryPlaceholder(),
        logicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.CaptionAttribute>() //,
        // $"Value = {logicNode.ValueAsString}"
      }
    ).WhereNonNull() ;

    //
    // For Double and String valued nodes, we add a line that acts as a placeholder
    // for the Value, that will be replaced by the actual value in the visual rendering.
    //

    private static string? ValueSummaryPlaceholder ( this LogicNode logicNode )
    => MagicStrings.VAL_PLACEHOLDER ;

    public static int ValueSummaryLengthExpected ( 
      this LogicNode logicNode 
    )
    => Clf.LogicSystem.Common.Utils.Helpers.GetEstimatedLengthOfStringValueOfType(logicNode.ValueType) ;

    public static string? ChannelNameAttributeOrNull ( this LogicNode logicNode )
    => logicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.ChannelNameAttribute>() ;

    public static string? RecordDescriptorAttributeOrNull ( this LogicNode logicNode )
    => logicNode.OptionalAttributeValueOrNull<Clf.LogicSystem.Attributes.RecordDescriptorAttribute>() ;

  }

}
