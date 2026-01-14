using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.ManifoldServices;
using FluidSystems.Core.Constants;
using FluidSystems.Core.Models.Enums;
using FluidSystems.UI.WPF.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class EmptyingChamberViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly IManifoldService _manifoldService;

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

        public EmptyingChamberViewModel(SimulationContext context, IManifoldService manifoldService)
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
            _manifoldService = manifoldService;
        }

        public void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_context?.System?.Components == null) return;

                Sinks.Clear();
                Chambers.Clear();

                var sinks = _context.System.Components.Where(c => c.Category == ComponentCategory.Sink).Select(c => new ComponentItem(c.Id, c.Name));
                foreach (var item in sinks) Sinks.Add(item);

                var chambers = _context.System.Components.Where(c => c.Category == ComponentCategory.Container && c.SubType == FluidSystemContants.Chamber).Select(c => new ComponentItem(c.Id, c.Name));
                foreach (var item in chambers) Chambers.Add(item);

                if (Sinks.Count > 0) SelectedSink = Sinks.First();
                if (Chambers.Count > 0) SelectedChamber = Chambers.First();
            });
        }

        [RelayCommand(CanExecute = nameof(CanEmpty))]
        private async Task EmptyChamber()
        {
            IsBusy = true;
            StatusMessage = "";

            try
            {
                var result = await _manifoldService.DrainChamberAsync(SelectedChamber.Id, SelectedSink.Id);
                if (!result.IsSuccess) StatusMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}