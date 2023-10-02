//
// CanvasDrawingTestPage_01.razor.js
//

//
// Here we're co-locating the Javascript code with the component :
//
//   https://docs.microsoft.com/en-us/aspnet/core/blazor/components/class-libraries?view=aspnetcore-6.0&tabs=visual-studio
//   https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-javascript-from-dotnet?view=aspnetcore-6.0#javascript-isolation-in-javascript-modules
//

export function ComputeSum ( a, b )
{
  return a + b ;
}

export function WriteToConsole ( message )
{
  console.log(message) ;
}

export function CanvasPutImageDataEx2 (
  canvas, 
  displayWidth, 
  displayHeight,
  rgbaByteValues // byte[displayWidth*displayHeight*4]
) {
  const context2D = canvas.getContext('2d') ;
  // https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/createImageData
  const imageData = context2D.createImageData(displayWidth,displayHeight) ;
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
  ) ;
}


