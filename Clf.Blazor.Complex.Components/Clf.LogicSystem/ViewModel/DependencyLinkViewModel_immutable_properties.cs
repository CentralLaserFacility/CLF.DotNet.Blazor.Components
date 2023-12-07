//
// DependencyLinkViewModel_immutable_properties.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem.ViewModel
{

  // These are properties that are set up when the ViewModel is created.
  // For example the identity of the Clf.LogicSystem element it refers to.

  public partial class DependencyLinkViewModel 
  {

    public int SourceNodeId { get ; }

    public int TargetNodeId { get ; }

    public string? ToolTipTextLine { get ; set ; }

    // These points are 'control points' for a smooth Bezier curve
    // that should be drawn to connect the first and last points.
 
    public IReadOnlyList<System.Drawing.PointF> PointsOnPath { get ; }

    public override IEnumerable<string> ToolTipTextLines => new []{
      ToolTipTextLine ?? $"#{SourceNodeId} influences #{TargetNodeId}"
    } ;

  }

}

