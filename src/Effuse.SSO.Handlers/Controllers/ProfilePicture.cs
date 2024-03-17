using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.SSO.Services;

namespace Effuse.SSO.Handlers.Controllers;

[Route(Method.Get, "/profile/pictures/{userid}")]
public class ProfilePicture(ProfileService profileService) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var userId = props.PathParameters["userid"];

    var picture = await profileService.GetProfilePicture(Guid.Parse(userId));

    return new(200, picture.Data, new Dictionary<string, string>
    {
      ["Content-Type"] = picture.Mime
    });
  }
}
