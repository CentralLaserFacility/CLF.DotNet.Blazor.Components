using Clf.Blazor.Basic.Components.Controls.ViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;
using Clf.ChannelAccess;

namespace Clf.Blazor.ExamplesServerApp.ViewModels
{
  public class AreaDetectorSimSetupViewModel
  {
    private ChannelsHandler _channelsHandler;
    private string _pvPrefix;
    public ComboBoxViewModel SimMode { get; set; }
    public ActionButtonViewModel ResetImageButton { get; set; }
    public TextEntryViewModel Offset { get; set; }
    public TextUpdateViewModel OffsetRBV { get; set; }
    public TextEntryViewModel Noise { get; set; }
    public TextUpdateViewModel NoiseRBV { get; set; }
    public TextEntryViewModel GainX { get; set; }
    public TextUpdateViewModel GainXRBV { get; set; }
    public TextEntryViewModel GainY { get; set; }
    public TextUpdateViewModel GainYRBV { get; set; }
    public TextEntryViewModel Gain { get; set; }
    public TextUpdateViewModel GainRBV { get; set; }
    public TextEntryViewModel GainRed { get; set; }
    public TextUpdateViewModel GainRedRBV { get; set; }
    public TextEntryViewModel GainGreen { get; set; }
    public TextUpdateViewModel GainGreenRBV { get; set; }
    public TextEntryViewModel GainBlue { get; set; }
    public TextUpdateViewModel GainBlueRBV { get; set; }
    public TextEntryViewModel PeakStartX { get; set; }
    public TextUpdateViewModel PeakStartXRBV { get; set; }
    public TextEntryViewModel PeakStartY { get; set; }
    public TextUpdateViewModel PeakStartYRBV { get; set; }
    public TextEntryViewModel PeakWidthX { get; set; }
    public TextUpdateViewModel PeakWidthXRBV { get; set; }
    public TextEntryViewModel PeakWidthY { get; set; }
    public TextUpdateViewModel PeakWidthYRBV { get; set; }
    public TextEntryViewModel PeakNumX { get; set; }
    public TextUpdateViewModel PeakNumXRBV { get; set; }
    public TextEntryViewModel PeakNumY { get; set; }
    public TextUpdateViewModel PeakNumYRBV { get; set; }
    public TextEntryViewModel PeakStepX { get; set; }
    public TextUpdateViewModel PeakStepXRBV { get; set; }
    public TextEntryViewModel PeakStepY { get; set; }
    public TextUpdateViewModel PeakStepYRBV { get; set; }
    public TextEntryViewModel PeakVariation { get; set; }
    public TextUpdateViewModel PeakVariationRBV { get; set; }
    public TextEntryViewModel XSine1Amplitude { get; set; }
    public TextUpdateViewModel XSine1AmplitudeRBV { get; set; }
    public TextEntryViewModel XSine1Frequency { get; set; }
    public TextUpdateViewModel XSine1FrequencyRBV { get; set; }
    public TextEntryViewModel XSine1Phase { get; set; }
    public TextUpdateViewModel XSine1PhaseRBV { get; set; }
    public TextEntryViewModel YSine1Amplitude { get; set; }
    public TextUpdateViewModel YSine1AmplitudeRBV { get; set; }
    public TextEntryViewModel YSine1Frequency { get; set; }
    public TextUpdateViewModel YSine1FrequencyRBV { get; set; }
    public TextEntryViewModel YSine1Phase { get; set; }
    public TextUpdateViewModel YSine1PhaseRBV { get; set; }
    public TextEntryViewModel XSine2Amplitude { get; set; }
    public TextUpdateViewModel XSine2AmplitudeRBV { get; set; }
    public TextEntryViewModel XSine2Frequency { get; set; }
    public TextUpdateViewModel XSine2FrequencyRBV { get; set; }
    public TextEntryViewModel XSine2Phase { get; set; }
    public TextUpdateViewModel XSine2PhaseRBV { get; set; }
    public TextEntryViewModel YSine2Amplitude { get; set; }
    public TextUpdateViewModel YSine2AmplitudeRBV { get; set; }
    public TextEntryViewModel YSine2Frequency { get; set; }
    public TextUpdateViewModel YSine2FrequencyRBV { get; set; }
    public TextEntryViewModel YSine2Phase { get; set; }
    public TextUpdateViewModel YSine2PhaseRBV { get; set; }
    public ComboBoxViewModel XSineOperation { get; set; }
    public ComboBoxViewModel YSineOperation { get; set; }

