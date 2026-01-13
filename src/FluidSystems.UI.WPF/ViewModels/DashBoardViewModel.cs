using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.Control.Core;
using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;

namespace FluidSystems.UI.WPF.ViewModels
{
    public partial class DashBoardViewModel : ObservableObject
    {
        private FluidSystem _system;
        private FluidSystemLayout _layout;
        private SimulationContext _context;

        private ISimulationManager _simulationManager;
        private IDiagramBuilder _diagramBuilder;

        [ObservableProperty] private DiagramViewModel _diagramContent;
        [ObservableProperty] private ComponentOverviewViewModel _componentOverviewViewModel;

        public DashBoardViewModel(DiagramViewModel diagramContent, IDiagramBuilder diagramBuilder, ISimulationManager simulationManager)
        { 
            _diagramContent = diagramContent;
            _diagramBuilder = diagramBuilder;
            _simulationManager = simulationManager;
        }

        internal void Update(FluidSystem system, FluidSystemLayout layout)
        {
            _system = system;
            _layout = layout;

            var diagram = _diagramBuilder.BuildDiagram(_system, _layout, 800, 600);
             _context = new SimulationContext(_system);
            _simulationManager.SetContext(_context);
            _diagramContent.UpdateDiagram(diagram, _context);

            foreach (var node in _diagramContent.Nodes) node.ComponentSelected += componentSelected;
        }

        private void componentSelected(object? sender, string e)
        {
            ComponentOverviewViewModel = new ComponentOverviewViewModel(e, _context.FluidState.Materials.First(p => p.Key == e).Value, _system.Components.First(comp => comp.Id == e).Parameters, _context.GetBehavior(e).GetState());
        }
    }
}