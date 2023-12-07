//
// IWindowOpeningService.cs
//

namespace Clf.LogicSystem.Common.UI
{

  public interface IWindowOpeningService
  {

    //
    // Creates a fresh Window, visualising the supplied Clf.Clf.LogicSystem
    // but only showing the specified nodes. If a 'null' visibility specifier
    // is provided, the default is to show all nodes.
    //

    void OpenNewWindow (
      Clf.LogicSystem.LogicSystemBase               logicSystem,
      Clf.LogicSystem.LogicNodeVisibilitySpecifier? logicNodeVisibilitySpecifier = null
    ) ;

  }
    
}
