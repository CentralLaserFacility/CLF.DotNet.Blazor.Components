using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Clf.Common.ImageProcessing;

namespace Clf.Blazor.Basic.Components.Controls.Models
{
    public struct GroupBoxStyle
  {
    public static int MARGIN = 8;
    public static int HORIZONTAL_PADDING = 8;
    public static int VERTICAL_PADDING = 7;
    public static int DEFAULT_WIDTH = 300;
    public static int DEFAULT_HEIGHT = 200;
    public static Colour DEFAULT_BACKGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.White);
    public static Colour DEFAULT_BORDER_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
  }

  public struct ActionButtonStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 30;
    public static Colour DEFAULT_BACKGROUND_COLOR = new Colour() { Red = 210, Green = 210, Blue = 210 };
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
  }

  public struct RadioButtonStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 60;
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
  }

  public struct FontStyle
  {
    public static string FINE_PRINT = "normal normal 12px liberation-sans, sans-serif"; //font-weight font-style font-size font-family
    public static string DEFAULT_FONT_STYLE = "normal normal 14px liberation-sans, sans-serif"; //font-weight font-style font-size font-family
    public static string BOLD_FONT_STYLE = "normal bold 14px liberation-sans, sans-serif";
    public static string TITLE_FONT_STYLE = "normal bold 22px liberation-sans, sans-serif";
  }

  public struct TextEntryStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 20;
    public static Colour DEFAULT_BACKGROUND_COLOR = new Colour() { Red = 128, Green = 255, Blue = 255 };
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static Colour DEFAULT_BORDER_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Gray);
    public static int DEFAULT_BORDER_WIDTH = 1;
  }

  public struct CheckboxStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 20;
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static int DEFAULT_BORDER_WIDTH = 1;
    public static string DEFAULT_BORDER_COLOR = "gray";
  }

  public struct SpinnerStyle
  {    
    public static Colour DEFAULT_BORDER_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Gray);
    public static double DEFAULT_MIN = 0.0;
    public static double DEFAULT_MAX = 100.0;
    public static double DEFAULT_INCREMENT = 1.0;
  }

  
  public struct LedStyle
  {
    public static int DEFAULT_WIDTH = 38;
    public static int DEFAULT_HEIGHT = 38;
    public static int DEFAULT_MARGIN = 1;
    public static Colour DFAULT_INVALID_BORDER_COLOR = new Colour() { Red = 165, Green = 165, Blue = 165 };
    public static Colour DEFAULT_INVALID_COLOR = new Colour() { Red = 208,Green = 213, Blue = 221 };
    public static Colour DEFAULT_ON_COLOR = new Colour() { Red = 0, Green = 222, Blue = 10 };
    public static Colour DEFAULT_OFF_COLOR = new Colour() { Red = 0, Green = 90, Blue = 2 };
    public static Colour DEFAULT_BORDER_COLOR = new Colour() { Red = 36, Green = 36, Blue = 36 };
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static string DEFAULT_ON_LABEL = "";
    public static string DEFAULT_OFF_LABEL = "";
    public static string DEFAULT_ON_ICON = "led-icon-checkmark icon-ic_fluent_checkmark_12_regular";
    public static string DEFAULT_OFF_ICON = "";
    public static string DEFAULT_ON_ICON_PATH = "";
    public static string DEFAULT_OFF_ICON_PATH = "";
    public static int DEFAULT_BORDER_WIDTH = 2;
  }

  public struct MultiStateLedStyle
  {
    public static int DEFAULT_WIDTH = 32;
    public static int DEFAULT_HEIGHT = 32;
    public static int DEFAULT_MARGIN = 1;
    public static Colour DEFAULT_COLOR = new Colour() { Red = 60, Green = 100, Blue = 60 };
    public static Colour DEFAULT_LINE_COLOR = new Colour() { Red = 50, Green = 50, Blue = 50, Alpha = 178 };
    public static Colour DEFAULT_FALLBACK_COLOR = new Colour() { Red = 255, Green = 0, Blue = 255, Alpha = 255 };
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static string DEFAULT_LABEL = "";
    public static string DEFAULT_FALLBACK_LABEL = "Err";
    public static string DEFAULT_ICON_PATH = "";
    public static int DEFAULT_BORDER_WIDTH = 2;
  }

  public struct LabelStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 20;
    public static Colour DEFAULT_BACKGROUND_COLOR = Colour.Transparent;
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static int DEFAULT_BORDER_WIDTH = 0;
    public static Colour DEFAULT_BORDER_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
  }

  public struct ImageViewerStyle
  {
    public static int DEFAULT_DISPLAY_WIDTH = 200;
    public static int DEFAULT_DISPLAY_HEIGHT = 200;
    public static int DEFAULT_IMAGE_WIDTH = 1000;
    public static int DEFAULT_IMAGE_HEIGHT = 1000;
    public static byte[] DEFAULT_RAW_IMAGE_DATA = new byte[] {
    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,

    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
    };
  }
  public struct TextUpdateStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 20;
    public static Colour DEFAULT_BACKGROUND_COLOR = new Colour() { Red = 240, Green = 240, Blue = 240 };
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static int DEFAULT_BORDER_WIDTH = 0;
    public static Colour DEFAULT_BORDER_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
  }
  public struct BooleanButtonStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 30;
    public static Colour DEFAULT_OFF_COLOR = new Colour() { Red = 60, Green = 100, Blue = 60 };
    public static Colour DEFAULT_ON_COLOR = new Colour() { Red = 0, Green = 255, Blue = 0 };
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static string DEFAULT_ON_LABEL = "On";
    public static string DEFAULT_OFF_LABEL = "Off";
    public static Colour DEFAULT_BORDER_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Gray);
    public static int DEFAULT_BORDER_WIDTH = 1;
  }
  public struct SlideButtonStyle
  {
    public static int DEFAULT_WIDTH = 50;
    public static int DEFAULT_HEIGHT = 30;
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static Colour DEFAULT_OFF_COLOR = new Colour() { Red = 158, Green = 158, Blue = 158 };
    public static Colour DEFAULT_ON_COLOR = new Colour() { Red = 67, Green = 160, Blue = 71 };
    public static string DEFAULT_LABEL = string.Empty;
  }
  public struct ComboBoxStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 30;
    public static Colour DEFAULT_BACKGROUND_COLOR = new Colour() { Red = 210, Green = 210, Blue = 210 };
    public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
  }
  public struct TabsStyle
  {
    public static int DEFAULT_WIDTH = 400;
    public static int DEFAULT_HEIGHT = 300;
  }
  public struct ProgressBarStyle
  {
    public static double DEFAULT_MIN = 0.0;
    public static double DEFAULT_MAX = 100.0;
    public static double DEFAULT_INCREMENT = 1.0;
  }

  public struct DisplayStyle
  {
    public static int DEFAULT_WIDTH = 800;
    public static int DEFAULT_HEIGHT = 600;
    public static Colour DEFAULT_BACKGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.White);
  }
  public struct ChoiceButtonStyle
  {
    readonly public static int DEFAULT_WIDTH = 100;
    readonly public static int DEFAULT_HEIGHT = 43;
    readonly public static Colour DEFAULT_FOREGROUND_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    readonly public static Colour DEFAULT_BACKGROUND_COLOR = new Colour() { Red = 210, Green = 210, Blue = 210 };
    readonly public static Colour DEFAULT_SELECTED_COLOR = new Colour() { Red = 100, Green = 100, Blue = 100 };
  }
  public struct TrafficLightColorsStyle
  {
    public static string NotConnected = "#808080";
    public static string Operational = "#73AD21";
    public static string Warning = "#ff6a00";
    public static string Error = "#ff0000";
  }

  public enum GraphType
  {
    Line,
    Step,
    ErrorBars,
    LineAndErrorBars,
    Area
  }

  public struct GraphStyle
  {
    public static int DEFAULT_WIDTH = 400;
    public static int DEFAULT_HEIGHT = 400;
    public static Colour DEFAULT_BACKGROUND_COLOR = Colour.Transparent;
    public static int DEFAULT_BORDER_WIDTH = 0;
    public static Colour DEFAULT_BORDER_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Black);
    public static string DEFAULT_XTitle = "X";
    public static int DEFAULT_TRACE_WIDTH = 1;
    public static int DEFAULT_OVERLAY_WIDTH = 2;
    public static string DEFAULT_YTitle = "Y";
    public static Colour DEFAULT_TRACE_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Blue);
    public static Colour DEFAULT_OVERLAY_COLOR = Colour.SystemDrawingColorToColour(System.Drawing.Color.Yellow);
    public static GraphType DEFAULT_GRAPH_TYPE = GraphType.Line;
    public static int DEFAULT_XMinimum = 0;
    public static int DEFAULT_XMaximum = 100;
    public static int DEFAULT_YMinimum = 0;
    public static int DEFAULT_YMaximum = 100;
  }

  public struct SymbolStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 100;
  }

  public struct PictureStyle
  {
    public static int DEFAULT_WIDTH = 100;
    public static int DEFAULT_HEIGHT = 100;
  }

  public struct SliderStyle
  {
    public static int DEFAULT_WIDTH = 200;
    public static int DEFAULT_HEIGHT = 10;
    public static double DEFAULT_MIN = 0.0;
    public static double DEFAULT_MAX = 100.0;
    public static double DEFAULT_INCREMENT = 1.0;
  }

  public struct ColorPickerStyle
  {
    public static string DEFAULT_COLOR = "#ff0000";
    public static int DEFAULT_WIDTH = 50;
    public static int DEFAULT_HEIGHT = 30;
  }

  public enum ModalTypes
  {
    OneButton,
    TwoButtons
  }
}

