namespace Effuse.Handlers.Contracts;

public interface IHandler<TBody, TResponse> where TBody : class {
  Task<HandlerResponse<TResponse>> Handle(HandlerProps<TBody> props);
}
