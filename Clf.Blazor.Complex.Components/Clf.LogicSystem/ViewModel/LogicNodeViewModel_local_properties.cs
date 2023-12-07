//
// LogicNodeViewModel_local_properties.cs
//

namespace Clf.LogicSystem.ViewModel
{

  //
  // These are UI-related properties that are
  // not linked to the underlying Clf.Clf.LogicSystem Node.
  //
  // For example properties relating to :
  //
  //   - Selection 
  //   - Highlighting
  //   - Pan-and-zoom
  //

  public partial class LogicNodeViewModel
  { 

    // An 'outline' is always drawn, to indicate the nominal 'bounds' of the Node.
    // Its colour is nominally black, but we modify this to indicate the Nodes's
    // HIGHLIGHTED status.

    // ??? Have an abstraction that encapsulates the color and the thickness ??

    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour NodeOutlineColour 
    => (
      HighlightingChoice switch {
      Clf.Common.UI.HighlightingOption.Highlighted         => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForHighlightedNodes,
      Clf.Common.UI.HighlightingOption.HighlightedAsAnchor => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForHighlightedAnchorNode,
      _                                                            => Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForNodeInnerBorder_Default 
      }
    ) ;
                                     
    public Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness NodeOutlineThickness
    => (
      HighlightingChoice switch {
      Clf.Common.UI.HighlightingOption.Highlighted         => Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness.Thick,
      Clf.Common.UI.HighlightingOption.HighlightedAsAnchor => Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness.Thick,
      _                                                            => Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness.Normal 
      }
    ) ;
                              
    // Hmm, we never actually change this ???
    // [ObservableProperty]
    // [GeneratedPropertyNameIs(nameof(ExternalBorderThickness))]
    public Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness ExternalBorderThickness { get ; }= Clf.LogicSystem.Common.GeomertyPrimitives.LineThickness.Thick ;

  }

}

