using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Effuse.Core.AWS.Integration.Utilities;
using Effuse.Core.Integration.Contracts;

namespace Effuse.Core.AWS.Integration;

public class DynamoDBDatabase : IDatabase
{
  private readonly IAmazonDynamoDB dynamoDB;

  public DynamoDBDatabase(IAmazonDynamoDB dynamoDB)
  {
    this.dynamoDB = dynamoDB;
  }

  public async Task<TExpect?> FindItem<TExpect>(GetItemCommand command)
  {
    var response = await this.dynamoDB.GetItemAsync(new GetItemRequest
    {
      Key = new Dictionary<string, AttributeValue>()
      {
        [command.KeyName] = new AttributeValue() { S = command.KeyValue }
      },
      TableName = command.TableName
    });

    if (!response.IsItemSet) return default;

    return response.Item.Unmarshal<TExpect>();
  }

  public async Task<TExpect> GetItem<TExpect>(GetItemCommand command)
  {
    return (await this.FindItem<TExpect>(command)) ?? throw new Exception("Could not find item");
  }

  public async Task<IEnumerable<TExpect>> Query<TExpect>(QueryCommand command)
  {
    var response = await this.dynamoDB.QueryAsync(new QueryRequest
    {
      TableName = command.TableName,
      IndexName = command.IndexName,
      KeyConditionExpression = $"{command.KeyName} = :{command.KeyName}",
      ExpressionAttributeValues = new Dictionary<string, AttributeValue>
      {
        [$":{command.KeyName}"] = new AttributeValue { S = command.KeyValue }
      }
    });

    return response.Items.Select(i => i.Unmarshal<TExpect>());
  }

  public Task AddItem<T>(string tableName, T item)
  {
    var marshalled = item.Marshal();
    return this.dynamoDB.PutItemAsync(new PutItemRequest
    {
      TableName = tableName,
      Item = marshalled
    });
  }

  public Task UpdateItem<T>(string tableName, T item)
  {
    return this.dynamoDB.PutItemAsync(new PutItemRequest
    {
      TableName = tableName,
      Item = item.Marshal()
    });
  }

  public Task DeleteItem(DeleteItemCommand command)
  {
    return this.dynamoDB.DeleteItemAsync(new DeleteItemRequest
    {
      TableName = command.TableName,
      Key = new Dictionary<string, AttributeValue>()
      {
        [command.KeyName] = new AttributeValue() { S = command.KeyValue }
      },
    });
  }
}