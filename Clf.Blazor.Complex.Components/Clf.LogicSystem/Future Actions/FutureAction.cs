//
// FutureAction.cs
//

using Clf.LogicSystem.LogicNodes;

namespace Clf.LogicSystem
{

  public sealed class FutureAction
  {

    //
    // Possible enhancements (needs some thought) ...
    //
    // - Allow rescheduling, to fire at a different time ??
    // - Allow rescheduling once fired, to fire again after the same delay ??
    //

    public readonly System.DateTime TimeWhenThisFutureActionWasScheduled = System.DateTime.Now ;

    public readonly System.DateTime IntendedInvocationTime ;

    public System.DateTime? ActualInvocationTime { get ; private set ; } = null ;

    public bool HasBeenInvoked => ActualInvocationTime.HasValue ;

    public readonly System.Action Action ;

    public readonly LogicNode? NodeWhoseValueChangeProvokedThisAction ;

    public readonly string? Description ;

    public readonly object? Tag ;

    public FutureAction ( 
      System.Action    action,
      System.DateTime? thresholdTimeBeyondWhichActionShouldHaveFired,
      LogicNode?       nodeWhoseValueChangeProvokedThisAction = null,
      string?          description                            = null,
      object?          tag                                    = null
    ) {
      Action                                 = action ;
      IntendedInvocationTime                 = thresholdTimeBeyondWhichActionShouldHaveFired ?? System.DateTime.Now ;
      NodeWhoseValueChangeProvokedThisAction = nodeWhoseValueChangeProvokedThisAction ;
      Description                            = description ;
      Tag                                    = tag ;
    }

    public FutureAction ( 
      System.Action   action,
      System.TimeSpan deltaTimeBeyondWhichActionShouldHaveFired,
      LogicNode?      nodeWhoseValueChangeProvokedThisAction = null,
      string?         description                            = null,
      object?         tag                                    = null
    ) :
    this(
      action,
      System.DateTime.Now + deltaTimeBeyondWhichActionShouldHaveFired,
      nodeWhoseValueChangeProvokedThisAction,
      description,
      tag
    ) {
    }

    public bool IsEligibleToBeInvoked => System.DateTime.Now >= IntendedInvocationTime ;

    //
    //         TimeWhenThisFutureActionWasScheduled
    //         |
    //         |                                   IntendedInvocationTime
    //         |                                   |                               ActualInvocationTime
    //         |                                   |                               | ( always a little bit late )
    //         |                                   |                               |
    //    -----|-----------------------------------|-------------------------------|---------------------> time
    //         |                                   |                               |
    //         |                                   |<- InvocationTimeDiscrepancy ->|
    //         |<-- IntendedInvocationTimeDelta -->|                               |
    //         |                                                                   |
    //         |                                                                   |
    //         |<-------------------- ActualInvocationTimeDelta ------------------>|
    //

    public System.TimeSpan IntendedInvocationTimeDelta => IntendedInvocationTime - TimeWhenThisFutureActionWasScheduled ;

    public System.TimeSpan? ActualDeltaTime => ActualInvocationTime - TimeWhenThisFutureActionWasScheduled ;

    public System.TimeSpan? InvocationTimeDiscrepancy => ActualInvocationTime - IntendedInvocationTime ;

    public void Invoke ( System.Action<string>? writeLogMessage = null )
    {
      ActualInvocationTime = System.DateTime.Now ;
      string sourceNodeChangedInfo = (
        NodeWhoseValueChangeProvokedThisAction is LogicNode node
        ? $"triggered by a change in '{node.PropertyName}', "
        : ""
      ) ;
      writeLogMessage?.Invoke(
        $"A FutureAction is about to be invoked, {sourceNodeChangedInfo}after waiting {ActualDeltaTime!.Value.TotalSeconds:F2} secs"
      ) ;
      writeLogMessage?.Invoke(
        $"  Description : '{Description??"not supplied"}'"
      ) ;
      writeLogMessage?.Invoke(
        $"  Intended waiting time was {IntendedInvocationTimeDelta.TotalSeconds:F2} secs"
      ) ;
      writeLogMessage?.Invoke(
        $"  Timing discrepancy was {InvocationTimeDiscrepancy!.Value.TotalSeconds:F2} secs (later than intended)"
      ) ;
      Action() ;
    }

  }

}
