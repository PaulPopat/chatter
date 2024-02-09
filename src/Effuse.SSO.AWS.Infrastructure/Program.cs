using System;
using System.IO;
using Amazon.CDK;

namespace Effuse.AWS.Infrastructure;

sealed class Program
{
  public static void Main(string[] args)
  {
    Console.WriteLine("Starting app in " + Directory.GetCurrentDirectory());
    var app = new App();
    new Stacks.EffuseCore(app, "core-stack", new StackProps
    {
    });
    app.Synth();
  }
}

