//
// DialogHandler_Blazor.cs
//

using Clf.Common.Utils;

namespace Clf.Blazor.Common.Logging;

public class LogEntriesCollection : ILogEntriesCollection
{

  public void AddLogEntry ( string messageLine )
  {
    System.Diagnostics.Debug.WriteLine(messageLine) ;
  }

  public void Clear ( )
  {
  }

}


