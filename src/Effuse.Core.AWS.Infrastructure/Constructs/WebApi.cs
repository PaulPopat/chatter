using Amazon.CDK.AWS.Apigatewayv2;
using Constructs;
using Amazon.CDK.AwsApigatewayv2Integrations;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Logs;
using Effuse.Core.AWS.Infrastructure.Utilities;
using System.Reflection;
using Effuse.Core.Handlers;

namespace Effuse.Core.AWS.Infrastructure.Constructs;

public struct Route
{
  public HttpMethod Method { get; set; }

  public string Path { get; set; }

  public Type Handler { get; set; }

  public static List<Route> FromAssembly(Assembly assembly)
  {
    return RouteInstance.FromAssembly(assembly)
      .Select(route =>
      {
        var method = Enum.GetName(route.Method) ?? throw new Exception("No method");

        return new Route
        {
          Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), method),
          Path = route.Endpoint,
          Handler = route.Type
        };
      })
      .ToList();
  }
}

public struct WebApiProps
{
  public string Description { get; set; }

  public Assembly Assembly { get; set; }

  public string Area { get; set; }

  public IDictionary<string, string> Environment { get; set; }

  public ILogGroup LogGroup { get; set; }

  public PolicyStatement[]? Policies { get; set; }
}

public class WebApi : HttpApi
{
  private readonly List<Lambda> lambdas = new List<Lambda>();

  public WebApi(Construct scope, string id, WebApiProps props) : base(
    scope,
    id,
    new HttpApiProps
    {
      Description = props.Description,
    })
  {
    foreach (var route in Route.FromAssembly(props.Assembly))
    {
      var lambda = new Lambda(this, route.Method.ToString() + route.Path + "_handler", new LambdaProps
      {
        Handler = route.Handler,
        Environment = props.Environment,
        LogGroup = props.LogGroup,
        Policies = props.Policies,
        Area = props.Area,
        Assembly = props.Assembly
      });

      this.AddRoutes(new AddRoutesOptions
      {
        Path = route.Path,
        Methods = [route.Method],
        Integration = new HttpLambdaIntegration(route.Path, lambda),
      });

      this.lambdas.Add(lambda);
    }
  }

  public string DomainName => $"{this.ApiId}.execute-api.{Config.AWSRegion}.amazonaws.com";

  public override string Url => $"https://{this.DomainName}";
}