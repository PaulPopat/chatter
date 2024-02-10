using Effuse.SSO.Domain;
using Effuse.SSO.Integration.Clients.User;

namespace Effuse.SSO.Services;

public struct UserPublicProfile
{
  public Guid UserId { get; set; }

  public string UserName { get; set; }

  public string Biography { get; set; }
}

public class ProfileService
{
  private readonly IUserClient userClient;

  public ProfileService(IUserClient userClient)
  {
    this.userClient = userClient;
  }

  public async Task<User> UpdateProfile(
    Guid userId,
    string username,
    string biography,
    MemoryStream image,
    string mime)
  {
    var user = await userClient.GetUser(userId);
    var updatedUser = user.WithUserName(username).WithBiography(biography);
    await this.userClient.UpdateUser(updatedUser);
    await this.userClient.UploadProfilePicture(updatedUser, image, mime);
    return updatedUser;
  }

  public async Task<User> GetUser(Guid userId)
  {
    return await userClient.GetUser(userId);
  }

  public async Task<UserPublicProfile> GetPublicProfile(Guid userId)
  {
    var user = await userClient.GetUser(userId);

    return new UserPublicProfile
    {
      UserId = user.UserId,
      UserName = user.UserName,
      Biography = user.Biography
    };
  }
}