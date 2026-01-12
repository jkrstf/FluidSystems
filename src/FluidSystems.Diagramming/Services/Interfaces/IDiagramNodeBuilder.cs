using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Core.Services.Interfaces
{
    public interface IDiagramNodeBuilder
    {
        List<DiagramNode> CreateNodes(FluidSystem system, FluidSystemLayout layout, double maxWidth, double maxHeight);
    }
}