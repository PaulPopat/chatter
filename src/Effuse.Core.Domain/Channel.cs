namespace Effuse.Core.Domain;

public class Channel
{
  public Channel(Guid channelId, ChannelType type, string name, bool @public)
  {
    ChannelId = channelId;
    Type = type;
    Name = name;
    Public = @public;
  }

  public Guid ChannelId { get; }

  public ChannelType Type { get; }

  public string Name { get; }

  public bool Public { get; }
}
