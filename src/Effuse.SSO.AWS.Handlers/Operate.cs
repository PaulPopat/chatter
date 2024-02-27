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

    // AWS Clients
    container.RegisterFactory<Amazon.DynamoDBv2.IAmazonDynamoDB>((con) => new Amazon.DynamoDBv2.AmazonDynamoDBClient());
    container.RegisterFactory<Amazon.SimpleSystemsManagement.IAmazonSimpleSystemsManagement>((con) => new Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient());
    container.RegisterFactory<Amazon.S3.IAmazonS3>((con) => new Amazon.S3.AmazonS3Client());

    // AWS Integrations
    container.RegisterType<Effuse.Core.Integration.Contracts.IDatabase, Effuse.Core.Integration.Implementations.StaticDatabase>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IParameters, Effuse.Core.AWS.Integration.ParameterStore>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IStatic, Effuse.Core.AWS.Integration.S3Statics>();
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
    return container;
  }

  private readonly static Lazy<UnityContainer> Container = new Lazy<UnityContainer>(GetContainer);


  protected override UnityContainer GetAppContainer()
  {
    return Container.Value;
  }
}