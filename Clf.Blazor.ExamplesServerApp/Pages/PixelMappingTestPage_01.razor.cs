//
// PixelMappingTestPage_01.razor.cs
//

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Clf.Common.ImageProcessing;
using System.ComponentModel.DataAnnotations;

namespace Clf.Blazor.ExamplesServerApp.Pages
{

  public class Slider
  {
    // TODO_NET7 : mark these as 'required'
    public string Name  { get ; set ; } = "" ;
    public double Min   { get ; set ; }
    public double Max   { get ; set ; }
    public double Step  { get ; set ; }
    public double Value { get ; set ; }
    public override string ToString ( ) => $"{Name}={Value:F2}" ;
  }

  public class Sliders : List<Slider>
  {
    public Slider this [ string name ] => this.Where(
      slider => slider.Name == name
    ).Single() ;
  }

  public partial class PixelMappingTestPage_01
  {

    public Sliders Sliders = new Sliders(){
      // new (){Name="MIN",Min=0.0,Max=1.0,Step=0.1,Value=0.2},
      // new (){Name="MAX",Min=0.0,Max=2.0,Step=0.1,Value=0.6},
      new (){Name="Contrast",Min=0.1,Max=10.0,Step=0.1,Value=1.0},
      new (){Name="Brightness",Min=0.0,Max=1.0,Step=0.1,Value=0.5}
    } ;

    public double GetSliderValue ( string name )
    => Sliders.Where(
      slider => slider.Name == name
    ).Single().Value ;

    private ElementReference m_canvasRef_original ;
    private ElementReference m_canvasRef_mapped ;

    // private void OnSliderChanged ( int i, ChangeEventArgs e )
    // {
    //   double value = GetDoubleFromObject(e.Value) ;
    //   System.Diagnostics.Debug.WriteLine( 
    //     $"Value #{i} ('{Sliders[i].Name}') => {value}"
    //   ) ;
    // }

    public string SliderValues => $"{Sliders["Contrast"]} {Sliders["Brightness"]}" ;

    // private string? m_sliderValues ;
    // public string SliderValues => ( m_sliderValues ??= $"{Sliders["Contrast"]} {Sliders["Brightness"]}" ) ;

    public string MapperValues => $"{m_pixelIntensityMapper.LowerThreshold} ... {m_pixelIntensityMapper.UpperThreshold} => 0 ... 255" ;

    private void OnSliderChanged ( Slider slider, ChangeEventArgs e )
    {
      double value = GetDoubleFromObject(e.Value) ;
      System.Diagnostics.Debug.WriteLine( 
        $"Value of '{slider.Name}' => {value}"
      ) ;
      slider.Value = value ;
      switch ( slider.Name )
      {
      case "Contrast":
        m_pixelIntensityMapper.Contrast_AsSlopeOfMappingLine = value ;
        break ;
      case "Brightness":
        m_pixelIntensityMapper.Brightness = value ;
        break ;
      }
      // Tacky, we need to explicitly 'invalidate' the
      // graph points so that they'll be recomputed ...
      m_pixelMappingProfileGraph?.InvalidateGraphPoints() ;
      // Actually this is not necessary ...
      // Any time a slider value changes,
      // we refresh the entire panel
      // m_sliderValues = null ;
      // this.InvokeAsync(
      //   ()=>StateHasChanged() 
      // ) ;
    }

    public static double GetDoubleFromObject ( 
      object? value, 
      double  valueToReturnIfNull = double.NaN 
    ) {
      if ( value is string s )
      {
        return (
          Double.TryParse(
            s, 
            out var result
          ) 
          ? result 
          : valueToReturnIfNull 
        ) ;
      }
      else
      {
        try
        {
          return Convert.ToDouble(value) ;
        }
        catch
        {
          return valueToReturnIfNull ;
        }
      }
    }

    private IJSObjectReference? m_jsObjectReferenceModule ;

    private int Width  = 512 ;
    private int Height = 100 ;

    private LinearArrayHoldingPixelIntensities m_grayScaleIntensityValues_Original ;

    private LinearArrayHoldingPixelBytes m_grayScaleByteValues_Original ;

    PixelIntensityMapper m_pixelIntensityMapper = new PixelIntensityMapper(
      ImagePixelBitDepth.TwelveBits
    ) ;
    
