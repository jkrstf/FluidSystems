using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.Diagramming.Models;
using System.Windows;
using System.Windows.Media;

namespace FluidSystems.UI.WPF.ViewModels.Diagrams
{
    public partial class DiagramConnectionViewModel : ObservableObject
    {
        public string ComponentId { get; private set; }

        [ObservableProperty] private Brush _color;
        [ObservableProperty] private string _label;
        [ObservableProperty] private double _labelX;
        [ObservableProperty] private double _labelY;
        [ObservableProperty] private string _visualStyle;
        [ObservableProperty] private PointCollection _pathPoints = new();

        public DiagramConnectionViewModel(DiagramConnection connection)
        {
            UpdateConnection(connection);
        }

        public void UpdateConnection(DiagramConnection connection)
        {
            ComponentId = connection.ComponentId;
            
            if (connection.Vertices == null || !connection.Vertices.Any()) PathPoints.Clear();
            PathPoints = [.. connection.Vertices.Select(v => new Point(v.X, v.Y))];

            Label = connection.Label;
            LabelX = connection.LabelPosition.X;
            LabelY = connection.LabelPosition.Y;
            VisualStyle = connection.VisualStyle;
            UpdateMaterial();
        }

        public void UpdateMaterial(string material = "")
        {
            Color = material switch
            {
                "Water" => Brushes.Blue,
                "Alcohol" => Brushes.Green,
                "Air" => Brushes.Orange,
                _ => Brushes.Black
            };
        }
    }
}