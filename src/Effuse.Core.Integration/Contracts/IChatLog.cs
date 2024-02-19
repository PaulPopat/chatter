using Effuse.Core.Domain;

namespace Effuse.Core.Integration.Contracts;

public interface IChatLog
{
  IAsyncEnumerable<Message> GetMessageLogs(Channel channel, long offset);

  Task PostMessage(Channel channel, Message message);
}
