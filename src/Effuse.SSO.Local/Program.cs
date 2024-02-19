using Effuse.Core.Local;
using Effuse.SSO.Handlers.Controllers;
using Unity;

namespace Effuse.SSO.Local;

class HttpServer
{
  public static void Main(string[] args)
  {
    var container = new UnityContainer();

    // // AWS Integrations
    container.RegisterType<Effuse.Core.Integration.Contracts.IDatabase, Effuse.Core.Integration.Implementations.StaticDatabase>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IParameters, Effuse.Core.Local.Integrations.SystemParameters>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IStatic, Effuse.Core.Local.Integrations.DiskStatics>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IEncryption, Effuse.Core.Integration.Implementations.Encryption>();

    // Generic Integrations
    container.RegisterType<Effuse.SSO.Integration.Clients.User.IUserClient, Effuse.SSO.Integration.Clients.User.DbUserClient>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IJwtClient, Effuse.Core.Integration.Implementations.ParameterJwtClient>();

    // Services
    container.RegisterType<Effuse.SSO.Services.AuthService>();
    container.RegisterType<Effuse.SSO.Services.ProfileService>();
    container.RegisterType<Effuse.SSO.Services.ServersService>();

    // Handlers
    container.RegisterType<Effuse.SSO.Handlers.Controllers.GetProfile>();
    container.RegisterType<Effuse.SSO.Handlers.Controllers.GetPublicProfile>();
    container.RegisterType<Effuse.SSO.Handlers.Controllers.HeartBeat>();
    container.RegisterType<Effuse.SSO.Handlers.Controllers.JoinServer>();
    container.RegisterType<Effuse.SSO.Handlers.Controllers.Login>();
    container.RegisterType<Effuse.SSO.Handlers.Controllers.Register>();
    container.RegisterType<Effuse.SSO.Handlers.Controllers.UpdateProfile>();

    new Server(
      3000,
      container,
      new List<Route>()
      {
        new() {
          Method = HttpMethod.Get,
          Path = "/api/v1/heartbeat",
          Handler = typeof(HeartBeat)
        },
        new() {
          Method = HttpMethod.Post,
          Path = "/api/v1/users",
          Handler = typeof(Register)
        },
        new() {
          Method = HttpMethod.Get,
          Path = "/api/v1/users/{userId}/profile",
          Handler = typeof(GetPublicProfile)
        },
        new() {
          Method = HttpMethod.Get,
          Path = "/api/v1/user/profile",
          Handler = typeof(GetProfile)
        },
        new() {
          Method = HttpMethod.Put,
          Path = "/api/v1/user/profile",
          Handler = typeof(UpdateProfile)
        },
        new() {
          Method = HttpMethod.Post,
          Path = "/api/v1/user/servers",
          Handler = typeof(JoinServer)
        },
        new() {
          Method = HttpMethod.Get,
          Path = "/api/v1/auth/token",
          Handler = typeof(Login)
        },
        new() {
          Method = HttpMethod.Get,
          Path = "/api/v1/auth/user",
          Handler = typeof(GetUserFromToken)
        },
        new() {
          Method = HttpMethod.Get,
          Path = "/api/v1/auth/invite",
          Handler = typeof(Invite)
        },
      }
    ).StartServer().GetAwaiter().GetResult();
  }
}
