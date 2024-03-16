namespace Effuse.Server.Domain;

public class UserPolicy(Guid channelId, UserPolicyAccess access)
{
  public Guid ChannelId => channelId;

  public bool Read => true;

  public bool Write => access == UserPolicyAccess.Write;

  public UserPolicyAccess Access => access;
}
