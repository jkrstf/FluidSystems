using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.Control.Core;
using FluidSystems.Diagramming.Models;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.Diagrams
{
    public partial class DiagramViewModel : ObservableObject
    {
        private SimulationContext _simulationContext;

        private readonly Dictionary<string, DiagramNodeViewModel> _nodeLookup = new();
        private readonly Dictionary<string, List<DiagramConnectionViewModel>> _connectionLookup = new();

        [ObservableProperty] private ObservableCollection<DiagramNodeViewModel> _nodes = new();
        [ObservableProperty] private ObservableCollection<DiagramConnectionViewModel> _connections = new();

        private void Context_ComponentStateChanged(object? sender, string id)
        {
            foreach (var node in _nodes.Where(node => node.ComponentId == id))
                if (_simulationContext.FluidState.Materials.ContainsKey(id)) node.UpdateMaterial(_simulationContext.FluidState.Materials[id]);
            foreach (var connection in _connections.Where(connection => connection.ComponentId == id))
                if (_simulationContext.FluidState.Materials.ContainsKey(id)) connection.UpdateMaterial(_simulationContext.FluidState.Materials[id]);
        }

        public void UpdateDiagram(SystemDiagram diagram, SimulationContext context)
        {
            _simulationContext = context;

            context.ComponentStateChanged -= Context_ComponentStateChanged;
            context.ComponentStateChanged += Context_ComponentStateChanged;

            Nodes.Clear();
            foreach (var nodeModel in diagram.Nodes) Nodes.Add(new DiagramNodeViewModel(nodeModel));

            Connections.Clear();
            foreach (var connModel in diagram.Connections) Connections.Add(new DiagramConnectionViewModel(connModel));
        }
    }
}