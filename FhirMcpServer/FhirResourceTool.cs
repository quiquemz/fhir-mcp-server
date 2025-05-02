using System.ComponentModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using ModelContextProtocol.Server;

namespace FhirMcpServer;

[McpServerToolType]
public class FhirResourceTool(FhirClient client) : FhirToolBase(client)
{
    [McpServerTool, Description(
         """
         Creates a new FHIR resource 
         - body: JSON string representing the resource.
         """
     )]
    public async Task<string> CreateResource(string body)
    {
        var resource = Parser.Parse<Resource>(body);
        try
        {
            var created = await Client.CreateAsync(resource);
            return created != null
                ? await Serializer.SerializeToStringAsync(created)
                : "Unable to create resource.";
        }
        catch (Exception ex)
        {
            return $"Failed to create resource: {ex.Message}";
        }
    }

    [McpServerTool, Description(
         """
         Reads a FHIR resource of the given type and ID. 
         resourceType: e.g. 'Patient'. 
         id: The resource identifier.
         """
     )]

    public async Task<string> ReadResource(string resourceType, string id)
    {
        try
        {
            var resource = await Client.ReadAsync<Resource>($"{resourceType}/{id}");
            return resource != null
                ? await Serializer.SerializeToStringAsync(resource)
                : $"Resource {resourceType}/{id} not found.";
        }
        catch (FhirOperationException ex)
        {
            return $"Failed to read {resourceType}/{id}: {ex.Message}";
        }
    }


    [McpServerTool, Description(
         """
         Deletes a FHIR resource of the given type and ID.
         resourceType: e.g. 'Patient'.
         id: The resource identifier.
         """
     )]
    public async Task<string> DeleteResource(string resourceType, string id)
    {
        var resourceIdentity = ResourceIdentity.Build(resourceType, id);
        try
        {
            await Client.DeleteAsync(resourceIdentity);
            return $"{resourceType}/{id} was successfully deleted.";
        }
        catch (FhirOperationException ex)
        {
            return $"Failed to delete {resourceType}/{id}: {ex.Message}";
        }
    }

    [McpServerTool, Description(
         """
         Updates an existing FHIR resource.
         resourceType: e.g. 'Patient'.
         id: The resource identifier.
         body: JSON string representing the updated resource.
         """
     )]
    public async Task<string> UpdateResource(string resourceType, string id, string body)
    {
        var resource = Parser.Parse<Resource>(body);
        resource.Id = id;
        try
        {

            var updated = await Client.UpdateAsync(resource);
            return updated != null
                ? await Serializer.SerializeToStringAsync(updated)
                : "Unable to update resource.";
        }
        catch (FhirOperationException ex)
        {
            return $"Failed to update {resourceType}/{id}: {ex.Message}";
        }
    }

    [McpServerTool, Description(
         """
         Searches for FHIR resources of the given type with the given criteria." +
         - resourceType: (Required) The type of resource to search for (e.g., 'Patient', 'Observation'). " 
         - criteria: (Optional) An array of search parameters in 'name=value' format (e.g., ['name=John', 'gender=male']). " 
         - pageSize: (Optional) Maximum number of results to return per page (default: 10). ")]
         """
     )]
    public async Task<string> SearchResources(
        string resourceType,
        string[]? criteria = null,
        int pageSize = 10)
    {
        try
        {
            var bundle = await Client.SearchAsync(resourceType, criteria, pageSize: pageSize);
            return bundle != null
                ? await Serializer.SerializeToStringAsync(bundle)
                : "Unable to complete search.";
        }
        catch (Exception ex)
        {
            return $"Search failed: {ex.Message}";
        }
    }

    [McpServerTool, Description(
         """
         Returns the total number of resources of a given FHIR resource type.
         resourceType: e.g. 'Patient', 'Observation', 'Encounter'.
         """
     )]
    public async Task<string> FindNumberOfResources(string resourceType)
    {
        try
        {
            var result = await Client.SearchAsync(resourceType, summary: SummaryType.Count);
            return result?.Total.ToString() ?? "0";

        }
        catch (Exception ex)
        {
            return "Failed to get resource count: " + ex.Message;
        }
    }

    [McpServerTool, Description(
         """
         Gets the history of a FHIR resource, showing all versions.
         resourceType: e.g. 'Patient'.
         id: The resource identifier.
         """
     )]
    public async Task<string> GetResourceHistory(string resourceType, string id)
    {
        try
        {
            var history = await Client.HistoryAsync($"{resourceType}/{id}");
            return history != null
                ? await Serializer.SerializeToStringAsync(history)
                : $"No history found for {resourceType}/{id}.";
        }
        catch (FhirOperationException ex)
        {
            return $"Failed to get history for {resourceType}/{id}: {ex.Message}";
        }
    }

    [McpServerTool, Description(
         """
         Performs a FHIR transaction with a bundle of operation. All operations in the bundle will be treated as a single atomic unit - either all succeed or all fail.
         body: JSON string representing a Bundle with type 'transaction'.
         """
     )]
    public async Task<string> ExecuteTransaction(string body)
    {
        var bundle = Parser.Parse<Bundle>(body);
        if (bundle.Type != Bundle.BundleType.Transaction)
            return "Bundle must have type 'transaction'.";

        try
        {
            var result = await Client.TransactionAsync(bundle);
            return result != null
                ? await Serializer.SerializeToStringAsync(result)
                : "Unable to complete transaction.";
        }
        catch (Exception ex)
        {
            return $"Transaction failed: {ex.Message}";
        }
    }

    // Execute Batch
    [McpServerTool, Description(
         """
         Performs a FHIR batch with a bundle of operation. Each operation in the batch is processed independently - some may succeed while others fail.
         body: JSON string representing a Bundle with type 'batch'.
         """
     )]
    public async Task<string> ExecuteBatch(string body)
    {
        var bundle = Parser.Parse<Bundle>(body);
        if (bundle.Type != Bundle.BundleType.Batch)
            return "Bundle must have type 'batch'.";

        try
        {
            var result = await Client.TransactionAsync(bundle);
            return result != null
                ? await Serializer.SerializeToStringAsync(result)
                : "Unable to complete batch.";
        }
        catch (Exception ex)
        {
            return $"Batch failed: {ex.Message}";
        }
    }
}
