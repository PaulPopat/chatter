using Amazon.CDK.AWS.Logs;
using Constructs;
using Effuse.Core.AWS.Infrastructure.Utilities;

namespace Effuse.Core.AWS.Infrastructure.Constructs;

public struct LogProps
{
  public string Name { get; set; }
}

public class Logs : LogGroup
{
  public Logs(Construct scope, string id, LogProps props): base(scope, id, new LogGroupProps
  {
    LogGroupName = $"/{Config.AppPrefix}/{props.Name}",
    Retention = RetentionDays.ONE_DAY
  })
  {
    
  }
}
