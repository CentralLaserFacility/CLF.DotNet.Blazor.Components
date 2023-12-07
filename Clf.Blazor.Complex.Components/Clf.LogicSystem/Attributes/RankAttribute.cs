//
// Rank.cs
//

namespace Clf.LogicSystem.Attributes
{

  //
  // This is used to give a hint to the GraphViz layout engine
  // about which 'rank' the associated npde should be placed in.
  // Hmm, not terribly successful ...
  //

  public sealed class RankAttribute : LogicNodeAttribute
  {
    public RankAttribute ( string value ) : base(value) { }
  }

}
