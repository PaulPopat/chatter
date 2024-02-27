using Amazon.Lambda.APIGatewayEvents;
using Unity;
using Effuse.Core.Handlers.Contracts;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Effuse.Core.Utilities;
using System.Reflection;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Effuse.Core.AWS.Handlers;

public abstract class Operate
{
  protected abstract UnityContainer GetAppContainer();

  private async Task<APIGatewayProxyResponse> Process
    (
      APIGatewayProxyRequest request,
      Type type
    )
  {
    var container = this.GetAppContainer();
    try
    {
      var handler = container.Resolve(type);

      var input = new HandlerProps
        (
          request.Path,
          request.HttpMethod,
          request.RequestContext.ConnectionId,
          request.PathParameters,
          request.QueryStringParameters.ToLowerCaseKeys(),
          request.Headers,
          request.Body
        );

      var response = await ((IHandler)handler).Handle(input);

      return new APIGatewayProxyResponse
      {
        StatusCode = response.StatusCode,
        Body = response.Body != null ? JsonSerializer.Serialize(response.Body) : string.Empty,
        Headers = response.Headers.WithKeyValue("Content-Type", "application/json")
      };
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new APIGatewayProxyResponse
      {
        StatusCode = 500,
        Body = JsonSerializer.Serialize(new { Message = "Internal Server Error" }),
        Headers = new Dictionary<string, string>()
        {
          ["Content-Type"] = "application/json"
        }
      };
    }
  }

  private static Type GetHandlerType()
  {
    var asm = Assembly.Load(Env.GetEnv("ASSEMBLY_NAME"));
    return asm.GetType(Env.GetEnv("HANDLER_NAME")) ?? throw new Exception("Could not find handler");
  }

  public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request)
  {
    return await Process(request, GetHandlerType());
  }
}