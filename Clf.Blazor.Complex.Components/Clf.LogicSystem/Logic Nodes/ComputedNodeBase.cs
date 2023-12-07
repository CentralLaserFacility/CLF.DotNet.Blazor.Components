//
// ComputedNodeBase.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes.ExtensionMethods;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Clf.LogicSystem.LogicNodes
{

  //
  // A 'computed node' has a 'value' that comes from
  // the values of 'contributing' nodes that feed into it.
  //
  // The value is saved in a cache field, and gets refreshed
  // whenever a Value of any contributing node suffers a change.
  //

  //
  // The 'CallerArgumentExpression' attribute gets us a cheap and cheerful way
  // to represent the 'formula' as text - without the nice possibilities afforded
  // by doing a proper Roslyn analysis, but gooed enough for starters ...
  //

  public abstract class ComputedNodeBase : LogicNode
  {

    //
    // We're interested in the text representation of the 'computeValueFunc',
    // but if we use the 'CallerArgumentExpression' here, then because of the way
    // this constructor is called, that just returns 'computeValueFunc'.
    // So we have to derive the textual representation in the calling function.
    //

    protected ComputedNodeBase (
      LogicSystemBase    logicSystem,
      string             propertyName,
      string             formulaTextLines,
      System.Action?     valueChangedAction = null
    ) :
    base(
      logicSystem,
      propertyName,
      valueChangedAction
    ) {
      FormulaTextLine_DerivedFromLambdaFunctionBody = CompressMultiLineExpressionToSingleLine(formulaTextLines) ;
    }

    // This will be overridden if the user supplies a [Formula] attribute.
    public string FormulaTextLine_DerivedFromLambdaFunctionBody { get ; private set ; }

    private static string CompressMultiLineExpressionToSingleLine ( 
      string expressionTextLines
    ) {
      // Should remove comments, if present ...
      Regex rgx = new(@"\/\*[\s\S]*?\*\/|\/\/.*");
      expressionTextLines = rgx.Replace(expressionTextLines,string.Empty);

      return new string(
        expressionTextLines.ToCharArray().Where(
          ch => ! char.IsWhiteSpace(ch)
        ).ToArray() 
      ).Replace("()","").Replace("=>","").TrimStart() ;
    }


    #if SUPPORTS_SHADOW_VALUES

    //
    // The Value of an 'output' node is computed from its inputs,
    // and therefore can't be 'Set' - that wouldn't make sense !
    //
    // However we might want to verify that our computed value
    // agrees with a Value obtained in some other way, and indicate
    // any discrepancy on a UI.
    //
    // This property lets us set a 'shadow' value for the Output Node,
    // which would typically be obtained from a PV in an actual Epics system
    // running in parallel, or from an algorithm that runs after any change
    // has been submitted and computes the shadow values using an algorithm
    // based directly on the Input node values (rather than theformulas
    // expressed in the tree of contributing nodes).
    //
    // If the shadow value is non null, it means that a true/false 'ShadowValue'
    // has been provided. A visualisation can then indicate situations where
    // the shadow value disagrees with the true/false Value computed by the Logic System.
    // For example the visualisation can display an additional 'external border'
    // surrounding the node, whose colour indicates a discrepancy.
    //

    private bool? m_shadowValue = null ;
    public bool? ShadowValue 
    { 
      get => m_shadowValue ; 
      set {
        if ( value != m_shadowValue )
        {
          m_shadowValue = value ;
          RaiseVisualisedPropertyChangedEvent() ;
        }
      }
    }

    public bool ComputedValueDisagreesWithShadowValue => false ;
    // (
    //   ShadowValue.HasValue
    //   // ? this.UniqueIntegerIdentifier % 2 == 0 // Hack for testing !!!
    //   ? ShadowValue.Value != this.Value 
    //   : false
    // ) ;

    #endif

    // If we have a ComputedValue that is an 'output' node,
    // we'll be transmitting its Value to other PV's - and we'd
    // expect that the value would not be null !

    public bool ComputedValueIsUnexpectedlyNull => (
    //   LogicNodeCategory is LogicNodeCategory.OutputNode_Boolean
    //&& 
      ValueIsNull
    ) ;

    public List<LogicNode>? NodesAccessedInComputedValueFuncXX { get ; set ; } = null ;

    public abstract void Initialise ( ) ;

    public abstract void SetPreviousValueFromCurrentValue ( ) ;

    public abstract void DeclareCachedComputedValueMightBeOutOfDate ( ) ;

    public abstract bool CurrentValueIsDifferentFromPrevious { get ; }

    public abstract bool CachedComputedValueIsBelievedToBeCorrect { get ; }

    public abstract bool CachedComputedValueMightBeOutOfDate { get ; }

    public abstract bool CachedComputedValueIsDefinitelyCorrect { get ; }

  }

  public static partial class Helpers
  {

    //
    // These are useful functions that don't make sense as Extension Methods,
    // largely because it's impossible to think of a name that would properly
    // indicate their purpose when used as 'x.DoSomething()'.
    //
    // So, better to package them as static Helper functions that are invoked as
    //
    //   Helpers.DoSomething(x)
    //

    public static IEnumerable<LogicNode> GetSourceNodesDirectlyContributingTo (
      ComputedNodeBase                 computedNode,
      System.Func<LogicNode,bool>? filter_optional = null
    ) => (
      computedNode.DirectlyContributingNodes().WithOptionalFilterApplied(
        filter_optional
      )
    ) ;

    public static IEnumerable<LogicNode> GetSourceNodesDirectlyContributingTo (
      LogicNode                    logicNode,
      System.Func<LogicNode,bool>? filter_optional = null
    ) => (
      logicNode is ComputedNodeBase computedNode
      ? computedNode.DirectlyContributingNodes().WithOptionalFilterApplied(
          filter_optional
        )
      : Enumerable.Empty<LogicNode>()
    ) ;

  }

  namespace ExtensionMethods
  {

    public static class ComputedLogicNode_ExtensionMethods
    {

      public static IEnumerable<LogicNode> DirectlyContributingNodes (
        this ComputedNodeBase node
      ) => (
        node.LogicNodesManager.Dependencies.Where(
          link => link.ToTarget == node
        ).Select(
          link => link.FromSource
        )
      ) ;

      // Useful for testing 

      public static bool HasExactlyTheseDirectlyContributingNodes (
        this ComputedNodeBase node,
        IEnumerable<LogicNode> directlyContributingNodesExpected
      ) => (
        // Enumerable.SequenceEqual(
        //   node.DirectlyContributingNodes().OrderBy(n=>n.UniqueIntegerIdentifier),
        //   directlyContributingNodesExpected.OrderBy(n=>n.UniqueIntegerIdentifier)
        // )
        node.DirectlyContributingNodes().ToHashSet().SetEquals(
          directlyContributingNodesExpected
        ) 
      ) ;

      public static bool HasExactlyTheseDirectlyContributingNodes (
        this ComputedNodeBase  node,
        params LogicNode[] directlyContributingNodesExpected
      ) => (
        node.HasExactlyTheseDirectlyContributingNodes(
          directlyContributingNodesExpected.AsEnumerable()
        )
      ) ;

      public static void VerifyCachedComputedPropertyValueIsUpToDate (
        this ComputedNodeBase computedNode
      ) {
        if ( computedNode.CachedComputedValueIsDefinitelyCorrect is false )
        {
          throw new Clf.Common.Utils.UnreachableException(
            $"{computedNode.PropertyName}.CachedComputedValueIsDefinitelyCorrect is not correct"
          ) ;
        }
      }

    }

  }

}
