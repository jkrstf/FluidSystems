using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.FlowSolvers;
using FluidSystems.Core.Constants;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.Topology;

namespace FluidSystems.Control.Services.Flow
{
    public class FlowSolver : IFlowSolver
    {
        public void InitializeFlow(SimulationContext context)
        {
            context.FluidState.Materials.Clear();
            if (context?.System?.Components == null) return;

            foreach (var component in context.System.Components)
            {
                if (component.Category == ComponentCategory.Manifold) continue;
                if (component.Category != ComponentCategory.Source)
                {
                    context.SetMaterial(component.Id, FluidSystemContants.Air);
                }
                else
                {
                    context.SetMaterial(component.Id, component.Parameters[FluidSystemContants.Material]);
                }
            }
            UpdateFlows(context);
        }

        public void UpdateFlows(SimulationContext context)
        {
            var sources = context.System.Components.Where(component => component.Category == ComponentCategory.Source);
            var queue = new Queue<(string nodeId, string material, string? fromEdgeId)>();

            var visitedStates = new HashSet<(string nodeId, string? fromEdgeId)>();

            foreach (var source in sources)
            {
                source.Parameters.TryGetValue(FluidSystemContants.Material, out string? material);
                if (material != null) queue.Enqueue((source.Id, material, null));
            }

            while (queue.Count > 0)
            {
                var (currentNodeId, material, fromEdgeId) = queue.Dequeue();
                if (visitedStates.Contains((currentNodeId, fromEdgeId))) continue;
                visitedStates.Add((currentNodeId, fromEdgeId));
                var connectedEdges = context.Graph.Edges.Where(e => e.ConnectedNodeIds.Contains(currentNodeId));

                foreach (var edge in connectedEdges)
                {
                    if (edge.Id == fromEdgeId) continue;
                    if (!CanFlowThrough(currentNodeId, fromEdgeId, edge, context)) continue;

                    context.SetMaterial(edge.Id, material);
                    var neighborNodes = edge.ConnectedNodeIds.Where(id => id != currentNodeId);

                    foreach (var nextNodeId in neighborNodes)
                    {
                        if (!CanEnterNode(nextNodeId, material, context)) continue;
                        context.SetMaterial(nextNodeId, material);
                        queue.Enqueue((nextNodeId, material, edge.Id));
                    }
                }
            }
        }

        private bool CanEnterNode(string nodeId, string material, SimulationContext context)
        {
            var behavior = context.GetBehavior(nodeId);
            if (behavior is TwoWayValveBehavior twoWay && !twoWay.IsOpen) return material == FluidSystemContants.Air;
            return true;
        }

        private bool CanFlowThrough(string nodeId, string? fromEdgeId, TopologyEdge toEdge, SimulationContext context)
        {
            if (fromEdgeId == null) return true;

            var behavior = context.GetBehavior(nodeId);
            if (behavior == null) return true;

            var component = context.System.Components.First(c => c.Id == nodeId);
            return behavior.IsPathActive(fromEdgeId, toEdge.Id, component);
        }
    }
}