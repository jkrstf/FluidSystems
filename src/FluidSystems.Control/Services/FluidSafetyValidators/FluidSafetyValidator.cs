using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Control.Core;
using FluidSystems.Control.Resources;
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
                if (CanReachAnotherSource(startSource.Id, toggleComponentId, startMaterial, context, out string message))
                {
                    return Result<bool>.Failure(message);
                }
            }

            return Result<bool>.Success(true);
        }

        private bool CanReachAnotherSource(string startNodeId, string toggleComponentId, string startMaterial, SimulationContext context, out string message)
        {
            message = "";
            var queue = new Queue<string>();
            var visited = new HashSet<string>();

            queue.Enqueue(startNodeId);
            visited.Add(startNodeId);

            while (queue.Count > 0)
            {
                var currentNodeId = queue.Dequeue();

                if (currentNodeId != startNodeId &&
                    context.System.Components.Any(c => c.Id == currentNodeId && c.Category == ComponentCategory.Source))
                {
                    message = string.Format(Messages.MixingSourcesText, toggleComponentId);
                    return true;
                }



                var connectedEdges = context.Graph.Edges.Where(e => e.ConnectedNodeIds.Contains(currentNodeId));

                foreach (var edge in connectedEdges)
                {
                    string otherMaterial = context.FluidState.Materials[edge.Id];
                    if (IsPathOpenInPotentialState(currentNodeId, edge, toggleComponentId, context))
                    {
                        if (otherMaterial != startMaterial && (otherMaterial != "Air" && startMaterial != "Air"))
                        {
                            message = string.Format(Messages.MixingFluidsText, toggleComponentId);
                            return true;
                        }
                        var neighbors = edge.ConnectedNodeIds.Where(id => id != currentNodeId);
                        foreach (var nextNodeId in neighbors)
                        {
                            if (!visited.Contains(nextNodeId))
                            {
                                visited.Add(nextNodeId);
                                queue.Enqueue(nextNodeId);
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsPathOpenInPotentialState(string currentNodeId, TopologyEdge edge, string toggleComponentId, SimulationContext context)
        {
            var behavior = context.GetBehavior(currentNodeId);
            bool isToggledComponent = (currentNodeId == toggleComponentId);

            if (behavior == null) return true;

            if (behavior is TwoWayValveBehavior twoWay)
            {
                return isToggledComponent ? true : twoWay.IsOpen;
            }
            if (behavior is ThreeWayValveBehavior threeWay)
            {
                var component = context.System.Components.First(c => c.Id == currentNodeId);
                component.Parameters.TryGetValue("DefaultEdge", out var defaultEdgeId);
                component.Parameters.TryGetValue("AlternativeEdge", out var altEdgeId);

                bool checkDefault = isToggledComponent ? false : threeWay.IsDefaultPosition;
                bool checkAlt = isToggledComponent ? true : threeWay.IsAlternativePosition;

                if (edge.Id == defaultEdgeId) return checkDefault;
                if (edge.Id == altEdgeId) return checkAlt;

                return true;
            }

            return true;
        }
    }
}