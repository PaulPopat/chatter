using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Handlers.Models.PublicProfile;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;


public class GetPublicProfile : IHandler<object, PublicProfileResponse>
{
  private readonly ProfileService profileService;

  public GetPublicProfile(ProfileService profileService)
  {
    this.profileService = profileService;
  }

  public async Task<HandlerResponse<PublicProfileResponse>> Handle(HandlerProps<object> props)
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
