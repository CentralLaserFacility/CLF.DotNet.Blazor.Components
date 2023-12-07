using Clf.Blazor.Common.FilePicker;

namespace Clf.Blazor.Complex.IntensityMap.ViewModels
{
  public partial class IntensityMapFixedDisplayViewModel
  {
    private void IntensityMapFixedDisplayViewModel_Logic_Initiliasation()
    {
      ExportProfileGraphXViewModel.OnActionButtonClicked = OnExportProfileGraphXClicked;
      ExportProfileGraphYViewModel.OnActionButtonClicked = OnExportProfileGraphYClicked;
      ExportImage.OnActionButtonClicked = OnExportImageClicked;
    }

        private async void OnExportProfileGraphXClicked()
        {

            var imageViewer = m_parent.IntensityMapImage.ImageViewer;
            var normalisedOriginalImage = imageViewer.OriginalImageData.CreateNormalisedClone(
              imageViewer.AutoNormalise
              ? imageViewer.OriginalImageData.MaxValue
              : imageViewer.NormalisationValue
            );

            var xProfileDataRaw = normalisedOriginalImage.GetRowOfPixelsAtOffsetFromTop(Convert.ToInt32(Convert.ToDouble(m_parent.ProfileCrosshairY)/ m_parent.YDisplayScalingFactor));
            var xProfileData = xProfileDataRaw.Select(x => Convert.ToInt16(x)).ToArray();

            if(xProfileData != null)
            {
                var options = new SaveFilePickerOptions()
                {
                    SuggestedName = m_parent.PvPrefix + "_ProfileX_Data",
                    Types = new SelectionType[]
                    {
                        new SelectionType()
                        {
                            Description = "CSV File",
                            Accept = new()
                            {
                                {"text/plain",new string[]{".csv"} }
                            }
                        }
                    }
                };
                var xstring = string.Join(Environment.NewLine, xProfileData);
                await m_parent.FilePickerService.SaveToFileUsingFilePickerAsync(options, xstring);
            }
        }

        private async void OnExportProfileGraphYClicked()
        {

            var imageViewer = m_parent.IntensityMapImage.ImageViewer;
            var normalisedOriginalImage = imageViewer.OriginalImageData.CreateNormalisedClone(
              imageViewer.AutoNormalise
              ? imageViewer.OriginalImageData.MaxValue
              : imageViewer.NormalisationValue
            );
 
            var yProfileDataRaw = normalisedOriginalImage.GetColumnOfPixelsAtOffsetFromLeft(Convert.ToInt32(Convert.ToDouble(m_parent.ProfileCrosshairX) / m_parent.XDisplayScalingFactor));
            var yProfileData = yProfileDataRaw.Select(x => Convert.ToInt16(x)).ToArray();

            if (yProfileData != null)
            {
                var options = new SaveFilePickerOptions()
                {
                    SuggestedName = m_parent.PvPrefix + "_ProfileY_Data",
                    Types = new SelectionType[]
                    {
                        new SelectionType()
                        {
                            Description = "CSV File",
                            Accept = new()
                            {
                                {"text/plain",new string[]{".csv"} }
                            }
                        }
                    }
                };
                var ystring = string.Join(Environment.NewLine, yProfileData);
                await m_parent.FilePickerService.SaveToFileUsingFilePickerAsync(options, ystring);
            }
        }
    
    private async void OnExportImageClicked()
        {
            var imageViewer = m_parent.IntensityMapImage.ImageViewer;
            var normalisedOriginalImage = imageViewer.OriginalImageData.CreateNormalisedClone(
              imageViewer.AutoNormalise
              ? imageViewer.OriginalImageData.MaxValue
              : imageViewer.NormalisationValue
            );
            var width = normalisedOriginalImage.Width;  
            var height = normalisedOriginalImage.Height;    

            byte[] flatImage = new byte[width * height];
            int count = 0;
            for(int i=0; i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    flatImage[count++] = normalisedOriginalImage.GetPixel(j,i);
                }
            }
            Clf.Tiff.Helpers.WriteGreyscaleImageAsTiffByteArray(width,height, flatImage, out var tiffBytes);

            var options = new SaveFilePickerOptions()
            {
                SuggestedName = m_parent.PvPrefix + "_tiff",
                Types = new SelectionType[]
                    {
                        new SelectionType()
                        {
                            Description = "Tiff File",
                            Accept = new()
                            {
                                {"image/tiff",new string[]{".tiff"} }
                            }
                        }
                    }
            };
            await m_parent.FilePickerService.SaveToFileUsingFilePickerAsync(options, tiffBytes);
        }

    }
  
}
