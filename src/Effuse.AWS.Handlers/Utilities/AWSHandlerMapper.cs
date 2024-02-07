using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Effuse.Handlers.Contracts;

namespace Effuse.AWS.Handlers.Utilities;

public static class AWSHandlerMapper
{
  public static async Task<APIGatewayProxyResponse> Process<TBody, TResponse, THandler>
    (
      APIGatewayProxyRequest request
    ) where THandler : IHandler<TBody, TResponse>
  {
    var handler = Bootstrap.CreateApp<THandler>();

    var input = new HandlerProps<TBody>
      (
        request.Path,
        request.HttpMethod,
        request.RequestContext.ConnectionId,
        request.PathParameters,
        request.QueryStringParameters,
        request.Headers,
        JsonSerializer.Deserialize<TBody>(JsonDocument.Parse(request.Body))
      );

    var response = await handler.Handle(input);

    return new APIGatewayProxyResponse
    {
      StatusCode = response.StatusCode,
      Body = JsonSerializer.Serialize(response.Body),
      Headers = response.Headers
    };
  }
}