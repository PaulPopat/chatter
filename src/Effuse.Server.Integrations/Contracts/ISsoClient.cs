namespace Effuse.Server.Integrations.Contracts;

public interface ISsoClient
{
  Task<Guid> GetUserId(string token);
}
