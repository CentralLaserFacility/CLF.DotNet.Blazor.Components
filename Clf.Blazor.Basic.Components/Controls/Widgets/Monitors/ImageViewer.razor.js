//
// IntensityMapImageEx.razor.js
//

//
// Gotcha !!!
//
// You can include javascript code needed by a component, in a 'MyComponent.razor.js' file.
// But in order for your function to be found, the source code needs to be 'included'
// as a script in the 'body' of the app :
//
// _Host.cshtml, or _Layout.cshtml ???? TINE ????
//  <body>
//    <script ... >
//    <script src="_content/Clf.Blazor.Basic.Components/Controls/Widgets/IntensityMap/ImageViewerEx.razor.js"></script>
//
// If you want to use a package such as paper.js to draw onto the canvas,
// you can just include that via a <script> and then invoke the functions from our own js code,.
//

//
// But there are other MUCH BETTER ways to link to js files defined on a per-component basis :
//   https://docs.microsoft.com/en-us/aspnet/core/blazor/components/class-libraries?view=aspnetcore-6.0&tabs=visual-studio
//   https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-javascript-from-dotnet?view=aspnetcore-6.0#javascript-isolation-in-javascript-modules
//

//
// Hmm, would be better to create the 'packed' array of byte value in C#,
// pass that across as an argument and load it into the 'imageData.data' array
// with a single API call, instead of writing individual bytes in a loop ...
// ... but, how best to do that copy in JavaScript ???
//
// https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Pixel_manipulation_with_canvas
//
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Uint8ClampedArray
//

function CanvasPutImageDataEx (
  canvas, 
  displayWidth, 
  displayHeight,
  rgbaByteValues // byte[displayWidth*displayHeight*4]
) {
  const context2D = canvas.getContext('2d') ;
  // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/createImageData
  const imageData = context2D.createImageData(displayWidth,displayHeight) ;
  // Uint8ClampedArray.prototype.set()
  // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypedArray/set
  imageData.data.set(rgbaByteValues)
  context2D.putImageData(
    imageData, 
    0, // destination x
    0, // destination y
    0, // source x
    0, // source y
    displayWidth, 
    displayHeight
  );
  return true;
}

// function CanvasPutImageData_old_01 (
//   canvas, 
//   grayScaleData,                        // byte[displayWidth*displayHeight]
//   colourMappingTable_byteValuesToRed,   // byte[256] 
//   colourMappingTable_byteValuesToGreen, // byte[256] 
//   colourMappingTable_byteValuesToBlue,  // byte[256] 
//   displayWidth, 
//   displayHeight
// ) {
//   const context2D = canvas.getContext('2d') ;
//   // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/createImageData
//   const imageData = context2D.createImageData(displayWidth,displayHeight) ;
//   // Load successive pixels of the image
//   var iImageData = 0 ;
//   for ( let iGrayScaleData = 0 ; iGrayScaleData < grayScaleData.length ; iGrayScaleData++ ) 
//   {
//     let greyScaleIntensity = grayScaleData[iGrayScaleData] ;
//     let red   = colourMappingTable_byteValuesToRed   [greyScaleIntensity] ;
//     let green = colourMappingTable_byteValuesToGreen [greyScaleIntensity] ;
//     let blue  = colourMappingTable_byteValuesToBlue  [greyScaleIntensity] ;
//     // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/createImageData#filling_a_blank_imagedata_object
//     imageData.data[iImageData + 0] = red   ; // R
//     imageData.data[iImageData + 1] = green ; // G
//     imageData.data[iImageData + 2] = blue  ; // B
//     imageData.data[iImageData + 3] = 255   ; // A
//     iImageData += 4 ;
//   }
//   // Draw the image data to the canvas
//   // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/putImageData
//   // let zz = imageData.data ; // This is a Uint8ClampedArray !!!
//   // This should load a black line of 8 pixels ...
//   // imageData.data.set([0,0,0,255,0,0,0,255,0,0,0,255,0,0,0,255,0,0,0,255,0,0,0,255,0,0,0,255,0,0,0,255])
//   context2D.putImageData(
//     imageData, 
//     0, // destination x
//     0, // destination y
//     0, // source x
//     0, // source y
//     displayWidth, 
//     displayHeight
//   ) ;
// }


