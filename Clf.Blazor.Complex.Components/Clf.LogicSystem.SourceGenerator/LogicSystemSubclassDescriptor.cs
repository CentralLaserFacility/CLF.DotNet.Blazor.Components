//
// LogicSystemSubclassDescriptor.cs
//

using System.Collections.Generic ;
using System.Linq ;

namespace Clf.LogicSystem.SourceGenerator
{

  /// <summary>
  /// Here we're defining the properties that will be collected from the Semantic Model
  /// THIS WILL BE A BETTER WAY OF DEALING WITH PARTIAL CLASSES
  /// </summary>
  public partial class LogicSystemSubclassDescriptor : System.IEquatable<LogicSystemSubclassDescriptor>
  {

    private static int g_nInstancesCreated = 0 ;

    public readonly int InstanceNumber = ++g_nInstancesCreated ;

    public string NamespaceName { get ; set ; } = "LogicNodeNamespaceName" ;

    public string ClassName     { get ; set ; } = "LogicNodeClassName" ;

    public string FullyQualifiedClassName => $"{NamespaceName}.{ClassName}" ;

    public List<InputNodeDescriptor> InputNodeDescriptors { get ; set ; } = new() ;

    public List<ComputedNodeDescriptor> ComputedNodeDescriptors { get ; set ; } = new() ;

    public List<string> ExceptionMessages { get ; set ; } = new() ;

    public bool Equals ( LogicSystemSubclassDescriptor other )
    {
      if ( this.FullyQualifiedClassName != other.FullyQualifiedClassName )
        return false ;
      if ( ! Enumerable.SequenceEqual(ComputedNodeDescriptors,other.ComputedNodeDescriptors) )
        return false ;
      return true ;
    }

  }
}