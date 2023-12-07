//
// LogicNode_Attribute_ExtensionMethods.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Diagnostics.CodeAnalysis;

namespace Clf.LogicSystem.Attributes
{

  public static partial class LogicNode_Attribute_ExtensionMethods
  {

    public static bool HasAttribute (
      this LogicNode                  node,
      string                          attributeName,
      [NotNullWhen(true)] out string? attributeValue
    ) => (
      node.AttributesDictionary.TryGetValue(
        attributeName,
        out attributeValue
      )
    ) ;

    public static bool HasAttribute (
      this LogicNode node,
      string         attributeName
    ) => (
      node.AttributesDictionary.TryGetValue(
        attributeName,
        out var _
      )
    ) ;

    public static string? OptionalAttributeValueOrNull (
      this LogicNode                                        node,
      [System.Runtime.CompilerServices.CallerMemberName] string? attributeName = null
    ) => (
      node.AttributesDictionary.ContainsKey(attributeName.VerifiedAsNonNullInstance())
      ? node.AttributesDictionary[attributeName.VerifiedAsNonNullInstance()]
      : null
    ) ;

    public static string? OptionalAttributeValueOrDefault (
      this LogicNode                                        node,
      string?                                                    defaultValue,
      [System.Runtime.CompilerServices.CallerMemberName] string? attributeName = null
    ) => (
      node.AttributesDictionary.ContainsKey(attributeName.VerifiedAsNonNullInstance())
      ? node.AttributesDictionary[attributeName.VerifiedAsNonNullInstance()]
      : defaultValue
    ) ;

    public static string RequiredAttributeValue (
      this LogicNode                                        node,
      [System.Runtime.CompilerServices.CallerMemberName] string? attributeName = null
    ) => node.AttributesDictionary[attributeName.VerifiedAsNonNullInstance()] ;

    // Versions of the above that infer the attribute name from the Type

    public static string? OptionalAttributeValueOrNull<TAttribute> (
      this LogicNode node
    )
    where TAttribute : System.Attribute
    => (
      node.OptionalAttributeValueOrNull(
        attributeName : typeof(TAttribute).AttributeName()
      )
    ) ;

    public static string? OptionalAttributeValueOrDefault<TAttribute> (
      this LogicNode node,
      string?             defaultValue
    )
    where TAttribute : System.Attribute
    => (
      node.OptionalAttributeValueOrDefault(
        defaultValue  : defaultValue,
        attributeName : typeof(TAttribute).AttributeName()
      )
    ) ;

    public static string RequiredAttributeValue<TAttribute> (
      this LogicNode node
    )
    where TAttribute : System.Attribute
    => (
      node.RequiredAttributeValue(
        attributeName : typeof(TAttribute).AttributeName()
      )
    ) ;

    public static string? Attribute (
      this LogicNode node, 
      string name
    ) => (
      node.OptionalAttributeValueOrNull(
        attributeName : name
      )
    ) ;

    // As a convenience, we can define extension methods that
    // refer directly to the Attribute names we know about ...

    public static string? DisplayName (
      this LogicNode node
    ) => (
      node.OptionalAttributeValueOrNull(
        attributeName : NodeAttributeNames.DisplayName
      )
    ) ;

    public static bool ShowAsCollapsed ( 
      this LogicNode logicNode
    ) => (
      logicNode.OptionalAttributeValueOrNull<
        Clf.LogicSystem.Attributes.ShowAsCollapsedAttribute
      >()?.ParsedAs<bool>(
      ) ?? false    
    ) ;

  }

}
