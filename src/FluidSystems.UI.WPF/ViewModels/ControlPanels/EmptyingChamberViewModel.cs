using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.ChamberHandling;
using FluidSystems.Control.Services.ManifoldServices;
using FluidSystems.Core.Models.Enums;
using FluidSystems.UI.WPF.Models;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class EmptyingChamberViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly IChamberDrainer _chamberDrainer;

        [NotifyCanExecuteChangedFor(nameof(EmptyChamberCommand))]
        [ObservableProperty]
        private ComponentItem? _selectedChamber;
        [NotifyCanExecuteChangedFor(nameof(EmptyChamberCommand))]
        [ObservableProperty]
        private ComponentItem? _selectedSink;
        [ObservableProperty] private string? _statusMessage;
        [ObservableProperty] private bool _isBusy;

        public ObservableCollection<ComponentItem> Chambers { get; } = new();
        public ObservableCollection<ComponentItem> Sinks { get; } = new();

        private bool CanEmpty => SelectedChamber != null && SelectedSink != null && !IsBusy;

        public EmptyingChamberViewModel(SimulationContext context, IChamberDrainer chamberDrainer)
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
            _chamberDrainer = chamberDrainer;
        }

        public void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            Sinks.Clear();
            Chambers.Clear();

            if (_context?.System?.Components == null) return;

            var sinks = _context.System.Components
                .Where(c => c.Category == ComponentCategory.Sink)
                .Select(c => new ComponentItem(c.Id, c.Name));

            foreach (var item in sinks) Sinks.Add(item);

            var chambers = _context.System.Components
                .Where(c => c.Category == ComponentCategory.Container && c.SubType == "Chamber")
                .Select(c => new ComponentItem(c.Id, c.Name));

            foreach (var item in chambers) Chambers.Add(item);

            if (Sinks.Count > 0) SelectedSink = Sinks.First();
            if (Chambers.Count > 0) SelectedChamber = Chambers.First();
        }

        [RelayCommand(CanExecute = nameof(CanEmpty))]
        private async Task EmptyChamber()
        {
            IsBusy = true;

            var fillResult = _chamberDrainer.DrainChamber(SelectedChamber.Id, SelectedSink.Id, _context);
            StatusMessage = fillResult.ErrorMessage;

            IsBusy = false;
        }
    }
}