//
// IntensityProfileGraphWithAxesViewModel.cs
//

using System.Collections.Generic;
using System.Linq;

using Clf.ChannelAccess;
using Clf.Blazor.Basic.Components.Controls.Helpers;
using Clf.Blazor.Basic.Components.Controls.Interfaces;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Blazor.Basic.Components.Controls.ViewModels;
using Clf.Common.ImageProcessing;
using Clf.Common.Graphs;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  //
  // Here we're just defining the 'core' area that displays the graph points.
  //
  // Plan for a version that draws Axes, Title and so on ...
  //
  // To properly draw the axes, we'll assign some 'border' areas as follows :
  //
  //
  //     +-------+-------------------------------------------+---+
  //     |                          A                            |
  //     +-------+-------------------------------------------+---+
  //     |       |                                           |   |
  //     |       |                                           |   |
  //     |   L   |                  G                        | R |
  //     |       |                                           |   |
  //     |       |                                           |   |
  //     |       |                                           |   |
  //     +-------+-------------------------------------------+---+
  //     |                                                       |
  //     |                          B                            |
  //     |                                                       |
  //     +-------+-------------------------------------------+---+
  //
  //   A  Above : Overall title
  //   L  Left  : Y axis title, labels and tick marks
  //   G  Graph : Plotted points
  //   R  Right : Right border
  //   B  Below : Y axis title, labels and tick marks
  //
  // The 'Below' area will itself be divided into strips ...
  //
  //    +--------------------------------------------------+
  //    |                Tick marks                        |
  //    +--------------------------------------------------+
  //    |                Labels                            |
  //    +--------------------------------------------------+
  //    |                Axis title                        |
  //    +--------------------------------------------------+
  //
  // Likewise for the 'Left' area that shows the Y axis info.
  //
  // These areas can be distinct components organised with a Grid.
  // They all map onto the same underlying ViewModel and have access
  // to Mapper instances pertaining to the various areas.
  //

  public class IntensityProfileGraphWithAxesViewModel : IntensityProfileGraphViewModel
  {

    public IntensityProfileGraphWithAxesViewModel (
      ChannelRecord  xChannelRecord,
      ChannelRecord  yChannelRecord,
      int            width,
      int            height,
      GraphAxisRange axisRange_X,
      GraphAxisRange axisRange_Y,
      bool?          showGrid       = false,
      GraphType?     graphType      = null,
      Colour?        traceColor     = null,
      int?           traceWidth     = 1
    ) :
    base(
      xChannelRecord: xChannelRecord,
      yChannelRecord: yChannelRecord,
      width: width,
      height: height,
      axisRange_X: axisRange_X,
      axisRange_Y: axisRange_Y,
      showGrid: showGrid,
      graphType: graphType,
      traceColor: traceColor,
      traceWidth: traceWidth
    ) {
    }

  }

}
