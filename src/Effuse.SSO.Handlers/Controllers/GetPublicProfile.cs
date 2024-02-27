using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.PublicProfile;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Get, "/api/v1/users/{userId}/profile")]
public class GetPublicProfile : IHandler
{
  private readonly ProfileService profileService;

  public GetPublicProfile(ProfileService profileService)
  {
    this.profileService = profileService;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var userId = Guid.Parse(props.PathParameters["userId"]);
    var user = await this.profileService.GetUser(userId);

    return new(200, new PublicProfileResponse()
    {
      UserId = user.UserId.ToString(),
      UserName = user.UserName,
      Biography = user.Biography,
    });
  }
}
