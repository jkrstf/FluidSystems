using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Diagramming.Models;
using System.Windows.Media;

namespace FluidSystems.UI.WPF.ViewModels.Diagrams
{
    public partial class DiagramNodeViewModel : ObservableObject
    {
        public string ComponentId { get; private set; }

        [ObservableProperty] private bool _isSelected;
        [ObservableProperty] private Brush _color;
        [ObservableProperty] private double _x;
        [ObservableProperty] private double _y;
        [ObservableProperty] private double _width;
        [ObservableProperty] private double _height;
        [ObservableProperty] private string _label;
        [ObservableProperty] private string _parameters;
        [ObservableProperty] private string _visualStyle;

        public EventHandler<string> ComponentSelected;

        public DiagramNodeViewModel(DiagramNode diagramNode)
        {
            UpdateNode(diagramNode);
            UpdateMaterial();
        }

        public void UpdateNode(DiagramNode node)
        {
            ComponentId = node.ComponentId;
            X = node.X;
            Y = node.Y;
            Width = node.Width;
            Height = node.Height;
            Label = node.Label;
            VisualStyle = node.VisualStyle;
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

        public void UpdateParameters(string parameters)
        {
            Parameters = parameters;
        }

        [RelayCommand]
        private void ToggleComponent(string? id) => ComponentSelected?.Invoke(this, id);
    }
}