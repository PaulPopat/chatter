namespace Effuse.Core.Integration.Contracts;

public interface IDatabase
{
  Task<TExpect?> FindItem<TExpect>(string tableName, string primaryKey)
    where TExpect : struct;

  Task<TExpect> GetItem<TExpect>(string tableName, string primaryKey)
    where TExpect : struct;

  Task AddItem<T>(string tableName, string primaryKey, T item)
    where T : struct;

  Task UpdateItem<T>(string tableName, string primaryKey, T item)
    where T : struct;

  Task DeleteItem(string tableName, string primaryKey);
}