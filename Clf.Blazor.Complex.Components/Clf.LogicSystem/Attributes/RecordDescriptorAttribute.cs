//
// RecordDescriptorAttribute.cs
//

namespace Clf.LogicSystem.Attributes
{

  //
  // This attribute lets us apply an encoded 'RecordDescriptor'
  // to a Logic Node ; that not only provides the PV name, but also
  // the expected data type :
  //
  //  eg 'MyChannelName|int'
  //
  // With this information embedded into the Clf.LogicSystem definition,
  // we can create a useful list of all the records that are expected 
  // to be available ; and those PV's can be provided either via ThinIocServer
  // or as LocalChannels.
  //

  //
  // NOT USED YET ...
  //

  public sealed class RecordDescriptorAttribute : ChannelNameAttribute
  {
    public RecordDescriptorAttribute ( string value ) : base(value) { }
  }

}
