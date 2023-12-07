using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Renci.SshNet;
using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using Clf.Common.ImageProcessing;
using Clf.Blazor.Common.FilePicker;
using System.IO;
using Clf.Blazor.Basic.Components.Controls.Enums;

namespace Clf.Blazor.Complex.IocManager.ViewModels
{
  public partial class IocManagerViewModel : ObservableObject
  {
    ChannelsHandler _channelsHandler;
    public const string LOCAL_CONNECTION_STATUS = "LOC:CONNECTION_STATUS";

    private string _prefix = "";
    public string Prefix
    {
      get => _prefix;
      set => SetProperty(ref _prefix, value);
    }
    private string _hostName = "";
    public string HostName
    {
      get => _hostName;
      set => SetProperty(ref _hostName, value);
    }
    private string _path1 = "";
    public string Path1
    {
      get => _path1;
      set => SetProperty(ref _path1, value);
    }
    private string _path2 = "";
    public string Path2
    {
      get => _path2;
      set => SetProperty(ref _path2, value);
    }
    private string _password = "";
    public string Password
    {
      get => _password;
      set { 
        SetProperty(ref _password, value);
      }
    }
    private SshClient? _sshClient=null;
    public SshClient? SshClient
    {
      get => _sshClient;
      set => SetProperty(ref _sshClient, value);
    }
    private string _errorMsg = "";
    public string ErrorMsg
    {
      get => _errorMsg;
      set => SetProperty(ref _errorMsg, value);
    }
    private bool _showProgressBar=false;
    public bool ShowProgressBar
    {
      get => _showProgressBar;
      set
      {
        SetProperty(ref _showProgressBar, value);
      }
    }

    #region TextUpdate Declaration
    public TextUpdateViewModel IOCPrefixTextUpdateViewModel { get; private set; }
    public TextUpdateViewModel HostNameTextUpdateViewModel { get; private set; }
    public TextUpdateViewModel PathTextUpdateViewModel { get; private set; }
    #endregion

    #region ActionButton Declaration
    public ActionButtonViewModel StartServiceFileActionButtonViewModel { get; private set; }
    public ActionButtonViewModel StopServiceFileActionButtonViewModel { get; private set; }
    public ActionButtonViewModel RestartServiceFileActionButtonViewModel { get; private set; }
    #endregion

    #region TextEntry Declaration
    public TextEntryViewModel IPAddressTextEntryViewModel { get; private set; }
    public TextEntryViewModel UsernameTextEntryViewModel { get; private set; }
    #endregion

    #region Modal Dialog Declaration
    public ModalDialogViewModel CredentialsModalDialogViewModel { get; private set; }
    #endregion

    #region ProgressBar Declaration
    public ProgressBarViewModel StartProgressBarViewModel { get; private set; }
    #endregion

    #region Led Declaration
    public LedViewModel IOCStatusLedViewModel { get; private set; }
    #endregion
    public IocManagerViewModel(ChannelsHandler channelsHandler, string prefix , string hostname, string path)
    {
      //TODO: Check if these items are assigned correctly or not
      _channelsHandler = channelsHandler;

      _prefix = prefix;
      #region TextUpdate Initialization
      IOCPrefixTextUpdateViewModel = new TextUpdateViewModel(
                  width: 150,
                  text: prefix,
                  borderStatus: BorderStatus.Connected
        );
      HostNameTextUpdateViewModel = new TextUpdateViewModel(
                  width: 150,
                  text: hostname,
                  borderStatus: BorderStatus.Connected
        );
      PathTextUpdateViewModel = new TextUpdateViewModel(
                  width: 350,
                  text: path,
                  borderStatus: BorderStatus.Connected
        );
      #endregion

      #region ActionButton Initialization
      StartServiceFileActionButtonViewModel = new ActionButtonViewModel(
                height: 20,
                text: "START"
      );
      StopServiceFileActionButtonViewModel = new ActionButtonViewModel(
                height: 20,
                text: "STOP"
      );
      RestartServiceFileActionButtonViewModel = new ActionButtonViewModel(
                height: 20,
                text: "RESTART"
      );
      #endregion

      #region TextEntry Initialization
      IPAddressTextEntryViewModel = new TextEntryViewModel(borderStatus: BorderStatus.Connected);
      UsernameTextEntryViewModel = new TextEntryViewModel(
                                      width: 180,
                                      borderStatus: BorderStatus.Connected
                                   );
      #endregion

      #region Modal Dialog Initialization
      CredentialsModalDialogViewModel = new ModalDialogViewModel(type: ModalTypes.TwoButtons);
      #endregion

      #region ProgressBar Initialization
      StartProgressBarViewModel = new ProgressBarViewModel(
              width: 300,
              borderStatus: BorderStatus.Connected,
              isVisible: false,
              isIndeterminate: true
      );
      #endregion     

      IOCStatusLedViewModel = new LedViewModel(
                offLabel: "Disconnected",
                onLabel: "Connected",
                width: 100,
                ledValue: false,
                borderStatus: BorderStatus.Connected,
                ledType: LedType.Binary,
                isSquare: true
                ) ;
      IocManagerLogicInitialization();
      
      #region Logic
      StartServiceFileActionButtonViewModel.OnActionButtonClicked = OnStartClick;
      StopServiceFileActionButtonViewModel.OnActionButtonClicked = OnStopClick;
      RestartServiceFileActionButtonViewModel.OnActionButtonClicked = OnRestartClick;
      CredentialsModalDialogViewModel.OnModalDialogButton1Clicked = OnModalDialogAccepted;
      CredentialsModalDialogViewModel.OnModalDialogButton2Clicked = OnModalDialogDeclined;
      #endregion
    }

