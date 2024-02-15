namespace Effuse.Core.Utilities;

public static class Dictionary
{
  public static IDictionary<TKey, TValue> WithKeyValue<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue value)
     where TKey : notnull
  {
    var result = new Dictionary<TKey, TValue>();
    foreach (var (k, v) in self)
    {
      result[k] = v;
    }

    result[key] = value;

    return result;
  }

  public static IDictionary<string, TValue> ToLowerCaseKeys<TValue>(this IDictionary<string, TValue> self)
  {
    var result = new Dictionary<string, TValue>();
    foreach (var (k, v) in self)
    {
      result[k.ToLowerInvariant()] = v;
    }

    return result;
  }
}
