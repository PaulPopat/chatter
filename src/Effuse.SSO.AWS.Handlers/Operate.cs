using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;
using Unity;
using System;
using Effuse.Core.Handlers.Contracts;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Effuse.Core.Utilities;
using System.Collections.Generic;
using System.Reflection;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Effuse.SSO.AWS.Handlers;

public class Operate : Effuse.Core.AWS.Handlers.Operate
{

  private static UnityContainer GetContainer()
  {
    var container = new UnityContainer();
    Effuse.Core.Integration.Deps.Register(container);
    Effuse.Core.AWS.Integration.Deps.Register(container);
    Effuse.SSO.Integration.Deps.Register(container);
    Effuse.SSO.Services.Deps.Register(container);
    Effuse.SSO.Handlers.Deps.Register(container);
    return container;
  }

  private readonly static Lazy<UnityContainer> Container = new Lazy<UnityContainer>(GetContainer);


  protected override UnityContainer GetAppContainer()
  {
    return Container.Value;
  }
}