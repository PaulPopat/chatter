namespace Effuse.SSO.Handlers.Models.Register;

public class RegisterForm
{
  public string UserName { get; set; } = "";

  public string Email { get; set; } = "";

  public string Password { get; set; } = "";

  public string InviteToken { get; set; } = "";
}