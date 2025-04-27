using System.ComponentModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using ModelContextProtocol.Server;

namespace FhirMcpServer;

[McpServerToolType]
public class FhirCapabilityTool(FhirClient client) : FhirToolBase(client)
{
    private readonly CapabilityStatement? _capabilityStatement = client.CapabilityStatement();

    [McpServerTool, Description("Returns the FHIR version this server supports.")]
    public string GetFhirVersion()
        => _capabilityStatement?.FhirVersion?.ToString() ?? "";

    [McpServerTool, Description("Lists supported formats the server can handle (e.g., JSON, XML).")]
    public List<string> ListSupportedFormats()
        => _capabilityStatement?.Format?.ToList() ?? [];

    [McpServerTool, Description("Lists all resource types the FHIR server supports.")]
    public List<string> ListResourceTypes()
        => _capabilityStatement?.Rest.First().Resource.Select(r => r.Type).ToList() ?? [];

    [McpServerTool, Description(
         "Lists resource capabilities, including supported search parameters, interactions, operations, etc. " +
         "resourceType: e.g. 'Patient'.")]
    public async Task<string> ListResourceCapabilities(string resourceType)
    {
        var capabilities = _capabilityStatement?.Rest.First().Resource.First(r => r.Type == resourceType);
        return capabilities is null
            ? string.Empty
            : await Serializer.SerializeToStringAsync(capabilities);
    }
}
