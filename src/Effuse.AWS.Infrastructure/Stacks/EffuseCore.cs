using Amazon.CDK;
using Constructs;
using Effuse.AWS.Infrastructure.Utilities;

namespace Effuse.AWS.Infrastructure.Stacks;

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

