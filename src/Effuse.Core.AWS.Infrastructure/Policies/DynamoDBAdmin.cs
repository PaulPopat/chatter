using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.IAM;

namespace Effuse.Core.AWS.Infrastructure.Policies;

public class DynamoDBAdmin : PolicyStatement
{
  public DynamoDBAdmin(Table table) : base(new PolicyStatementProps
  {
    Effect = Effect.ALLOW,
    Actions = new string[] { "dynamodb:*" },
    Resources = new string[] { table.TableArn, table.TableArn + "/*" }
  })
  { }
}
