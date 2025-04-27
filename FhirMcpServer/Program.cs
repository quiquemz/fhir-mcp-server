using Hl7.Fhir.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Environment;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);
var fhirServerUrl = GetEnvironmentVariable("FHIR_SERVER_URL") ?? "http://localhost:8080";

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services
    .AddSingleton(_ => new FhirClient(fhirServerUrl));

await builder.Build().RunAsync();
