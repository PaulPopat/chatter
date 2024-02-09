using System;
using Unity;

namespace Effuse.SSO.AWS.Handlers.Utilities;

public static class Bootstrap
{
  private static UnityContainer GetContainer()
  {
    var container = new UnityContainer();

    // AWS Clients
    container.RegisterFactory<Amazon.DynamoDBv2.IAmazonDynamoDB>((con) => new Amazon.DynamoDBv2.AmazonDynamoDBClient());
    container.RegisterFactory<Amazon.SimpleSystemsManagement.IAmazonSimpleSystemsManagement>((con) => new Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient());

    // AWS Integrations
    container.RegisterType<Effuse.Core.Integration.Contracts.IDatabase, Effuse.Core.AWS.Integration.DynamoDBDatabase>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IParameters, Effuse.Core.AWS.Integration.ParameterStore>();

    // Generic Integrations
    container.RegisterType<Effuse.SSO.Integration.Clients.User.IUserClient, Effuse.SSO.Integration.Clients.User.DbUserClient>();
    container.RegisterType<Effuse.SSO.Integration.Clients.Jwt.IJwtClient, Effuse.SSO.Integration.Clients.Jwt.ParameterJwtClient>();

    // Services
    container.RegisterType<Effuse.SSO.Services.AuthService>();

    // Handlers
    container.RegisterType<Effuse.SSO.Handlers.Controllers.HeartBeat>();
    return container;
  }

  public readonly static Lazy<UnityContainer> Container = new Lazy<UnityContainer>(() =>
  {
    return GetContainer();
  });
}