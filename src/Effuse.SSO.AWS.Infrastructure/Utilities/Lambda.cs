using System;
using System.Diagnostics;
using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace Effuse.AWS.Infrastructure.Utilities;

class LocalBuilder : ILocalBundling
{
  private bool run_command(string command, string working_directory)
  {

    var start_info = new ProcessStartInfo()
    {
      FileName = "/bin/bash",
      Arguments = command,
      WorkingDirectory = working_directory,
      UseShellExecute = false,
      RedirectStandardError = true,
      RedirectStandardInput = true,
      RedirectStandardOutput = true
    };

    using (var proc = Process.Start(start_info))
    {
      Console.WriteLine("Attempting to run " + command);
      Console.WriteLine(proc.StandardOutput.ReadToEnd());
      Console.WriteLine(proc.StandardError.ReadToEnd());

      proc.WaitForExit();

      Console.WriteLine($"The exit code was {proc.ExitCode}.");
      return proc.ExitCode == 0;
    }
  }

  public bool TryBundle(string outputDir, IBundlingOptions options)
  {
    try
    {
      return
        this.run_command(
          "dotnet build",
          Path.Combine(Directory.GetCurrentDirectory(), "src")
        ) &&
        this.run_command(
          $"dotnet lambda package --output-package {outputDir}/function.zip",
          Path.Combine(Directory.GetCurrentDirectory(), $"src/{Config.HandlersProject}")
        );
    }
    catch
    {
      return false;
    }
  }
}

class LambdaBuilderOptions : BundlingOptions
{
  public LambdaBuilderOptions()
  {
    this.Image = Runtime.DOTNET_6.BundlingImage;
    this.User = "root";
    this.OutputType = BundlingOutput.ARCHIVED;
    this.Command = new string[] {
        "/bin/sh",
        "-c",
        " dotnet tool install -g Amazon.Lambda.Tools && dotnet build && dotnet lambda package --output-package /asset-output/function.zip"
      };
    this.WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"src/{Config.HandlersProject}");

    this.Local = new LocalBuilder();
  }
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
      Code = Code.FromAsset(Path.Combine(Directory.GetCurrentDirectory(), $"src/{Config.HandlersProject}/bin/Release/net6.0/{Config.HandlersProject}.zip")),
      Handler = $"{Config.HandlersProject}::Effuse.AWS.Handlers.Controllers.{props.Handler}::Handler"
    })
  {
  }
}
