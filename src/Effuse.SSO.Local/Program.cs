using System.Reflection;
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

    var assembly = Assembly.Load("Effuse.SSO.Handlers") ?? throw new Exception("Could not find server assembly");

    new Server(3000, container, assembly)
      .StartServer()
      .GetAwaiter()
      .GetResult();
  }
}
