using Effuse.Core.Domain;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Server.Services;

public class Channels
{
  private readonly IChannelClient channelClient;
  private readonly Auth auth;

  public Channels(IChannelClient channelClient, Auth auth)
  {
    this.channelClient = channelClient;
    this.auth = auth;
  }

  public async Task<IEnumerable<Channel>> GetChannels(string localToken)
  {
    var user = await this.auth.GetUser(localToken);
    return await this.channelClient.ListChannels().Where(c => user.Admin || user.MayRead(c) || user.MayWrite(c)).ToArrayAsync();
  }
}
