using System.Diagnostics;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace Effuse.Core.AWS.Infrastructure.Utilities;

public struct LambdaProps
{
  public string Area { get; set; }

  public string Handler { get; set; }

  public IDictionary<string, string> Environment { get; set; }
}

public class Lambda : Function
{
  public Lambda(Construct scope, string id, LambdaProps props) : base(
    scope,
    id,
    new FunctionProps
    {
      Runtime = Runtime.DOTNET_6,
      Code = Code.FromAsset(Config.ProjectPath($"src/{Config.HandlersProject(props.Area)}/bin/Release/net6.0/{Config.HandlersProject(props.Area)}.zip")),
      Handler = $"{Config.HandlersProject(props.Area)}::Effuse.{props.Area}.AWS.Handlers.Controllers.Operate::{props.Handler}",
      Environment = props.Environment,
      Timeout = Duration.Seconds(30)
    })
  {
  }
}
