using System;
using Unity;

namespace Effuse.AWS.Handlers.Utilities;

public static class Bootstrap
{
  private static UnityContainer container
  {
    get
    {
      var container = new UnityContainer();

      container.RegisterFactory<Amazon.DynamoDBv2.IAmazonDynamoDB>((con) => new Amazon.DynamoDBv2.AmazonDynamoDBClient());

      container.RegisterType<Effuse.Handlers.Controllers.HeartBeat>();
      container.RegisterType<Effuse.Integration.Contracts.IDatabase, Integration.DynamoDBDatabase>();
      return container;
    }
  }

  public readonly static Lazy<UnityContainer> Container = new Lazy<UnityContainer>(() =>
  {
    return container;
  });
}