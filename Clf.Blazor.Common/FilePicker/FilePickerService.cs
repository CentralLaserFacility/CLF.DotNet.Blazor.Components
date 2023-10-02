using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Clf.Blazor.Common.FilePicker
{
  public class FilePickerService:IAsyncDisposable
  {
    private IJSRuntime _jsRuntime;
    private IJSObjectReference? _module;
    public FilePickerService(IJSRuntime jsRuntime)
    {
      _jsRuntime = jsRuntime;
      
    }

    public async Task SaveToFileUsingFilePickerAsync(SaveFilePickerOptions options, object fileContent)
    {
      try
      {
        await LoadModule();
        await _module!.InvokeVoidAsync("saveToFileUsingFilePicker", options, fileContent);
      }
      catch (JSException ex)
      {
        if (ex.Message.ToLowerInvariant().Contains("user aborted a request") == false)
          throw ex;
      }
    }

    public async Task<FileAsText[]?> OpenFileAsTextUsingFilePickerAsync(OpenFilePickerOptions options)
    {
      FileAsText[]? files = null;
      try
      {
        await LoadModule();
        files = await _module!.InvokeAsync<FileAsText[]>("getFilesUsingFilePicker", options, true);      
      }
      catch (JSException ex)
      {
        if(ex.Message.ToLowerInvariant().Contains("user aborted a request")==false)
          throw ex;        
      }
      return files;
    }

    public async Task<FileAsBytes[]?> OpenFileAsBytesUsingFilePickerAsync(OpenFilePickerOptions options)
    {
      FileAsBytes[]? files = null;
      try
      {
        await LoadModule();
        files = await _module!.InvokeAsync<FileAsBytes[]>("getFilesUsingFilePicker", options, false);
      }
      catch (JSException ex)
      {
        if (ex.Message.ToLowerInvariant().Contains("user aborted a request") == false)
          throw ex;
      }
      return files;
    }

    private async Task LoadModule()
    {
      if (_module == null)
      {
        _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Clf.Blazor.Common/FilePicker/FilePicker.razor.js");
      }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
      if (_module is not null)
      {
        await _module.DisposeAsync();
      }
    }
  }
}

/* Usage Example
 * var options = new SaveFilePickerOptions()
  {
    SuggestedName = "TestName",
    ExcludeAcceptAllOption = true,
    Types = new SelectionType[]
      {
      new()
      {
        Description = "Text",
        Accept = new()
        {
          { "text/plain", new string[] { ".txt" } }
        }
      }
      }
  };
try
{
  await FilePicker.SaveToFileUsingFilePicker(options, "Content.");
}
catch(Exception exp)
{
  //TODO: 
}
*/
