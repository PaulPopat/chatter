using Effuse.Core.Domain;

namespace Effuse.Core.Integration.Contracts;

public interface ISubscriptions
{
  Task Subscribe(Subscription subscription);

  Task Unsubscribe(string subscriptionId);

  Task<Subscription> GetSubscription(string subscriptionId);

  Task Broadcast(Subscription subscription, Message message);
}
