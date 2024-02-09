namespace Effuse.Core.Integration.Contracts;

public struct GetItemCommand
{
  public string TableName { get; set; }

  public string KeyName { get; set; }

  public string KeyValue { get; set; }
}

public struct QueryCommand
{
  public string TableName { get; set; }

  public string? IndexName { get; set; }

  public string KeyName { get; set; }

  public string KeyValue { get; set; }
}


public struct DeleteItemCommand
{
  public string TableName { get; set; }

  public string KeyName { get; set; }

  public string KeyValue { get; set; }
}

public interface IDatabase
{
  Task<TExpect?> FindItem<TExpect>(GetItemCommand command);

  Task<TExpect> GetItem<TExpect>(GetItemCommand command);

  Task<IEnumerable<TExpect>> Query<TExpect>(QueryCommand command);

  Task AddItem<T>(string tableName, T item);

  Task UpdateItem<T>(string tableName, T item);

  Task DeleteItem(DeleteItemCommand command);
}