namespace Effuse.Server.Domain;

public class UserPolicy
{
  public UserPolicy(Guid channelId, bool read, bool write, bool admin)
  {
    ChannelId = channelId;
    Read = read;
    Write = write;
    Admin = admin;
  }

  public Guid ChannelId { get; }

  public bool Read { get; }

  public bool Write { get; }

  public bool Admin { get; }
}
