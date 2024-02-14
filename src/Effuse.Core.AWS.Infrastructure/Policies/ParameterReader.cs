using Amazon.CDK.AWS.IAM;
using Effuse.Core.AWS.Infrastructure.Constructs;

namespace Effuse.Core.AWS.Infrastructure.Policies;

public class ParameterReader : PolicyStatement
{
  public ParameterReader(params Parameter[] parameters) : base(new PolicyStatementProps()
  {
    Effect = Effect.ALLOW,
    Actions = new string[] { "ssm:GetParameter" },
    Resources = parameters.Select(p => p.ParameterArn).ToArray()
  })
  { }
}
