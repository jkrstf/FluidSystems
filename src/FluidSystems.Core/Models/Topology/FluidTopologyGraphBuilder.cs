using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Core.Models.Topology
{
    public class FluidTopologyGraphBuilder
    {
        public TopologyGraph Build(FluidSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));

            var nodes = new List<TopologyNode>();
            var edges = new List<TopologyEdge>();

            foreach (var component in system.Components)
            {
                if (component.Category == ComponentCategory.Conduit) continue;
                nodes.Add(new TopologyNode(component.Id));
            }

            foreach (var conduit in system.Components.Where(c => c.Category == ComponentCategory.Conduit))
            {
                if (conduit.Connectors == null) continue;

                var connectedNodeIds = conduit.Connectors
                    .Where(c => c.ConnectedComponent != null)
                    .Select(c => c.ConnectedComponent.Id)
                    .Distinct()
                    .ToList();

                edges.Add(new TopologyEdge(conduit.Id, connectedNodeIds));
            }

            return new TopologyGraph(nodes, edges);
        }

    }
}