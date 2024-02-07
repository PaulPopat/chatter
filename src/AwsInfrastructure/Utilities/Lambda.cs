using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace Effuse.AWS.Utilities
{
  internal class LambdaBuilderOptions : BundlingOptions
  {
    public LambdaBuilderOptions()
    {
      this.Image = Runtime.DOTNET_6.BundlingImage;
      this.User = "root";
      this.OutputType = BundlingOutput.ARCHIVED;
      this.Command = new string[] {
        "/bin/sh",
        "-c",
        " dotnet tool install -g Amazon.Lambda.Tools"+
        " && dotnet build"+
        " && dotnet lambda package --output-package /asset-output/function.zip"
      };
    }

    internal class LambdaProps
    {
      public string Handler { get; set; }
    }

    internal class Lambda : Function
    {
      public Lambda(Construct scope, string id, LambdaProps props) : base(
        scope,
        id,
        new FunctionProps
        {
          Runtime = Runtime.DOTNET_6,
          Code = Code.FromAsset("./src/AwsHandler"),
          Handler = props.Handler
        })
      {
      }
    }
  }
}