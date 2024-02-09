
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;
using Effuse.SSO.AWS.Handlers.Utilities;
using Base = Effuse.SSO.Handlers.Controllers;
using Effuse.SSO.Handlers.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Effuse.SSO.AWS.Handlers.Controllers;

public class HeartBeat
{
  public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request)
  {
    return await AWSHandlerMapper.Process<object, TestHandlerResponse, Base.HeartBeat>(request);
  }
}