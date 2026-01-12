using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Diagramming.Services.Strategies
{
    public class PipeConnectionStrategy : IDiagramConnectionStrategy
    {
        public List<DiagramConnection> Build(FluidComponent component, Dictionary<string, DiagramNode> nodeLookup)
        {
            var connections = new List<DiagramConnection>();
            if (component.Connectors == null || component.Connectors.Count < 2) return connections;

            for (int i = 0; i < component.Connectors.Count - 1; i++)
            {
                var startId = component.Connectors[i].ConnectedComponent.Id;
                var endId = component.Connectors[i + 1].ConnectedComponent.Id;

                if (!nodeLookup.TryGetValue(startId, out var startNode)) continue;
                if (!nodeLookup.TryGetValue(endId, out var endNode)) continue;

                var connection = new DiagramConnection
                {
                    ComponentId = component.Id,
                    Label = component.Name,
                    VisualStyle = "Default"
                };

                connection.ConnectNodes(startNode, endNode);
                connections.Add(connection);
            }
            return connections;
        }
    }
}