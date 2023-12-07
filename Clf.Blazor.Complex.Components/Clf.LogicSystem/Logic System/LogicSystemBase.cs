//
// LogicSystemBase.cs
//

using Clf.LogicSystem.Common.ExtensionMethods;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Clf.LogicSystem.Miscellaneous;
using Clf.LogicSystem.LogicNodes;
using Clf.LogicSystem.ChangeDescriptors;
using Clf.LogicSystem.Attributes;
using Clf.LogicSystem.LogicNodes.ExtensionMethods;
using Clf.LogicSystem.Dependencies;
using Clf.LogicSystem.LogicNodesManagers;
using Clf.Common.ExtensionMethods;

namespace Clf.LogicSystem
{

  //
  // Hmm, subclass members defined as Nodes should be Properties of this Clf.Clf.LogicSystem base class,
  // but other properties should be nested inside a 'container' class ??
  //

  public abstract partial class LogicSystemBase : Clf.LogicSystem.Common.PlatformServices.HasPlatformServicesProperty, IAcceptsNamedInputNodeChangeSubmissions
  {
    
    public string? SourceFilePath { get ; protected set ; }

    public string Name { get ; init ; }
    
    public string LogicSystemClassName => this.GetType().FullName! ;

    private static int g_nInstancesCreated = 0 ;

    public int InstanceNumber { get ; }
    
    public string? Description { get ; init ; }

    public IEnumerable<string> WarningMessages { get ; private set ; } = Enumerable.Empty<string>() ;
    
    public IEnumerable<string> ErrorMessages { get ; private set ; } = Enumerable.Empty<string>() ;
    
    public IEnumerable<LogicNode> AllLogicNodes => LogicNodesManager.AllLogicNodes ;

    public IEnumerable<LogicNode> NoLogicNodes => Enumerable.Empty<LogicNode>() ;

    public IEnumerable<Dependency> Dependencies => LogicNodesManager.Dependencies ;

    //
    // This 'LogicSystemBase' class is already quite complicated,
    // so everything to do with managing Nodes and Dependencies
    // is delegated to a helper object, the LogicNodesManager.
    //

    internal readonly LogicNodesManager LogicNodesManager ;

    public bool ShouldInvokeNodeSpecificValueActionChangeHandlers { get ; set ; }
    #if SUPPORTS_FUTURE_ACTIONS
      // To implement the somewhat dodgy 'future actions' feature,
      // we rely on client code setting up a value-changed handler
      // on specific logic nodes, where the body of the handler
      // typically contains code to schedule a 'future update'.
      = true ;
    #else
      = false ;
    #endif

    // This property gives us a convenient way of responding
    // to any and all changes in computed-node Values that result from an input change.

    public System.Action<LogicNodeChangesArisingFromInputValueChange>? LogicNodeValueChangesHandler { get ; set ; } = null ;

    protected LogicSystemBase ( string? description = null )
    {
      Description = description ;
      Name = this.ClassName() ;
      InstanceNumber = ++g_nInstancesCreated ;
      LogicNodesManager = new LogicNodesManager(
        this
      ) ;

      BuildDependencies();
      RestoreScenario();
      EnsureMonitoredChannelsConfigured();
    }

    protected void OnBuildDependencies_Begin()
    {
      LogicNode.ParentLogicSystemBeingConfigured = this;
      LogicNodesManager.BuildDependencies_Begin();
    }
    
    protected void OnBuildDependencies_End()
    {
      LogicNodesManager.BuildDependencies_End();
    }

    protected void RestoreScenario()
    {
      (
        WarningMessages,
        ErrorMessages
      ) = LogicNodesManager.ValidateLogicSystem() ;
      ScanTargetObjectProperties_BuildingScenarioCollections(this) ;
      #if true
        // This is marginally less efficient than the alternative
        // version below, but this way we avoid duplicating code
        // and can be 100% confident that state will be totally restored.
        RestoreInputsAsSpecifiedInScenario(DefaultScenario) ;
      #else
        // Our scan of the Computed Properties will have triggered
        // evaluations, and we need to discard those spurious values.
        ComputedLogicNodes.ForEachItem(
          computedLogicNode => computedLogicNode.Initialise() 
        ) ;
        ComputedLogicNodes.ForEachItem(
          computedLogicNode => _ = computedLogicNode.Value 
        ) ;
        ComputedLogicNodes.ForEachItem(
          computedLogicNode => {
            computedLogicNode.VerifyCachedComputedPropertyValueIsUpToDate() ;
            computedLogicNode.SetPreviousValueFromCurrentValue() ;
          }
        ) ;
      #endif
    }

