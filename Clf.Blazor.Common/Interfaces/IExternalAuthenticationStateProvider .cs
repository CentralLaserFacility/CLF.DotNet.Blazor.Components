using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Common.Interfaces
{
  public interface IExternalAuthenticationStateProvider
  {
    Task LogInAsync();
    void LogOut();
  }
}
