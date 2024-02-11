namespace Effuse.Core.Handlers.Contracts;

public interface IHandler {
  Task<HandlerResponse> Handle(HandlerProps props);
}
