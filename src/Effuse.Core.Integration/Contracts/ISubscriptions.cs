using Effuse.Core.Domain;

namespace Effuse.Core.Integration.Contracts;

public interface ISubscriptions
{
  Task Subscribe(Channel channel, string connectionId);

  Task Broadcast(Channel channel, Message message);
}
