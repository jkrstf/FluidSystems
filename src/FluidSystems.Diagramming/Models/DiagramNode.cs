using FluidSystems.Core.Models.Enums;
using FluidSystems.Diagramming.Models.Enums;

namespace FluidSystems.Diagramming.Models
{
    public class DiagramNode
    {
        public string ComponentId { get; set; }
        public ComponentCategory Category { get; set; }
        public string VisualStyle { get; set; }
        public string Label { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; }
        public int ZIndex { get; set; }

        public DiagramPoint GetAnchorPoint(PortSide side, int index = 0, int total = 1)
        {
            double ratio = (index + 1.0) / (total + 1.0);

            return side switch
            {
                PortSide.Top => new DiagramPoint { X = X + Width * ratio, Y = Y },
                PortSide.Bottom => new DiagramPoint { X = X + Width * ratio, Y = Y + Height },
                PortSide.Left => new DiagramPoint { X = X, Y = Y + Height * ratio },
                PortSide.Right => new DiagramPoint { X = X + Width, Y = Y + Height * ratio },
                _ => new DiagramPoint { X = X + Width / 2, Y = Y + Height / 2 }
            };
        }
    }
}