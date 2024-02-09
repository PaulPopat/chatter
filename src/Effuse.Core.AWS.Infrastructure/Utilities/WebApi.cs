using Amazon.CDK.AWS.Apigatewayv2;
using Constructs;
using Amazon.CDK.AwsApigatewayv2Integrations;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;

namespace Effuse.Core.AWS.Infrastructure.Utilities;

public class Route
{
  public HttpMethod Method { get; set; }

  public string Path { get; set; }

  public string Area { get; set; }

  public string Handler { get; set; }
}

public class WebApiProps
{
  public string Description { get; set; }

  public IEnumerable<Route> Routes { get; set; }
}

public class WebApi : HttpApi
{

  public WebApi(Construct scope, string id, WebApiProps props) : base(
    scope,
    id,
    new HttpApiProps
    {
      Description = props.Description,
    })
  {
    foreach (var route in props.Routes)
    {
      this.AddRoutes(new AddRoutesOptions
      {
        Path = route.Path,
        Methods = new HttpMethod[] { route.Method },
        Integration = new HttpLambdaIntegration(
          route.Path,
          new Lambda(this, route.Path + "_handler", new LambdaProps
          {
            Handler = route.Handler,
            Area = route.Area
          }))
      });
    }
  }

  public string DomainName => $"{this.ApiId}.execute-api.{Config.AWSRegion}.amazonaws.com";

  public override string Url => $"https://{this.DomainName}";
}