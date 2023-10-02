using Clf.Blazor.Common.FilePicker;
using Clf.Blazor.Common.Services;
using Clf.ChannelAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ThemeSelectorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

// Makes wwwroot folder accessible to the application
app.UseStaticFiles();

// We can add any folder that we want to be accessible by our application in this way 
// 1. Use Environment WebRootPath path
// 2. Combine it with the folder that we want to use in our application
// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0

app.UseStaticFiles(new StaticFileOptions
{
  FileProvider = new PhysicalFileProvider(
    Path.Combine(builder.Environment.WebRootPath, "../Pages")),
  RequestPath = "/Pages"
});

//app.UseStaticFiles(new StaticFileOptions
//{
//  FileProvider = new PhysicalFileProvider(
//    Path.Combine(builder.Environment.ContentRootPath, "../Clf.Blazor.Basic.Components/Data/")),
//  RequestPath = "/StaticImages"
//});

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Hub.GetOrCreateLocalChannel(ChannelDescriptor.FromEncodedString($"Blazor:TestLed|i16|1"), true);

// Clf.IntensityMapLib.ColourDescriptor.RunUnitTests() ;
// 
// IntensityMapLib.ColourMappingTable.RunUnitTests() ;
// Clf.IntensityMapLib.LinearArrayHoldingPixelBytes.RunUnitTests() ;
// 
// List<string> performanceTestResults = new() ;
// 
// Clf.IntensityMapLib.ColouredPixelArrayEncodedAsBytes_Tests.RunIntegrityTestsOnSmallImage(
//   handleMessage : (message) => performanceTestResults.Add(message)
// ) ;
// 
// Clf.IntensityMapLib.ColouredPixelArrayEncodedAsBytes_Tests.RunPerformanceTests(
//   width         : 700,
//   height        : 500,
//   handleMessage : (message) => performanceTestResults.Add(message)
// ) ;
// 
// Clf.IntensityMapLib.ColouredPixelArrayEncodedAsBytes_Tests.RunPerformanceTests(
//   width         : 2000,
//   height        : 1800,
//   handleMessage : (message) => performanceTestResults.Add(message)
// ) ;
// 
// Clf.IntensityMapLib.LinearArrayHoldingPixelBytes.RunPerformanceTests(
//   width               : 700,
//   height              : 500,
//   sizeReductionFactor : 3,
//   handleMessage       : (message) => performanceTestResults.Add(message)
// ) ;
// Clf.IntensityMapLib.LinearArrayHoldingPixelBytes.RunPerformanceTests(
//   width               : 1000,
//   height              : 800,
//   sizeReductionFactor : 3,
//   handleMessage       : (message) => performanceTestResults.Add(message)
// ) ;
// Clf.IntensityMapLib.LinearArrayHoldingPixelBytes.RunPerformanceTests(
//   width               : 2000,
//   height              : 1800,
//   sizeReductionFactor : 6,
//   handleMessage       : (message) => performanceTestResults.Add(message)
// ) ;
// 
// Clf.IntensityMapLib.ColouredPixelArrayEncodedAsBytes_PerformanceTests.RunPerformanceTests(
//   width                 : 700,
//   height                : 500,
//   handleMessage         : (message) => performanceTestResults.Add(message)
// ) ;
// Clf.IntensityMapLib.ColouredPixelArrayEncodedAsBytes_PerformanceTests.RunPerformanceTests(
//   width                 : 1000,
//   height                : 800,
//   handleMessage         : (message) => performanceTestResults.Add(message)
// ) ;
// Clf.IntensityMapLib.ColouredPixelArrayEncodedAsBytes_PerformanceTests.RunPerformanceTests(
//   width                 : 2000,
//   height                : 1800,
//   handleMessage         : (message) => performanceTestResults.Add(message)
// ) ;
// 
// Clf.IntensityMapLib.SpanExperiments.Experiment_01() ;
// Clf.IntensityMapLib.SpanExperiments.Experiment_02() ;
// Clf.IntensityMapLib.SpanExperiments.Experiment_03() ;
// Clf.IntensityMapLib.SpanExperiments.Experiment_04() ;
// 
// var resultSummary = string.Join(
//   "\r\n",
//   performanceTestResults
// ) ;

// try
// {
//   System.IO.File.WriteAllText(
//     #if DEBUG
//     @"C:\tmp\LinearArrayOfPixelBytes_Timings_DEBUG.txt",
//     #else
//     @"C:\tmp\LinearArrayOfPixelBytes_Timings_RELEASE.txt",
//     #endif
//     resultSummary
//   ) ;
// }
// catch
// { 
//   // In case C:\tmp doesn't exist ...
// }

app.Run();
