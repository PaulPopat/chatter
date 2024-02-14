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

  [TestMethod]
  public async Task CorrectlySendsData()
  {
    await this.sut.AddItem("TestTable", new
    {
      Text = "Hello world"
    });

    Assert.AreEqual(this.database.PutItems.Count, 1);
    var item = this.database.PutItems[0];
    CollectionAssert.AreEqual(item.Item.Keys, new List<string> { "Text" });
  }
}