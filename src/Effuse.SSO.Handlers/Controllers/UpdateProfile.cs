using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Utilities;
using Effuse.SSO.Handlers.Models.Profile;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Put, "/api/v1/user/profile")]
public class UpdateProfile : IHandler
{
  private readonly ProfileService profileService;
  private readonly AuthService authService;

  public UpdateProfile(ProfileService profileService, AuthService authService)
  {
    this.profileService = profileService;
    this.authService = authService;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);
    var userId = await this.authService.Verify(token, UserAccess.Admin);
    var body = props.Body<ProfileForm>();
    var imageData = new MemoryStream(Convert.FromBase64String(body.Picture.Base64Data));

    var user = await this.profileService.UpdateProfile(
      userId,
      body.UserName,
      body.Biography,
      imageData,
      body.Picture.MimeType);

    return new(200, new ProfileResponse()
    {
      UserId = user.UserId.ToString(),
      UserName = user.UserName,
      Biography = user.Biography,
      RegisteredAt = user.RegisteredAt.ToISOString(),
      LastSignIn = user.LastSignIn.ToISOString(),
      Servers = user.Servers
        .Select(s => new ProfileResponseServer
        {
          Url = s.Url,
          JoinedAt = s.JoinedAt.ToISOString()
        })
        .ToArray()
    });
  }
}
