
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Effuse.AWS.Integrations.Handlers;

public class HandlerResponse
{
  public string Text { get; set; }
}

public class TestHandler
{
  public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request)
  {
    return new APIGatewayProxyResponse
    {
      StatusCode = 200,
      Body = JsonSerializer.Serialize(new HandlerResponse { Text = "Hello world" })
    };
  }
}