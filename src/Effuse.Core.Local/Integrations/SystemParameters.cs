using Effuse.Core.Integration.Contracts;
using Effuse.Core.Utilities;

namespace Effuse.Core.Local.Integrations;

public class SystemParameters : IParameters
{
  private static string BaseDir => Env.GetEnv("DATA_DIR");

  private static string Location(string name)
  {
    return Path.Combine(BaseDir, name);
  }

  public async Task<string> GetParameter(ParameterName name)
  {
    return name switch
    {
      ParameterName.JWT_CERTIFICATE => await File.ReadAllTextAsync(Location("private_key.pem")),
      ParameterName.JWT_SECRET => await File.ReadAllTextAsync(Location("signing_key.key")),
      _ => Env.GetEnv(Enum.GetName(name) ?? throw new Exception("Could not get enum name")),
    };
  }
}
