# MCP Server for FHIR

A C#-based Model Context Protocol (MCP) server that enables interaction with FHIR servers. This server is primarily
designed for developers working with FHIR R4 implementations.

## Tools

### CRUD, Search and Other Helpful Operations

- `CreateResource`: Create a new resource on the server.
- `ReadResource`: Retrieve a resource by its ID.
- `UpdateResource`: Update an existing resource.
- `DeleteResource`: Delete a resource from the server.
- `SearchResources`: Search for resources based on specific criteria.
- `FindNumberOfResources`: Count the number of resources that match a search criteria.
- `GetResourceHistory`: Retrieve the history of a resource.
- `ExecuteTransaction`: Execute a FHIR transaction.

### FHIR Server Capability Statements

- `GetFhirVersion`: Get the FHIR version of the server.
- `ListSupportedFormats`: List the formats supported by the server.
- `ListResourceTypes`: List the resource types supported by the server.
- `ListResourceCapabilities`: List the capabilities of a specific resource type.

## Getting Started

### Use MCP Server in VS Code

For detailed setup information
follow [the official documentation](https://code.visualstudio.com/docs/copilot/chat/mcp-servers).

You can use the configuration file located under [.vscode/mcp.json](.vscode/mcp.json). You need to update your path to
point to the .csproj in you local machine.

### Setting Up Infrastructure

To set up a local FHIR server environment, run:

```bash
docker compose up -d --wait
```

This will start two FHIR servers using Docker Compose:

1. HAPI FHIR Server - Comes pre-loaded with sample data
2. Azure FHIR Server - Starts with an empty database

