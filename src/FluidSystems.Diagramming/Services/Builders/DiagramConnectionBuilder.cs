using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Diagramming.Services.Builders
{
    public class DiagramConnectionBuilder : IDiagramConnectionBuilder
    {
        private readonly Dictionary<string, IDiagramConnectionStrategy> _strategies;

        public DiagramConnectionBuilder(Dictionary<string, IDiagramConnectionStrategy> strategies)
        {
            _strategies = strategies;
        }

        public List<DiagramConnection> CreateConnections(FluidSystem system, Dictionary<string, DiagramNode> nodeLookup)
        {
            var connections = new List<DiagramConnection>();
            foreach (var component in system.Components)
            {
                if (_strategies.TryGetValue(component.SubType, out var strategy))
                {
                    connections.AddRange(strategy.Build(component, nodeLookup));
                }
            }
            return connections;
        }
    }
}