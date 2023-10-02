//
// CanvasDrawingTestPage_01.razor.cs
//

using Clf.Common.ImageProcessing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

// using static Microsoft.JSInterop.JSObjectReferenceExtensions ;

namespace Clf.Blazor.ExamplesServerApp.Pages
{

  public partial class CanvasDrawingTestPage_01
  {

    private ElementReference m_canvasRef ;

    private int Width => 1200 ;

    private int Height => Width * 2 / 3 ;

    private static int g_instanceNumber = 0 ;

    private bool Thick => ( g_instanceNumber % 2 ) == 0 ;

    private LinearArrayHoldingPixelBytes m_linearArrayHoldingPixelBytes = new(200,100) ;

    private ColouredPixelArrayEncodedAsBytes_Base m_colouredPixelArrayEncodedAsBytes ;

    private IJSObjectReference? module ;

    public CanvasDrawingTestPage_01 ( )
    {
      g_instanceNumber++ ;
      m_linearArrayHoldingPixelBytes = new(Width,Height) ;
      // m_linearArrayHoldingPixelBytes.LoadLinearArrayWithAscendingValues_ForTesting() ;
      m_colouredPixelArrayEncodedAsBytes = new ColouredPixelArrayEncodedAsBytes_A_B_C(
        m_linearArrayHoldingPixelBytes, 
        ColourMappingTable.GreyScale
      ) ;
      m_colouredPixelArrayEncodedAsBytes.SetAllPixels(
        RgbByteValues.LightBlue
      ) ;
      var overlays = new OverlaysDescriptor(
        OverlayBoxDescriptor.FromCentrePoint(
          x      : 40,
          y      : 30,
          height : 20,
          width  : 30,
          colour : RgbByteValues.Red,
          thick  : Thick
        ),
        OverlayClosedPolygonDescriptor.CreateTiltedBox(
          centreX  : 40,
          centreY  : 30,
          height  : 20,
          width   : 30,
          tiltAngle_anticlockwiseDegrees : 20.0,
          colour  : RgbByteValues.Blue,
          thick   : Thick
        ),
        new OverlayLineDescriptor(
          Colour : RgbByteValues.Red,
          Thick : Thick,
          40,
          30,
          140,
          130
        ),
        new OverlayCircleDescriptor(
          Colour : RgbByteValues.Red,
          Thick  : Thick,
          140,
          130,
          20
        ),
        new OverlayClosedPolygonDescriptor(
          Colour : RgbByteValues.Red,
          Thick  : Thick,
          new[]{(200,200),(200,250),(250,200)}
        ),
        new OverlayOpenPolygonDescriptor(
          Colour : RgbByteValues.Red,
          Thick  : Thick,
          new[]{(400,200),(400,250),(450,200)}
        ),
        new OverlayPointsDescriptor(
          Colour : RgbByteValues.Red,
          Thick  : Thick,
          new[]{(400,300),(400,350),(450,300)}
        ),
        OverlayPointsDescriptor.Create(
          colour : RgbByteValues.Black,
          thick  : Thick,
          new []{300,300,300,350,350,300,0,0,0,0}
        )
      ) ;
      overlays.Draw(m_colouredPixelArrayEncodedAsBytes) ;
    }

    protected override void OnInitialized ( )
    {
      base.OnInitialized() ;
    }

    protected override async Task OnAfterRenderAsync ( bool firstRender )
    {
      // The needs to be 'async' so that we can 'await' the call to 'InvokeVoidAsync'.
      if ( firstRender )
      {
        var selfReference = DotNetObjectReference.Create(this);
        // m_jsObjectReferenceModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Pages/CanvasDrawingTestPage_01.razor.js");
        // m_jsObjectReferenceModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./CanvasDrawingTestPage_01.razor.js");
        module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/CanvasDrawingTestPage_01.razor.js");
        object sum = await module!.InvokeAsync<int>("ComputeSum", new object[]{1,2});
        sum = await module!.InvokeAsync<int>("ComputeSum", 3,4);
        await module!.InvokeVoidAsync("WriteToConsole", "Written from CanvasDrawingTestPage_01");
      }

      using ( 
        var timer = new ExecutionTimer_ShowingMillisecsElapsed(
          $"Invoking 'CanvasPutImageDataEx' with size {m_colouredPixelArrayEncodedAsBytes.Width} x {m_colouredPixelArrayEncodedAsBytes.Height}",
          message => System.Diagnostics.Debug.WriteLine(message)
        ) 
      ) {
        // await JSRuntime.InvokeVoidAsync(
        await module!.InvokeVoidAsync(
          "CanvasPutImageDataEx2", 
          m_canvasRef, 
          m_colouredPixelArrayEncodedAsBytes.Width, 
          m_colouredPixelArrayEncodedAsBytes.Height, 
          m_colouredPixelArrayEncodedAsBytes.ColouredImageBytesLinearArray
        ) ;
      }
    }

  }

}

