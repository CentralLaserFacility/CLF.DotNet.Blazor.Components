//
// LayoutSizingParameters.cs
//

namespace Clf.LogicSystem.Common
{

  //
  // This structure goes hand in hand with the NetworkLayoutDescriptor.
  //
  //   LayoutSizingParameters + LogicNodes ----------------------> NetworkLayoutDescriptor
  //                                          layout algorithm
  //                                            eg GraphViz                
  // 
  // Then subsequently :
  //
  //    NetworkLayoutDescriptor -----------> LogicSystemViewModel
  //                                           |
  //                                           DrawingCanvasViewModel
  //                                           LogicNodeViewModel[]
  //                                           DependencyLinkViewModel[]
  //

  //
  // When we compute a Layout for a set of Logic Nodes,
  // each node needs to have been assigned a Width and Height.
  //
  // We'll allocate the size based on the textual content of the node,
  // allowing (A) one character cell's worth of space for each character,
  // plus (B) a 'margin' at the top and bottom.
  //
  //   +-----------------------------------+ <--------------------- NodeOutlineRectangle
  //   |###################################| 
  //   |###################################| <---- SpaceAroundLabelText
  //   |###################################| 
  //   |#####+---+---+---+---+---+---+#####| <---- LabelTextRectangle
  //   |#####| A | A | A | A | A | A |#####|
  //   |#####+---+---+---+---+---+---+#####|     
  //   |#####| B | B | B | B |   |   |#####|     
  //   |#####+---+---+---+---+---+---+#####|       +---+  LabelTextCellSize
  //   |#####| C | C | C | C | C |   |#####|       | A |
  //   |#####+---+---+---+---+---+---+#####|       +---+
  //   |###################################| 
  //   |###################################| 
  //   |###################################| 
  //   +-----------------------------------+
  //
  // The 'rectangles' mentioned here are abstract definitions that don't have a thickness.
  // When they are rendered onto a screen, we use a 'pen' that has a non zero thickness,
  // and that thickness needs to be accommodated in our calculations.
  //
  // The size of a node's box does NOT account for the width of the 'outline' we'll draw around the box.
  // That outline will extend both 'outside' the box, and also 'inside' the box where it'll overlap
  // with some of the 'SpaceAroundLabelText' that we've allowed for, surrounding the grid of cells.
  //
  //       TopLeft point of the NodeOutlineRectangle
  //       |
  //       *xxxxxxxxxxxxxxx ---- NodeOutlineRectangle (shown here with minimum thickness)
  //       x###############
  //       x###############           Lines drawn between nodes start and end
  //       x###############           on points that coincide with the NodeOutline.
  //       x######+---------+         
  //       x######|         |         
  //       x######|         |         When we draw a character into a text cell, we need to know
  //       x######|         |         the offset from the top-left point to the reference position
  //       x######|         |         for that first character, which is a little bit above
  //       x######|*        | <------ the bottom of the cell (depending on the font).
  //       x######|         |         
  //       x######+---------+         
  //

  //
  // When we draw a 'thick' outline, it extends (A) 'outside' the box,
  // and also (B) 'inside' the box where it'll eat into some of
  // the space we've allowed for around the grid of cells.
  //
  //      xxxxxxxxxxxxxxxxxxx                               
  //      x[*]xxxxxxxxxxxxxxx ---- NodeOutlineRectangle with a significant thickness
  //      xxxxxxxxxxxxxxxxxxx
  //      xxxxx#############
  //      xxxxx#############
  //      xxxxx####+---------+---- LabelTextRectangle
  //      xxxxx####|         |
  //      xxxxx####|         |
  //      xxxxx####|         |
  //      xxxxx####|         | 
  //      xxxxx####|*        | 
  //      xxxxx####|         | 
  //      xxxxx####+---------+---
  //               |         |
  //
  // When we draw the node outline, it will have half its thickness
  // in each direction 'around' the nominal path of the rectangle.
  //
  // Our layout algorithm will assign positions to the nodes
  // based on the size of the NodeOutlineRectangle.
  //

  //
  // We also want the possibility of drawing an additional 'external border'
  // on each node, for example to indicate when the node is selected,
  // or when there's a discrepancy between a computed output value
  // and the equivalent value coming from a PV.
  //
  //      bbbbbbbbbbbbbbbbbbbbbb
  //      bb*bbbbbbbbbbbbbbbbbbb ---- ExternalBorder thickness ------+---
  //      bbbbbbbbbbbbbbbbbbbbbb                                     | External
  //      bbbb                       <-- ExternalBorder spacing      | border
  //      bbbb   xxxxxxxxxxxxxxxxxxx                                 | offset
  //      bbbb   xxxxxxxxxxxxxxxxxxx ---- Outline thickness ---------+------------- NodeOutlineRectangle
  //      bbbb   xxxxxxxxxxxxxxxxxxx
  //      bbbb   xxxxx#############
  //      bbbb   xxxxx#############
  //             xxxxx####+---------+
  //             xxxxx####|         |
  //             xxxxx####|         | 
  //             xxxxx####|         |
  //             xxxxx####|         | 
  //             xxxxx####|*        | 
  //             xxxxx####|         | 
  //             xxxxx####+---------+ 
  //
  // The size of this 'external' border does NOT contribute to the NodeOutline rectangle.
  // 

