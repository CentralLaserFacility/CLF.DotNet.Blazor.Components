//
// GraphDescriptor.cs
//

namespace Clf.LogicSystem.Common
{

  internal sealed class GraphDescriptor
  {

    public float ScaleFactor { get ; init ; }

    public float WidthX      { get ; init ; }

    public float HeightY     { get ; init ; }

    public System.Drawing.SizeF TotalSize => (
      new System.Drawing.SizeF(
        WidthX,
        HeightY
      ) 
    ) ;

  }

}
