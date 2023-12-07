//
// IAcceptsIncomingChannelValueChangeNotifications.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem
{

  public interface IAcceptsIncomingChannelValueChangeNotifications
  {

    // Report the PV names that are of interest.

    IEnumerable<string> QueryChannelNamesOfInterest ( ) ;

    // Accept notifications about a change to a PV value.
    // This method can be invoke on any thread.

    void AcceptChannelValueChangeNotification ( string pvName, object? value ) ;

  }

}
