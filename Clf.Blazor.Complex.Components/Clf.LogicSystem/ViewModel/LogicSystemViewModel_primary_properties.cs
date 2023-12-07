//
// LogicSystemViewModel_primary_properties.cs
//

using Clf.Common.Utils;

namespace Clf.LogicSystem.ViewModel
{

  public partial class LogicSystemViewModel
  {

    public Clf.LogicSystem.LogicSystemBase LogicSystem => m_logicSystem ;

    // private Clf.Clf.LogicSystem.LogicNodeVisibilitySpecifier m_logicNodeVisibilitySpecifier = null! ;

    public Clf.LogicSystem.LogicNodeVisibilitySpecifier LogicNodeVisibilitySpecifier { get ; private set ; }

    public string WhichNodesAreIncluded => LogicNodeVisibilitySpecifier.ToString() ;

    public System.Action? LogicSystemInstanceChanged ;

    public System.Action? LogicSystemDiagramViewModelChanged ;

    public Clf.LogicSystem.Common.NetworkLayoutDescriptor? m_networkLayoutDescriptor = null ;

    private readonly ILogEntriesCollection m_logOfInterestingEvents ;

    private Clf.LogicSystem.LogicSystemBase m_logicSystem ;

    // 
    // The optional 'Description' property can indicate
    // that we're showing 'nodes influencing ZZZ',
    // or 'nodes for Scenario XYZ' ...
    //

    public string? Description { get ; set ; }

  }

}

