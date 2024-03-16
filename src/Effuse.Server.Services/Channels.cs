using Effuse.Core.Domain;
using Effuse.Core.Integration;
using Effuse.Core.Integration.Contracts;
using Effuse.Server.Domain;
using Effuse.Server.Integrations.Contracts;

namespace Effuse.Server.Services;

public class ChannelUser(Guid userId, bool mayRead, bool mayWrite)
{
  public Guid UserId => userId;

  public bool MayRead => mayRead;

  public bool MayWrite => mayWrite;
}

public class Channels(IChannelClient channelClient, Auth auth, IUserClient userClient)
{
  public async Task<IEnumerable<Channel>> GetChannels(string localToken)
  {
    var user = await auth.GetUser(localToken);
    return await channelClient.ListChannels().Where(c => user.Admin || user.MayRead(c) || user.MayWrite(c)).ToArrayAsync();
  }

  public async Task<IEnumerable<ChannelUser>> GetChannelUsers(string localToken, Guid channelId)
  {
    var user = await auth.GetUser(localToken);

    var channel = await channelClient.GetChannel(channelId);

    if (!user.MayRead(channel)) throw new AuthException("No read access");

    return await userClient
      .ListUsers()
      .Where(u => u.MayRead(channel))
      .Select(u => new ChannelUser(u.UserId, u.MayRead(channel), u.MayWrite(channel)))
      .ToListAsync();
  }
}
