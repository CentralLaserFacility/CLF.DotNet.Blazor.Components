//
// Dependency.cs
//

using Clf.Common.ExtensionMethods;
using Clf.LogicSystem.Common.ExtensionMethods;
using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.Dependencies
{

  public record Dependency ( LogicNode FromSource, ComputedNodeBase ToTarget ) // : IDependency
  {

    public bool HasSameSourceAndTargetAs ( Dependency other ) 
    => (
       this.FromSource == other.FromSource
    && this.ToTarget   == other.ToTarget
    ) ;

    public bool RefersToSourceAndTargetNodes ( LogicNode a, ComputedNodeBase b )
    => (
       FromSource == a
    && ToTarget   == b
    ) ;

    public bool Involves ( LogicNode endpoint ) => (
       endpoint == FromSource
    || endpoint == ToTarget
    ) ;

    public bool InvolvesOneOf ( LogicNode endpoint_a, LogicNode endpoint_b ) => (
       Involves(endpoint_a)
    || Involves(endpoint_b)
    ) ;

    public bool InvolvesBothOf ( LogicNode endpoint_a, LogicNode endpoint_b ) => (
       Involves(endpoint_a)
    && Involves(endpoint_b)
    ) ;

    public override string ToString ( )
    => $"'{FromSource.PropertyName}' --> '{ToTarget.PropertyName}'" ;

    //
    // Hmm, this is a very dodgy HACK to let us associate a set of POINTS with a Dependency.
    //
    // THIS REALLY NEEDS TO BE HANDLED DIFFERENTLY !!! Visual info shouldn't be directly
    // associated with a link, as there could easily be several visual displays
    // rendering different 'views' of a Logic System ...
    //

    // public List<System.Drawing.PointF> VisualPoints = new() ;
    // 
    // public Common.DirectionVector DirectionVectorAtEndPoint => new Common.DirectionVector(
    //   VisualPoints[^2],
    //   VisualPoints[^1]
    // ) ;

    public static string CreateDependenciesSummary ( IEnumerable<Dependency> dependencies )
    {
      return dependencies.Select(
        dependency => dependency.ToString()
      ).OrderBy(x=>x).ToDelimitedSummaryString("\r\n") ;
    }
    
  }

}
