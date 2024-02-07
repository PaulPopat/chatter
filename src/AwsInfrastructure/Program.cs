using Amazon.CDK;

namespace Effuse.AWS.CDK;

sealed class Program
{
  public static void Main(string[] args)
  {
    var app = new App();
    new Stacks.EffuseCore(app, "core-stack", new StackProps
    {
    });
    app.Synth();
  }
}

