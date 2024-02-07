using Amazon.CDK;

namespace Effuse
{
  sealed class Program
  {
    public static void Main(string[] args)
    {
      var app = new App();
      new EffuseCoreStack(app, "core-stack", new StackProps
      {
      });
      app.Synth();
    }
  }
}
