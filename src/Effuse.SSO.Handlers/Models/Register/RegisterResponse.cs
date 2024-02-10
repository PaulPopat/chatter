namespace Effuse.SSO.Handlers.Models.Register;

public struct RegisterResponse
{
  public string AdminToken { get; set; }

  public string ServerToken { get; set; }

  public string UserId { get; set; }
}