using System.Security.Cryptography.X509Certificates;
using Effuse.Core.Integration.Contracts;
using JWT.Algorithms;
using JWT.Builder;

namespace Effuse.SSO.Integration.Clients.Jwt;

public class ParameterJwtClient : IJwtClient
{
  private readonly IParameters parameters;

  private readonly Lazy<Task<X509Certificate2>> certificate;

  public ParameterJwtClient(IParameters parameters)
  {
    this.parameters = parameters;

    this.certificate = new(async () =>
    {
      var cert = await this.parameters.GetParameter("JWT_CERTIFICATE");
      var key = await this.parameters.GetParameter("JWT_SECRET");
      return X509Certificate2.CreateFromPem(cert, key);
    });
  }

  private struct Payload<TData>
  {
    public long exp { get; set; }

    public TData data { get; set; }
  }

  private static long SecondsSinceEpoch(TimeSpan duration)
  {
    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    return Convert.ToInt64((DateTime.Now.ToUniversalTime() - epoch).TotalSeconds) + (long)duration.TotalSeconds;
  }

  public async Task<string> CreateJwt<TData>(TData payload, TimeSpan expires)
  {
    return JwtBuilder.Create()
      .WithAlgorithm(new RS4096Algorithm(await this.certificate.Value))
      .Encode(new Payload<TData>
      {
        exp = SecondsSinceEpoch(expires),
        data = payload
      });
  }

  public async Task<TData> DecodeJwt<TData>(string jwt)
  {
    var result = JwtBuilder.Create()
      .WithAlgorithm(new RS4096Algorithm(await this.certificate.Value))
      .MustVerifySignature()
      .Decode<Payload<TData>>(jwt);

    return result.data;
  }
}