//
// FutureActionsScheduler.cs
//

using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem
{

  public partial class FutureActionsScheduler 
  {

    //
    // A FutureActionsScheduler is external to a Logic System,
    // and is not coupled to it in any way.
    //
    // One of its uses is to respond to changes in a Node value
    // by scheduling an action that would be carried out at some
    // future time.
    //

    //
    // Client code is expected to create a Scheduler and periodically
    // wake it up by invoking 'HandleTimerWakeup_InvokingAllEligibleActions',
    // at intervals roughly equivalent to the DesiredWakeupInterval.
    //

    public FutureActionsScheduler ( System.TimeSpan? desiredWakeupInterval = null )
    { 
      DesiredWakeupInterval = desiredWakeupInterval ?? System.TimeSpan.FromMilliseconds(100) ;
    }

    public System.TimeSpan DesiredWakeupInterval { get ; set ; } 

    //
    // Hook into this event to get a notification when one or more
    // scheduled actions have been invoked, and might have triggered
    // a change in state that requires attention, eg a repaint of a UI.
    //

    public event System.Action? FutureActionsHaveBeenInvoked ;

    public void ScheduleFutureAction ( FutureAction futureAction )
    {
      Clf.Common.Utils.DebugHelpers.WriteDebugLines(
        $"A FutureAction has been scheduled to fire after {futureAction.IntendedInvocationTimeDelta.TotalSeconds:F2} secs",
        $"  Description : '{futureAction.Description??"not supplied"}'"
      ) ;
      m_futureActionsNotYetInvoked.Add(
        futureAction
      ) ;
    }

    public void ScheduleFutureAction ( 
      System.Action   action, 
      System.DateTime thresholdTimeBeyondWhichActionShouldHaveFired, 
      string?         description = null 
    ) {
      ScheduleFutureAction(
        new FutureAction(
          action,
          thresholdTimeBeyondWhichActionShouldHaveFired,
          description : description         
        )
      ) ;
    }

    public void ScheduleFutureAction ( 
      System.Action   action, 
      System.TimeSpan deltaTimeFromNow, 
      string?         description = null 
    ) {
      ScheduleFutureAction(
        new FutureAction(
          action,
          deltaTimeFromNow,
          description : description 
        )
      ) ;
    }

    public void ScheduleFutureActionOnNextWakeup ( 
      System.Action action, 
      string?       description = null 
    ) {
      ScheduleFutureAction(
        new FutureAction(
          action,
          thresholdTimeBeyondWhichActionShouldHaveFired : null,
          description                                   : description
        )
      ) ;
    }

    private List<FutureAction> m_futureActionsNotYetInvoked = new List<FutureAction>() ;

    public IEnumerable<FutureAction> FutureActionsNotYetInvoked => m_futureActionsNotYetInvoked ;

    public void HandleTimerWakeup_InvokingAllEligibleActions ( 
      System.DateTime               currentTime, 
      out IEnumerable<FutureAction> actionsThatWereInvoked
    ) {
      var futureActionsEligibleForExecution = m_futureActionsNotYetInvoked.Where(
        futureAction => futureAction.IsEligibleToBeInvoked
      ).ToList() ;
      if ( futureActionsEligibleForExecution.Any() )
      {
        futureActionsEligibleForExecution.ForEach(
          futureAction => {
            try
            {
              futureAction.Invoke(
                message => Clf.Common.Utils.DebugHelpers.WriteDebugLines(message)
              ) ;
            }
            catch
            {
              // Hmm, should log an error !!
            }
            m_futureActionsNotYetInvoked.Remove(futureAction) ;
          }
        ) ;
        FutureActionsHaveBeenInvoked?.Invoke() ;
      }
      actionsThatWereInvoked = futureActionsEligibleForExecution ;
    }

    public void HandleTimerWakeup_InvokingAllEligibleActions ( System.DateTime currentTime )
    {
      HandleTimerWakeup_InvokingAllEligibleActions(
        currentTime,
        out var _
      ) ;
    }  

    public void CancelFutureActions ( System.Func<FutureAction,bool> filter )
    {
      var futureActionsToRemove = m_futureActionsNotYetInvoked.Where(filter).ToList() ;
      futureActionsToRemove.ForEach(
        futureActionToRemove => m_futureActionsNotYetInvoked.Remove(futureActionToRemove)
      ) ;
    }

    public void CancelAllFutureActions ( )
    {
      m_futureActionsNotYetInvoked.Clear() ;
    }

  }

}


