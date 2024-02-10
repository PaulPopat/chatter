namespace Effuse.Core.Handlers.Contracts;

public interface IHandler<TBody, TResponse> {
  Task<HandlerResponse<TResponse>> Handle(HandlerProps<TBody> props);
}