    private void OnModalDialogDeclined()
    {
      if (ErrorMsg != "")
        ErrorMsg = "";
      CredentialsModalDialogViewModel.Close();
    }

    private async void OnModalDialogAccepted()
    {
      StartProgressBarViewModel.IsVisible = true;
      try
      {
        if (CredentialsModalDialogViewModel.Button1Text == "Start")
        {
          await SendCommand("start");
        }
        if (CredentialsModalDialogViewModel.Button1Text == "Stop")
        {
          await SendCommand("stop");
        }
        if (CredentialsModalDialogViewModel.Button1Text == "Restart")
        {
          await SendCommand("restart");
        }
      }
      catch (Exception ex)
      {
        ErrorMsg = "Error: " + ex.Message;
      }
      finally
      {
        ErrorMsg = "";
        StartProgressBarViewModel.IsVisible = false;
        CredentialsModalDialogViewModel.Close();
      }
    }

    private async Task SendCommand(string cmd)
    {
      SshClient = new SshClient(HostNameTextUpdateViewModel.Text, UsernameTextEntryViewModel.Text, Password);
      SshClient.Connect();
      var txt = string.Format("ls /etc/systemd/system | grep ioc-{0}", Prefix);
      var serviceFileCmd = SshClient.CreateCommand(txt);
      var serviceFileName = serviceFileCmd.Execute();
      var modes = new Dictionary<Renci.SshNet.Common.TerminalModes, uint>();
      using (var stream = SshClient.CreateShellStream("xterm", 255, 50, 800, 600, 1024, modes))
      {
        stream.Write("sudo systemctl " + cmd + " " + serviceFileName);
        await Task.Delay(1500);
        stream.Write(Password + "\n");
        await Task.Delay(1500);
      }
      Password = "";
    }


    private void OnStartClick()
    {
      CredentialsModalDialogViewModel.Button1Text = "Start";
      CredentialsModalDialogViewModel.Button2Text = "Cancel";
      CredentialsModalDialogViewModel.Title = "Start the service on " + HostNameTextUpdateViewModel.Text;
      CredentialsModalDialogViewModel.Open();
    }
    private void OnStopClick()
    {
      CredentialsModalDialogViewModel.Button1Text = "Stop";
      CredentialsModalDialogViewModel.Button2Text = "Cancel";
      CredentialsModalDialogViewModel.Title = "Stop the service on " + HostNameTextUpdateViewModel.Text;
      CredentialsModalDialogViewModel.Open();
    }
    private void OnRestartClick()
    {
      CredentialsModalDialogViewModel.Button1Text = "Restart";
      CredentialsModalDialogViewModel.Button2Text = "Cancel";
      CredentialsModalDialogViewModel.Title = "Restart the service on " + HostNameTextUpdateViewModel.Text;
      CredentialsModalDialogViewModel.Open();
    }
  }
}