    //
    // This restores the logic system to the same state 
    // that it has immediately after a new instance has been created.
    //
    // Hmm, should write a test to verify this ...
    //

    public void RestoreInputsAsSpecifiedInScenario ( Scenario scenario )
    {
      try
      {
        LogicNodesManager.IsRestoringInputsFromSavedScenario = true ;
        // First of all, set all Input nodes to null.
        // Can set Boolean inputs to true/false if the scenario specifies that.
        // bool? initialValueForAllBooleanInputs = scenario.RootScenario.ValueForAllBooleanInputs ;
        NodesOfType<InputNodeBase>().ForEachItem(
          inputNode => {
            inputNode.SetValueAsDefault_WITHOUT_PROPAGATING_CHANGES() ;
          }
        ) ;
        // Reset all the Computed nodes,
        // declaring that their cached values are not valid
        ComputedLogicNodes.ForEachItem(
          computedLogicNode => computedLogicNode.Initialise() 
        ) ;
        // The scenario might mention specific values 
        // for specific input nodes, so apply those values.
        // We expect that all the values will be able to be successfully applied,
        // and if that's not the case, we want to know about it. For example,
        // our NodeAndValueDescriptor might mention a node that's not present
        // in the Clf.Clf.LogicSystem, or which has the wrong type. That could happen
        // if we try to apply an out-of-date scenario, or one that's been
        // edited incorrectly.
        List<NodeAndValueDescriptor> changesThatCouldNotBeApplied = new() ;
        scenario.AllInputChangesRelativeToRootScenario.ForEachItem(
          change => change.LogicNode_AsInputNode().CanSetValue_ParsedFromString(
            change.ValueAsString
          ).WithActionOnFalseValue(
            () => changesThatCouldNotBeApplied.Add(change)
          ) 
        ) ;
        if ( changesThatCouldNotBeApplied.Any() )
        {
          // Impossible !!!
          throw new System.Diagnostics.UnreachableException("Failed to apply changes") ;
        }
        // Now ensure that the computed node values are up to date.
        // Note that the act of visiting a Value may trigger a paranoid check
        // that the cached value is the same as a freshly computed evaluation.
        ComputedLogicNodes.ForEachItem(
          computedLogicNode => _ = computedLogicNode.ValueAsObject
        ) ;
        ComputedLogicNodes.ForEachItem(
          computedLogicNode => {
            computedLogicNode.VerifyCachedComputedPropertyValueIsUpToDate() ;
            computedLogicNode.SetPreviousValueFromCurrentValue() ;
          }
        ) ;
      }
      finally
      {
        LogicNodesManager.IsRestoringInputsFromSavedScenario = false ;
        LogicNodesManager.VerifyComputedPropertyCachedValuesAllUpToDate() ;
      }
    }


    public IEnumerable<T> NodesOfType<T> ( ) 
    where T : LogicNode
    => LogicNodesManager.LogicNodesOfType<T>() ;

    // Input Logic Nodes

    public IEnumerable<InputNodeBase> InputLogicNodes
    => LogicNodesManager.LogicNodesOfType<
      InputNodeBase
    >() ;

    //
    // Computed Logic Nodes
    //

    public IEnumerable<ComputedNodeBase> ComputedLogicNodes
    => LogicNodesManager.LogicNodesOfType<
      ComputedNodeBase
    >() ;

    //
    // Output nodes (ie nodes that don't feed into any Computed other nodes)
    //

    public IEnumerable<ComputedNodeBase> OutputLogicNodes ( ) => ComputedLogicNodes.Where(
      node => node.IsOutputNode_NotFeedingIntoOtherComputedNodes()
    ) ;

    // Look up an Input node 

    public bool InputNodeExists<T> (
      string                                propertyNameOrChannelName,
      [NotNullWhen(true)] out InputNode<T>? logicNode
    ) where T:struct => (
      LogicNodesManager.LogicNodesOfType<
        InputNode<T>
      >().HasSingleMatchingItem(
        node => node.HasPropertyNameOrChannelNameMatching(propertyNameOrChannelName),
        out logicNode
      )
    ) ;

