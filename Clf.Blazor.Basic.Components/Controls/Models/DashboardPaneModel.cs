using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Basic.Components.Controls.Models
{
  public record DashboardPaneProfile (string Title, int Order, bool isPinned);

  public class DashboardProfile
  {
    public List<DashboardPaneProfile> Panes { get; set; } = new();
  }
}
