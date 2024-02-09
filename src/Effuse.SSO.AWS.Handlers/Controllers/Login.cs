using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;
using Effuse.SSO.AWS.Handlers.Utilities;
using Base = Effuse.SSO.Handlers.Controllers;
using Effuse.SSO.Handlers.Models.Login;

namespace Effuse.SSO.AWS.Handlers.Controllers;

public class Login
{
  public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request)
  {
    return await AWSHandlerMapper.Process<object, LoginResponse, Base.Login>(request);
  }
}