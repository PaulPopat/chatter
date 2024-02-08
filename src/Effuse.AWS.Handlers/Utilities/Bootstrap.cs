using System;
using Effuse.Handlers.Controllers;
using Unity;

namespace Effuse.AWS.Handlers.Utilities;

public static class Bootstrap
{
  private static UnityContainer container
  {
    get
    {
      var container = new UnityContainer();

      container.RegisterType<HeartBeat>();
      return container;
    }
  }

  public readonly static Lazy<UnityContainer> Container = new Lazy<UnityContainer>(() =>
  {
    return container;
  });
}