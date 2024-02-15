using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Effuse.Core.Stubs;

public struct PutItem
{
  public Dictionary<string, AttributeValue> Item { get; set; }

  public string TableName { get; set; }
}

public class DynamoDB : IAmazonDynamoDB
{
  private List<PutItem> PutItems { get; set; } = new();

  public void Clear()
  {
    this.PutItems = new();
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
    Assert.AreEqual(one.BOOL, two.BOOL)
    Assert.AreEqual(one.S, two.S);
    CollectionAssert.AreEqual(one.SS, two.SS);
    this.AssertMatches(one.M, two.M);
    this.AssertMatches(one.L, two.L);
    Assert.AreEqual(one.N, two.N);
    CollectionAssert.AreEqual(one.NS, two.NS);
    Assert.AreEqual(one.NULL, two.NULL);
  }


  private void AssertMatches(PutItem one, PutItem two)
  {
    Assert.AreEqual(one.TableName, two.TableName);
    this.AssertMatches(one.Item, two.Item);
  }

  public void AssertItems(params PutItem[] items)
  {
    Assert.AreEqual(items.Length, this.PutItems.Count);
    for (var i = 0 ; i < items.Length ; i++)
    {
      this.AssertMatches(items[i], this.PutItems[i]);
    }
  }

  public IDynamoDBv2PaginatorFactory Paginators => throw new NotImplementedException();

  public IClientConfig Config => throw new NotImplementedException();

