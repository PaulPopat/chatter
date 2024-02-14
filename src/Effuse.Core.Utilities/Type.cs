using System.Reflection;

namespace Effuse.Core.Utilities;

public static class TypeExtensions
{
  public static IEnumerable<PropertyInfo> GetBasicProperties(this Type? type)
  {
    if (type == null) return new List<PropertyInfo>();

    return type.GetProperties()
      .Where(p => p.CanRead)
      .Where(p => p.CanWrite)
      .Where(p => p.MemberType == MemberTypes.Property)
      .Where(p => !p.GetIndexParameters().Any());
  }
}
