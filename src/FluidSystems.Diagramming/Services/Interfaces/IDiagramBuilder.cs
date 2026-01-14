using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Core.Services.Interfaces
{
    public interface IDiagramBuilder
    {
        SystemDiagram BuildDiagram(FluidSystem system, FluidSystemLayout layout, double maxWidth, double maxHeight);
    }
}