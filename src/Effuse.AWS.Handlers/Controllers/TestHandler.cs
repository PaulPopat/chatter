
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;
using Effuse.AWS.Handlers.Utilities;
using Effuse.Handlers.Models;
using Base = Effuse.Handlers.Controllers;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Effuse.AWS.Handlers.Controllers;

public class TestHandler
{
  public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request)
  {
    return await AWSHandlerMapper.Process<object, TestHandlerResponse, Base.TestHandler>(request);
  }
}