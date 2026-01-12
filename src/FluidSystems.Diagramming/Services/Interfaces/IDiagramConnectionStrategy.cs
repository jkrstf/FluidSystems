using FluidSystems.Core.Models.System;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Core.Services.Interfaces
{
    public interface IDiagramConnectionStrategy
    {
        List<DiagramConnection> Build(FluidComponent component, Dictionary<string, DiagramNode> nodeLookup);
    }
}