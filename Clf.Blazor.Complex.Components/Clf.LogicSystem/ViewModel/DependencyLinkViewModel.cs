//
// DependencyLinkViewModel.cs
//

using Clf.LogicSystem.Common;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.Common.GeomertyPrimitives;
using Clf.LogicSystem.Miscellaneous;
using System.Collections.Generic;

namespace Clf.LogicSystem.ViewModel
{

  // We can provide ToolTip text etc by querying the model element.

  public sealed partial class DependencyLinkViewModel : LogicSystemViewModelElementBase
  {

    public void RaisePropertyChanged ( string? propertyName = null )
    {
      OnPropertyChanged(propertyName??"") ;
    }

    public Clf.LogicSystem.Dependencies.Dependency DependencyLink { get ; private set ; }

    public DependencyLinkViewModel ( 
      LogicSystemViewModel                 parent,
      int                                  sourceNodeId, 
      int                                  targetNodeId, 
      IReadOnlyList<System.Drawing.PointF> pointsOnPath 
    ) :
    base(parent)
    {
      SourceNodeId = sourceNodeId ;
      TargetNodeId = targetNodeId ;
      PointsOnPath = pointsOnPath ;
      DependencyLink = parent.LogicSystem.LookupDependency_FromSourceAndTargetNodeUniqueIntegerIdentifiers(
        SourceNodeId,
        TargetNodeId
      ) ;
    }

    // If we draw a Bezier curve, it'll be necessary
    // to access 'control points' that lie before and after the
    // first and last point respectively. We'll try faking these
    // by interpolating back and forward linearly.

    public System.Drawing.PointF ControlPointBeforeStartPoint 
    {
      get
      {
        var delta = DeltaXY.FromPoints(
          PointsOnPath[0],
          PointsOnPath[1]
        ) ;
        return PointsOnPath[0].MovedBy(-delta) ;
      }
    }

    public System.Drawing.PointF ControlPointAfterEndPoint 
    {
      get
      {
        var delta = DeltaXY.FromPoints(
          PointsOnPath[^2],
          PointsOnPath[^1]
        ) ;
        return PointsOnPath[^1].MovedBy(delta) ;
      }
    }

    // Default constructor is required for JSON persistence ?
    // public DependencyLinkViewModel ( ) :
    // this(
    //   sourceNodeId : -1,
    //   targetNodeId : -1,
    //   pointsOnPath       : new List<System.Drawing.PointF>()
    // ) {
    // }

    public override string ToString ( ) => $"Link #{SourceNodeId} -> #{TargetNodeId}" ;

    public override void HandleMouseRightButtonEvent_PopulatingContextMenu (
      Clf.Common.MenuHandling.MenuDescriptor contextMenu 
    ) { 
      // Nothing to do ???
    }

    public override void HandleMouseLeftButtonEvent ( )
    { 
      // Just to demonstrate that we can detect clicks on Links,
      // let's toggle the colour of the Link line. This is actually useful
      // when we've got lots of links bunched up, and we want to see the path
      // followed by a particular link.

      ShowAsHighlighted = ! ShowAsHighlighted ;

      // Note that the 'ShowAsHighlighted' property affects not only
      // (A) the rendering colour of a 'link', but also (B) the order
      // in which elements should be painted - because we want 'highlighted' lines
      // to come out 'on top' and therefore to be painted 'last'. Changing the order
      // requires rebuilding the entire display, and ordering the elements appropriately
      // to honour the Z order that has now changed due to the highlighting.

      Parent.RaisePropertyChangedEvent(MagicStrings.REPAINT) ;

    }

  }

}

