//
// Disposer.cs
//

using System.Collections.Generic ;
using System.Linq ;

namespace Clf.LogicSystem.SourceGenerator.Helpers
{

  //
  // This is a container that holds a collection of Disposable items.
  // 
  // When the container is 'Disposed' (eg via a 'using' statement)
  // it disposes all the items.
  //

  public class Disposer : System.IDisposable
  {

    private IList<System.IDisposable> m_disposables ;

    public Disposer ( params System.IDisposable[] disposables )
    {
      m_disposables = disposables.ToList() ;
    }

    public void Add ( System.IDisposable disposable )
    {
      m_disposables.Add(disposable) ;
    }

    public void Dispose ( )
    {
      foreach ( System.IDisposable disposable in m_disposables ) 
      {
        try
        {
          disposable.Dispose() ;
        }
        catch ( System.Exception x ) 
        { 
          // Would be nice to log this somehow ...
        }
      }
    }

  }

}