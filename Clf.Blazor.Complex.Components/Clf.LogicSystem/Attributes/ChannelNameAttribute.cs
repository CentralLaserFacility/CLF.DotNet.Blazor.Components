//
// ChannelNameAttribute.cs
//

using Clf.Common.Helper_functions;

namespace Clf.LogicSystem.Attributes
{

  //
  // The 'display name' is typically the name of the associated PV,
  // which often can't be represented as a valid C# property name as it
  // contains special characters.
  //
  // If a [ChannelName(...)] attribute omitted, the Property Name will be used.
  //

  //
  // TODO : change to 'ChannelName' ???
  //

  public class ChannelNameAttribute : LogicNodeAttribute
  {
    public ChannelNameAttribute(string value) : base(value) { }
  }

}
