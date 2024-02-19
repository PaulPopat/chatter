using Effuse.Core.Domain;

namespace Effuse.Core.Integration.Contracts;

public interface IChannelClient
{
  Task<Channel> CreateChannel(string name, ChannelType type, bool @public);

  Task<Channel> UpdateChannel(Channel channel);

  Task<Channel> GetChannel(Guid channelId);

  IAsyncEnumerable<Channel> ListChannels();
}
