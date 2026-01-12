namespace FluidSystems.Diagramming.Models
{
    public class DiagramConnection
    {
        public string ComponentId { get; set; }
        public string StartNodeId { get; set; }
        public string EndNodeId { get; set; }
        public string Label { get; set; }
        public string VisualStyle { get; set; }
        public List<DiagramPoint> Vertices { get; private set; } = new();
        public DiagramPoint LabelPosition { get; private set; } = new();

        public void UpdatePath(IEnumerable<DiagramPoint> points)
        {
            Vertices.Clear();
            if (points != null)
            {
                Vertices.AddRange(points);
                CalculateLabelPosition();
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

        private void CalculateLabelPosition()
        {
            if (Vertices.Count < 2) return;

            var p1 = Vertices[0];
            var p2 = Vertices[1];

            double midX = (p1.X + p2.X) / 2;
            double midY = (p1.Y + p2.Y) / 2;

            bool isHorizontal = Math.Abs(p1.Y - p2.Y) < 1.0;

            double offsetX = 0;
            double offsetY = 0;

            if (isHorizontal)
            {
                offsetY = -15;
                offsetX = -5;
            }
            else
            {
                offsetX = 5;
            }

            LabelPosition = new DiagramPoint
            {
                X = midX + offsetX,
                Y = midY + offsetY
            };
        }
    }
}