using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers.Admin;

[Route(Method.Get, "/api/v1/users")]
public class ListUsers(AdminService admin) : IHandler
{
  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var token = props.AuthToken;
    if (token == null || token == string.Empty)
      return new(403, new { Message = "Error" });

    var data = await admin.GetAllUsers(token);

    return new(200, data.Select(
      u =>
        new
        {
          UserId = u.UserId.ToString(),
          u.Admin,
          u.Banned
        }));
  }
}