  //
  // An instance of 'LayoutSizingParameters' is available as a property of the Clf.Clf.LogicSystem.
  // This instance has reasonable 'default' settings, allowing a typical height for each cell
  // which is derived from the font size in pixels.
  //
  // If desired, client code can provide an alternative set of 'LayoutSizingParameters'
  // and install it into the Clf.Clf.LogicSystem as a property. That is actually very desirable,
  // as the client code is in control of the font that'll be used for rendering the UI
  // and is best placed to define a reasonable size for the character cells.
  //
  // When we come to render a Node in a visualiser, the drawing code should examine
  // the NodeLayoutSizingParameters in order to choose the appropriate font size
  // and place the text at an appropriate position within the assigned rectangle.
  //
  // The 'border' that we'll draw around the text is not accommodated in the
  // box-size calculation, but it is represented here so that all the factors that
  // influence the dimensions of drawn items are in one place.
  //
  // Typically the 'border line' will be drawn to coincide with the boundary of the rectangle.
  // Half of the line will lie 'inside' that infinitely thin rectangle, and half 'outside'.
  //
  // The colour of the border line can be used to indicate the status of a node. Additional lines
  // indicating other attributes can in principle be drawn 'outside' of the main border, and their
  // thickness and spacing would ordinarily be related to the nominal border-line-thickness.
  //

  //
  // We also specify the width and height of the blank border that will be assigned around the
  // area of the diagram that accommodates the nodes.
  //
  // The overall layout generated from a set of Logic Nodes
  // is described by a NetworkLayoutDescriptor :
  //
  //  0,0
  //   +-----------------------------------------------+
  //   | Outer Margin                                  |
  //   |   +---------------------------------------+   |
  //   |   | Inner Margin                          |   |    The outlines of the boxes for nodes 'A' and 'D'
  //   |   |   +------+------------------------+   |   |    define the 'nominal' boundary of the diagram,    
  //   |   |   |  A   |             +------+   |   |   |    as computed by GraphViz.    
  //   |   |   +------+             |  B   |   |   |   |    
  //   |   |   |                    +------+   |   |   |    We allow an additional 'inner' margin so that
  //   |   |   |        +------+               |   |   |    graphics drawn 'outside' the nominal area, eg showing
  //   |   |   |        |  C   |        +------+   |   |    an external border around nodes, won't be truncated.
  //   |   |   |        +------+        |  D   |   |   |   
  //   |   |   +------------------------+------+   |   |    And also an 'outer' margin so that we'll have 
  //   |   |                                       |   |    a distinct region *outside* the diagram itself,
  //   |   +---------------------------------------+   |    which can be painted in a distinguishing colour.
  //   |                                               |   
  //   +-----------------------------------------------+
  //
  //  0,0
  //   +-----------------------------------------------+  -- OuterBoundingRectangleEnclosingAllNodesAndBothMargins
  //   | Outer Margin                                  |
  //   |   +---------------------------------------+   |  -- InnerBoundingRectangleEnclosingAllNodesAndInnerMargin
  //   |   | Inner Margin                          |   |  
  //   |   |   +------+------------------------+   |   |  -- InnerBoundingRectangleEnclosingAllNodes
  //   |   |   |  A   |             +------+   |   |   |  
  //   |   |   +------+             |  B   |   |   |   |  
  //   |   |   |                    +------+   |   |   |  
  //   |   |   |        +------+               |   |   |  
  //   |   |   |        |  C   |        +------+   |   |  
  //   |   |   |        +------+        |  D   |   |   |  
  //   |   |   +------------------------+------+   |   |  
  //   |   |                                       |   |  
  //   |   +---------------------------------------+   |  
  //   |                                               |  
  //   +-----------------------------------------------+
  //
  //  0,0
  //   +-----------------------------------------------+  OuterBoundingRectangleEnclosingAllNodesAndBothMargins
  //   | Outer Margin                                  |
  //   |   +---------------------------------------+   |  InnerBoundingRectangleEnclosingAllNodesAndInnerMargin
  //   |   | Inner Margin                          |   |  
  //   |   |   +-------------------------------+   |   |  InnerBoundingRectangleEnclosingAllNodes
  //   |   |   |                               |   |   |  
  //   |   |   |   NodesRectangleMinimumSize   |   |   |  If there are no nodes present, the innermost
  //   |   |   |                               |   |   |  rectangle still takes on a non zero Minimum Size.
  //   |   |   +-------------------------------+   |   |  It remains visible and can be clicked on.
  //   |   |                                       |   |  
  //   |   +---------------------------------------+   |  
  //   |                                               |  
  //   +-----------------------------------------------+
  //

  //
  // All dimensions are in pixels.
  //

  //
  // FIX_THIS : we could create an instance of this in the UI (eg WPF or Blazor)
  // at which point we know the size of a character cell, from the font definition.
  // Then pass it from there into the Clf.Clf.LogicSystem ; from there it will be given to
  // the ViewModels and the Views.
  //

