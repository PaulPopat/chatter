using Effuse.Handlers.Contracts;
using Effuse.Handlers.Models;

namespace Effuse.Handlers.Controllers;


public class TestHandler : IHandler<object, TestHandlerResponse>
{
  public async Task<HandlerResponse<TestHandlerResponse>> Handle(HandlerProps<object> props)
  {
    return new HandlerResponse<TestHandlerResponse>(200, new TestHandlerResponse
    {
      Text = "Hello world"
    });
  }
}