    public IEnumerable<string> NodeChannelNames => AllLogicNodes.Select(
      node => node.ChannelNameAttributeOrNull()
    ).OfType<string>(
    ).ToArray() ;

    //
    // Client code may install an alternative set of parameters that determine the sizes of nodes.
    // These dimensions are used (A) when we're computing the positions of the nodes etc
    // using GraphViz, and (B) when we're drawing the nodes onto a display panel.
    // 
    // The default provides for a character-cell height of 10 pixels, and all other settings
    // are computed from that.
    //

    public Clf.LogicSystem.Common.LayoutSizingParameters LayoutSizingParameters { get ; set ; } 
    = new Clf.LogicSystem.Common.LayoutSizingParameters(
      CharacterCellDescriptor : new Clf.LogicSystem.Common.CharacterCellDescriptor(
        CharacterHeightInPixels : Clf.LogicSystem.Common.CharacterCellDescriptor.CharacterHeightInPixels_Default
      )
    ) ;

    // Client code can install a custom algorithm to compute the size of a node.

    public static System.Action<LogicNode> ConfigureNodeWidthAndHeightAttributes 
    = ConfigureNodeWidthAndHeightAttributesBasedOnTextualContent ;

    // This is the 'default' algorithm used to compute the size of a node.

    // Note : we only need width/height attributes (for the layout algorithm)
    // when we're generating a visualisation. And it's the visualisation that knows
    // the appropriate 'LayoutSizingParameters', eg character cell sizes.
    // So it'd be better if the layout was performed on an as-needed basis,
    // with the node sizes computed by referring to a LayoutSizingParameters
    // instance provided by the Visualiser !!!
    //
    // Here we're assuming a fixed size per character cell, but in principle
    // we could compute the size based on the actual text to be rendered,
    // if the font had varying widths per cell.

    public static void ConfigureNodeWidthAndHeightAttributesBasedOnTextualContent ( 
      LogicNode logicNode
    ) {
      // For computing the overall box width, we need to accommodate a '(null)' value,
      // which is the one that takes the widest number of characters (6), 
      // even though the current value might be 'True' (4) or 'False' (5).
      // This is only an issue in cases where the line showing the 'value' 
      // happens to be the widest line of the sequence. In this case our fix makes
      // the box a little bit wider than it needs to be for a 'True' value,
      // but this is a better compromise than having a subsequent 'False' value
      // have no space at all to the left or right and be squished against 
      // the side of the box.

      var boxTextualContentLines = logicNode.LabelTextLinesTemplate() ;
      int nCharactersOnLongestLineX = boxTextualContentLines.Max(
        textLine => textLine.Length
      ) ;
      bool willShowValue = boxTextualContentLines.Any(
        line => line.Contains(MagicStrings.VAL_PLACEHOLDER)
      ) ;
      if ( willShowValue ) 
      {
        int nCharsToAllowForValue = logicNode.ValueSummaryLengthExpected() ;
        if ( nCharsToAllowForValue > nCharactersOnLongestLineX )
        {
          nCharactersOnLongestLineX = nCharsToAllowForValue ;
        }
        if ( 
          logicNode.HasAttribute(
            nameof(MinimumNodeWidthInCharactersAttribute),
            out string? minimumNodeWidthInCharacters
          ) 
        && int.TryParse(
             minimumNodeWidthInCharacters,
             out int minimumNodeWidth
           )
        ) {
          if ( minimumNodeWidth > nCharactersOnLongestLineX )
          {
            nCharactersOnLongestLineX = minimumNodeWidth ;
          }
        }
      }

      int nTextualContentLinesY = boxTextualContentLines.Count() ;
      var nodeLayoutSizingParameters = logicNode.LogicSystem.LayoutSizingParameters ;
      var totalBoxSize = nodeLayoutSizingParameters.ComputeNodeOutlineRectangleSize(
        nCharactersOnLongestLineX, 
        nTextualContentLinesY
      ) ;
   
      logicNode.AddAttribute(
        NodeAttributeNames.WidthX,
        $"{totalBoxSize.Width:F3}"
      ) ;
      logicNode.AddAttribute(
        NodeAttributeNames.HeightY,
        $"{totalBoxSize.Height:F3}"
      ) ;
    }

    public virtual void BuildDependencies() {
        System.Console.WriteLine("Source generation was supposed to override it in child class !!");
    }

  }

}