  public Task<BatchExecuteStatementResponse> BatchExecuteStatementAsync(BatchExecuteStatementRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<BatchGetItemResponse> BatchGetItemAsync(Dictionary<string, KeysAndAttributes> requestItems, ReturnConsumedCapacity returnConsumedCapacity, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<BatchGetItemResponse> BatchGetItemAsync(Dictionary<string, KeysAndAttributes> requestItems, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<BatchGetItemResponse> BatchGetItemAsync(BatchGetItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<BatchWriteItemResponse> BatchWriteItemAsync(Dictionary<string, List<WriteRequest>> requestItems, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<BatchWriteItemResponse> BatchWriteItemAsync(BatchWriteItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateBackupResponse> CreateBackupAsync(CreateBackupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateGlobalTableResponse> CreateGlobalTableAsync(CreateGlobalTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateTableResponse> CreateTableAsync(string tableName, List<KeySchemaElement> keySchema, List<AttributeDefinition> attributeDefinitions, ProvisionedThroughput provisionedThroughput, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteBackupResponse> DeleteBackupAsync(DeleteBackupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteItemResponse> DeleteItemAsync(string tableName, Dictionary<string, AttributeValue> key, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteItemResponse> DeleteItemAsync(string tableName, Dictionary<string, AttributeValue> key, ReturnValue returnValues, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteItemResponse> DeleteItemAsync(DeleteItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteTableResponse> DeleteTableAsync(string tableName, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeBackupResponse> DescribeBackupAsync(DescribeBackupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeContinuousBackupsResponse> DescribeContinuousBackupsAsync(DescribeContinuousBackupsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeContributorInsightsResponse> DescribeContributorInsightsAsync(DescribeContributorInsightsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeEndpointsResponse> DescribeEndpointsAsync(DescribeEndpointsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeExportResponse> DescribeExportAsync(DescribeExportRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeGlobalTableResponse> DescribeGlobalTableAsync(DescribeGlobalTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeGlobalTableSettingsResponse> DescribeGlobalTableSettingsAsync(DescribeGlobalTableSettingsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeImportResponse> DescribeImportAsync(DescribeImportRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeKinesisStreamingDestinationResponse> DescribeKinesisStreamingDestinationAsync(DescribeKinesisStreamingDestinationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeLimitsResponse> DescribeLimitsAsync(DescribeLimitsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeTableResponse> DescribeTableAsync(string tableName, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeTableResponse> DescribeTableAsync(DescribeTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeTableReplicaAutoScalingResponse> DescribeTableReplicaAutoScalingAsync(DescribeTableReplicaAutoScalingRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeTimeToLiveResponse> DescribeTimeToLiveAsync(string tableName, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeTimeToLiveResponse> DescribeTimeToLiveAsync(DescribeTimeToLiveRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Amazon.Runtime.Endpoints.Endpoint DetermineServiceOperationEndpoint(AmazonWebServiceRequest request)
  {
    throw new NotImplementedException();
  }

  public Task<DisableKinesisStreamingDestinationResponse> DisableKinesisStreamingDestinationAsync(DisableKinesisStreamingDestinationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public void Dispose()
  {
    throw new NotImplementedException();
  }

  public Task<EnableKinesisStreamingDestinationResponse> EnableKinesisStreamingDestinationAsync(EnableKinesisStreamingDestinationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ExecuteStatementResponse> ExecuteStatementAsync(ExecuteStatementRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ExecuteTransactionResponse> ExecuteTransactionAsync(ExecuteTransactionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ExportTableToPointInTimeResponse> ExportTableToPointInTimeAsync(ExportTableToPointInTimeRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetItemResponse> GetItemAsync(string tableName, Dictionary<string, AttributeValue> key, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetItemResponse> GetItemAsync(string tableName, Dictionary<string, AttributeValue> key, bool consistentRead, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetItemResponse> GetItemAsync(GetItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ImportTableResponse> ImportTableAsync(ImportTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListBackupsResponse> ListBackupsAsync(ListBackupsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListContributorInsightsResponse> ListContributorInsightsAsync(ListContributorInsightsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListExportsResponse> ListExportsAsync(ListExportsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListGlobalTablesResponse> ListGlobalTablesAsync(ListGlobalTablesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListImportsResponse> ListImportsAsync(ListImportsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListTablesResponse> ListTablesAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListTablesResponse> ListTablesAsync(string exclusiveStartTableName, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListTablesResponse> ListTablesAsync(string exclusiveStartTableName, int limit, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListTablesResponse> ListTablesAsync(int limit, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListTablesResponse> ListTablesAsync(ListTablesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListTagsOfResourceResponse> ListTagsOfResourceAsync(ListTagsOfResourceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<PutItemResponse> PutItemAsync(string tableName, Dictionary<string, AttributeValue> item, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<PutItemResponse> PutItemAsync(string tableName, Dictionary<string, AttributeValue> item, ReturnValue returnValues, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<PutItemResponse> PutItemAsync(PutItemRequest request, CancellationToken cancellationToken = default)
  {
    this.PutItems.Add(new PutItem
    {
      TableName = request.TableName,
      Item = request.Item
    });

    return Task.FromResult(new PutItemResponse());
  }

  public Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<RestoreTableFromBackupResponse> RestoreTableFromBackupAsync(RestoreTableFromBackupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<RestoreTableToPointInTimeResponse> RestoreTableToPointInTimeAsync(RestoreTableToPointInTimeRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ScanResponse> ScanAsync(string tableName, List<string> attributesToGet, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ScanResponse> ScanAsync(string tableName, Dictionary<string, Condition> scanFilter, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ScanResponse> ScanAsync(string tableName, List<string> attributesToGet, Dictionary<string, Condition> scanFilter, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ScanResponse> ScanAsync(ScanRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<TagResourceResponse> TagResourceAsync(TagResourceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<TransactGetItemsResponse> TransactGetItemsAsync(TransactGetItemsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<TransactWriteItemsResponse> TransactWriteItemsAsync(TransactWriteItemsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UntagResourceResponse> UntagResourceAsync(UntagResourceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateContinuousBackupsResponse> UpdateContinuousBackupsAsync(UpdateContinuousBackupsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateContributorInsightsResponse> UpdateContributorInsightsAsync(UpdateContributorInsightsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateGlobalTableResponse> UpdateGlobalTableAsync(UpdateGlobalTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateGlobalTableSettingsResponse> UpdateGlobalTableSettingsAsync(UpdateGlobalTableSettingsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateItemResponse> UpdateItemAsync(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateItemResponse> UpdateItemAsync(string tableName, Dictionary<string, AttributeValue> key, Dictionary<string, AttributeValueUpdate> attributeUpdates, ReturnValue returnValues, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateKinesisStreamingDestinationResponse> UpdateKinesisStreamingDestinationAsync(UpdateKinesisStreamingDestinationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateTableResponse> UpdateTableAsync(string tableName, ProvisionedThroughput provisionedThroughput, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateTableReplicaAutoScalingResponse> UpdateTableReplicaAutoScalingAsync(UpdateTableReplicaAutoScalingRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateTimeToLiveResponse> UpdateTimeToLiveAsync(UpdateTimeToLiveRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}
