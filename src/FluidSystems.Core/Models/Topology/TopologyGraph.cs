namespace FluidSystems.Core.Models.Topology
{
    public class TopologyGraph
    {
        public IReadOnlyList<TopologyNode> Nodes { get; }
        public IReadOnlyList<TopologyEdge> Edges { get; }

        public TopologyGraph(IReadOnlyList<TopologyNode> nodes, IReadOnlyList<TopologyEdge> edges) 
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}