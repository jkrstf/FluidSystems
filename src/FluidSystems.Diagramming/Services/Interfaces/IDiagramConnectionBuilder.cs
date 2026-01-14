using FluidSystems.Core.Models.System;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Core.Services.Interfaces
{
    public interface IDiagramConnectionBuilder
    {
        List<DiagramConnection> CreateConnections(FluidSystem system, Dictionary<string, DiagramNode> nodeLookup);
    }
}