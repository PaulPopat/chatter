
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;
using Effuse.SSO.AWS.Handlers.Utilities;
using Base = Effuse.SSO.Handlers.Controllers;
using Effuse.SSO.Handlers.Models;
using Effuse.SSO.Handlers.Models.Register;

namespace Effuse.SSO.AWS.Handlers.Controllers;

public class Register
{
  public async Task<APIGatewayProxyResponse> Handler(APIGatewayProxyRequest request)
  {
    return await AWSHandlerMapper.Process<RegisterForm, RegisterResponse, Base.Register>(request);
  }
}