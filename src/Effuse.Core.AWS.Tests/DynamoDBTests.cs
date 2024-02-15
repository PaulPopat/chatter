using Amazon.DynamoDBv2.Model;
using Effuse.Core.AWS.Integration;

namespace Effuse.Core.AWS.Tests;


[TestClass]
public class DynamoDBTests
{
  private readonly Stubs.DynamoDB database;
  private readonly DynamoDBDatabase sut;

  public DynamoDBTests()
  {
    this.database = new();
    this.sut = new(this.database);
  }

  [TestInitialize]
  public void Setup()
  {
    this.database.Clear();
  }

  [TestMethod]
  public async Task CorrectlySendsData()
  {
    await this.sut.AddItem("TestTable", new
    {
      Text = "Hello world"
    });

    this.database.AssertItems(new Stubs.PutItem
    {
      TableName = "TestTable",
      Item = new Dictionary<string, AttributeValue>
      {
        ["Text"] = new AttributeValue { S = "Hello world" }
      }
    });
  }

  [TestMethod]
  public async Task SendsAListOfObjects()
  {
    await this.sut.AddItem("TestTable", new
    {
      Text = "Hello world",
      Other = new List<object>()
      {
        new
        {
          Test = "text",
          Other = 123
        }
      }
    });

    this.database.AssertItems(new Stubs.PutItem
    {
      TableName = "TestTable",
      Item = new Dictionary<string, AttributeValue>
      {
        ["Text"] = new() { S = "Hello world" },
        ["Other"] = new()
        {
          L = new()
        {
          new()
          {
            M = new()
            {
              ["Test"] = new()
              {
                S = "text"
              },
              ["Other"] = new()
              {
                N = "123"
              }
            }
          }
        }
        }
      }
    });
  }

  private struct TestModel
  {
    public string Text { get; set; }
  }

  [TestMethod]
  public async Task CorrectlySendsAModel()
  {
    await this.sut.AddItem("TestTable", new TestModel
    {
      Text = "Hello world"
    });

    this.database.AssertItems(new Stubs.PutItem
    {
      TableName = "TestTable",
      Item = new Dictionary<string, AttributeValue>
      {
        ["Text"] = new AttributeValue { S = "Hello world" }
      }
    });
  }

  private struct TestListItem
  {
    public string Test { get; set; }

    public int Other { get; set; }
  }

  private struct TestListModel
  {
    public string Text { get; set; }

    public List<TestListItem> Other { get; set; }
  }

  [TestMethod]
  public async Task SendsAListOfModels()
  {
    await this.sut.AddItem("TestTable", new TestListModel
    {
      Text = "Hello world",
      Other = new List<TestListItem>()
      {
        new()
        {
          Test = "text",
          Other = 123
        }
      }
    });

    this.database.AssertItems(new Stubs.PutItem
    {
      TableName = "TestTable",
      Item = new Dictionary<string, AttributeValue>
      {
        ["Text"] = new() { S = "Hello world" },
        ["Other"] = new()
        {
          L = new()
        {
          new()
          {
            M = new()
            {
              ["Test"] = new()
              {
                S = "text"
              },
              ["Other"] = new()
              {
                N = "123"
              }
            }
          }
        }
        }
      }
    });
  }
}