    // Mapper values to render on the graph

    public double[] MapperValuesX => m_pixelIntensityMapper.OriginalPixelValues.Select(
      value => (double) value
    ).ToArray() ;

    public double[] MapperValuesY => m_pixelIntensityMapper.MappedPixelValues.Select(
      value => (double) value
    ).ToArray() ;

    public double[] MapperMinAndMax => new double[]{
      m_pixelIntensityMapper.LowerThreshold,
      m_pixelIntensityMapper.UpperThreshold
    } ;

    static int g_instanceNumber = 1 ;

    public PixelMappingProfileGraph? m_pixelMappingProfileGraph ;

    public PixelMappingTestPage_01 ( )
    {
      m_grayScaleByteValues_Original = LinearArrayHoldingPixelBytes.Create_WithRandomValues(
        width     : Width,
        height    : Height,
        patchSize : 10
      ) ;
      m_grayScaleIntensityValues_Original = new LinearArrayHoldingPixelIntensities(
        width             : Width,
        height            : Height,
        evaluatePixelFunc : (x,y) => (ushort) (
          ( 16 * x ) % 4096
        )
      ) ;
      m_pixelIntensityMapper.Contrast_AsSlopeOfMappingLine = g_instanceNumber++ ;
    }

    private async Task InitialiseJsObjectReferenceModule ( )
    {
      // This initialises our local 'module' reference to point to the
      // JavaScript functions defined in the '.razor.js' file
      // associated with the Component. To access those functions,
      // we use 'module.Invoke' and so on.
      m_jsObjectReferenceModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
        "import", 
        "./Pages/PixelMappingTestPage_01.razor.js"
      ) ;
      // Let's verify that we can access our JavaScript functions ...
      int sum = await m_jsObjectReferenceModule!.InvokeAsync<int>(
        identifier : "ComputeSum",
        args       : new object[]{1,2}
      ) ;
      sum = await m_jsObjectReferenceModule!.InvokeAsync<int>(
        identifier : "ComputeSum", 
        // These multiple arguments are passed via 'params object?[]? args' ...
        3,
        4
      ) ;
      await m_jsObjectReferenceModule!.InvokeVoidAsync(
        identifier : "WriteToConsole",
        args       : "Written from PixelMappingTestPage_01"
      ) ;
    }

    protected override async Task OnAfterRenderAsync ( bool firstRender )
    {
      if ( firstRender )
      {
        await InitialiseJsObjectReferenceModule() ;
      }

      {
        var colouredPixelArrayEncodedAsBytes = ColouredPixelArrayEncodedAsBytes_Base.CreateInstance(
          m_grayScaleByteValues_Original, 
          ColourMappingTable.GreyScale
        ) ;
        await m_jsObjectReferenceModule!.InvokeVoidAsync(
          "CanvasPutPixelMappingImageData", 
          m_canvasRef_original, 
          colouredPixelArrayEncodedAsBytes.Width, 
          colouredPixelArrayEncodedAsBytes.Height, 
          colouredPixelArrayEncodedAsBytes.ColouredImageBytesLinearArray
        ) ;
      }

      {
        m_pixelIntensityMapper.ApplyTransformation_ConvertingIntensitiesToByteValues(
          m_grayScaleIntensityValues_Original,
          out var grayScaleIntensityValues_MappedToByteValues
        ) ;
        var colouredPixelArrayEncodedAsBytes = ColouredPixelArrayEncodedAsBytes_Base.CreateInstance(
          grayScaleIntensityValues_MappedToByteValues, 
          ColourMappingTable.GreyScale
        ) ;
        // colouredPixelArrayEncodedAsBytes.LoadFromIntensityBytesArray(
        //   m_grayScaleIntensityValues_Original,
        //   ColourMappingTable.GreyScale
        // ) ;
        await m_jsObjectReferenceModule!.InvokeVoidAsync(
          "CanvasPutPixelMappingImageData", 
          m_canvasRef_mapped, 
          colouredPixelArrayEncodedAsBytes.Width, 
          colouredPixelArrayEncodedAsBytes.Height, 
          colouredPixelArrayEncodedAsBytes.ColouredImageBytesLinearArray
        ) ;
      }

    }

  }

}