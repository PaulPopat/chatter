namespace Effuse.SSO.Handlers.Models.Profile;

public struct ProfileResponseServer
{
  public string Url { get; set; }

  public string JoinedAt { get; set; }
}

public struct ProfileResponse
{
  public string UserId { get; set; }

  public string UserName { get; set; }

  public string Biography { get; set; }

  public string RegisteredAt { get; set; }

  public string LastSignIn { get; set; }

  public ProfileResponseServer[] Servers { get; set; }
}