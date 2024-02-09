

using System.Security.Cryptography.X509Certificates;
using System.Text;
using Effuse.Core.Integration.Contracts;
using Effuse.SSO.Integration.Utilities;
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
      new X509Certificate2(Encoding.ASCII.GetBytes(await this.parameters.GetParameter("JWT_SECRET"))));
  }

  public async Task<string> CreateJwt(string value, DateTime expires)
  {
    return JwtBuilder.Create()
      .WithAlgorithm(new RS4096Algorithm(await this.certificate.Value))
      .AddClaim("exp", expires.ToISOString())
      .AddClaim("data", value)
      .Encode();
  }

  public async Task<string> DecodeJwt(string jwt)
  {
    var json = JwtBuilder.Create()
      .WithAlgorithm(new RS4096Algorithm(await this.certificate.Value))
      .MustVerifySignature()
      .Decode<IDictionary<string, object>>(jwt);

    var data = json["data"];
    if (data is not string stringData || stringData == string.Empty)
      throw new Exception("Could not find JWT data");

    return stringData;
  }
}