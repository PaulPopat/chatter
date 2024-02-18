namespace Effuse.Core.Handlers.Contracts;

public interface IWebSocketHandler
{
  Task OnConnect(HandlerProps props);

  Task<object> OnMessage(string connectionId, string message);

  Task OnClose(string connectionId);
}
