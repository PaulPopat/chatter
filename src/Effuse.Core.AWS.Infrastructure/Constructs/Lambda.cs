using Effuse.Core.Utilities;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Amazon.CDK.AWS.Logs;
using Effuse.Core.AWS.Infrastructure.Utilities;
using Amazon.CDK.AWS.IAM;
using System.Reflection;

namespace Effuse.Core.AWS.Infrastructure.Constructs;

public struct LambdaProps
{
  public Assembly Assembly { get; set; }

  public Type Handler { get; set; }

  public IDictionary<string, string> Environment { get; set; }

  public ILogGroup LogGroup { get; set; }

  public string Area { get; set; }

  public PolicyStatement[]? Policies { get; set; }
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
      Handler = $"Effuse.{props.Area}.AWS.Handlers::Effuse.{props.Area}.AWS.Handlers.Operate::Handle",
      Environment = props.Environment
        .WithKeyValue("HANDLER_NAME", props.Handler.FullName ?? throw new Exception("Could not find type name"))
        .WithKeyValue("ASSEMBLY_NAME", props.Assembly.FullName ?? throw new Exception("Could not find assembly name")),
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
