using Amazon.Runtime;
using Amazon.Runtime.Endpoints;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Effuse.Core.Utilities;

namespace Effuse.Core.Stubs;

public class ParameterStore : IAmazonSimpleSystemsManagement
{
  private readonly IDictionary<string, string> parameters;

  public ParameterStore(IDictionary<string, string> parameters)
  {
    this.parameters = parameters.SelectKeys(k => $"/{Env.GetEnv("APP_PREFIX")}/{k}");
  }

  public ISimpleSystemsManagementPaginatorFactory Paginators => throw new NotImplementedException();

  public IClientConfig Config => throw new NotImplementedException();

  public Task<AddTagsToResourceResponse> AddTagsToResourceAsync(AddTagsToResourceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<AssociateOpsItemRelatedItemResponse> AssociateOpsItemRelatedItemAsync(AssociateOpsItemRelatedItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CancelCommandResponse> CancelCommandAsync(string commandId, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CancelCommandResponse> CancelCommandAsync(string commandId, List<string> instanceIds, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CancelCommandResponse> CancelCommandAsync(CancelCommandRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CancelMaintenanceWindowExecutionResponse> CancelMaintenanceWindowExecutionAsync(CancelMaintenanceWindowExecutionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateActivationResponse> CreateActivationAsync(CreateActivationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateAssociationResponse> CreateAssociationAsync(string instanceId, string name, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateAssociationResponse> CreateAssociationAsync(CreateAssociationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateAssociationBatchResponse> CreateAssociationBatchAsync(CreateAssociationBatchRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateDocumentResponse> CreateDocumentAsync(string content, string name, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateDocumentResponse> CreateDocumentAsync(CreateDocumentRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateMaintenanceWindowResponse> CreateMaintenanceWindowAsync(CreateMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateOpsItemResponse> CreateOpsItemAsync(CreateOpsItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateOpsMetadataResponse> CreateOpsMetadataAsync(CreateOpsMetadataRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreatePatchBaselineResponse> CreatePatchBaselineAsync(CreatePatchBaselineRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<CreateResourceDataSyncResponse> CreateResourceDataSyncAsync(CreateResourceDataSyncRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteActivationResponse> DeleteActivationAsync(DeleteActivationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteAssociationResponse> DeleteAssociationAsync(string instanceId, string name, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteAssociationResponse> DeleteAssociationAsync(DeleteAssociationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteDocumentResponse> DeleteDocumentAsync(string name, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteDocumentResponse> DeleteDocumentAsync(DeleteDocumentRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteInventoryResponse> DeleteInventoryAsync(DeleteInventoryRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteMaintenanceWindowResponse> DeleteMaintenanceWindowAsync(DeleteMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteOpsItemResponse> DeleteOpsItemAsync(DeleteOpsItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteOpsMetadataResponse> DeleteOpsMetadataAsync(DeleteOpsMetadataRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteParameterResponse> DeleteParameterAsync(DeleteParameterRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteParametersResponse> DeleteParametersAsync(DeleteParametersRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeletePatchBaselineResponse> DeletePatchBaselineAsync(DeletePatchBaselineRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteResourceDataSyncResponse> DeleteResourceDataSyncAsync(DeleteResourceDataSyncRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeleteResourcePolicyResponse> DeleteResourcePolicyAsync(DeleteResourcePolicyRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeregisterManagedInstanceResponse> DeregisterManagedInstanceAsync(DeregisterManagedInstanceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeregisterPatchBaselineForPatchGroupResponse> DeregisterPatchBaselineForPatchGroupAsync(DeregisterPatchBaselineForPatchGroupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeregisterTargetFromMaintenanceWindowResponse> DeregisterTargetFromMaintenanceWindowAsync(DeregisterTargetFromMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DeregisterTaskFromMaintenanceWindowResponse> DeregisterTaskFromMaintenanceWindowAsync(DeregisterTaskFromMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeActivationsResponse> DescribeActivationsAsync(DescribeActivationsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeAssociationResponse> DescribeAssociationAsync(string instanceId, string name, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeAssociationResponse> DescribeAssociationAsync(DescribeAssociationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeAssociationExecutionsResponse> DescribeAssociationExecutionsAsync(DescribeAssociationExecutionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeAssociationExecutionTargetsResponse> DescribeAssociationExecutionTargetsAsync(DescribeAssociationExecutionTargetsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeAutomationExecutionsResponse> DescribeAutomationExecutionsAsync(DescribeAutomationExecutionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeAutomationStepExecutionsResponse> DescribeAutomationStepExecutionsAsync(DescribeAutomationStepExecutionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeAvailablePatchesResponse> DescribeAvailablePatchesAsync(DescribeAvailablePatchesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeDocumentResponse> DescribeDocumentAsync(string name, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeDocumentResponse> DescribeDocumentAsync(DescribeDocumentRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeDocumentPermissionResponse> DescribeDocumentPermissionAsync(DescribeDocumentPermissionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeEffectiveInstanceAssociationsResponse> DescribeEffectiveInstanceAssociationsAsync(DescribeEffectiveInstanceAssociationsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeEffectivePatchesForPatchBaselineResponse> DescribeEffectivePatchesForPatchBaselineAsync(DescribeEffectivePatchesForPatchBaselineRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeInstanceAssociationsStatusResponse> DescribeInstanceAssociationsStatusAsync(DescribeInstanceAssociationsStatusRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeInstanceInformationResponse> DescribeInstanceInformationAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeInstanceInformationResponse> DescribeInstanceInformationAsync(DescribeInstanceInformationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeInstancePatchesResponse> DescribeInstancePatchesAsync(DescribeInstancePatchesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeInstancePatchStatesResponse> DescribeInstancePatchStatesAsync(DescribeInstancePatchStatesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeInstancePatchStatesForPatchGroupResponse> DescribeInstancePatchStatesForPatchGroupAsync(DescribeInstancePatchStatesForPatchGroupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeInventoryDeletionsResponse> DescribeInventoryDeletionsAsync(DescribeInventoryDeletionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowExecutionsResponse> DescribeMaintenanceWindowExecutionsAsync(DescribeMaintenanceWindowExecutionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowExecutionTaskInvocationsResponse> DescribeMaintenanceWindowExecutionTaskInvocationsAsync(DescribeMaintenanceWindowExecutionTaskInvocationsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowExecutionTasksResponse> DescribeMaintenanceWindowExecutionTasksAsync(DescribeMaintenanceWindowExecutionTasksRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowsResponse> DescribeMaintenanceWindowsAsync(DescribeMaintenanceWindowsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowScheduleResponse> DescribeMaintenanceWindowScheduleAsync(DescribeMaintenanceWindowScheduleRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowsForTargetResponse> DescribeMaintenanceWindowsForTargetAsync(DescribeMaintenanceWindowsForTargetRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowTargetsResponse> DescribeMaintenanceWindowTargetsAsync(DescribeMaintenanceWindowTargetsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeMaintenanceWindowTasksResponse> DescribeMaintenanceWindowTasksAsync(DescribeMaintenanceWindowTasksRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeOpsItemsResponse> DescribeOpsItemsAsync(DescribeOpsItemsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeParametersResponse> DescribeParametersAsync(DescribeParametersRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribePatchBaselinesResponse> DescribePatchBaselinesAsync(DescribePatchBaselinesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribePatchGroupsResponse> DescribePatchGroupsAsync(DescribePatchGroupsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribePatchGroupStateResponse> DescribePatchGroupStateAsync(DescribePatchGroupStateRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribePatchPropertiesResponse> DescribePatchPropertiesAsync(DescribePatchPropertiesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<DescribeSessionsResponse> DescribeSessionsAsync(DescribeSessionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Endpoint DetermineServiceOperationEndpoint(AmazonWebServiceRequest request)
  {
    throw new NotImplementedException();
  }

  public Task<DisassociateOpsItemRelatedItemResponse> DisassociateOpsItemRelatedItemAsync(DisassociateOpsItemRelatedItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public void Dispose()
  {
    throw new NotImplementedException();
  }

  public Task<GetAutomationExecutionResponse> GetAutomationExecutionAsync(GetAutomationExecutionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetCalendarStateResponse> GetCalendarStateAsync(GetCalendarStateRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetCommandInvocationResponse> GetCommandInvocationAsync(GetCommandInvocationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetConnectionStatusResponse> GetConnectionStatusAsync(GetConnectionStatusRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetDefaultPatchBaselineResponse> GetDefaultPatchBaselineAsync(GetDefaultPatchBaselineRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetDeployablePatchSnapshotForInstanceResponse> GetDeployablePatchSnapshotForInstanceAsync(GetDeployablePatchSnapshotForInstanceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetDocumentResponse> GetDocumentAsync(string name, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetDocumentResponse> GetDocumentAsync(GetDocumentRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetInventoryResponse> GetInventoryAsync(GetInventoryRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetInventorySchemaResponse> GetInventorySchemaAsync(GetInventorySchemaRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetMaintenanceWindowResponse> GetMaintenanceWindowAsync(GetMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetMaintenanceWindowExecutionResponse> GetMaintenanceWindowExecutionAsync(GetMaintenanceWindowExecutionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetMaintenanceWindowExecutionTaskResponse> GetMaintenanceWindowExecutionTaskAsync(GetMaintenanceWindowExecutionTaskRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetMaintenanceWindowExecutionTaskInvocationResponse> GetMaintenanceWindowExecutionTaskInvocationAsync(GetMaintenanceWindowExecutionTaskInvocationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetMaintenanceWindowTaskResponse> GetMaintenanceWindowTaskAsync(GetMaintenanceWindowTaskRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetOpsItemResponse> GetOpsItemAsync(GetOpsItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetOpsMetadataResponse> GetOpsMetadataAsync(GetOpsMetadataRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetOpsSummaryResponse> GetOpsSummaryAsync(GetOpsSummaryRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetParameterResponse> GetParameterAsync(GetParameterRequest request, CancellationToken cancellationToken = default)
  {
    var result = this.parameters[request.Name];

    return Task.FromResult(new GetParameterResponse
    {
      Parameter = new()
      {
        Value = result
      }
    });
  }

  public Task<GetParameterHistoryResponse> GetParameterHistoryAsync(GetParameterHistoryRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetParametersResponse> GetParametersAsync(GetParametersRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetParametersByPathResponse> GetParametersByPathAsync(GetParametersByPathRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetPatchBaselineResponse> GetPatchBaselineAsync(GetPatchBaselineRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetPatchBaselineForPatchGroupResponse> GetPatchBaselineForPatchGroupAsync(GetPatchBaselineForPatchGroupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetResourcePoliciesResponse> GetResourcePoliciesAsync(GetResourcePoliciesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<GetServiceSettingResponse> GetServiceSettingAsync(GetServiceSettingRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<LabelParameterVersionResponse> LabelParameterVersionAsync(LabelParameterVersionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListAssociationsResponse> ListAssociationsAsync(ListAssociationsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListAssociationVersionsResponse> ListAssociationVersionsAsync(ListAssociationVersionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListCommandInvocationsResponse> ListCommandInvocationsAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListCommandInvocationsResponse> ListCommandInvocationsAsync(string commandId, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListCommandInvocationsResponse> ListCommandInvocationsAsync(ListCommandInvocationsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListCommandsResponse> ListCommandsAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListCommandsResponse> ListCommandsAsync(string commandId, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListCommandsResponse> ListCommandsAsync(ListCommandsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListComplianceItemsResponse> ListComplianceItemsAsync(ListComplianceItemsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListComplianceSummariesResponse> ListComplianceSummariesAsync(ListComplianceSummariesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListDocumentMetadataHistoryResponse> ListDocumentMetadataHistoryAsync(ListDocumentMetadataHistoryRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListDocumentsResponse> ListDocumentsAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListDocumentsResponse> ListDocumentsAsync(ListDocumentsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListDocumentVersionsResponse> ListDocumentVersionsAsync(ListDocumentVersionsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListInventoryEntriesResponse> ListInventoryEntriesAsync(ListInventoryEntriesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListOpsItemEventsResponse> ListOpsItemEventsAsync(ListOpsItemEventsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListOpsItemRelatedItemsResponse> ListOpsItemRelatedItemsAsync(ListOpsItemRelatedItemsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListOpsMetadataResponse> ListOpsMetadataAsync(ListOpsMetadataRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListResourceComplianceSummariesResponse> ListResourceComplianceSummariesAsync(ListResourceComplianceSummariesRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListResourceDataSyncResponse> ListResourceDataSyncAsync(ListResourceDataSyncRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ListTagsForResourceResponse> ListTagsForResourceAsync(ListTagsForResourceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ModifyDocumentPermissionResponse> ModifyDocumentPermissionAsync(ModifyDocumentPermissionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<PutComplianceItemsResponse> PutComplianceItemsAsync(PutComplianceItemsRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<PutInventoryResponse> PutInventoryAsync(PutInventoryRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<PutParameterResponse> PutParameterAsync(PutParameterRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<PutResourcePolicyResponse> PutResourcePolicyAsync(PutResourcePolicyRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<RegisterDefaultPatchBaselineResponse> RegisterDefaultPatchBaselineAsync(RegisterDefaultPatchBaselineRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<RegisterPatchBaselineForPatchGroupResponse> RegisterPatchBaselineForPatchGroupAsync(RegisterPatchBaselineForPatchGroupRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<RegisterTargetWithMaintenanceWindowResponse> RegisterTargetWithMaintenanceWindowAsync(RegisterTargetWithMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<RegisterTaskWithMaintenanceWindowResponse> RegisterTaskWithMaintenanceWindowAsync(RegisterTaskWithMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<RemoveTagsFromResourceResponse> RemoveTagsFromResourceAsync(RemoveTagsFromResourceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ResetServiceSettingResponse> ResetServiceSettingAsync(ResetServiceSettingRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<ResumeSessionResponse> ResumeSessionAsync(ResumeSessionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<SendAutomationSignalResponse> SendAutomationSignalAsync(SendAutomationSignalRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<SendCommandResponse> SendCommandAsync(string documentName, List<string> instanceIds, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<SendCommandResponse> SendCommandAsync(SendCommandRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<StartAssociationsOnceResponse> StartAssociationsOnceAsync(StartAssociationsOnceRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<StartAutomationExecutionResponse> StartAutomationExecutionAsync(StartAutomationExecutionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<StartChangeRequestExecutionResponse> StartChangeRequestExecutionAsync(StartChangeRequestExecutionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<StartSessionResponse> StartSessionAsync(StartSessionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<StopAutomationExecutionResponse> StopAutomationExecutionAsync(StopAutomationExecutionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<TerminateSessionResponse> TerminateSessionAsync(TerminateSessionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UnlabelParameterVersionResponse> UnlabelParameterVersionAsync(UnlabelParameterVersionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateAssociationResponse> UpdateAssociationAsync(UpdateAssociationRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateAssociationStatusResponse> UpdateAssociationStatusAsync(UpdateAssociationStatusRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateDocumentResponse> UpdateDocumentAsync(UpdateDocumentRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateDocumentDefaultVersionResponse> UpdateDocumentDefaultVersionAsync(UpdateDocumentDefaultVersionRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateDocumentMetadataResponse> UpdateDocumentMetadataAsync(UpdateDocumentMetadataRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateMaintenanceWindowResponse> UpdateMaintenanceWindowAsync(UpdateMaintenanceWindowRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateMaintenanceWindowTargetResponse> UpdateMaintenanceWindowTargetAsync(UpdateMaintenanceWindowTargetRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateMaintenanceWindowTaskResponse> UpdateMaintenanceWindowTaskAsync(UpdateMaintenanceWindowTaskRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateManagedInstanceRoleResponse> UpdateManagedInstanceRoleAsync(UpdateManagedInstanceRoleRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateOpsItemResponse> UpdateOpsItemAsync(UpdateOpsItemRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateOpsMetadataResponse> UpdateOpsMetadataAsync(UpdateOpsMetadataRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdatePatchBaselineResponse> UpdatePatchBaselineAsync(UpdatePatchBaselineRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateResourceDataSyncResponse> UpdateResourceDataSyncAsync(UpdateResourceDataSyncRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<UpdateServiceSettingResponse> UpdateServiceSettingAsync(UpdateServiceSettingRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}
