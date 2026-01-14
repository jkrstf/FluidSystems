using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.FlowSolvers;
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
                if (component.Category != ComponentCategory.Source)
                {
                    context.SetMaterial(component.Id, "Air");
                }
                else
                {
                    context.SetMaterial(component.Id, component.Parameters["Material"]);
                }
            }
            UpdateFlows(context);
        }

        public void UpdateFlows(SimulationContext context)
        {
            var sources = context.System.Components.Where(component => component.Category == ComponentCategory.Source);

            var queue = new Queue<(string nodeId, string material)>();
            var visitedNodes = new HashSet<string>();

            foreach (var source in sources)
            {
                source.Parameters.TryGetValue("Material", out string? material);
                if (material != null) queue.Enqueue((source.Id, material));
            }

            while (queue.Count > 0)
            {
                (string currentNodeId, string material) = queue.Dequeue();
                if (visitedNodes.Contains(currentNodeId)) continue;
                visitedNodes.Add(currentNodeId);

                var connectedEdges = context.Graph.Edges.Where(e => e.ConnectedNodeIds.Contains(currentNodeId));

                foreach (var edge in connectedEdges)
                {
                    if (IsPathOpen(currentNodeId, edge, context))
                    {
                        context.SetMaterial(edge.Id, material);
                        context.SetMaterial(currentNodeId, material);

                        var neighborNodes = edge.ConnectedNodeIds.Where(id => id != currentNodeId);

                        foreach (var nextNodeId in neighborNodes)
                        {
                            if (material == "Air") context.SetMaterial(nextNodeId, material);
                            queue.Enqueue((nextNodeId, material));
                        }
                    }
                }
            }
        }

        private bool IsPathOpen(string currentNodeId, TopologyEdge edge, SimulationContext context)
        {
            var behavior = context.GetBehavior(currentNodeId);

            if (behavior == null) return true;
            if (behavior is TwoWayValveBehavior twoWay) return twoWay.IsOpen;
            if (behavior is ThreeWayValveBehavior threeWay)
            {
                var component = context.System.Components.First(component => component.Id == currentNodeId);

                component.Parameters.TryGetValue("DefaultEdge", out var defaultEdgeId);
                component.Parameters.TryGetValue("AlternativeEdge", out var altEdgeId);

                if ((string.IsNullOrEmpty(defaultEdgeId) && threeWay.IsDefaultPosition) || (string.IsNullOrEmpty(altEdgeId) && threeWay.IsAlternativePosition)) return false;
                if (edge.Id == defaultEdgeId) return threeWay.IsDefaultPosition;
                if (edge.Id == altEdgeId) return threeWay.IsAlternativePosition;

                return true;
            }

            return true;
        }
    }
}