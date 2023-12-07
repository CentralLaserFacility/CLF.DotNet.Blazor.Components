//
// ChannelNameAndValueDescriptor.cs
//

namespace Clf.LogicSystem
{

  // This is used to represent a Channel value
  // that we've received from Epics,
  // or from a simulation.

  public record ChannelNameAndValueDescriptor ( 
    string ChannelName, 
    string ValueAsString 
  ) {

    public static ChannelNameAndValueDescriptor CreateFromEncodedString ( string channelNameAndValue )
    {
      string[] fields = channelNameAndValue.Split(';') ;
      return new ChannelNameAndValueDescriptor(
        fields[0],
        fields[1]
      ) ;
    }

    public string ToEncodedString ( ) => $"{ChannelName};{ValueAsString}" ;

  }

}
