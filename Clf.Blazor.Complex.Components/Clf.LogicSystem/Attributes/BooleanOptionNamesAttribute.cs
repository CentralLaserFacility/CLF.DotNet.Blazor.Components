//
// BooleanOptionNamesAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  public sealed class BooleanOptionNamesAttribute : LogicNodeAttribute
  {

    // Syntax : "TrueValue[,TrueValueB]*|FalseValue[,FalseValue]*"
    // Examples :
    //   "On|Off"
    //   "On,PoweredOn|Off"
    public BooleanOptionNamesAttribute ( string trueAndFalseValueOptions ) : 
    base(trueAndFalseValueOptions) 
    { }

  }

}