  public record LayoutSizingParameters ( CharacterCellDescriptor CharacterCellDescriptor ) 
  {

    // An instance of 'LayoutSizingParameters' is available as a property of the Clf.Clf.LogicSystem.
    // By default that instance uses the default settings defined here, but a different
    // set of sizes could be installed by client code if desired.

    float TextHeightInPixels => CharacterCellDescriptor.CharacterHeightInPixels ;

    float TextWidthAsFractionOfTextHeight => CharacterCellDescriptor.CharacterWidthAsFractionOfHeight ;

    float TextWidthInPixels => CharacterCellDescriptor.CharacterWidthInPixels ;

    public System.Drawing.SizeF CharacterCellSize => CharacterCellDescriptor.SizeInPixels ;

    // FIX_THIS ??? Scale these by the character cell height ???
    // In effect, our dimensions will be multiples of 'ems' ...

    public System.Drawing.SizeF SpaceAroundLabelText   { get ; init ; } = new ( width : 5.0f  , height : 5.0f  ) ;

    public System.Drawing.SizeF DiagramOuterMarginSize { get ; init ; } = new ( width : 4.0f  , height : 4.0f  ) ;

    public System.Drawing.SizeF DiagramInnerMarginSize { get ; init ; } = new ( width : 20.0f , height : 20.0f ) ;

    public System.Drawing.SizeF DiagramTotalMarginSize => DiagramOuterMarginSize + DiagramInnerMarginSize ;

    public float NodeOutlineThickness        { get ; init ; } = 2.0f  ;

    public float NodeExternalBorderThickness { get ; init ; } = 2.0f  ;
    
    public float NodeExternalBorderSpacing   { get ; init ; } = 5.0f  ; 

    public float NodeExternalBorderOffset => (
      NodeOutlineThickness / 2
    + NodeExternalBorderThickness / 2
    + NodeExternalBorderSpacing
    ) ;
  
    public System.Drawing.SizeF ComputeLabelTextRectangleSize ( int nCharactersX, int nCharactersY )
    => new System.Drawing.SizeF(
      width : ( 
        nCharactersX * this.CharacterCellSize.Width 
      ),
      height : ( 
        nCharactersY * this.CharacterCellSize.Height 
      )
    ) ;

    public System.Drawing.SizeF ComputeNodeOutlineRectangleSize ( int nCharactersX, int nCharactersY )
    => new System.Drawing.SizeF(
      width : ( 
        ComputeLabelTextRectangleSize(nCharactersX,nCharactersY).Width 
      + SpaceAroundLabelText.Width // Space to left of the text
      + SpaceAroundLabelText.Width // Space to right of the text
      ),
      height : ( 
        ComputeLabelTextRectangleSize(nCharactersX,nCharactersY).Height 
      + SpaceAroundLabelText.Height // Space above the text line
      + SpaceAroundLabelText.Height // Space below the text line
      )
    ) ;

    public System.Drawing.SizeF NodeOutlineRectangleMinimumSize { get ; init ; } = new(30,10) ;

    // FIX_THIS : the value can be cached rather than being computed every time ...

    public System.Drawing.SizeF OffsetFromTopLeftPointToFirstCharacterBaselinePosition 
    => new System.Drawing.SizeF(
      width : (
        SpaceAroundLabelText.Width                              // Space to left of the character cell
      ),
      height : (
        SpaceAroundLabelText.Height                             // Space above the first character cell
      + CharacterCellDescriptor.OffsetOfTextBaseLineFromCellTop // Distance down from the cell top
      )
    ) ;

    // Thickness for the lines that represent DependencyLinks

    public float DependencyLinkLineThickness_Normal { get ; init ; } = 2.0f  ;
    public float DependencyLinkLineThickness_Thick  { get ; init ; } = 4.0f  ;
    public float DependencyLinkLineThickness_Thin   { get ; init ; } = 1.0f  ;

    public float GetDependencyLinkLineThicknessInPixels ( Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness lineThickness )
    => lineThickness switch {
    Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness.Thin   => DependencyLinkLineThickness_Thin,
    Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness.Normal => DependencyLinkLineThickness_Normal,
    Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness.Thick  => DependencyLinkLineThickness_Thick,
    _                                          => 0.0f // Impossible !!!
    } ;

    public float GetDependencyLinkLineThicknessInPixels ( bool showAsHighlighted )
    => (
      showAsHighlighted 
      ? DependencyLinkLineThickness_Thick
      : DependencyLinkLineThickness_Normal
    ) ;

    // FIX_THIS : should scale with cell size ??

    public float ArrowheadLength { get ; init ; } = 10.0f ; 

    public float SeparationBetweenNodes_Horizontal => 20.0f ;

    public float SeparationBetweenNodes_Vertical   => 20.0f ;

    // We also need :
    // Factors by which to modify the thicknesses for 'thin' and 'thick' options.

  }

  // public static class ScalingExtensions
  // {
  //   public static float ScaledByCharacterHeight ( this float dimensionInPixels )
  //   {
  //     return dimensionInPixels * CharacterCellDescriptor.CharacterHeightInPixels_Default ;
  //   }
  // }

}
