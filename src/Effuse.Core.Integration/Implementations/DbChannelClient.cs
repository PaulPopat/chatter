using Effuse.Core.Domain;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.Integration.Implementations;

public class DbChannelClient : IChannelClient
{
  private struct ChannelDto
  {
    public int Type { get; set; }

    public string Name { get; set; }

    public bool Public { get; set; }
  }

  private struct ChannelListDto
  {
    public List<string> Channels { get; set; }
  }

  private static string TableName => "Channels";
  private static string ListTableName => "ChannelList";
  private static string ListItemName => "AllChannels";

  private readonly IDatabase database;

  public DbChannelClient(IDatabase database)
  {
    this.database = database;
  }

  private static ChannelDto ToDto(Channel channel)
  {
    return new ChannelDto
    {
      Type = (int)channel.Type,
      Name = channel.Name,
      Public = channel.Public
    };
  }

  private static Channel FromDto(Guid id, ChannelDto dto)
  {
    return new Channel(id, (ChannelType)dto.Type, dto.Name, dto.Public);
  }

  public async Task<Channel> CreateChannel(string name, ChannelType type, bool @public)
  {
    var result = new Channel(Guid.NewGuid(), type, name, @public);
    await this.database.AddItem(TableName, result.ChannelId.ToString(), ToDto(result));

    var channels = await this.database.FindItem<ChannelListDto>(ListTableName, ListItemName);
    if (channels == null)
    {
      await this.database.AddItem(ListTableName, ListItemName, new ChannelListDto
      {
        Channels = new List<string>() { result.ChannelId.ToString() }
      });
    }
    else
    {
      await this.database.UpdateItem(ListTableName, ListItemName, new ChannelListDto
      {
        Channels = channels.Value.Channels.Append(result.ChannelId.ToString()).ToList()
      });
    }

    return result;
  }

  public async Task<Channel> GetChannel(Guid channelId)
  {
    var dto = await this.database.GetItem<ChannelDto>(TableName, channelId.ToString());

    return FromDto(channelId, dto);
  }

  public async IAsyncEnumerable<Channel> ListChannels()
  {
    var channels = await this.database.FindItem<ChannelListDto>(ListTableName, ListItemName);
    var list = channels.HasValue ? channels.Value.Channels : new List<string>();

    foreach (var item in list)
    {
      if (item == null) continue;
      yield return await this.GetChannel(Guid.Parse(item));
    }
  }

  public async Task<Channel> UpdateChannel(Channel channel)
  {
    await this.database.UpdateItem(TableName, channel.ChannelId.ToString(), ToDto(channel));
    return channel;
  }
}
