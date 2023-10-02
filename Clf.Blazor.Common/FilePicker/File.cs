using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Common.FilePicker
{
  public class File
  {
    public DateTime LastModified { get; set; }
    public string? Name { get; set; }
    public string? WebkitRelativePath { get; set; }
    public int Size { get; set; }
    public string? Type { get; set; }
  }

  public class FileAsText : File
  {
    public string? Content { get; set; }
  }

  public class FileAsBytes: File
  {
    public byte[]? Content { get; set; }
  }

}
