namespace Effuse.Integration.Contracts;

public struct GetItemCommand
{
  public string TableName { get; set; }

  public string Key { get; set; }
}

public struct QueryCommand
{
  public string TableName { get; set; }

  public string? IndexName { get; set; }

  public string KeyName { get; set; }

  public string KeyValue { get; set; }
}

public interface IDatabase
{
  Task<TExpect> GetItem<TExpect>(GetItemCommand command);

  Task<IEnumerable<TExpect>> Query<TExpect>(QueryCommand command);
}