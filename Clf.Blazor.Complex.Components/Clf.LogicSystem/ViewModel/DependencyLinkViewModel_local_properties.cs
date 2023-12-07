//
// DependencyLinkViewModel_local_properties.cs
//

using CommunityToolkit.Mvvm.ComponentModel;

namespace Clf.LogicSystem.ViewModel
{

  // These are properties that are not linked to the underlying Clf.Clf.LogicSystem.
  // For example Highlight related properties.

  public partial class DependencyLinkViewModel 
  { 
  
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LineThicknessInPixels))]
    [NotifyPropertyChangedFor(nameof(LineColour))]
    [NotifyPropertyChangedFor(nameof(StackingOrderZ))]
    public bool m_showAsHighlighted ;
                                     
    public float LineThicknessInPixels => LayoutSizingParameters.GetDependencyLinkLineThicknessInPixels(ShowAsHighlighted) ;
        
    public Clf.LogicSystem.Common.GeomertyPrimitives.Colour LineColour => (
      ShowAsHighlighted 
      ? Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForLinesBetweenNodes_Highighted 
      : Clf.LogicSystem.Miscellaneous.LogicSystemVisualisationSettings.Instance.ColourForLinesBetweenNodes 
    ) ;

    public override int StackingOrderZ => ShowAsHighlighted ? 3 : 2 ;

  }

}

