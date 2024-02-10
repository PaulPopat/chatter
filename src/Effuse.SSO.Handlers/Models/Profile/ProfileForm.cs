namespace Effuse.SSO.Handlers.Models.Profile;

public struct ProfileFormPicture
{
  public string Base64Data { get; set; }

  public string MimeType { get; set; }
}

public struct ProfileForm
{
  public string UserName { get; set; }

  public string Biography { get; set; }

  public ProfileFormPicture Picture { get; set; }
}