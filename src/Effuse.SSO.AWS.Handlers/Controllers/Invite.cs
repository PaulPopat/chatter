
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;
using Effuse.SSO.AWS.Handlers.Utilities;
using Base = Effuse.SSO.Handlers.Controllers;
using Effuse.SSO.Handlers.Models;
using Unity;
using Effuse.SSO.Services;

namespace Effuse.SSO.AWS.Handlers.Controllers;

public class InviteProps
{
  public string Email { get; set; }
}

public class Invite
{
  public async Task<APIGatewayProxyResponse> Handler(InviteProps props)
  {
    var service = Bootstrap.Container.Value.Resolve<AuthService>();

    return new APIGatewayProxyResponse
    {
      StatusCode = 200,
      Body = await service.CreateInvite(props.Email)
    };
  }
}