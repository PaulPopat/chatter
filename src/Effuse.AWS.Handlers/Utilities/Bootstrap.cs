using System;
using Unity;

namespace Effuse.AWS.Handlers.Utilities;

public static class Bootstrap
{
  private static UnityContainer GetContainer()
  {
    var container = new UnityContainer();

    // AWS Clients
    container.RegisterFactory<Amazon.DynamoDBv2.IAmazonDynamoDB>((con) => new Amazon.DynamoDBv2.AmazonDynamoDBClient());
    container.RegisterFactory<Amazon.SimpleSystemsManagement.IAmazonSimpleSystemsManagement>((con) => new Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient());

    // AWS Integrations
    container.RegisterType<Effuse.Integration.Contracts.IDatabase, Integration.DynamoDBDatabase>();
    container.RegisterType<Effuse.Integration.Contracts.IParameters, Integration.ParameterStore>();

    // Generic Integrations
    container.RegisterType<Effuse.Integration.Clients.User.IUserClient, Effuse.Integration.Clients.User.DbUserClient>();

    // Services
    container.RegisterType<Effuse.Services.Core.AuthService>();

    // Handlers
    container.RegisterType<Effuse.Handlers.Controllers.HeartBeat>();
    return container;
  }

  public readonly static Lazy<UnityContainer> Container = new Lazy<UnityContainer>(() =>
  {
    return GetContainer();
  });
}