using System.Reflection;
using Effuse.Core.Local;
using Unity;

namespace Effuse.Server.Local;

class HttpServer
{
  public static void Main(string[] args)
  {
    var container = new UnityContainer();

    // System Integrations
    container.RegisterType<Effuse.Core.Integration.Contracts.IParameters, Effuse.Core.Local.Integrations.SystemParameters>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IStatic, Effuse.Core.Local.Integrations.DiskStatics>();
    container.RegisterType<Effuse.Core.Integration.Contracts.ISubscriptions, Effuse.Core.Local.Integrations.WebsocketSubscriptions>();

    // Generic Integrations    
    container.RegisterType<Effuse.Core.Integration.Contracts.IDatabase, Effuse.Core.Integration.Implementations.StaticDatabase>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IEncryption, Effuse.Core.Integration.Implementations.Encryption>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IChannelClient, Effuse.Core.Integration.Implementations.DbChannelClient>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IChatLog, Effuse.Core.Integration.Implementations.ChatLog>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IJwtClient, Effuse.Core.Integration.Implementations.ParameterJwtClient>();

    container.RegisterType<Effuse.Server.Integrations.Contracts.IUserClient, Effuse.Server.Integrations.DbUserClient>();
    container.RegisterType<Effuse.Server.Integrations.Contracts.ISsoClient, Effuse.Server.Integrations.HttpSsoClient>();

    // Services
    container.RegisterType<Effuse.Server.Services.Admin>();
    container.RegisterType<Effuse.Server.Services.Auth>();
    container.RegisterType<Effuse.Server.Services.Messaging>();

    // Handlers
    container.RegisterType<Effuse.Server.Handlers.Controllers.Authenticate>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Chat>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Channels>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Admin.AddUserToChannel>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Admin.BanUser>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Admin.CreateChatChannel>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Admin.GiveUserAdmin>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Admin.KickUserFromChannel>();
    container.RegisterType<Effuse.Server.Handlers.Controllers.Admin.RenameChannel>();

    var assembly = Assembly.Load("Effuse.Server.Handlers") ?? throw new Exception("Could not find server assembly");

    _ = new WebSocketServer(3003, container, assembly);

    new Effuse.Core.Local.Server(3002, container, assembly)
      .StartServer()
      .GetAwaiter()
      .GetResult();
  }
}
