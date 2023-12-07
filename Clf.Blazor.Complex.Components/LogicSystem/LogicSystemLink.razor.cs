using Clf.LogicSystem.ViewModel;
using Microsoft.AspNetCore.Components;

namespace Clf.Blazor.Complex.LogicSystem
{
  public partial class LogicSystemLink: ComponentBase, System.IDisposable
  {

    [Parameter]
    [EditorRequired]
    public DependencyLinkViewModel DependencyLinkViewModel { get ; set ; } = null! ;

    [CascadingParameter]
    public LogicSystemDisplayPanel ParentDisplayPanel { get ; set ; }

    public enum EdgeRenderMethod
    {
      BezierCurves,
      Line,
      Polyline,
    }

    private readonly EdgeRenderMethod RenderEdgeAs = EdgeRenderMethod.BezierCurves;

    private string PolylinePoints 
    {
      get 
      {
        // https://developer.mozilla.org/en-US/docs/Web/SVG/Element/polyline
        string points = "" ;
        foreach ( var pointOnPath in DependencyLinkViewModel.PointsOnPath ) 
        {
          points += $"{pointOnPath.X},{pointOnPath.Y} " ;
        }
        return points ;
      }
    }

    private string BezierPoints
    {
      get
      {
        /* The initial M directive moves the pen to the first point. 
        Other coordinates follow the C: the first control point, the second control point, ..., and the final ending point. 
        You can also use a lowercase c to denote relative rather than absolute coordinates (but it doesn't render properly here).
        refer: https://www.sitepoint.com/html5-svg-cubic-curves/
        */
        string points = "";
        foreach (var pointOnPath in DependencyLinkViewModel.PointsOnPath)
        {
          if (points == "")
            points += $"M{pointOnPath.X},{pointOnPath.Y} C";
          else
            points += $"{pointOnPath.X},{pointOnPath.Y} ";
        }
        return points;
      }
    }

    // Bezier points ?
    // https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths#curve_commands
    // https://css-tricks.com/svg-path-syntax-illustrated-guide/
    // https://vanseodesign.com/web-design/svg-paths-curve-commands/

    //
    // Hmm, drawing smoothed polylines with Bezier paths is quite fiddly,
    // here are some useful links but it's probably not worth it ...
    //
    // Smooth a Svg path with cubic bezier curves
    // https://francoisromain.medium.com/smooth-a-svg-path-with-cubic-bezier-curves-e37b49d46c74
    // 
    // Svg path LineTo
    // https://codepen.io/francoisromain/pen/QMbMwp
    // 
    // Smooth a Svg path with functional programming
    // https://francoisromain.medium.com/smooth-a-svg-path-with-functional-programming-1b9876b8bf7e
    // 
    // Smooth a Svg path with cubic bezier curves
    // https://observablehq.com/@ndry/smooth-a-svg-path-with-cubic-bezier-curves
    // 
    // Smooth a Svg path with a cubic bezier curve
    // https://codepen.io/francoisromain/pen/XabdZm?html-preprocessor=slim
    // 
    // Smooth a Svg path with a cubic bezier curve
    // https://codepen.io/francoisromain/pen/XabdZm?html-preprocessor=slim
    // 
    // <path> - SVG: Scalable Vector Graphics
    // https://developer.mozilla.org/en-US/docs/Web/SVG/Element/path
    // 
    // SVG Basics—Creating Paths With Curve Commands
    // https://vanseodesign.com/web-design/svg-paths-curve-commands/
    // 

    public LogicSystemLink( )
    {
    }

    protected override void OnInitialized ( )
    {
      base.OnInitialized() ;
      DependencyLinkViewModel.PropertyChanged += OnPropertyChanged ;
    }

    public void OnPropertyChanged ( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
      InvokeAsync(StateHasChanged) ;
    }

    private int RenderNumber = 0 ;

    protected override void OnAfterRender ( bool isFirstRender )
    {
      RenderNumber++ ;
    }

    public void Dispose()
    {
      DependencyLinkViewModel.PropertyChanged -= OnPropertyChanged;
    }
  }

}