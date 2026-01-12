using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Diagramming.Services.Builders
{
    public class DiagramBuilderService : IDiagramBuilder
    {
        private IDiagramNodeBuilder _nodeBuilder;
        private IDiagramConnectionBuilder _connectionBuilder;
        private IDiagramConnectionRouter _router;

        public DiagramBuilderService(IDiagramNodeBuilder nodeBuilder, IDiagramConnectionBuilder connectionBuilder, IDiagramConnectionRouter router)
        {
            _nodeBuilder = nodeBuilder;
            _connectionBuilder = connectionBuilder;
            _router = router;
        }

        public SystemDiagram BuildDiagram(FluidSystem system, FluidSystemLayout layout, double maxWidth, double maxHeight)
        {
            var diagram = new SystemDiagram();

            var nodes = _nodeBuilder.CreateNodes(system, layout, maxWidth, maxHeight);
            diagram.Nodes.AddRange(nodes);

            var nodeLookup = nodes.ToDictionary(node => node.ComponentId);
            var connections = _connectionBuilder.CreateConnections(system, nodeLookup);

            _router.PreprocessNodes(nodes, connections);

            foreach (var connection in connections)
            {
                if (connection.VisualStyle == "CommonRail" || connection.VisualStyle == "CommonRailBranch") continue;
                var path = _router.Route(connection);
                connection.UpdatePath(path);
            }

            diagram.Connections.AddRange(connections);
            return diagram;
        }
    }
}