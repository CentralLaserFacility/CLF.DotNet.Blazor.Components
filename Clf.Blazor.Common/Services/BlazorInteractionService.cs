using Clf.Blazor.Common.Interfaces;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Common.Services
{
  public class BlazorInteractionService : IFrameworkInteractionService
  {
    private IJSRuntime _jsRuntime;

    public IJSRuntime JSRuntime => _jsRuntime;
    public BlazorInteractionService(IJSRuntime jsRuntime)
    {
      _jsRuntime = jsRuntime;
    }

    public async void OpenPageInWindow(string pagePath, string pageTitle)
    {
      // Opening page in a separate tab
      // The javascript file is wwwroot/js/BlazorInteraction.js
      // As javascrip isolation only works with razor components
      await _jsRuntime.InvokeAsync<object>("openNewWindowTab", pagePath, pageTitle);
    }
  }
}
