namespace Effuse.Core.Integration;

public interface ISubscriptions
{
  Task Subscribe(string channel, string connectionId);

  Task Broadcast(string channel, object message);
}
