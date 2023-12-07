//
// LogicSystemBase_timings.cs
//

namespace Clf.LogicSystem
{

  partial class LogicSystemBase
  {

    public int MillisecsTakenDiscoveringDependencies { get ; internal set ; }
    
    public int MillisecsTakenBuildingDotText         { get ; internal set ; }
    
    public int MillisecsTakenRunningGraphViz         { get ; internal set ; }
    
    public int MillisecsTakenParsingGraphVizOutput   { get ; internal set ; }

    public int MillisecsTakenPopulatingViewModel     { get ; internal set ; }

    public string Timings => (
      $"Dependencies={
        MillisecsTakenDiscoveringDependencies
      } Dot={
        MillisecsTakenBuildingDotText
      } GraphViz={
        MillisecsTakenRunningGraphViz
      } PlainParsing={
        MillisecsTakenParsingGraphVizOutput
      } ViewModel={
        MillisecsTakenPopulatingViewModel
      }" 
    ) ;

  }

}
