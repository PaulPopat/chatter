namespace Effuse.Core.Handlers.Contracts;

public interface IWebSocketHandler
{
  Task OnConnect(HandlerProps props);

  Task<object> OnMessage(string connectionId, string message)
  {
    return Task.FromResult<object>(new { });
  }

  Task OnClose(string connectionId);
}
