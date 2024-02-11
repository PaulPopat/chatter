using Amazon.SimpleSystemsManagement;
using Effuse.Core.Integration.Contracts;
using Amazon.SimpleSystemsManagement.Model;
using Effuse.Core.Utilities;

namespace Effuse.Core.AWS.Integration;

public class ParameterStore : IParameters
{
  private readonly IAmazonSimpleSystemsManagement ssm;

  public ParameterStore(IAmazonSimpleSystemsManagement ssm)
  {
    this.ssm = ssm;
  }

  private static string AppPrefix => Env.GetEnv("APP_PREFIX");

  public async Task<string> GetParameter(string name)
  {
    var response = await this.ssm.GetParameterAsync(new GetParameterRequest
    {
      Name = $"/{AppPrefix}/{name}"
    });

    if (response.Parameter == null || response.Parameter.Value == string.Empty)
      throw new Exception("Parameter not found");

    return response.Parameter.Value;
  }
}