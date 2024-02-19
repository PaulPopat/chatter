namespace Effuse.Server.Domain;

public class UserPolicy
{
  public UserPolicy(Guid channelId, bool read, bool write)
  {
    ChannelId = channelId;
    Read = read;
    Write = write;
  }

  public Guid ChannelId { get; }

  public bool Read { get; }

  public bool Write { get; }
}
