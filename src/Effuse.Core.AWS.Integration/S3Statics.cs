using Effuse.Core.Integration.Contracts;
using Amazon.S3;
using Amazon.S3.Model;
using Effuse.Core.Utilities;

namespace Effuse.Core.AWS.Integration;

public class S3Statics : IStatic
{
  private readonly IAmazonS3 s3;

  public S3Statics(IAmazonS3 s3)
  {
    this.s3 = s3;
  }

  private static string BucketName => Env.GetEnv("BUCKET_NAME");

  public async Task<StaticFile> Download(string name)
  {
    using var response = await this.s3.GetObjectAsync(new GetObjectRequest
    {
      BucketName = BucketName,
      Key = name
    });
    var memory = new MemoryStream();
    await response.ResponseStream.CopyToAsync(memory);

    return new StaticFile
    {
      Name = name,
      Data = memory,
      Mime = response.Headers.ContentType
    };
  }

  public Task Upload(StaticFile file)
  {
    return this.s3.PutObjectAsync(new PutObjectRequest
    {
      BucketName = BucketName,
      Key = file.Name,
      InputStream = file.Data,
      ContentType = file.Mime
    });
  }

  public async Task<StaticTextFile> DownloadText(string name)
  {
    using var response = await this.s3.GetObjectAsync(new GetObjectRequest
    {
      BucketName = BucketName,
      Key = name
    });

    return new StaticTextFile
    {
      Name = name,
      Data = await new StreamReader(response.ResponseStream).ReadToEndAsync(),
      Mime = response.Headers.ContentType
    };
  }

  public Task UploadText(StaticTextFile file)
  {
    var stream = new MemoryStream();
    var writer = new StreamWriter(stream);
    writer.Write(file.Data);
    writer.Flush();
    stream.Position = 0;

    return this.s3.PutObjectAsync(new PutObjectRequest
    {
      BucketName = BucketName,
      Key = file.Name,
      InputStream = stream,
      ContentType = file.Mime
    });
  }
}