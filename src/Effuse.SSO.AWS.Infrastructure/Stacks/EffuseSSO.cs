using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Constructs;
using Effuse.Core.AWS.Infrastructure.Constructs;
using Effuse.Core.AWS.Infrastructure.Policies;
using Effuse.Core.AWS.Infrastructure.Utilities;
using Effuse.Core.Utilities;

namespace Effuse.AWS.Infrastructure.Stacks;

public class EffuseSSO : Stack
{
  internal EffuseSSO(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
  {
    var logGroup = new Logs(this, "log-group", new LogProps
    {
      Name = "api-logs"
    });

    var assetsBucket = new Bucket(this, "assets-bucket", new BucketProps { });

    var secret = new Parameter(this, "jwt-parameter", new()
    {
      Name = "JWT_SECRET",
      Value = File.ReadAllText(Config.ProjectPath("resources/data/signing_key.key"))
    });

    var certificate = new Parameter(this, "jwt-certificate", new()
    {
      Name = "JWT_CERTIFICATE",
      Value = File.ReadAllText(Config.ProjectPath("resources/data/private_key.pem"))
    });

    var encryptionKey = new Parameter(this, "encryption-key", new()
    {
      Name = "ENCRYPTION_PASSPHRASE",
      Value = Env.GetEnv("ENCRYPTION_PASSPHRASE")
    });

    var appEnv = new Dictionary<string, string>()
    {
      ["BUCKET_NAME"] = assetsBucket.BucketName,
      ["APP_PREFIX"] = Config.AppPrefix
    };

    var assembly = Assembly.Load("Effuse.Server.Handlers") ?? throw new Exception("Could not find server assembly");

    _ = new WebApi(this, "core-services", new()
    {
      Description = "The core services API",
      Environment = appEnv,
      Area = "SSO",
      LogGroup = logGroup,
      Policies = new PolicyStatement[]
      {
        new S3Admin(assetsBucket),
        new ParameterReader(secret, certificate, encryptionKey)
      },
      Assembly = assembly
    });
  }
}

