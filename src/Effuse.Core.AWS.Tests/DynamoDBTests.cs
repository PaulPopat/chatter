using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Effuse.Core.AWS.Integration;
using Effuse.Core.AWS.Tests.Stubs;

namespace Effuse.Core.AWS.Tests;


[TestClass]
public class DynamoDBTests
{
  private readonly BasicDynamoDB database;
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


  private void AssertMatches(IDictionary<string, AttributeValue> one, IDictionary<string, AttributeValue> two)
  {
    CollectionAssert.AreEqual(one.Keys.ToList(), two.Keys.ToList());
    foreach (var (key, value) in one)
    {
      this.AssertMatches(value, two[key]);
    }
  }


  private void AssertMatches(List<AttributeValue> one, List<AttributeValue> two)
  {
    Assert.AreEqual(one.Count, two.Count);
    for (var i = 0; i < one.Count; i++)
    {
      this.AssertMatches(one[i], two[i]);
    }
  }

  private void AssertMatches(AttributeValue one, AttributeValue two)
  {
    Assert.AreEqual(one.B, two.B);
    Assert.AreEqual(one.BOOL, two.BOOL);
    Assert.AreEqual(one.S, two.S);
    CollectionAssert.AreEqual(one.SS, two.SS);
    this.AssertMatches(one.M, two.M);
    this.AssertMatches(one.L, two.L);
    Assert.AreEqual(one.N, two.N);
    CollectionAssert.AreEqual(one.NS, two.NS);
    Assert.AreEqual(one.NULL, two.NULL);
  }

  [TestMethod]
  public async Task CorrectlySendsData()
  {
    await this.sut.AddItem("TestTable", new
    {
      Text = "Hello world"
    });

    Assert.AreEqual(this.database.PutItems.Count, 1);
    var item = this.database.PutItems[0];
    Assert.AreEqual(item.TableName, "TestTable");
    this.AssertMatches(item.Item, new Dictionary<string, AttributeValue>
    {
      ["Text"] = new AttributeValue { S = "Hello world" }
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

    Assert.AreEqual(this.database.PutItems.Count, 1);
    var item = this.database.PutItems[0];
    Assert.AreEqual(item.TableName, "TestTable");
    this.AssertMatches(item.Item, new Dictionary<string, AttributeValue>
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

    Assert.AreEqual(this.database.PutItems.Count, 1);
    var item = this.database.PutItems[0];
    Assert.AreEqual(item.TableName, "TestTable");
    this.AssertMatches(item.Item, new Dictionary<string, AttributeValue>
    {
      ["Text"] = new AttributeValue { S = "Hello world" }
    });
  }
}