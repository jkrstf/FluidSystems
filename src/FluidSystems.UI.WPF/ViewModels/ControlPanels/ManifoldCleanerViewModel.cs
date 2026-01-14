using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.ChamberHandling;
using FluidSystems.Core.Models.Enums;
using FluidSystems.UI.WPF.Models;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ManifoldCleanerViewModel : ObservableObject
    {
        private readonly SimulationContext _context;

        [NotifyCanExecuteChangedFor(nameof(CleanManifoldCommand))]
        [ObservableProperty]
        private ComponentItem? _selectedSink;
        [ObservableProperty] private string? _statusMessage;
        [ObservableProperty] private bool _isBusy;

        public ObservableCollection<ComponentItem> Sinks { get; } = new();

        private bool CanClean => SelectedSink != null && !IsBusy;

        public ManifoldCleanerViewModel(SimulationContext context)
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
        }

        private void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            Sinks.Clear();
            if (_context?.System?.Components == null) return;
            var sinks = _context.System.Components.Where(c => c.Category == ComponentCategory.Sink).Select(c => new ComponentItem(c.Id, c.Name));
            foreach (var sink in sinks) Sinks.Add(sink);
            if (Sinks.Count > 0) SelectedSink = Sinks.First();
        }

        [RelayCommand(CanExecute = nameof(CanClean))]
        private async Task CleanManifold()
        {
            IsBusy = true;

            var cleaner = new ManifoldCleaner();
            cleaner.CleanManifold(SelectedSink.Id, _context);

            IsBusy = false;
        }
    }
}