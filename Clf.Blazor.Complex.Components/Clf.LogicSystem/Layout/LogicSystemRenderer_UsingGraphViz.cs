//
// LogicSystemRenderer_UsingGraphViz.cs
//

using Clf.Common;
using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Attributes;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem
{

  public sealed class LogicSystemRenderer_UsingGraphViz : ILogicSystemRenderer
  {

    public static LogicSystemRenderer_UsingGraphViz Instance = new LogicSystemRenderer_UsingGraphViz() ;

    public Clf.LogicSystem.Common.NetworkLayoutDescriptor BuildNetworkLayoutDescriptor (
      LogicSystemBase        logicSystem,
      IEnumerable<LogicNode> nodesToInclude
    ) {
      var dotTextAndPlainText = BuildDotTextAndPlainText(
        logicSystem,
        nodesToInclude
      ) ;
      using Clf.Common.Utils.Stopwatch timer = new(
        "Creating NetworkLayoutDescriptor from 'plain' text",
        (elapsedMillseconds) => logicSystem.MillisecsTakenParsingGraphVizOutput = elapsedMillseconds
      ) ;
      // The original 'PlainTextParser_original' (left over from Rodemeyer) was quite slow.
      // The 'PlainTextParser' version using 'Split()' is a lot quicker and also much simpler.
      // The 'PlainTextParser_using_spans' version uses Span<TCustomAttribute> to avoid creating lots of strings,
      // and is faster still ; but it's more complicated due to the necessary restrictions
      // on the usage of Span<TCustomAttribute>, and the speedup isn't really worthwhile as the
      // simpler 'PlainTextParser' is perfectly fast enough.
      return (
        // Clf.LogicSystem.Common.PlainTextParser_original.ParsePlainTextFile(
        Clf.LogicSystem.Common.PlainTextParser.ParsePlainTextFile(
        // Clf.LogicSystem.Common.PlainTextParser_using_spans.ParsePlainTextFile(
          dotTextAndPlainText.PlainText,
          logicSystem.LayoutSizingParameters
        )
      ) ;
    }

    private Clf.GraphViz.IGraphVizRunner m_graphVizRunner = (
      // new GraphViz.GraphVizRunner_UsingInstalledExe() 
      new Clf.GraphViz.GraphVizRunner_CallingLibraryFunction() 
    ) ;

    private LogicSystemRenderer_UsingGraphViz ( )
    { }

    // Removed because it's better to have client code explicitly use the 
    // LogicNodeVisibilityRuleDescriptor to determine the nodes to include.
    // private GraphViz.DotTextAndPlainText BuildDotTextAndPlainText ( 
    //   LogicSystemBase                   logicSystem,
    //   LogicNodeVisibilityRuleDescriptor logicNodeVisibilityRuleDescriptor
    // ) => BuildDotTextAndPlainText(
    //   logicSystem,
    //   logicSystem.GetQualifyingLogicNodes(
    //     logicNodeVisibilityRuleDescriptor
    //   )
    // ) ;

    //
    // We scan the Clf.Clf.LogicSystem visiting the specified nodes, and build a '.dot' representation
    // that mentions (A) the nodes and (B) the links between them. Each node has properties
    // that define its desired size.
    //
    // Then we run a GraphViz layout algorithm that generates a result, in '.plain' format,
    // which assigns (A) a location for each node, and (B) a 'path' for each link to follow.
    // 

    //
    // GraphViz documentation
    // https://graphviz.org/documentation/
    //
    // Web based editor, useful for seeing the result of a DOT file.
    // http://magjac.com/graphviz-visual-editor/
    //
    // Clusters support :
    // https://graphviz.org/Gallery/directed/cluster.html
    //

    private Clf.GraphViz.DotTextAndPlainText BuildDotTextAndPlainText ( 
      LogicSystemBase        logicSystem,
      IEnumerable<LogicNode> nodesToInclude
    ) {
      string dotText_withEmbeddedNewlines ;
      using (
        var dotTextTimer = new Clf.Common.Utils.Stopwatch(
          "Building DOT text",
          (elapsedMillseconds) => logicSystem.MillisecsTakenBuildingDotText = elapsedMillseconds 
        ) 
      ) {
        m_dotText.Clear() ;
        BuildDirectedGraphHeader(logicSystem.LayoutSizingParameters) ;
        // We don't necessarily include *all* the nodes,
        // just the ones specified by the 'visibility rule'.
        // The default would be be to specify 'logicSystem.AllNodes'
        List<int> nodeIDs = nodesToInclude.Select(
          node => node.UniqueIntegerIdentifier
        ).ToList() ;
        nodesToInclude.ForEachItem(
          logicNode => {
            AddIndentedLines(
              $"{logicNode.UniqueIntegerIdentifierAsString} {GetGraphVizAttributesTextForNode(logicNode)}"
            ) ;
            SetNodeRank(logicNode) ;
            // If the node has a 'PositionedBelow', add a fake 'edge'.
            // This will be honoured by GraphViz when it performs the layout, 
            // and it will appear in the generated '.plain' file, but as it
            // doesn't relate to a Link in the actual network, no 'visual'
            // will be generated on the diagram.
            string? positionedValue = logicNode.OptionalAttributeValueOrNull<PositionedBelowAttribute>() ;
            if ( positionedValue != null )
            {
              string[] aboveAndBelow = positionedValue.Split(":") ;
              int fromNodeID = aboveAndBelow[0].ParsedAs<int>() ;
              int toNodeID = aboveAndBelow[1].ParsedAs<int>()  ;
              if ( 
                 nodeIDs.Contains(fromNodeID)
              && nodeIDs.Contains(toNodeID)
              ) {
                // Both nodes are to be incuded !
                AddIndentedLines(
                  $"{fromNodeID} -> {toNodeID} [ arrowhead=none ]"
                ) ;
              }
              else
              {
                // AHA !! Tricky one. Only add that additional edge if both the nodes
                // are present in the 'nodesToInclude' list ... otherwise the presence of
                // the unnecessary edge will provoke GraphViz into creating a new node
                // of that ID and it will subsequently appear in the generated diagram.
              }
            }
          }
        ) ;
        logicSystem.Dependencies.Where(
          // We don't necessarily include *all* the links,
          // just the ones that connect the Nodes we're showing ...
          link => (
             nodesToInclude.Contains(link.FromSource)
          && nodesToInclude.Contains(link.ToTarget)
          )
        ).ForEachItem(
          link => {
            // This 'arrowhead=none' attribute is neceesary, because if we leave it out,
            // the ends of the lines won't go right up to the box edges !
            string linkAttributes_commaSeparated = "arrowhead=none" ;
            AddIndentedLines(
              $"{link.FromSource.UniqueIntegerIdentifierAsString} -> {link.ToTarget.UniqueIntegerIdentifierAsString} [ {linkAttributes_commaSeparated} ]"
            ) ;
          }
        ) ;
        m_indentationLevel-- ;
        AddIndentedLines(
          "}"
        ) ;
        dotText_withEmbeddedNewlines = (
          m_dotText.ToString()
          .Replace( "`"     , "\""   )
          .Replace( " [  ]" , " [ ]" )
          .Replace( "{  }"  , "{ }"  ) 
        ) ;
      }
      using (
        var graphVizTimer = new Clf.Common.Utils.Stopwatch(
          "Running GraphViz",
          (elapsedMillseconds) => logicSystem.MillisecsTakenRunningGraphViz = elapsedMillseconds 
        ) 
      ) {
        return m_graphVizRunner.RunGraphViz_GeneratingDotTextAndPlainTextFromDotText(
          dotText_withEmbeddedNewlines
        ) ;
      }
    }

    private System.Text.StringBuilder m_dotText = new() ;

    private int m_indentationLevel = 0 ;

    private string IndentationSpaces ( ) => "  ".Repeated(m_indentationLevel) ;

    private void AddIndentedLines ( params string[] lines )
    {
      lines.ForEachItem(
        line => m_dotText.AppendLine(
          IndentationSpaces() + line
        )
      ) ;
    }

    private void SetGraphAttributes ( params string[] attributes_asNameValuePairs )
    {
      AddIndentedLines(
        attributes_asNameValuePairs
      ) ;
    }

    // The attributes will be applied to nodes subsequently added,
    // until a further call to this method overrides those values with new ones.

    private void SetDefaultNodeAttibutes ( params string[] attributes_asNameValuePairs )
    {
      AddIndentedLines(
        $"node[{attributes_asNameValuePairs.ToDelimitedList(",")}]"
      ) ;
    }

    private void SetDefaultEdgeAttibutes ( params string[] attributes_asNameValuePairs )
    {
      AddIndentedLines(
        $"edge[{attributes_asNameValuePairs.ToDelimitedList(",")}]"
      ) ;
    }

    private void SetNodeRank ( LogicNode logicNode )
    {
      string? rank = logicNode.OptionalAttributeValueOrDefault<RankAttribute>(
        logicNode is InputNodeBase
        ? "Input"
        : null
      ) ;
      if ( rank != null )
      {
        AddIndentedLines(
          $"{{ rank = same ; {logicNode.UniqueIntegerIdentifierAsString} {rank} }}"
        ) ;
      }
    }

    private string GetGraphVizAttributesTextForNode ( LogicNode node ) 
    {
      // The only attributes we're interested in are ones to do with
      // the size of the rectangle for the Node.
      return BuildAttributesList(
        "shape=rectangle",
        "fixedsize=true",
        $"width={node.RequiredAttributeValue(NodeAttributeNames.WidthX)}",
        $"height={node.RequiredAttributeValue(NodeAttributeNames.HeightY)}"
      ) ;
      string BuildAttributesList ( params string?[] attributes )
      => attributes.WhereNonNull().ToDelimitedList(",").EnclosedInBrackets("[]") ;
    }

    private void BuildDirectedGraphHeader ( 
      Clf.LogicSystem.Common.LayoutSizingParameters layoutSizingParameters,
      Clf.GraphViz.RankDirection                   rankDirection           = Clf.GraphViz.RankDirection.LR_LeftToRight
    ) {
      string rankDirectionMnemonic = rankDirection.ToString().Substring(0,2) ;
      AddIndentedLines(
        "strict digraph", // Hmm, 'digraph Graph' is not legal !!! ???
        "{"
      ) ;
      m_indentationLevel++ ;
      SetGraphAttributes(
        $"layout=dot",
        $"splines=true",
        $"rankdir={rankDirectionMnemonic}",
        $"nodesep={layoutSizingParameters.SeparationBetweenNodes_Vertical:F2}",
        $"ranksep={layoutSizingParameters.SeparationBetweenNodes_Horizontal:F2}"
      ) ;
      // Ranks
      AddIndentedLines(
        // Note that these 'rank' names are 'magic strings'
        // that are mentioned in Attributes. They should be replaced with C# definitions.
        "Input -> a -> b -> c -> d -> Output"
      ) ;
      SetDefaultEdgeAttibutes(
        "arrowhead=none"
      ) ;
    }

  }

}
