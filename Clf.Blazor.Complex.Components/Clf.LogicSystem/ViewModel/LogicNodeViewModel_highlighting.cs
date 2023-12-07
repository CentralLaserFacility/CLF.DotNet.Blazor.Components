//
// LogicNodeViewModel_highlighting.cs
//

using Clf.LogicSystem.Attributes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicNodeViewModel
  { 

    [ObservableProperty]
    [GeneratedPropertyNameIs(nameof(HighlightingChoice))]
    private Clf.Common.UI.HighlightingOption m_highlightingChoice = Clf.Common.UI.HighlightingOption.NotHighlighted ;
    
  }

}

