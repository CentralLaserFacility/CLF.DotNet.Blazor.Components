using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Common.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clf.Blazor.Basic.Components.Controls.Interfaces
{
  public interface IWidgetViewModelBase
  {    
    /// <value>Property <c>Width</c> represents the width of the Widget.</value>
    public int? Width { get; set; }

    /// <value>Property <c>Height</c> represents the height of the Widget.</value>
    public int? Height { get; set; }

    public bool IsVisible { get; set; }
  }
}
