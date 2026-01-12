using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;

namespace FluidSystems.Diagramming.Services.Builders
{
    public class DiagramNodeBuilder : IDiagramNodeBuilder
    {
        public List<DiagramNode> CreateNodes(FluidSystem system, FluidSystemLayout layout, double maxWidth, double maxHeight)
        {
            var nodes = new List<DiagramNode>();
            
            (double sizeX, double sizeY) = CalculateGridSizes(layout, maxWidth, maxHeight);

            foreach (var entry in layout.Elements)
            {
                var component = system.Components.FirstOrDefault(component => component.Id == entry.Key);
                if (component == null) continue;

                var node = new DiagramNode { ComponentId = entry.Key };
                ApplyLayout(node, component, entry.Value, sizeX, sizeY);
                nodes.Add(node);
            }
            return nodes;
        }

        private (double sizeX, double sizeY) CalculateGridSizes(FluidSystemLayout layout, double width, double height) =>
            (width / layout.Settings.Columns, height / layout.Settings.Rows);

        private void ApplyLayout(DiagramNode node, FluidComponent component, FluidComponentLayout layoutInfo, double sizeX, double sizeY)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
            if (layoutInfo == null) throw new ArgumentNullException(nameof(layoutInfo));

            node.Label = component.Name;
            node.Category = component.Category;
            node.VisualStyle = component.SubType;
            node.X = layoutInfo.X * sizeX;
            node.Y = layoutInfo.Y * sizeY;
            node.Width = layoutInfo.ColumnSpan * sizeX;
            node.Height = layoutInfo.RowSpan * sizeY;
            node.Rotation = layoutInfo.Rotation;
            node.ZIndex = layoutInfo.ZIndex;
        }
    }
}