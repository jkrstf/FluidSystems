namespace FluidSystems.Diagramming.Models
{
    public class SystemDiagram
    {
        public List<DiagramNode> Nodes { get; set; } = new();
        public List<DiagramConnection> Connections { get; set; } = new();
    }
}