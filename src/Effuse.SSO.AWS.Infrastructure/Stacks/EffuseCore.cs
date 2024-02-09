using Amazon.CDK;
using Amazon.CDK.AWS.Apigatewayv2;
using Constructs;
using Effuse.Core.AWS.Infrastructure.Utilities;

namespace Effuse.AWS.Infrastructure.Stacks;

public class EffuseCore : Stack
{
  internal EffuseCore(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
  {
    _ = new Lambda(this, "inviter", new LambdaProps
    {
      Handler = "Invite",
      Area = "SSO"
    });

    _ = new WebApi(this, "core-services", new WebApiProps
    {
      Description = "The core services API",
      Routes = new Route[] {
        new() {
          Method = HttpMethod.GET,
          Path = "/api/v1/heartbeat",
          Handler = "HeartBeat",
          Area = "SSO"
        },
        new() {
          Method = HttpMethod.POST,
          Path = "/api/v1/users",
          Handler = "Register",
          Area = "SSO"
        },
        new() {
          Method = HttpMethod.GET,
          Path = "/api/v1/auth/token",
          Handler = "Login",
          Area = "SSO"
        }
      }
    });
  }
}

