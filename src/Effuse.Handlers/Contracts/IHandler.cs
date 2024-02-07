namespace Effuse.Handlers.Contracts;

public interface IHandler<TInput, TResponse> {
  Task<HandlerResponse<TResponse>> Handle(HandlerProps<TInput> props);
}