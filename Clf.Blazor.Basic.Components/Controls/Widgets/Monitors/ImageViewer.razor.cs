//
// ImageViewerEx.razor.cs
//

using System.Threading.Tasks;
using Microsoft.JSInterop;
using Clf.Common.ImageProcessing;

namespace Clf.Blazor.Basic.Components.Controls.Widgets.Monitors
{

  public partial class ImageViewer
  {

    private ColouredPixelArrayEncodedAsBytes_Base? m_colouredPixelArrayEncodedAsBytes = null;
    bool isRendering = false;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (ViewModel != null && !isRendering)
      {
        try
        {
          isRendering = true;
          Clf.Common.ImageProcessing.Helpers.BuildColouredPixelArrayEncodedAsBytes(
            ViewModel.DisplayImageData,
            Clf.Common.ImageProcessing.ColourMappingTable.ForColourMapOption(
              ViewModel.ColourMapOption
            ),
            ref m_colouredPixelArrayEncodedAsBytes
          );
          ViewModel.OverlaysDescriptor.Draw(
            m_colouredPixelArrayEncodedAsBytes!
          );
          var isComplete = await JSRuntime.InvokeAsync<bool>(
            "CanvasPutImageDataEx",
            m_canvasRef,
            ViewModel.DisplayWidth,
            ViewModel.DisplayHeight,
            m_colouredPixelArrayEncodedAsBytes!.ColouredImageBytesLinearArray
          );
        }
        catch (System.Exception ex)
        {
          // Probably the data size doesn't match the Width and Height values ...
          // HMM, WE NEED A SENSIBLE WAY OF LOGGING ERRORS SUCH AS THIS !!!
          int nImageBytesExpected = ViewModel.ImageWidth * ViewModel.ImageHeight;
          int nImageBytesActual = ViewModel.OriginalImageData.PixelCount;
          if (nImageBytesActual != nImageBytesExpected)
          {
            System.Diagnostics.Debug.WriteLine(
              $"Image byte count ({nImageBytesActual}) doesn't match expected value of {nImageBytesExpected} ({ViewModel.ImageWidth} x {ViewModel.ImageHeight})"
            );
          }
          else
          {
            System.Diagnostics.Debug.WriteLine(
              ex.Message
            );
          }
        }
        finally
        {
          isRendering = false;
        }
      }
    }

  }

}