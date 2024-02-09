using System.Reflection;
using Amazon.DynamoDBv2.Model;

namespace Effuse.Core.AWS.Integration.Utilities;

public static class AttributeUtilities
{
  private static Type GetListType(Type target)
  {
    if (!typeof(List<>).IsAssignableFrom(target)) throw new Exception("Expected a list");

    return target.GenericTypeArguments[0] ?? throw new Exception("No generic type arguments");
  }

  private static object? UnmarshalValue(Type target, AttributeValue value)
  {
    if (value.B != null) return value.B;
    if (value.IsBOOLSet) return value.BOOL;
    if (value.BS != null) return value.BS;
    if (value.S != null) return value.S;
    if (value.SS != null) return value.SS;
    if (value.M != null) return UnmarshalBasic(target, value.M);
    if (value.L != null)
    {
      var arrayType = GetListType(target);
      return value.L.Select(n => UnmarshalValue(arrayType, n));
    }
    if (value.N != null) return Convert.ChangeType(double.Parse(value.N), target);
    if (value.NS != null)
    {
      var arrayType = GetListType(target);
      return value.NS.Select(n => Convert.ChangeType(double.Parse(n), arrayType));
    }
    if (value.NULL) return null;

    throw new Exception("Could not find type");
  }

  private static object? UnmarshalBasic(Type type, IDictionary<string, AttributeValue> input)
  {
    var result = Activator.CreateInstance(type);

    foreach (var property in type.GetProperties())
    {
      if (property == null) continue;
      if (!input.TryGetValue(property.Name, out var value))
      {
        throw new Exception($"Error unmarshalling, looking for {property.Name} but not found");
      }

      var unmarshalled = UnmarshalValue(property.PropertyType, value);
      property.SetValue(result, unmarshalled);
    }

    return result;
  }

  public static T Unmarshal<T>(this IDictionary<string, AttributeValue> input)
  {
    return (T)(UnmarshalBasic(typeof(T), input) ?? throw new Exception("Could not unmarshall object"));
  }

  private static bool TryMake<TTarget>(object original, PropertyInfo property, out TTarget? result)
  {
    if (!typeof(TTarget).IsAssignableFrom(property.PropertyType))
    {
      result = default;
      return false;
    }

    result = (TTarget)(property.GetValue(original) ?? throw new Exception("Could not find value"));
    return true;
  }

  private static AttributeValue MarshalValue(object? item)
  {
    if (item is MemoryStream stream)
    {
      return new AttributeValue() { B = stream };
    }
    else if (item is bool boolean)
    {
      return new AttributeValue() { BOOL = boolean };
    }
    else if (item is List<MemoryStream> bs)
    {
      return new AttributeValue() { BS = bs };
    }
    else if (item is string s)
    {
      return new AttributeValue() { S = s };
    }
    else if (item is List<string> ss)
    {
      return new AttributeValue() { SS = ss };
    }
    else if
      (
        item is byte ||
        item is sbyte ||
        item is ushort ||
        item is uint ||
        item is ulong ||
        item is short ||
        item is int ||
        item is long ||
        item is decimal ||
        item is double ||
        item is float
      )
    {
      return new AttributeValue() { N = item.ToString() };
    }
    else if
      (
        item is List<byte> ||
        item is List<sbyte> ||
        item is List<ushort> ||
        item is List<uint> ||
        item is List<ulong> ||
        item is List<short> ||
        item is List<int> ||
        item is List<long> ||
        item is List<decimal> ||
        item is List<double> ||
        item is List<float>
      )
    {
      return new AttributeValue() { NS = ((List<object>)item).Select(i => i.ToString()).ToList() };
    }
    else if (item is List<object> l)
    {
      return new AttributeValue() { L = l.Select(l => MarshalValue(l)).ToList() };
    }
    else if (item is object m)
    {
      return new AttributeValue() { M = Marshal(m) };
    }
    else
    {
      return new AttributeValue() { NULL = true };
    }
  }

  public static Dictionary<string, AttributeValue> Marshal(this object? input)
  {
    var type = input?.GetType() ?? throw new Exception("Cannot marshall a null value");
    var result = new Dictionary<string, AttributeValue>();

    foreach (var property in type.GetProperties())
    {
      result[property.Name] = MarshalValue(property.GetValue(input));
    }

    return result;
  }
}