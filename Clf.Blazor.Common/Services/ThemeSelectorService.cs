using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Common.Services
{
  public class ThemeSelectorService
  {
    public delegate void ThemeChangedHandler(string theme);

    public event ThemeChangedHandler? ThemeChanged;

    private string _currentTheme = "Light";

    public string CurrentTheme
    {
      get { return _currentTheme; }
      set 
      { 
        _currentTheme = value;
        ThemeChanged?.Invoke(_currentTheme);
      }
    }
  }

}
