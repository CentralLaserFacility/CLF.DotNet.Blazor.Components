//
// ScenarioDiscrepanciesReport.cs
//

using System.Collections.Immutable;

namespace Clf.LogicSystem
{

  public record ScenarioDiscrepanciesReport (
    string                                      LogicSystemClassName,
    string                                      SenarioName,
    ImmutableList<ComputedNodeValueDiscrepancy> ComputedNodeValueDiscrepancies
  ) ;

}
