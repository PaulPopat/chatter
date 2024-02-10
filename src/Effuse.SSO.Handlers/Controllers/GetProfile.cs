using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Utilities;
using Effuse.SSO.Handlers.Models.Profile;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;


public class GetProfile : IHandler<object, ProfileResponse>
{
  private readonly ProfileService profileService;
  private readonly AuthService authService;

  public GetProfile(ProfileService profileService, AuthService authService)
  {
    this.profileService = profileService;
    this.authService = authService;
  }

  public async Task<HandlerResponse<ProfileResponse>> Handle(HandlerProps<object> props)
  {
    var token = props.AuthToken;
    if (token == null) return new(403);
    var userId = await this.authService.Verify(token, UserAccess.Admin);

    var user = await this.profileService.GetUser(userId);

    return new(201, new ProfileResponse()
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
