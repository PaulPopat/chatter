using Amazon.CDK;
using Amazon.CDK.AWS.Apigatewayv2;
using Constructs;
using Effuse.AWS.Infrastructure.Utilities;

namespace Effuse.AWS.Infrastructure.Stacks;

public class EffuseCore : Stack
{
  internal EffuseCore(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
  {
    new WebApi(this, "core-services", new WebApiProps
    {
      Description = "The core services API",
      Routes = new Route[] {
        new Route() {
          Method = HttpMethod.GET,
          Path = "/api/v1/heartbeat",
          Handler = "HeartBeat"
        }
      }
    });
  }
}
