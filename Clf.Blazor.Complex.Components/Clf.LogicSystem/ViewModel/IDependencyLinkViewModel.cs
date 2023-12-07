//
// IDependencyLinkViewModel.cs
//

namespace Clf.LogicSystem.ViewModel
{

  public interface IDependencyLinkViewModel : System.ComponentModel.INotifyPropertyChanged
  {

    float LineThicknessInPixels { get ; }
        
    bool ShowAsHighlighted { get ; }
                                     
    Clf.LogicSystem.Common.GeomertyPrimitives.Colour LineColour { get ; }

    int StackingOrderZ { get ; }

  }

}

