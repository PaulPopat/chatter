namespace Effuse.Server.Integrations;

public interface ISsoClient
{
  Task<Guid> GetUserId(string token);
}
