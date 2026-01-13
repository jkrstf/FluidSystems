using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.Control.Core;
using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.UI.WPF.ViewModels.ControlPanels;
using FluidSystems.UI.WPF.ViewModels.Diagnostics;
using FluidSystems.UI.WPF.ViewModels.Diagrams;
using FluidSystems.UI.WPF.ViewModels.SystemLogs;

namespace FluidSystems.UI.WPF.ViewModels.Main
{
    public partial class HomeViewModel : ObservableObject
    {
        private FluidSystem _system;
        private FluidSystemLayout _layout;
        private SimulationContext _context;

        private ISimulationManager _simulationManager;
        private IDiagramBuilder _diagramBuilder;

        [ObservableProperty] private DiagramViewModel _diagramContent;
        [ObservableProperty] private DiagnosticsViewModel _diagnosticsViewModel;
        [ObservableProperty] private ControlPanelViewModel _controlPanelViewModel;
        [ObservableProperty] private LogsViewModel _logsViewModel;

        [ObservableProperty] private double _diagramWidth = 800;
        [ObservableProperty] private double _diagramHeight = 600;

        public HomeViewModel(DiagramViewModel diagramContent, DiagnosticsViewModel diagnosticsViewModel, ControlPanelViewModel controlPanelViewModel, LogsViewModel logsViewModel, IDiagramBuilder diagramBuilder, ISimulationManager simulationManager)
        {
            DiagramContent = diagramContent;
            DiagnosticsViewModel = diagnosticsViewModel;
            ControlPanelViewModel = controlPanelViewModel;
            LogsViewModel = logsViewModel;
            
            _diagramBuilder = diagramBuilder;
            _simulationManager = simulationManager;
        }

        internal void Update(FluidSystem system, FluidSystemLayout layout)
        {
            foreach (var node in DiagramContent?.Nodes ?? new()) node.ComponentSelected -= OnComponentSelected;

            _system = system;
            _layout = layout;

            var diagram = _diagramBuilder.BuildDiagram(_system, _layout, _diagramWidth, _diagramHeight);
             _context = new SimulationContext(_system);
            _simulationManager.SetContext(_context);
            _diagramContent.UpdateDiagram(diagram, _context);

            foreach (var node in _diagramContent.Nodes) node.ComponentSelected += OnComponentSelected;
        }

        private void OnComponentSelected(object? sender, string e) => DiagnosticsViewModel.LoadComponentData(e, _system, _context);
    }
}