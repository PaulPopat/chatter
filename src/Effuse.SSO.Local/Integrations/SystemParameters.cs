using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;

namespace Effuse.SSO.Local.Integrations;

public class SystemParameters : IParameters
{
  public Task<string> GetParameter(string name)
  {
    return Task.FromResult(Env.GetEnv(name));
  }
}
