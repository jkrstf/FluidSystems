namespace FluidSystems.Core.Models.Topology
{
    public class TopologyEdge
    {
        public string Id { get; }
        public IReadOnlyList<string> ConnectedNodeIds { get; }

        public TopologyEdge(string id, IReadOnlyList<string> connectedNodeIds)
        {
            Id = id;
            ConnectedNodeIds = connectedNodeIds;
        }
    }
}
