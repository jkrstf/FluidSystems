using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;
using FluidSystems.Diagramming.Models.Enums;

namespace FluidSystems.Diagramming.Services.Strategies
{
    public class CommonRailConnectionStrategy : IDiagramConnectionStrategy
    {
        private record RailContext(bool IsVertical, double Coordinate);
        private record NodeSideInfo(DiagramNode Node, PortSide Side);

        public List<DiagramConnection> Build(FluidComponent component, Dictionary<string, DiagramNode> nodeLookup)
        {
            var connectedNodes = component.Connectors
                .Select(c => nodeLookup.TryGetValue(c.ConnectedComponent.Id, out var node) ? node : null)
                .Where(n => n != null)
                .Cast<DiagramNode>()
                .ToList();

            if (connectedNodes.Count < 2) return new List<DiagramConnection>();

            var context = CreateRailContext(connectedNodes);
            var nodeInfos = connectedNodes.Select(n => new NodeSideInfo(n, DeterminePortSide(n, context))).ToList();
            var sideStats = nodeInfos.GroupBy(x => (x.Node.ComponentId, x.Side)).ToDictionary(g => g.Key, g => g.Count());

            var connections = new List<DiagramConnection>
            {
                CreateMainRail(component.Id, connectedNodes, context)
            };

            var currentIndices = new Dictionary<(string, PortSide), int>();
            foreach (var info in nodeInfos)
            {
                var key = (info.Node.ComponentId, info.Side);
                int currentIndex = currentIndices.GetValueOrDefault(key, 0);

                connections.Add(CreateBranch(component.Id, info, context, currentIndex, sideStats[key]));

                currentIndices[key] = currentIndex + 1;
            }

            return connections;
        }

        private RailContext CreateRailContext(List<DiagramNode> nodes)
        {
            bool isVertical = (nodes.Max(n => n.Y + n.Height) - nodes.Min(n => n.Y)) > (nodes.Max(n => n.X + n.Width) - nodes.Min(n => n.X));
            double coordinate = isVertical ? (nodes.Min(n => n.X) + nodes.Max(n => n.X + n.Width)) / 2 : (nodes.Min(n => n.Y) + nodes.Max(n => n.Y + n.Height)) / 2;
            return new RailContext(isVertical, coordinate);
        }

        private DiagramConnection CreateMainRail(string componentId, List<DiagramNode> nodes, RailContext ctx)
        {
            double start = nodes.Min(n => ctx.IsVertical ? n.Y + n.Height / 2 : n.X + n.Width / 2);
            double end = nodes.Max(n => ctx.IsVertical ? n.Y + n.Height / 2 : n.X + n.Width / 2);

            var points = new List<DiagramPoint>
            {
                ctx.IsVertical ? new DiagramPoint { X = ctx.Coordinate, Y = start } : new DiagramPoint { X = start, Y = ctx.Coordinate },
                ctx.IsVertical ? new DiagramPoint { X = ctx.Coordinate, Y = end } : new DiagramPoint { X = end, Y = ctx.Coordinate }
            };

            var rail = new DiagramConnection { ComponentId = componentId, VisualStyle = "CommonRail" };
            rail.UpdatePath(points);
            return rail;
        }

        private DiagramConnection CreateBranch(string componentId, NodeSideInfo info, RailContext ctx, int index, int total)
        {
            var startPoint = info.Node.GetAnchorPoint(info.Side, index, total);
            var endPoint = ctx.IsVertical ? new DiagramPoint { X = ctx.Coordinate, Y = startPoint.Y } : new DiagramPoint { X = startPoint.X, Y = ctx.Coordinate };

            var branch = new DiagramConnection { ComponentId = componentId, VisualStyle = "CommonRailBranch" };
            branch.UpdatePath(new List<DiagramPoint> { startPoint, endPoint });
            return branch;
        }

        private PortSide DeterminePortSide(DiagramNode node, RailContext ctx)
        {
            double nodeMid = ctx.IsVertical ? node.X + node.Width / 2 : node.Y + node.Height / 2;

            if (ctx.IsVertical) return nodeMid > ctx.Coordinate ? PortSide.Left : PortSide.Right;
            return nodeMid > ctx.Coordinate ? PortSide.Top : PortSide.Bottom;
        }
    }
}