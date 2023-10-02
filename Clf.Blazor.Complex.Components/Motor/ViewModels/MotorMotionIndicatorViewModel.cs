using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.ChannelAccess;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clf.Blazor.Complex.Components.Motor.ViewModels
{
  public class MotorMotionIndicatorViewModel : ObservableObject
  {
    private double _height;
    public double Height
    {
      get => _height;
      set => SetProperty(ref _height, value);
    }
    private double _borderWidth;
    public double BorderWidth
    {
      get => _borderWidth;
      set => SetProperty(ref _borderWidth, value);
    }
    private double _softLimitLow;
    public double SoftLimitLow
    {
      get => (_userLowLimit + _adjustmentForNegativeValue) * 100 / (_hardLimitHigh - _hardLimitLow);
      set => SetProperty(ref _softLimitLow, value);
    }
    private double _softLimitHigh;
    public double SoftLimitHigh
    {
      get => (_userHighLimit + _adjustmentForNegativeValue) * 100 / (_hardLimitHigh - _hardLimitLow);
      set => SetProperty(ref _softLimitHigh, value);
    }
    private double _hardLimitLow;
    public double HardLimitLow
    {
      get => _hardLimitLow;
      set => SetProperty(ref _hardLimitLow, value);
    }
    private double _hardLimitHigh;
    public double HardLimitHigh
    {
      get => _hardLimitHigh;
      set => SetProperty(ref _hardLimitHigh, value);
    }
    private double _userDesiredValue;
    public double UserDesiredValue
    {
      get
      {
        return (_userDesiredValue + _adjustmentForNegativeValue) * 100 / (_hardLimitHigh - _hardLimitLow);
      }
      set => SetProperty(ref _userDesiredValue, value);
    }
    private double _userReadbackValue;
    public double UserReadbackValue
    {
      get
      {
        return (_userReadbackValue - _userLowLimit) * 100 / (_userHighLimit - _userLowLimit);
      }
      set => SetProperty(ref _userReadbackValue, value);
    }
    private double _userLowLimit;
    public double UserLowLimit
    {
      get => _userLowLimit;
      set => SetProperty(ref _userLowLimit, value);
    }
    private double _userHighLimit;
    public double UserHighLimit
    {
      get => _userHighLimit;
      set => SetProperty(ref _userHighLimit, value);
    }
    private BorderStatus _borderStatus = BorderStatus.NotConnected;
    public BorderStatus BorderStatus
    {
      get => _borderStatus;
      set { SetProperty(ref _borderStatus, value); }
    }

    private bool _isVisible = true;
    public bool IsVisible
    {
      get => _isVisible;
      set => SetProperty(ref _isVisible, value);
    }

    private string _indicatorColor = "black";
    public string IndicatorColor
    {
      get
      {
        if (_userReadbackValue.CompareTo(_userLowLimit) == 0 || _userReadbackValue.CompareTo(_userHighLimit) == 0)
        {
          return "white";
        }
        else
        {
          return "black";
        }
      }
      set => SetProperty(ref _indicatorColor, value);
    }

    private double _adjustmentForNegativeValue = 0;
    private string _pvPrefix;
    private ChannelsHandler _channelsHandler;
    private IChannel? _userHighLimitChannel = null;
    private IChannel? _userLowLimitChannel = null;
    private IChannel? _userReadbackValueChannel = null;
    private IChannel? _userDesiredValueChannel = null;
    private IChannel? _hardLimitLowValueChannel = null;
    private IChannel? _hardLimitHighValueChannel = null;

    public double _instantSoftLeft = 0;
    public double _instantSoftRight = 0;

    public MotorMotionIndicatorViewModel(
            string pvPrefix,
            ChannelsHandler channelsHandler,
            double height = 15,
            double borderWidth = 1,
            bool isVisible = true
        )
    {
      _pvPrefix = pvPrefix;
      _channelsHandler = channelsHandler;
      _height = height;
      _borderWidth = borderWidth;
      _isVisible = isVisible;

      

      _channelsHandler.InstallChannel(
      _userHighLimitChannel = Hub.GetOrCreateChannel(pvPrefix + ".HLM"),
      (isConnected, _) =>
      {
        _borderStatus = isConnected ? BorderStatus.Connected : BorderStatus.NotConnected;
        OnPropertyChanged(nameof(BorderStatus));
      },
      (valueInfo, _) => SetUserHighLimit((double)valueInfo.Value));

      _channelsHandler.InstallChannel(
      _userLowLimitChannel = Hub.GetOrCreateChannel(pvPrefix + ".LLM"),
      (valueInfo, _) => SetUserLowLimit((double)valueInfo.Value));

      _channelsHandler.InstallChannel(
      _userReadbackValueChannel = Hub.GetOrCreateChannel(pvPrefix + ".RBV"),
      (valueInfo, _) => SetUserReadbackValue((double)valueInfo.Value));

      _channelsHandler.InstallChannel(
      _userDesiredValueChannel = Hub.GetOrCreateChannel(pvPrefix + ".VAL"),
      (valueInfo, _) => SetUserDesiredValue((double)valueInfo.Value));

      _channelsHandler.InstallChannel(
      _hardLimitLowValueChannel = Hub.GetOrCreateChannel(pvPrefix + ".LOLO"),
      (valueInfo, _) => SetHardLimitLowValue((double)valueInfo.Value));

      _channelsHandler.InstallChannel(
      _hardLimitHighValueChannel = Hub.GetOrCreateChannel(pvPrefix + ".HIHI"),
      (valueInfo, _) => SetHardLimitHighValue((double)valueInfo.Value));
    }

    public void InitialiseStartupValues()
    {
      _instantSoftLeft = UserLowLimit;
      _instantSoftRight = UserHighLimit;
    }

    private void SetUserReadbackValue(double value)
    {
      UserReadbackValue = value;
    }

    private void SetUserDesiredValue(double value)
    {
      UserDesiredValue = value;
    }

    private void SetUserLowLimit(double value)
    {
      UserLowLimit = value;
    }

    private void SetUserHighLimit(double value)
    {
      UserHighLimit = value;
    }

    private void SetHardLimitHighValue(double value)
    {
      HardLimitHigh = value;
    }

    private void SetHardLimitLowValue(double value)
    {
      HardLimitLow = value;
      if (HardLimitLow < 0)
      {
        _adjustmentForNegativeValue = Math.Abs(HardLimitLow);
      }
    }
  }
}
