using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;
using FluidSystems.Diagramming.Models.Enums;

namespace FluidSystems.Diagramming.Services.Routing
{
    public class ManhattanRouter : IDiagramConnectionRouter
    {
        private record PortInfo(PortSide Side, int Index, int Total);
        private record RouteCacheInfo(PortInfo Start, PortInfo End);
        private record SideKey(string NodeId, PortSide Side);

        private Dictionary<string, DiagramNode> _nodeLookup = new();
        private Dictionary<string, RouteCacheInfo> _routeCache = new();

        public void PreprocessNodes(IEnumerable<DiagramNode> nodes, IEnumerable<DiagramConnection> connections)
        {
            _nodeLookup = nodes.ToDictionary(n => n.ComponentId);
            _routeCache = new Dictionary<string, RouteCacheInfo>();

            var sideCounters = new Dictionary<SideKey, int>();
            var connectionSides = new List<(DiagramConnection Connection, PortSide Start, PortSide End)>();

            foreach (var conn in connections)
            {
                if (!TryGetNodes(conn, out var start, out var end)) continue;

                var sSide = DetermineSide(start, end);
                var eSide = DetermineSide(end, start);

                IncrementCounter(start.ComponentId, sSide, sideCounters);
                IncrementCounter(end.ComponentId, eSide, sideCounters);

                connectionSides.Add((conn, sSide, eSide));
            }

            var currentIndices = new Dictionary<SideKey, int>();
            foreach (var item in connectionSides)
            {
                var sKey = new SideKey(item.Connection.StartNodeId, item.Start);
                var eKey = new SideKey(item.Connection.EndNodeId, item.End);

                _routeCache[item.Connection.ComponentId] = new RouteCacheInfo(
                    new PortInfo(item.Start, GetNextIndex(sKey, currentIndices), sideCounters[sKey]),
                    new PortInfo(item.End, GetNextIndex(eKey, currentIndices), sideCounters[eKey])
                );
            }
        }

        public List<DiagramPoint> Route(DiagramConnection connection)
        {
            if (!_routeCache.TryGetValue(connection.ComponentId, out var info)) return new List<DiagramPoint>();

            var startNode = _nodeLookup[connection.StartNodeId];
            var endNode = _nodeLookup[connection.EndNodeId];

            var pStart = startNode.GetAnchorPoint(info.Start.Side, info.Start.Index, info.Start.Total);
            var pEnd = endNode.GetAnchorPoint(info.End.Side, info.End.Index, info.End.Total);

            return CalculateManhattanPath(pStart, pEnd, info.Start.Side, 4.0);
        }

        private List<DiagramPoint> CalculateManhattanPath(DiagramPoint start, DiagramPoint end, PortSide startSide, double tolerance)
        {
            var points = new List<DiagramPoint> { start };

            bool isCloseX = Math.Abs(start.X - end.X) < tolerance;
            bool isCloseY = Math.Abs(start.Y - end.Y) < tolerance;

            if (!isCloseX && !isCloseY)
            {
                if (startSide == PortSide.Top || startSide == PortSide.Bottom)
                {
                    double midY = start.Y + (end.Y - start.Y) / 3;
                    points.Add(new DiagramPoint { X = start.X, Y = midY });
                    points.Add(new DiagramPoint { X = end.X, Y = midY });
                }
                else
                {
                    double midX = start.X + (end.X - start.X) / 3;
                    points.Add(new DiagramPoint { X = midX, Y = start.Y });
                    points.Add(new DiagramPoint { X = midX, Y = end.Y });
                }
            }

            points.Add(end);
            return points;
        }

        private PortSide DetermineSide(DiagramNode source, DiagramNode target)
        {
            double dx = target.X - source.X;
            double dy = target.Y - source.Y;
            return Math.Abs(dx) < Math.Abs(dy) ? (dy > 0 ? PortSide.Bottom : PortSide.Top) : (dx > 0 ? PortSide.Right : PortSide.Left);
        }

        private void IncrementCounter(string id, PortSide side, Dictionary<SideKey, int> counters)
        {
            var key = new SideKey(id, side);
            counters[key] = counters.GetValueOrDefault(key, 0) + 1;
        }

        private int GetNextIndex(SideKey key, Dictionary<SideKey, int> counters)
        {
            int idx = counters.GetValueOrDefault(key, 0);
            counters[key] = idx + 1;
            return idx;
        }

        private bool TryGetNodes(DiagramConnection conn, out DiagramNode start, out DiagramNode end)
        {
            start = null;
            end = null;
            return conn.StartNodeId != null && conn.EndNodeId != null && _nodeLookup.TryGetValue(conn.StartNodeId, out start) && _nodeLookup.TryGetValue(conn.EndNodeId, out end);
        }
    }
}