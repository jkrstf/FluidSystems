using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.Control.Core;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Models;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.Diagrams
{
    public partial class DiagramViewModel : ObservableObject
    {
        private readonly IDiagramBuilder _diagramBuilder;
        private readonly SimulationContext _context;

        [ObservableProperty] private ObservableCollection<DiagramNodeViewModel> _nodes = new();
        [ObservableProperty] private ObservableCollection<DiagramConnectionViewModel> _connections = new();

        [ObservableProperty] private double _diagramWidth = 800;
        [ObservableProperty] private double _diagramHeight = 600;

        public EventHandler<string> ComponentSelected;

        public DiagramViewModel(SimulationContext context, IDiagramBuilder diagramBuilder)
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
            _diagramBuilder = diagramBuilder;
        }

        private void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            SystemDiagram diagram = _diagramBuilder.BuildDiagram(_context.System, _context.Layout, DiagramWidth, DiagramHeight);
            UpdateDiagram(diagram);

        }

        private void Context_ComponentStateChanged(object? sender, string id)
        {
            foreach (var node in _nodes.Where(node => node.ComponentId == id))
                if (_context.FluidState.Materials.ContainsKey(id)) node.UpdateMaterial(_context.FluidState.Materials[id]);
            foreach (var connection in _connections.Where(connection => connection.ComponentId == id))
                if (_context.FluidState.Materials.ContainsKey(id)) connection.UpdateMaterial(_context.FluidState.Materials[id]);
        }

        private void UpdateDiagram(SystemDiagram diagram)
        {
            _context.ComponentStateChanged -= Context_ComponentStateChanged;
            _context.ComponentStateChanged += Context_ComponentStateChanged;

            foreach (var node in Nodes) node.ComponentSelected -= OnComponentSelected;
            Nodes.Clear();
            foreach (var nodeModel in diagram.Nodes)
            {
                var vm = new DiagramNodeViewModel(nodeModel);
                vm.ComponentSelected += OnComponentSelected;
                Nodes.Add(vm);
            }

            Connections.Clear();
            foreach (var connModel in diagram.Connections) Connections.Add(new DiagramConnectionViewModel(connModel));
        }

        private void OnComponentSelected(object? sender, string componentId) => ComponentSelected?.Invoke(this, componentId);
    }
}