using Clf.Blazor.Common.FilePicker;
using Clf.Blazor.Common.Services;
using Clf.ChannelAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;

Clf.ChannelAccess.Settings.WhichDllsToUse = WhichDllsToUse.DaresburyReleaseDlls;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
builder.Services.AddScoped<FilePickerService>();
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


app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
