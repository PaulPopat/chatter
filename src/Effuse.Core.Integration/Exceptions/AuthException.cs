using Effuse.Core.Integration.Exceptions;

namespace Effuse.Core.Integration;

public class AuthException : HttpException
{
  public AuthException(string reason) : base(403, "Access Denied")
  {
    Console.WriteLine($"AccessDenied: {reason}");
  }
}
