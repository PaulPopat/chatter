namespace Effuse.SSO.Handlers.Models.Login;

public struct LoginResponse
{
  public string AdminToken { get; set; }

  public string ServerToken { get; set; }

  public string UserId { get; set; }
}