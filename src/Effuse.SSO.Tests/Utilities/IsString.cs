using System.Security.Cryptography;

namespace Effuse.SSO.Tests.Utilities;

public class IsString
{
  private readonly Func<string, bool> compare;

  private IsString(Func<string, bool> compare)
  {
    this.compare = compare;
  }

  public override bool Equals(object? obj)
  {
    if (obj == null || obj is not string s)
    {
      return false;
    }

    return this.compare(s);
  }

  public static IsString DateNear(DateTime target, double msTolerance)
  {
    return new IsString(s =>
    {
      var found = DateTime.Parse(s);

      var diff = found - target;

      var ms = Math.Abs(diff.TotalMilliseconds);

      return ms <= msTolerance;
    });
  }

  public static IsString PasswordMatching(string password)
  {
    return new IsString(hashString =>
    {
      var segments = hashString.Split(':');
      var hash = Convert.FromHexString(segments[0]);
      var salt = Convert.FromHexString(segments[1]);
      var iterations = int.Parse(segments[2]);
      var algorithm = new HashAlgorithmName(segments[3]);
      byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
          password,
          salt,
          iterations,
          algorithm,
          hash.Length
      );
      return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    });
  }
}
