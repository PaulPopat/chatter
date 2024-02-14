using System.Collections.Generic;
using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.Apigatewayv2;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AwsApigatewayv2Authorizers;
using Constructs;
using Effuse.Core.AWS.Infrastructure.Constructs;
using Effuse.Core.AWS.Infrastructure.Policies;
using Effuse.Core.AWS.Infrastructure.Utilities;

namespace Effuse.AWS.Infrastructure.Stacks;

public class EffuseSSO : Stack
{
  internal EffuseSSO(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
  {
    var logGroup = new Logs(this, "log-group", new LogProps
    {
      Name = "api-logs"
    });

    var usersTable = new Table(this, "users", new TableProps
    {
      PartitionKey = new Attribute
      {
        Name = "UserId",
        Type = AttributeType.STRING
      }
    });

    var emailIndexName = "EmailIndex";

    usersTable.AddGlobalSecondaryIndex(new GlobalSecondaryIndexProps
    {
      IndexName = emailIndexName,
      PartitionKey = new Attribute
      {
        Name = "Email",
        Type = AttributeType.STRING
      }
    });

    var assetsBucket = new Bucket(this, "assets-bucket", new BucketProps { });

    var secret = new Parameter(this, "jwt-parameter", new()
    {
      Name = "JWT_SECRET",
      Value = File.ReadAllText(Config.ProjectPath("resources/signing_key.key"))
    });

    var certificate = new Parameter(this, "jwt-certificate", new()
    {
      Name = "JWT_CERTIFICATE",
      Value = File.ReadAllText(Config.ProjectPath("resources/private_key.pem"))
    });

    var appEnv = new Dictionary<string, string>()
    {
      ["BUCKET_NAME"] = assetsBucket.BucketName,
      ["USER_TABLE_NAME"] = usersTable.TableName,
      ["USER_TABLE_EMAIL_INDEX"] = emailIndexName,
      ["APP_PREFIX"] = Config.AppPrefix
    };

    _ = new WebApi(this, "core-services", new()
    {
      Description = "The core services API",
      Environment = appEnv,
      Area = "SSO",
      LogGroup = logGroup,
      Policies = new PolicyStatement[]
      {
        new DynamoDBAdmin(usersTable),
        new S3Reader(assetsBucket),
        new ParameterReader(secret, certificate)
      },
      Routes = new Route[] {
        new() {
          Method = HttpMethod.GET,
          Path = "/api/v1/heartbeat",
          Handler = "HeartBeat"
        },
        new() {
          Method = HttpMethod.POST,
          Path = "/api/v1/users",
          Handler = "Register"
        },
        new() {
          Method = HttpMethod.GET,
          Path = "/api/v1/users/{userId}/profile",
          Handler = "GetPublicProfile"
        },
        new() {
          Method = HttpMethod.GET,
          Path = "/api/v1/user/profile",
          Handler = "GetProfile"
        },
        new() {
          Method = HttpMethod.PUT,
          Path = "/api/v1/user/profile",
          Handler = "UpdateProfile"
        },
        new() {
          Method = HttpMethod.POST,
          Path = "/api/v1/user/servers",
          Handler = "JoinServer"
        },
        new() {
          Method = HttpMethod.GET,
          Path = "/api/v1/auth/token",
          Handler = "Login",
        },
        new() {
          Method = HttpMethod.GET,
          Path = "/api/v1/auth/invite",
          Handler = "Invite",
          Authorizer = new HttpIamAuthorizer()
        }
      }
    });
  }
}

