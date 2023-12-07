//
// QueueHoldingChannelAndValueDescriptors.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem
{

  public class QueueHoldingChannelAndValueDescriptors 
  : IChannelAndValueDescriptorsAcceptor
  , ISourceOfIncomingChannelAndValueDescriptors
  {

    private System.Collections.Generic.Queue<ChannelNameAndValueDescriptor> m_queue = new() ;

    public void AcceptChannelNameAndValueDescriptor ( ChannelNameAndValueDescriptor channelAndValueDescriptor )
    {
      m_queue.Enqueue(channelAndValueDescriptor) ;
    }

    public IEnumerable<ChannelNameAndValueDescriptor> TakeAllAvailableChannelNameAndValueDescriptors ( )
    {
      while ( m_queue.Count > 0 )
      {
        yield return m_queue.Dequeue() ;
      }
    }

  }

}
