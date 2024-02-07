using Effuse.Handlers.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Effuse.AWS.Handlers.Utilities;

public static class Bootstrap {
  public static T CreateApp<T>()
  {
    // no buenop by sosso
    // poupou loves you

    var builder = Host.CreateApplicationBuilder();

    builder.Services.AddSingleton<TestHandler>();

    var host = builder.Build();

    return host.Services.GetService<T>();
  }
}