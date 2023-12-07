//
// ISourceOfIncomingChannelAndValueDescriptors.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem
{

  public interface ISourceOfIncomingChannelAndValueDescriptors
  {
    IEnumerable<ChannelNameAndValueDescriptor> TakeAllAvailableChannelNameAndValueDescriptors ( ) ;
  }

}
