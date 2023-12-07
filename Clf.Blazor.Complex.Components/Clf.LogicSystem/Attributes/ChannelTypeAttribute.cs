using Clf.ChannelAccess;

namespace Clf.LogicSystem.Attributes
{
    public class ChannelTypeAttribute : LogicNodeAttribute
    {
        public ChannelTypeAttribute(ChannelType channelType) : base($"{channelType}")
        {
        }
    }
}
