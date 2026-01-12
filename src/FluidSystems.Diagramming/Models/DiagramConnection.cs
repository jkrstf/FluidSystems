namespace FluidSystems.Diagramming.Models
{
    public class DiagramConnection
    {
        public string ComponentId { get; set; }
        public string StartNodeId { get; set; }
        public string EndNodeId { get; set; }
        public string VisualStyle { get; set; }
        public double StartX => Vertices.FirstOrDefault()?.X ?? 0;
        public double StartY => Vertices.FirstOrDefault()?.Y ?? 0;
        public double EndX => Vertices.LastOrDefault()?.X ?? 0;
        public double EndY => Vertices.LastOrDefault()?.Y ?? 0;
        public List<DiagramPoint> Vertices { get; private set; } = new();

        public void UpdatePath(IEnumerable<DiagramPoint> points)
        {
            Vertices.Clear();
            if (points != null)
            {
                Vertices.AddRange(points);
            }
        }

        public void ConnectNodes(DiagramNode startNode, DiagramNode endNode)
        {
            if (startNode == null || endNode == null) return;

            StartNodeId = startNode.ComponentId;
            EndNodeId = endNode.ComponentId;

            UpdatePath(new List<DiagramPoint>
            {
                new DiagramPoint { X = startNode.X, Y = startNode.Y },
                new DiagramPoint { X = endNode.X, Y = endNode.Y }
            });
        }
    }
}