using Effuse.Core.Domain;
using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;

namespace Effuse.Core.Integration.Implementations;

public class ChatLog : IChatLog
{
  private readonly IStatic @static;
  private readonly IEncryption encryption;

  public ChatLog(IStatic @static, IEncryption encryption)
  {
    this.@static = @static;
    this.encryption = encryption;
  }

  private static long BundleSize => 200;
  private static long LogsPerRequest => 20;
  private static char Delimiter => ';';

  private long LogFileIndex(long messageIndex)
  {
    return messageIndex / BundleSize;
  }

  private long LineNumber(long messageIndex)
  {
    return messageIndex % BundleSize;
  }

  private string LogPath(Channel channel, long messageIndex)
  {
    return $"chat/{channel.ChannelId}/messages/bundle-{this.LogFileIndex(messageIndex)}.log";
  }

  private async Task<long> NumberOfMessages(Channel channel)
  {
    try
    {
      var path = $"chat/{channel.ChannelId}/message-count";
      var text = await this.@static.DownloadText(path);
      return long.Parse(text.Data);
    }
    catch
    {
      return 0L;
    }
  }


  private async Task AddMessageNumber(Channel channel)
  {
    var path = $"chat/{channel.ChannelId}/message-count";
    try
    {
      var text = await this.@static.DownloadText(path);
      var current = long.Parse(text.Data);
      await this.@static.UploadText(new()
      {
        Name = path,
        Data = (current + 1).ToString(),
        Mime = "text"
      });
    }
    catch
    {
      await this.@static.UploadText(new()
      {
        Name = path,
        Data = 1.ToString(),
        Mime = "text"
      });
    }
  }

  public async IAsyncEnumerable<Message> GetMessageLogs(Channel channel, long offset)
  {
    var count = await this.NumberOfMessages(channel);
    if (count <= offset) throw new Exception("At end of messages");

    var messageIndex = count - offset;

    var file = await this.@static.DownloadText(this.LogPath(channel, messageIndex));
    var lines = file.Data.Split('\n');
    var start = this.LineNumber(messageIndex) - 1;

    for (var i = start; start - i < LogsPerRequest && i >= 0; i--)
    {
      var line = await this.encryption.Decrypt(lines[i]);
      var lineParts = line.Split(Delimiter);
      var authorId = Guid.Parse(lineParts[0]);
      var when = DateTime.Parse(lineParts[1]);
      var message = string.Join(Delimiter, lineParts.Skip(2));

      yield return new Message(message, when, authorId);
    }
  }

  private string FormatMessage(Message message)
  {
    return $"{message.UserId}{Delimiter}{message.When.ToISOString()}{Delimiter}{message.Text}";
  }

  public async Task PostMessage(Channel channel, Message message)
  {
    var count = await this.NumberOfMessages(channel);
    if (this.LineNumber(count) == 0L)
    {
      await this.@static.UploadText(new StaticTextFile
      {
        Name = this.LogPath(channel, count),
        Data = await this.encryption.Encrypt(this.FormatMessage(message)),
        Mime = "text"
      });
    }
    else
    {
      var existing = await this.@static.DownloadText(this.LogPath(channel, count));
      await this.@static.UploadText(new StaticTextFile
      {
        Name = this.LogPath(channel, count),
        Data = existing.Data + "\n" + (await this.encryption.Encrypt(this.FormatMessage(message))),
        Mime = "text"
      });
    }

    await this.AddMessageNumber(channel);
  }
}
