//
// LogicSystemVisualisationSettings.cs
//

using Clf.LogicSystem.Attributes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clf.LogicSystem.Miscellaneous
{

  // FIX_THIS - names of colours etc could be improved ???

  // This is an 'ObservableObject' in the expectation that we might
  // build a UI that lets you interactively adjust the colours ...

  public partial class LogicSystemVisualisationSettings : ObservableObject
  {

    public readonly static LogicSystemVisualisationSettings Instance = new() ;

    public LogicSystemVisualisationSettings ( )
    {
    }

    // Colours for the overall diagram background

    [ObservableProperty]
    [GeneratedPropertyNameIs(nameof(ColourForOuterBoundingRectangleEnclosingAllNodesAndBothMargins))]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForOuterBoundingRectangleEnclosingAllNodesAndBothMargins = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Yellow ;

    [ObservableProperty]
    [GeneratedPropertyNameIs(nameof(ColourForInnerBoundingRectangleEnclosingAllNodesAndInnerMargin))]
    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForInnerBoundingRectangleEnclosingAllNodesAndInnerMargin = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.LightBlue ;

    [ObservableProperty]
    [GeneratedPropertyNameIs(nameof(ColourForInnerBoundingRectangleEnclosingAllNodes))]
    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForInnerBoundingRectangleEnclosingAllNodes               = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.LightYellow ;

    // 
    // In principle there's a 'margin' area 'outside' the space occupied by the network diagram.
    // However this needn't respond to clicks and mouse moves in the same way
    // as the background area that we paint to represent the 'entire network'.
    //

    // Colours for the body of a node

    [ObservableProperty]
    // [GeneratedPropertyNameIs(ColourForNodesOfValueNull)]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForNodesOfValueNull                  = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.LightGrey ;

    [ObservableProperty]
    // [GeneratedPropertyNameIs(ColourForNodesOfValueTrue)]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForNodesOfValueTrue                  = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Green ; // LightGreen ;

    [ObservableProperty]
    // [GeneratedPropertyNameIs(ColourForNodesOfValueFalse)]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForNodesOfValueFalse                 = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Red ; // LightPink ;

    [ObservableProperty]
    // [GeneratedPropertyNameIs(ColourForNodesOfValueOther)]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForNodesOfValueOther                 = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Yellow ;

    [ObservableProperty]
    // [GeneratedPropertyNameIs(ColourForNodesOfValueEnum)]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForNodesOfValueEnum                 = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Cyan ;

    // Colours for the outline of a node

    [ObservableProperty]
    // [GeneratedPropertyNameIs(ColourForNodeInnerBorder_Default)]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForNodeInnerBorder_Default           = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Black ;

    #if SUPPORTS_SHADOW_VALUES
    [ObservableProperty]
    [GeneratedPropertyNameIs()]
    private Clf.LogicSystem.Common.Colour m_colourToIndicateShadowValueDiscrepancy     = Clf.LogicSystem.Common.Colour.Red ;
    #endif

    // Colours for the 'outer border' surrounding a node (highlighting)

    [ObservableProperty]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForHighlightedNodes                  = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Orange ;

    [ObservableProperty]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForHighlightedAnchorNode             = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Cyan ;

    // Colours for a connecting link

    [ObservableProperty]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForLinesBetweenNodes                 = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Blue ;

    [ObservableProperty]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForLinesBetweenNodes_Highighted      = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Red ;

    // ------------------------

    [ObservableProperty]
    private bool m_showMouseCoordinatesOnTooltips = true ;

    [ObservableProperty]
    public bool m_writeMouseMoveMessages              = false ;

    [ObservableProperty]
    public bool m_writeMouseMoveHitTestResultMessages = false ;

    [ObservableProperty]
    private Clf.LogicSystem.Common.GeomertyPrimitives.Colour m_colourForCirclesDrawnAtLinkPoints          = Clf.LogicSystem.Common.GeomertyPrimitives.Colour.Red ;

  }

}
