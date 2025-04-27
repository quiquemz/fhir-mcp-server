using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace FhirMcpServer;

/// <summary>
/// Base class for FHIR tools providing common functionality
/// </summary>
public abstract class FhirToolBase(FhirClient client)
{
    protected readonly FhirClient Client = client;
    protected readonly FhirJsonParser Parser = new();
    protected readonly FhirJsonSerializer Serializer = new();
}