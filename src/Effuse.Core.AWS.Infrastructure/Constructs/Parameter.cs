using Amazon.CDK.AWS.SSM;
using Constructs;
using Effuse.Core.AWS.Infrastructure.Utilities;

namespace Effuse.Core.AWS.Infrastructure.Constructs;

public struct ParameterProps
{
  public string Name { get; set; }

  public string Value { get; set; }
}

public class Parameter : StringParameter
{
  public Parameter(Construct scope, string id, ParameterProps props) : base(
    scope,
    id,
    new StringParameterProps
    {
      ParameterName = $"/{Config.AppPrefix}/{props.Name}",
      StringValue = props.Value
    }
  )
  {

  }
}