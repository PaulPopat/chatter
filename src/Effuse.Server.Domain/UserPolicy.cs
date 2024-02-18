namespace Effuse.Server.Domain;

public class UserPolicy
{
  public UserPolicy(Guid resourceId, bool read, bool write, bool admin)
  {
    ResourceId = resourceId;
    Read = read;
    Write = write;
    Admin = admin;
  }

  public Guid ResourceId { get; }

  public bool Read { get; }

  public bool Write { get; }

  public bool Admin { get; }
}
