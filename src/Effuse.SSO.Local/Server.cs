using System.Net;
using Unity;
using Effuse.Core.Handlers.Contracts;

namespace Effuse.SSO.Local;

public class Server
{
  public HttpListener listener = new HttpListener();

  public string url = "http://localhost:3000/";

  private static UnityContainer Container
  {
    get
    {
      var container = new UnityContainer();

      // // AWS Integrations
      container.RegisterType<Effuse.Core.Integration.Contracts.IDatabase, Effuse.Core.Integration.Implementations.StaticDatabase>();
      container.RegisterType<Effuse.Core.Integration.Contracts.IParameters, Effuse.SSO.Local.Integrations.SystemParameters>();
      container.RegisterType<Effuse.Core.Integration.Contracts.IStatic, Effuse.SSO.Local.Integrations.DiskStatics>();

      // Generic Integrations
      container.RegisterType<Effuse.SSO.Integration.Clients.User.IUserClient, Effuse.SSO.Integration.Clients.User.DbUserClient>();
      container.RegisterType<Effuse.SSO.Integration.Clients.Jwt.IJwtClient, Effuse.SSO.Integration.Clients.Jwt.ParameterJwtClient>();

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
      return container;
    }
  }

  private static readonly IEnumerable<Route> Handlers = new List<Route>()
  {
    new() {
      Method = HttpMethod.Get,
      Path = "/api/v1/heartbeat",
      Handler = "HeartBeat"
    },
    new() {
      Method = HttpMethod.Post,
      Path = "/api/v1/users",
      Handler = "Register"
    },
    new() {
      Method = HttpMethod.Get,
      Path = "/api/v1/users/{userId}/profile",
      Handler = "GetPublicProfile"
    },
    new() {
      Method = HttpMethod.Get,
      Path = "/api/v1/user/profile",
      Handler = "GetProfile"
    },
    new() {
      Method = HttpMethod.Put,
      Path = "/api/v1/user/profile",
      Handler = "UpdateProfile"
    },
    new() {
      Method = HttpMethod.Post,
      Path = "/api/v1/user/servers",
      Handler = "JoinServer"
    },
    new() {
      Method = HttpMethod.Get,
      Path = "/api/v1/auth/token",
      Handler = "Login",
    },
    new() {
      Method = HttpMethod.Get,
      Path = "/api/v1/auth/user",
      Handler = "GetUserFromToken",
    },
    new() {
      Method = HttpMethod.Get,
      Path = "/api/v1/auth/invite",
      Handler = "Invite",
    },
  };


  private async Task HandleIncomingConnections()
  {
    var container = Container;

    while (true)
    {
      var ctx = await listener.GetContextAsync();

      var req = ctx.Request;
      var res = ctx.Response;

      try
      {

        if (req.Url == null)
        {
          await res.ApplyResponse(new(404));
          continue;
        }

        var route = Handlers.First(h => h.Matches(req.Url.AbsolutePath));
        var handler = (IHandler)container.Resolve(route.HandlerType);
        var response = await handler.Handle(await req.HandlerProps(route));

        await res.ApplyResponse(response);
      }
      catch (Exception err)
      {
        Console.WriteLine(err);
        await res.ApplyResponse(new(500));
      }
    }
  }


  public async Task StartServer()
  {
    listener.Prefixes.Add(url);
    listener.Start();
    Console.WriteLine("Listening for connections on {0}", url);

    await HandleIncomingConnections();

    listener.Close();
  }
}
