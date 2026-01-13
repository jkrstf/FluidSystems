using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.UI.WPF.ViewModels.ControlPanels;
using FluidSystems.UI.WPF.ViewModels.Diagnostics;
using FluidSystems.UI.WPF.ViewModels.Diagrams;
using FluidSystems.UI.WPF.ViewModels.SystemLogs;

namespace FluidSystems.UI.WPF.ViewModels.Main
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty] private DiagramViewModel _diagramContent;
        [ObservableProperty] private DiagnosticsViewModel _diagnosticsViewModel;
        [ObservableProperty] private ControlPanelViewModel _controlPanelViewModel;
        [ObservableProperty] private LogsViewModel _logsViewModel;

        public HomeViewModel(DiagramViewModel diagramContent, DiagnosticsViewModel diagnosticsViewModel, ControlPanelViewModel controlPanelViewModel, LogsViewModel logsViewModel)
        {
            DiagramContent = diagramContent;
            DiagnosticsViewModel = diagnosticsViewModel;
            ControlPanelViewModel = controlPanelViewModel;
            LogsViewModel = logsViewModel;

            DiagramContent.ComponentSelected += OnComponentSelected;
        }

        private void OnComponentSelected(object? sender, string selectedComponentId)
        {
            DiagnosticsViewModel.OnComponentSelected(selectedComponentId);
            ControlPanelViewModel.OnComponentSelected(selectedComponentId);
        }
    }
}