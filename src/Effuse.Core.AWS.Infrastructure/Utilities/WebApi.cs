using Amazon.CDK.AWS.Apigatewayv2;
using Constructs;
using Amazon.CDK.AwsApigatewayv2Integrations;
using HttpMethod = Amazon.CDK.AWS.Apigatewayv2.HttpMethod;
using Amazon.CDK.AWS.IAM;

namespace Effuse.Core.AWS.Infrastructure.Utilities;

public struct Route
{
  public HttpMethod Method { get; set; }

  public string Path { get; set; }

  public string Area { get; set; }

  public string Handler { get; set; }
}

public struct WebApiProps
{
  public string Description { get; set; }

  public IEnumerable<Route> Routes { get; set; }

  public IDictionary<string, string> Environment { get; set; }
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
    foreach (var route in props.Routes)
    {
      var lambda = new Lambda(this, route.Method.ToString() + route.Path + "_handler", new LambdaProps
      {
        Handler = route.Handler,
        Area = route.Area,
        Environment = props.Environment,
      });

      this.AddRoutes(new AddRoutesOptions
      {
        Path = route.Path,
        Methods = new HttpMethod[] { route.Method },
        Integration = new HttpLambdaIntegration(route.Path, lambda)
      });

      this.lambdas.Add(lambda);
    }
  }

  public string DomainName => $"{this.ApiId}.execute-api.{Config.AWSRegion}.amazonaws.com";

  public override string Url => $"https://{this.DomainName}";

  public void AddToPrincipalPolicy(PolicyStatementProps statement)
  {
    foreach (var lambda in this.lambdas)
      lambda.Role?.AddToPrincipalPolicy(new PolicyStatement(statement));
  }
}