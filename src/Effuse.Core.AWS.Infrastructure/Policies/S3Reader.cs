using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;

namespace Effuse.Core.AWS.Infrastructure.Policies;

public class S3Reader : PolicyStatement
{
  public S3Reader(Bucket bucket) : base(new PolicyStatementProps()
  {
    Effect = Effect.ALLOW,
    Actions = new string[] { "s3:GetObject", "s3:ListBucket" },
    Resources = new string[] { bucket.BucketArn, bucket.BucketArn + "/*"  }
  })
  { }
}