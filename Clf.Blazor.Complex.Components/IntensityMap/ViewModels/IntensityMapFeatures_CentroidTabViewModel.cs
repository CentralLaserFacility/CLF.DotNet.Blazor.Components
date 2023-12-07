using Clf.Blazor.Basic.Components.Controls.ViewModels.UpdateWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.MonitorWidgetViewModels;
using Clf.Blazor.Basic.Components.Controls.ViewModels.ContainerWidgetViewModels;
using Clf.ChannelAccess;
using System.Collections.ObjectModel;
using Clf.Blazor.Basic.Components.Controls.Widgets.Updates;

using Clf.Blazor.Basic.Components.Controls.ViewModels.HelperWidgetViewModels;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{

  public partial class IntensityMapFeatures_CentroidTabViewModel
  {
    private IntensityMapViewerViewModel m_parent;
    private string centroidPluginPrefix;
    private string statsPluginPrefix;
    public IntensityMapFeatures_CentroidTabSettingsViewModel CentroidSettingsTab { get; }
    public LabelViewModel BeamTiltLabel { get; }
    public LabelViewModel BeamDiameterLabel { get; }
    public LabelViewModel BeamHeightLabel { get; }
    public LabelViewModel BeamWidthLabel { get; }
    public CheckboxViewModel CentroidEnable { get; }
    public TextUpdateViewModel CentroidTimeElapsed { get; set; }
    public TextUpdateViewModel CentroidX { get; set; }
    public TextUpdateViewModel CentroidY { get; set; }
    public RadioButtonViewModel CentroidAlgorithm { get; private set; }
    public RadioButtonViewModel ThresholdAlgorithm { get; private set; }
    public TextEntryViewModel ThresholdManualSet { get; }
    public TextUpdateViewModel ThresholdAppliedRBV { get; set; }
    public GroupBoxViewModel EdgeDetectionGroupBox { get; set; }
    public CheckboxViewModel EdgeDetectionContourShow { get; set; }
    public ColorPickerViewModel EdgeDetectionContourColor { get; }
    public CheckboxViewModel EdgeDetectionContourThick { get; set; }
    public RadioButtonViewModel BeamShape { get; private set; }
    public TextUpdateViewModel BeamDiameter { get; set; }
    public TextUpdateViewModel BeamHeight { get; set; }
    public TextUpdateViewModel BeamWidth { get; set; }
    public TextUpdateViewModel BeamTilt { get; set; }
    public TextUpdateViewModel ActualCentroidAreaPercent { get; set; }
    public TextUpdateViewModel SetCentroidAreaPercent { get; set; }
    public ActionButtonViewModel CentroidSettingsED { get; }
    public ModalDialogViewModel CentroidSettingsEDModal { get; }
    public CheckboxViewModel EDBeamShapeShow { get; set; }
    public ColorPickerViewModel EDBeamShapeColor { get; }
    public CheckboxViewModel EDBeamShapeThick { get; set; }
    public IntensityMapFeatures_CentroidTabViewModel(IntensityMapViewerViewModel parent)
    {
      m_parent = parent;
      centroidPluginPrefix = ":Centroid1:";
      statsPluginPrefix = ":Stats1:";
      BeamDiameterLabel = new LabelViewModel(
        text: "Diameter",
        isVisible: false);
      BeamHeightLabel = new LabelViewModel(
        text: "Height",
        isVisible: false);
      BeamWidthLabel = new LabelViewModel(
        text: "Width",
        isVisible: false);
      BeamTiltLabel = new LabelViewModel(
        text: "Tilt",
        isVisible: false);

      CentroidSettingsTab = new IntensityMapFeatures_CentroidTabSettingsViewModel(parent);
      CentroidEnable = new CheckboxViewModel(
      label: "Enable",
      width: 100,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidEnable")
      );

      CentroidTimeElapsed = new TextUpdateViewModel(
      width: 70,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidTimeElapsed")
      );

      CentroidX = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidX")
      );

      CentroidY = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidY")
      );

      CentroidAlgorithm = new RadioButtonViewModel(
                items: new ObservableCollection<string> { "Centre of Mass", "Edge Detection" },
                isHorizontal: true,
                width: 150,
                height: 20,
                channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidAlgorithm")
      );

      ThresholdAlgorithm = new RadioButtonViewModel(
          items: new ObservableCollection<string> { "Auto", "Manual" },
          isHorizontal: true,
          width: 150,
          height:20,
          channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "ThresholdAlgorithm")
      );

      ThresholdManualSet = new TextEntryViewModel(
        width: 50,
        isVisible: false,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + statsPluginPrefix + "CentroidThreshold")
      );

      ThresholdAppliedRBV = new TextUpdateViewModel(
        width: 50,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "ThresholdApplied")
      );

      EdgeDetectionGroupBox = new GroupBoxViewModel(
        title: "Edge Detection",
        isVisible: false

      );

      EdgeDetectionContourShow = new CheckboxViewModel(
        label: "Show ED Contour",
        width: 140,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":ShowContourXhair")
      );
      EdgeDetectionContourColor = new ColorPickerViewModel(
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":ContourXhairColour")
      );
      EdgeDetectionContourThick = new CheckboxViewModel(
        label: "Thick",
        width: 60,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":ContourXhairThick")
      );

      BeamShape = new RadioButtonViewModel(
          items: new ObservableCollection<string> { "Circular", "Rectangle" },
          isHorizontal: false,
          width: 100,
          channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":BeamShape")
      );

      BeamDiameter = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CircleDiameter")
      );

      BeamHeight = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "RectangleBeamHeight")
      );

      BeamWidth = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "RectangleBeamWidth")
      );

      BeamTilt = new TextUpdateViewModel(
      width: 50,
      isVisible: false,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "RectangleBeamTiltAngle")
      );

      ActualCentroidAreaPercent = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "CentroidAreaPercentRBV")
      );

      SetCentroidAreaPercent = new TextUpdateViewModel(
      width: 50,
      channelRecord: parent.CreateChannelRecord(parent.PvPrefix + centroidPluginPrefix + "SetCentroidAreaPercent")
      );
      EDBeamShapeShow = new CheckboxViewModel(
        label: "Show ED Shape",
        width: 140,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":ShowEDBeamShapeXhair")
      );
      EDBeamShapeColor = new ColorPickerViewModel(
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":EDBeamShapeXhairColour")
      );
      EDBeamShapeThick = new CheckboxViewModel(
        label: "Thick",
        width: 60,
        channelRecord: parent.CreateChannelRecord(parent.PvPrefix + ":EDBeamShapeXhairThick")
      );

    CentroidSettingsEDModal = new ModalDialogViewModel(
        title: "Centroid Settings (Edge Detection)",
        type: Basic.Components.Controls.Models.ModalTypes.OneButton,
        button1Text: "OK"
        );
      CentroidSettingsEDModal.OnModalDialogButton1Clicked = () => { CentroidSettingsEDModal.Close(); };

      CentroidSettingsED = new ActionButtonViewModel(
        text: "Centroid Settings",
        width: 120
        )
      {
        OnActionButtonClicked = () => { CentroidSettingsEDModal.Open(); }
      };
      IntensityMapFeatures_CentroidTabViewModel_Logic_Initiliasation();
    }

  }
}