    public AreaDetectorSimSetupViewModel(string pvPrefix, ChannelsHandler channelsHandler)
    {
      _pvPrefix = pvPrefix;
      _channelsHandler = channelsHandler;

      SimMode = new ComboBoxViewModel(
        itemsFromPv: true,
        channelRecord: new ChannelRecord(_pvPrefix + "SimMode", _channelsHandler)
      );

      ResetImageButton = new ActionButtonViewModel(
        text: "Reset Image"
      )
      {
        OnActionButtonClicked = async () =>
        {
          await ChannelAccess.Hub.PutValueAsync(_pvPrefix + "Reset", (int)1);
        }
      };

      Offset = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "Offset", _channelsHandler));

      OffsetRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "Offset_RBV", _channelsHandler));

      Noise = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "Noise", _channelsHandler));

      NoiseRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "Noise_RBV", _channelsHandler));

      GainX = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainX", _channelsHandler));

      GainXRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainX_RBV", _channelsHandler));

      GainY = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainY", _channelsHandler));

      GainYRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainY_RBV", _channelsHandler));

      Gain = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "Gain", _channelsHandler));

      GainRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "Gain_RBV", _channelsHandler));

      GainRed = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainRed", _channelsHandler));

      GainRedRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainRed_RBV", _channelsHandler));

      GainGreen = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainGreen", _channelsHandler));

      GainGreenRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainGreen_RBV", _channelsHandler));

      GainBlue = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainBlue", _channelsHandler));

      GainBlueRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "GainBlue_RBV", _channelsHandler));

      PeakStartX = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStartX", _channelsHandler));

      PeakStartXRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStartX_RBV", _channelsHandler));

      PeakStartY = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStartY", _channelsHandler));

      PeakStartYRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStartY_RBV", _channelsHandler));

      PeakWidthX = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakWidthX", _channelsHandler));

      PeakWidthXRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakWidthX_RBV", _channelsHandler));

      PeakWidthY = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakWidthY", _channelsHandler));

      PeakWidthYRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakWidthY_RBV", _channelsHandler));

      PeakNumX = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakNumX", _channelsHandler));

      PeakNumXRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakNumX_RBV", _channelsHandler));

      PeakNumY = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakNumY", _channelsHandler));

      PeakNumYRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakNumY_RBV", _channelsHandler));

      PeakStepX = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStepX", _channelsHandler));

      PeakStepXRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStepX_RBV", _channelsHandler));

      PeakStepY = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStepY", _channelsHandler));

      PeakStepYRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakStepY_RBV", _channelsHandler));

      PeakVariation = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakVariation", _channelsHandler));

      PeakVariationRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "PeakVariation_RBV", _channelsHandler));

      XSine1Amplitude = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine1Amplitude", _channelsHandler));
      
      XSine1AmplitudeRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine1Amplitude_RBV", _channelsHandler));
      
      XSine1Frequency = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine1Frequency", _channelsHandler));
      
      XSine1FrequencyRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine1Frequency_RBV", _channelsHandler));
      
      XSine1Phase = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine1Phase", _channelsHandler)); 
      
      XSine1PhaseRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine1Phase_RBV", _channelsHandler));
      
      YSine1Amplitude = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine1Amplitude", _channelsHandler));
      
      YSine1AmplitudeRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine1Amplitude_RBV", _channelsHandler));
      
      YSine1Frequency = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine1Frequency", _channelsHandler));
      
      YSine1FrequencyRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine1Frequency_RBV", _channelsHandler));
      
      YSine1Phase = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine1Phase", _channelsHandler));
      
      YSine1PhaseRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine1Phase_RBV", _channelsHandler));
      
      XSine2Amplitude = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine2Amplitude", _channelsHandler));
      
      XSine2AmplitudeRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine2Amplitude_RBV", _channelsHandler));
      
      XSine2Frequency = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine2Frequency", _channelsHandler));
      
      XSine2FrequencyRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine2Frequency_RBV", _channelsHandler));
      
      XSine2Phase = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine2Phase", _channelsHandler));
      
      XSine2PhaseRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "XSine2Phase_RBV", _channelsHandler));
      
      YSine2Amplitude = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine2Amplitude", _channelsHandler));
      
      YSine2AmplitudeRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine2Amplitude_RBV", _channelsHandler));
      
      YSine2Frequency = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine2Frequency", _channelsHandler));
      
      YSine2FrequencyRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine2Frequency_RBV", _channelsHandler));
      
      YSine2Phase = new TextEntryViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine2Phase", _channelsHandler));
      
      YSine2PhaseRBV = new TextUpdateViewModel(channelRecord: new ChannelRecord(_pvPrefix + "YSine2Phase_RBV", _channelsHandler));
      
      XSineOperation = new ComboBoxViewModel(itemsFromPv: true, channelRecord: new ChannelRecord(_pvPrefix + "XSineOperation", _channelsHandler));
      
      YSineOperation = new ComboBoxViewModel(itemsFromPv: true, channelRecord: new ChannelRecord(_pvPrefix + "YSineOperation", _channelsHandler));
    }
  }
}
