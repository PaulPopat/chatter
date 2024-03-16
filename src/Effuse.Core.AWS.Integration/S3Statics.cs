using Effuse.Core.Integration.Contracts;
using Amazon.S3;
using Amazon.S3.Model;
using Effuse.Core.Utilities;

namespace Effuse.Core.AWS.Integration;

public class S3Statics(IAmazonS3 s3) : IStatic
{
  private static string BucketName => Env.GetEnv("BUCKET_NAME");

  public async Task<StaticFile> Download(string name)
  {
    using var response = await s3.GetObjectAsync(new GetObjectRequest
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
    return s3.PutObjectAsync(new PutObjectRequest
    {
      BucketName = BucketName,
      Key = file.Name,
      InputStream = file.Data,
      ContentType = file.Mime
    });
  }

  public async Task<StaticTextFile> DownloadText(string name)
  {
    using var response = await s3.GetObjectAsync(new GetObjectRequest
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

    return s3.PutObjectAsync(new PutObjectRequest
    {
      BucketName = BucketName,
      Key = file.Name,
      InputStream = stream,
      ContentType = file.Mime
    });
  }

  public Task Delete(string name)
  {
    return s3.DeleteObjectAsync(new DeleteObjectRequest
    {
      BucketName = BucketName,
      Key = name
    });
  }

  public async Task<bool> Exists(string name)
  {
    try
    {
      using var response = await s3.GetObjectAsync(new GetObjectRequest
      {
        BucketName = BucketName,
        Key = name
      });

      return response.ContentLength > 0;
    }
    catch
    {
      return false;
    }
  }
}