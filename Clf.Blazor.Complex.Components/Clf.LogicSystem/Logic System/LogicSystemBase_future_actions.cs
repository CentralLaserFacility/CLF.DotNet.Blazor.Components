//
// LogicSystemBase_future_actions.cs
//


#if SUPPORTS_FUTURE_ACTIONS

using Clf.LogicSystem.LogicNodes;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem
{

  //
  // Hmm, are 'future actions' really a good idea ??
  //
  // They are orthogonal to the purpose of the Logic System, and make it really hard to test.
  //
  // Better to have an external component that handles time-related changes ??
  //
  // Without 'future actions', we can entirely remove 'change handling' methods from the API.
  //

  public partial class LogicSystemBase 
  {

    //
    // Future Actions are a somewhat dodgy addition to the Logic System,
    // and it would be better to find an alternative way of handling them,
    // externally to the Logic System.
    //

    //
    // Client code 'hosting' a Logic System is expected to
    // periodically wake us up by invoking 'HandleTimerWakeup'
    //

    public System.TimeSpan DesiredWakeupInterval { get ; set ; } = System.TimeSpan.FromMilliseconds(100) ;

    public void ScheduleFutureAction ( FutureAction futureAction )
    {
      string sourceNodeChangedInfo = (
        futureAction.NodeWhoseValueChangeProvokedThisAction is LogicNode node
        ? $"triggered by a change in '{node.PropertyName}', "
        : ""
      ) ;
      Clf.Common.Utils.DebugHelpers.WriteDebugLines(
        $"A FutureAction has been scheduled, {sourceNodeChangedInfo}",
        $"to fire after waiting {futureAction.IntendedInvocationTimeDelta.TotalSeconds:F2} secs",
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
    )
    {
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

    public void HandleTimerWakeup ( System.DateTime currentTime )
    {
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
    }

    public IEnumerable<FutureAction> FutureActionsNotYetInvoked => m_futureActionsNotYetInvoked ;

    //
    // Hook into this event to get a notification when a scheduled action
    // has been invoked and might have triggered a change in state
    // that requires attention, eg a repaint of the UI.
    //

    public event System.Action? FutureActionsHaveBeenInvoked ;

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

#endif
