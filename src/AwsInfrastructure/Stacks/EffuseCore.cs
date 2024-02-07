using Amazon.CDK;
using Constructs;
using Effuse.AWS.CDK.Utilities;

namespace Effuse.AWS.CDK.Stacks;

public class EffuseCore : Stack
{
  internal EffuseCore(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
  {
    new Lambda(this, "test-lambda", new LambdaProps
    {
      Handler = "TestHandler"
    });
  }
}

