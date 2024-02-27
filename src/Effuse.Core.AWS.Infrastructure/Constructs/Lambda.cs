using Effuse.Core.Utilities;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Amazon.CDK.AWS.Logs;
using Effuse.Core.AWS.Infrastructure.Utilities;
using Amazon.CDK.AWS.IAM;

namespace Effuse.Core.AWS.Infrastructure.Constructs;

public struct LambdaProps
{
  public string Area { get; set; }

  public string Handler { get; set; }

  public IDictionary<string, string> Environment { get; set; }

  public ILogGroup LogGroup { get; set; }

  public PolicyStatement[]? Policies {get;set; }


}

public class Lambda : Function
{
  public Lambda(Construct scope, string id, LambdaProps props) : base(
    scope,
    id,
    new FunctionProps
    {
      Runtime = Runtime.DOTNET_6,
      Code = Code.FromAsset(Config.ProjectPath($"src/{Config.HandlersProject(props.Area)}/bin/Release/net8.0/{Config.HandlersProject(props.Area)}.zip")),
      Handler = $"{Config.HandlersProject(props.Area)}::Effuse.{props.Area}.AWS.Handlers.Controllers.Operate::Handle",
      Environment = props.Environment.WithKeyValue("HANDLER_NAME", props.Handler),
      Timeout = Duration.Seconds(30),
      LogGroup = props.LogGroup
    })
  {
    foreach (var policy in props.Policies ?? Array.Empty<PolicyStatement>())
    {
      this.Role?.AddToPrincipalPolicy(policy);
    }
  }
}
