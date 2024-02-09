using Amazon.SimpleSystemsManagement;
using Effuse.Core.Integration.Contracts;
using Amazon.SimpleSystemsManagement.Model;

namespace Effuse.Core.AWS.Integration;

public class ParameterStore : IParameters
{
  private readonly IAmazonSimpleSystemsManagement ssm;

  public ParameterStore(IAmazonSimpleSystemsManagement ssm)
  {
    this.ssm = ssm;
  }

  private static string AppPrefix => Environment.GetEnvironmentVariable("APP_PREFIX") ?? throw new Exception("An app prefix is required");

  public async Task<string> GetParameter(string name)
  {
    var response = await this.ssm.GetParameterAsync(new GetParameterRequest
    {
      Name = $"{AppPrefix}/{name}"
    });

    if (response.Parameter == null || response.Parameter.Value == string.Empty)
      throw new Exception("Parameter not found");

    return response.Parameter.Value;
  }
}