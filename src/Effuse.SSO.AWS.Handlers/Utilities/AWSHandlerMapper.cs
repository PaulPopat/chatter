using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Effuse.Core.Handlers.Contracts;
using Unity;

namespace Effuse.SSO.AWS.Handlers.Utilities;

public static class AWSHandlerMapper
{
  public static async Task<APIGatewayProxyResponse> Process<TBody, TResponse, THandler>
    (
      APIGatewayProxyRequest request
    )
      where TBody : class
      where THandler : IHandler<TBody, TResponse>
  {
    try
    {
      var handler = Bootstrap.Container.Value.Resolve<THandler>();

      var input = new HandlerProps<TBody>
        (
          request.Path,
          request.HttpMethod,
          request.RequestContext.ConnectionId,
          request.PathParameters,
          request.QueryStringParameters,
          request.Headers,
          request.Body != null || request.Body == string.Empty ? JsonSerializer.Deserialize<TBody>(JsonDocument.Parse(request.Body)) : null
        );

      var response = await handler.Handle(input);

      return new APIGatewayProxyResponse
      {
        StatusCode = response.StatusCode,
        Body = JsonSerializer.Serialize(response.Body),
        Headers = response.Headers
      };
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new APIGatewayProxyResponse
      {
        StatusCode = 500,
        Body = JsonSerializer.Serialize(new { Message = "Internal Server Error" }),
      };
    }
  }
}