//
// NetworkLayoutDescriptor_CreatedFromParsingGraphVizPlainFile.cs
//

namespace Clf.LogicSystem.Common
{

  internal class NetworkLayoutDescriptor_CreatedFromParsingGraphVizPlainFile : Clf.LogicSystem.Common.NetworkLayoutDescriptor
  {

    public NetworkLayoutDescriptor_CreatedFromParsingGraphVizPlainFile ( 
      Clf.LogicSystem.Common.LayoutSizingParameters layoutSizingParameters 
    ) :
    base(layoutSizingParameters)
    { }

    public void AddNode ( NodeLayoutDescriptor node )
    => m_nodeLayoutDescriptors.Add(node) ;

    public void AddEdge ( EdgeLayoutDescriptor edge )
    => m_edgeLayoutDescriptors.Add(edge) ;

  }

}
