using System.Globalization;

namespace Effuse.Integration.Utilities;

internal static class DateTimeUtilities
{
  public static string ToISOString(this DateTime date)
  {
    return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
  }
}