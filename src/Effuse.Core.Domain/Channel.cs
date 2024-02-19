namespace Effuse.Core.Domain;

public class Channel
{
  public Channel(Guid channelId, ChannelType type, string name)
  {
    ChannelId = channelId;
    Type = type;
    Name = name;
  }

  public Guid ChannelId { get; }

  public ChannelType Type { get; }

  public string Name { get; }
}
