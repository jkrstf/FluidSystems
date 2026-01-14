using FluidSystems.Control.Core;
using FluidSystems.Control.Resources;
using FluidSystems.Core.Constants;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.Topology;
using FluidSystems.Shared.Common.Results;

namespace FluidSystems.Control.Services.FluidSafetyValidators
{
    public class FluidSafetyValidator : IFluidSafetyValidator
    {
        public Result<bool> ValidateToggle(string toggleComponentId, SimulationContext context)
        {
            var sources = context.System.Components
                .Where(c => c.Category == ComponentCategory.Source)
                .ToList();

            if (sources.Count <= 1) return Result<bool>.Success(true);

            foreach (var startSource in sources)
            {
                string startMaterial = context.FluidState.Materials[startSource.Id];

                if (CanReachAnotherSourceDirected(startSource.Id, toggleComponentId, startMaterial, context, out string message))
                {
                    return Result<bool>.Failure(message);
                }
            }

            return Result<bool>.Success(true);
        }

        private bool CanReachAnotherSourceDirected(string startNodeId, string toggleComponentId, string startMaterial, SimulationContext context, out string message)
        {
            message = "";

            var queue = new Queue<(string nodeId, string? fromEdgeId)>();
            var visited = new HashSet<(string nodeId, string? fromEdgeId)>();

            queue.Enqueue((startNodeId, null));

            while (queue.Count > 0)
            {
                var (currentNodeId, fromEdgeId) = queue.Dequeue();

                if (visited.Contains((currentNodeId, fromEdgeId))) continue;

                visited.Add((currentNodeId, fromEdgeId));

                if (currentNodeId != startNodeId &&
                    context.System.Components.Any(component => component.Id == currentNodeId && component.Category == ComponentCategory.Source))
                {
                    message = string.Format(Messages.MixingSourcesText, toggleComponentId);
                    return true;
                }

                var connectedEdges = context.Graph.Edges.Where(e => e.ConnectedNodeIds.Contains(currentNodeId));

                foreach (var edge in connectedEdges)
                {
                    if (edge.Id == fromEdgeId) continue;
                    if (!CanFlowThrough(currentNodeId, fromEdgeId, edge, toggleComponentId, context)) continue;

                    var edgeMaterial = context.FluidState.Materials[edge.Id];

                    if (edgeMaterial != startMaterial && startMaterial == FluidSystemContants.Air)
                    {
                        bool airInvolved = edgeMaterial == FluidSystemContants.Air || startMaterial == FluidSystemContants.Air;
                        if (!airInvolved)
                        {
                            message = string.Format(Messages.MixingFluidsText, toggleComponentId);
                            return true;
                        }
                        else if(!AirSourceHasSinkPath(toggleComponentId, context))
                        {
                            message = string.Format(Messages.SinkNotReachableText, toggleComponentId);
                            return true;
                        }
                    }

                    foreach (var nextNodeId in edge.ConnectedNodeIds.Where(id => id != currentNodeId))
                        queue.Enqueue((nextNodeId, edge.Id));
                }
            }

            return false;
        }

        private bool CanFlowThrough(string nodeId, string? fromEdgeId, TopologyEdge toEdge, string toggleComponentId, SimulationContext context)
        {
            if (fromEdgeId == null) return true;

            var behavior = context.GetBehavior(nodeId);
            if (behavior == null) return true;

            var component = context.System.Components.First(c => c.Id == nodeId);
            bool isTargetComponent = (nodeId == toggleComponentId);
            return behavior.IsPathActive(fromEdgeId, toEdge.Id, component, simulateToggle: isTargetComponent);
        }

        private bool AirSourceHasSinkPath(string toggleComponentId, SimulationContext context)
        {
            var airSources = context.System.Components.Where(component => component.Category == ComponentCategory.Source && context.FluidState.Materials[component.Id] == FluidSystemContants.Air);

            foreach (var source in airSources)
                if (CanReachSinkFromSource(source.Id, toggleComponentId, context)) return true;

            return false;
        }

        private bool CanReachSinkFromSource(string sourceId, string toggleComponentId, SimulationContext context)
        {
            var queue = new Queue<(string nodeId, string? fromEdgeId)>();
            var visited = new HashSet<(string nodeId, string? fromEdgeId)>();

            queue.Enqueue((sourceId, null));

            while (queue.Count > 0)
            {
                var (currentNodeId, fromEdgeId) = queue.Dequeue();

                if (!visited.Add((currentNodeId, fromEdgeId))) continue;
                if (context.System.Components.Any(component => component.Id == currentNodeId && component.Category == ComponentCategory.Sink)) return true;

                var edges = context.Graph.Edges.Where(e => e.ConnectedNodeIds.Contains(currentNodeId));

                foreach (var edge in edges)
                {
                    if (edge.Id == fromEdgeId) continue;
                    if (!CanFlowThrough(currentNodeId, fromEdgeId, edge, toggleComponentId, context)) continue;
                    foreach (var nextNodeId in edge.ConnectedNodeIds.Where(id => id != currentNodeId)) queue.Enqueue((nextNodeId, edge.Id));
                }
            }

            return false;
        }
    }
}