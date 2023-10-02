//
// IntensityMapProfileGraphViewModel.cs
//

using Clf.Blazor.Basic.Components.Controls.Models ;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public class IntensityMapProfileGraphViewModel 
  {

    public IntensityProfileGraphViewModel IntensityProfileGraph { get ; }

    public IntensityMapProfileGraphViewModel ( 
      IntensityMapViewerViewModel parent,
      int                         width,      
      int                         height     

    ) {
      IntensityProfileGraph = new IntensityProfileGraphViewModel(
        width          : width,
        height         : height,
        axisRange_X    : new(0,100),
        axisRange_Y    : new(0,255),
        xborderStatus  : BorderStatus.Connected,
        yborderStatus  : BorderStatus.Connected,
        showGrid       : (
                           true
                           // false
                         ),
        graphType      : (
                           // GraphType.Area 
                           GraphType.Line 
                         )
      ) ;
    }

  }

}
