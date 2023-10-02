//
// IntensityMapImageViewModel.cs
//

using System.Collections.Generic;
using System.Linq;

using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;

using Clf.Blazor.Basic.Components.Controls.ViewModels;
using Clf.Common.ImageProcessing;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public class IntensityMapImageViewModel
  {
    public ImageViewerViewModel ImageViewer { get; private set; }

    public IntensityMapImageViewModel(IntensityMapViewerViewModel parent, double displayScalingFactor)
    {
      ImageViewer = new ImageViewerViewModel(
        displayImageScalingFactor: displayScalingFactor
      )
      {
        OverlaysDescriptor = new OverlaysDescriptor(
      // Default ...
      // m_overlaysDescriptor[0] Profile Crosshairs
      // m_overlaysDescriptor[1] Software Crosshairs
      // m_overlaysDescriptor[2] Software Box
      // m_overlaysDescriptor[3] Contour (Under consideration for Centroid calculation) 
      // m_overlaysDescriptor[4] Best fit rectangle for Centroid Calculation using Edge Detection
      // m_overlaysDescriptor[5] Best fit circle for Centroid Calculation using Edge Detection
      OverlayCrossDescriptor.FromCentrePoint(20, 20, 10, RgbByteValues.Red, false),
      OverlayCrossDescriptor.FromCentrePoint(50, 50, 10, RgbByteValues.Red, false),
      OverlayBoxDescriptor.FromCentrePoint(50, 50, 10, 10, RgbByteValues.Red, false),
      OverlayClosedPolygonDescriptor.Create(RgbByteValues.Red, false, 10, 10, 20, 20, 30, 30, 0, 0, 0, 0),
      OverlayClosedPolygonDescriptor.CreateTiltedBox(50, 50, 10, 10, 0, RgbByteValues.Red, false),
      new OverlayCircleDescriptor(RgbByteValues.Red, false, 50, 50, 5)
      )
      };
      ImageViewer.OverlaysDescriptor.Overlays[0]!.CanDraw = false;
      ImageViewer.OverlaysDescriptor.Overlays[1]!.CanDraw = false;
      ImageViewer.OverlaysDescriptor.Overlays[2]!.CanDraw = false;
      ImageViewer.OverlaysDescriptor.Overlays[3]!.CanDraw= false;
      ImageViewer.OverlaysDescriptor.Overlays[4]!.CanDraw = false;
      ImageViewer.OverlaysDescriptor.Overlays[5]!.CanDraw = false;
    }

  }

}
