using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels
{
  public class FileSelectorViewModel
  {
    public long MaxFileSize { get; set; } = 1024 * 5;
    public bool IsLoading { get; set; } = false;
    public ushort MaxAllowedFiles { get; set; } = 1;
    public List<IBrowserFile>? Files { get; set; }
    public Action OnFileSelectedAsync { get; set; } = delegate { };
    public IWebHostEnvironment Environment { get; set; }
  }
}