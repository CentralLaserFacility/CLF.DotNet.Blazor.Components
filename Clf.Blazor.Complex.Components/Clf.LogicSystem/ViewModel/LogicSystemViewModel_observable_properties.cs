//
// LogicSystemViewModel_observable_properties.cs
//

using Clf.LogicSystem.Attributes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clf.LogicSystem.ViewModel
{

  //
  // These properties are tagged as [ObservableProperty]
  // which makes them 'observable' in that they have get/set functionality
  // and automatic support for change propagation.
  //
  // However, note that *all* public properties are accessible
  // to a View, not just these that are explicitly marked.
  //
  // Properties that are not relevant to a View are marked as 'internal'.
  //

  public partial class LogicSystemViewModel 
  { 
  
    [ObservableProperty]
    [GeneratedPropertyNameIs(nameof(InputChangesAreEnabled))]
    private bool m_inputChangesAreEnabled = false ;

    [ObservableProperty]
    private string m_mainDescriptionTextLine = "No description" ;

    // Won't be needed when we have a 'Diagram' element ?
    // public int StackingOrderZ => 0 ;

    // TransformMatrix ; 3x3 defining a 2-D affine transform

    // This is initially set up by the View, as a transform which
    // when applied to the Diagram (defined in 'diagram coordinates')
    // maps the coordinates to ones that produce a rendering of the
    // diagram items on the entire available canvas of the View.
    public float[]? TransformMatrix_Default = null ;

    // This matrix gets updated as we pan-and-zoom.
    // It can be used (A) to drive the panned-and-zoomed view,
    // and also (B) to draw an 'active area' box on a thumbnail view.
    public float[]? TransformMatrix_PannedAndZoomed = null ;

  }

}

