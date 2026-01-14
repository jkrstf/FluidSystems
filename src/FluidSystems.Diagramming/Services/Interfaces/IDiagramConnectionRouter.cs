using FluidSystems.Diagramming.Models;

namespace FluidSystems.Core.Services.Interfaces
{
    public interface IDiagramConnectionRouter
    {
        void PreprocessNodes(IEnumerable<DiagramNode> nodes, IEnumerable<DiagramConnection> connections);
        List<DiagramPoint> Route(DiagramConnection connection);
    }
}