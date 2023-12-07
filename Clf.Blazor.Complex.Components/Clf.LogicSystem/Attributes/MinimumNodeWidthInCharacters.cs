//
// MinimumNodeWidthInCharacters.cs
//

namespace Clf.LogicSystem.Attributes
{

  public sealed class MinimumNodeWidthInCharactersAttribute : LogicNodeAttribute
  {

    public MinimumNodeWidthInCharactersAttribute ( string nodeWidthInCharacters ) : base(nodeWidthInCharacters) 
    { }

    public int AsInteger => int.Parse(Value) ;

  }

}
