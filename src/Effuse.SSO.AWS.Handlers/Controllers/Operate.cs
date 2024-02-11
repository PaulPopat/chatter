using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;
using Effuse.SSO.Handlers.Controllers;
using Unity;
using System;
using Effuse.Core.Handlers.Contracts;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Effuse.SSO.AWS.Handlers.Controllers;

public class InviteProps
{
  public string Email { get; set; }
}

public class Operate
{

  private static UnityContainer GetContainer()
  {
    var container = new UnityContainer();

    // AWS Clients
    container.RegisterFactory<Amazon.DynamoDBv2.IAmazonDynamoDB>((con) => new Amazon.DynamoDBv2.AmazonDynamoDBClient());
    container.RegisterFactory<Amazon.SimpleSystemsManagement.IAmazonSimpleSystemsManagement>((con) => new Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient());
    container.RegisterFactory<Amazon.S3.IAmazonS3>((con) => new Amazon.S3.AmazonS3Client());

    // AWS Integrations
    container.RegisterType<Effuse.Core.Integration.Contracts.IDatabase, Effuse.Core.AWS.Integration.DynamoDBDatabase>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IParameters, Effuse.Core.AWS.Integration.ParameterStore>();
    container.RegisterType<Effuse.Core.Integration.Contracts.IStatic, Effuse.Core.AWS.Integration.S3Statics>();

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

  private readonly static Lazy<UnityContainer> Container = new Lazy<UnityContainer>(() =>
  {
    return GetContainer();
  });

  private static async Task<APIGatewayProxyResponse> Process
    (
      APIGatewayProxyRequest request,
      Type type
    )
  {
    try
    {
      var handler = Container.Value.Resolve(type);

      var input = new HandlerProps
        (
          request.Path,
          request.HttpMethod,
          request.RequestContext.ConnectionId,
          request.PathParameters,
          request.QueryStringParameters,
          request.Headers,
          request.Body
        );

      var response = await ((IHandler)handler).Handle(input);

      return new APIGatewayProxyResponse
      {
        StatusCode = response.StatusCode,
        Body = response.Body != null ? JsonSerializer.Serialize(response.Body) : string.Empty,
        Headers = response.Headers
      };
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return new APIGatewayProxyResponse
      {
        StatusCode = 500,
        Body = JsonSerializer.Serialize(new { Message = "Internal Server Error" }),
      };
    }
  }

  public async Task<APIGatewayProxyResponse> GetProfile(APIGatewayProxyRequest request)
  {
    return await Process(request, typeof(GetProfile));
  }

  public async Task<APIGatewayProxyResponse> GetPublicProfile(APIGatewayProxyRequest request)
  {
    return await Process(request, typeof(GetPublicProfile));
  }

  public async Task<APIGatewayProxyResponse> HeartBeat(APIGatewayProxyRequest request)
  {
    return await Process(request, typeof(HeartBeat));
  }

  public async Task<APIGatewayProxyResponse> JoinServer(APIGatewayProxyRequest request)
  {
    return await Process(request, typeof(JoinServer));
  }

  public async Task<APIGatewayProxyResponse> Login(APIGatewayProxyRequest request)
  {
    return await Process(request, typeof(Login));
  }

  public async Task<APIGatewayProxyResponse> Register(APIGatewayProxyRequest request)
  {
    return await Process(request, typeof(Register));
  }

  public async Task<APIGatewayProxyResponse> UpdateProfile(APIGatewayProxyRequest request)
  {
    return await Process(request, typeof(UpdateProfile));
  }

  public async Task<APIGatewayProxyResponse> Invite(InviteProps props)
  {
    var service = Container.Value.Resolve<Effuse.SSO.Services.AuthService>();

    return new APIGatewayProxyResponse
    {
      StatusCode = 200,
      Body = await service.CreateInvite(props.Email)
    };
  }
}