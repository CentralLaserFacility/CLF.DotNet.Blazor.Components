using Microsoft.AspNetCore.Components.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;

namespace Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels
{
  public class ModalDialogViewModel : WidgetViewModelBase
  {
    private string _title = "";
    private string _bodyText = "";
    private string _button1Text = "";
    private string _button2Text = "";  
    
    public string Title
    {
      get => _title;
      set => SetProperty(ref _title, value);
    }
    public string BodyText
    {
      get => _bodyText;
      set => SetProperty(ref _bodyText, value);
    }

    public string Button1Text
    {
      get => _button1Text;
      set => SetProperty(ref _button1Text, value);
    }
    public string Button2Text
    {
      get => _button2Text;
      set => SetProperty(ref _button2Text, value);
    }

    public ModalTypes Type { get; set; }

    // It is the responsibility of the user to close the modal dialog in OnModalDialogAccepted.
    public Action OnModalDialogButton1Clicked { get; set; } = delegate { };
    public Action OnModalDialogButton2Clicked { get; set; } = delegate { };

    public ModalDialogViewModel(
      bool isVisible = false,
      string? title = "",
      string? bodyText = null,
      ModalTypes type = ModalTypes.OneButton,
      string? button1Text = "",
      string? button2Text = "")
      :base(
         isVisible:isVisible
         )
    {
      Title = title!;
      BodyText = bodyText!;
      Type = type;
      Button1Text = button1Text!;
      Button2Text = button2Text!;
    }

    public void Open()
    {
      IsVisible = true;
    }

    public void Close()
    {
      IsVisible = false;
    }
  }
}
