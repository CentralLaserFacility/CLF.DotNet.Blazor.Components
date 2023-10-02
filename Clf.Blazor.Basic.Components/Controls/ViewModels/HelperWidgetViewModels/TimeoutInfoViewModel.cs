using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using System;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels
{
  public class TimeoutInfoViewModel : WidgetViewModelBase
  {
    private int m_noOfTries = 0;
    public int NoOfTries
    {
      get { return m_noOfTries; }
      set { SetProperty(ref m_noOfTries, value); }
    }
    private string m_message = "Time out, No. of tries:";
    public string Message
    {
      get { return m_message; }
      set { SetProperty(ref m_message, value); }
    }

    public Action OnOKButtonClicked { get; set; } = delegate { };

    public TimeoutInfoViewModel(

      bool isVisible = false,
      int noOfTries = 0,
      string? fontStyle = null,
      string? message = null
      )
      :base(
         isVisible: isVisible,
         fontStyle: fontStyle?? FontStyle.FINE_PRINT
         )

    {
      NoOfTries = noOfTries;
      Message = message ?? m_message;
    }

  }
}

