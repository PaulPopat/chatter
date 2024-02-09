using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models;

namespace Effuse.SSO.Handlers.Controllers;


public class HeartBeat : IHandler<object, TestHandlerResponse>
{
  public async Task<HandlerResponse<TestHandlerResponse>> Handle(HandlerProps<object> props)
  {
    return new HandlerResponse<TestHandlerResponse>(200, new TestHandlerResponse
    {
      Text = "Hello world"
    });
  }
}
