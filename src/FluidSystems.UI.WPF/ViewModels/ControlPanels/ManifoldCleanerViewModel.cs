using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.ManifoldServices;
using FluidSystems.Core.Models.Enums;
using FluidSystems.UI.WPF.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ManifoldCleanerViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly IManifoldService _manifoldService;

        [NotifyCanExecuteChangedFor(nameof(CleanManifoldCommand))]
        [ObservableProperty]
        private ComponentItem? _selectedSink;
        [ObservableProperty] private string? _statusMessage;
        [ObservableProperty] private bool _isBusy;

        public ObservableCollection<ComponentItem> Sinks { get; } = new();

        private bool CanClean => SelectedSink != null && !IsBusy;

        public ManifoldCleanerViewModel(SimulationContext context, IManifoldService manifoldService)
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
            _manifoldService = manifoldService;
        }

        private void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_context?.System?.Components == null) return;

                Sinks.Clear();
                var sinks = _context.System.Components.Where(c => c.Category == ComponentCategory.Sink).Select(c => new ComponentItem(c.Id, c.Name));
                foreach (var sink in sinks) Sinks.Add(sink);
                if (Sinks.Count > 0) SelectedSink = Sinks.First();
            });
        }

        [RelayCommand(CanExecute = nameof(CanClean))]
        private async Task CleanManifold()
        {
            IsBusy = true;
            StatusMessage = "";

            try
            {
                var fillResult = await _manifoldService.CleanManifoldAsync(SelectedSink.Id);
                StatusMessage = fillResult.ErrorMessage;
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