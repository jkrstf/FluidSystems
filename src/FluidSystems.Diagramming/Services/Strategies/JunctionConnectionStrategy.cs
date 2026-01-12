using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Diagramming.Services.Strategies
{
    public class JunctionConnectionStrategy : IDiagramConnectionStrategy
    {
        public List<DiagramConnection> Build(FluidComponent component, Dictionary<string, DiagramNode> nodeLookup)
        {
            var connections = new List<DiagramConnection>();
            if (!nodeLookup.TryGetValue(component.Id, out var junctionNode)) return connections;

            foreach (var connector in component.Connectors)
            {
                if (!nodeLookup.TryGetValue(connector.ConnectedComponent.Id, out var targetNode)) continue;

                var connection = new DiagramConnection
                {
                    ComponentId = targetNode.ComponentId,
                    VisualStyle = "Default"
                };

                connection.ConnectNodes(junctionNode, targetNode);
                connections.Add(connection);
            }
            return connections;
        }
    }
}