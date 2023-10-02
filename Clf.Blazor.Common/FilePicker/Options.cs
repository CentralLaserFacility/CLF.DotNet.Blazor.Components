using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Common.FilePicker
{
  public class Options
  {
    public SelectionType[]? Types { get; set; }
    public bool ExcludeAcceptAllOption { get; set; }
    public string? Id { get; set; }
  }

  public class OpenFilePickerOptions : Options
  {
    public bool Multiple { get; set; }
  }

  // https://developer.mozilla.org/en-US/docs/Web/API/Window/showSaveFilePicker
  public class SaveFilePickerOptions: Options
  {
    public string? SuggestedName { get; set; }
  }

  //https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
  public class SelectionType
  {
    public string? Description { get; set; }
    public Dictionary<string, string[]>? Accept { get; set; }
  }
}
