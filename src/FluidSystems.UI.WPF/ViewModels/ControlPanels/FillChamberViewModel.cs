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
    public partial class FillChamberViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly IManifoldService _manifoldService;

        [NotifyCanExecuteChangedFor(nameof(FillChamberCommand))]
        [ObservableProperty] 
        private ComponentItem? _selectedLiquid;
        [NotifyCanExecuteChangedFor(nameof(FillChamberCommand))]
        [ObservableProperty] 
        private ComponentItem? _selectedChamber;
        [ObservableProperty] private string? _statusMessage;
        [ObservableProperty] private bool _isBusy;

        public ObservableCollection<ComponentItem> Liquids { get; } = new();
        public ObservableCollection<ComponentItem> Chambers { get; } = new();

        private bool CanFill => SelectedLiquid != null && SelectedChamber != null && !IsBusy;

        public FillChamberViewModel(SimulationContext context, IManifoldService manifoldService)
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
            _manifoldService = manifoldService;
        }

        private void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                if (_context?.System?.Components == null) return;

                Liquids.Clear();
                Chambers.Clear();

                var liquidSupplies = _context.System.Components.Where(c => c.Category == ComponentCategory.Source && c.SubType == FluidSystemContants.LiquidSupply).Select(c => new ComponentItem(c.Id, c.Name));
                foreach (var item in liquidSupplies) Liquids.Add(item);

                var chambers = _context.System.Components.Where(c => c.Category == ComponentCategory.Container && c.SubType == FluidSystemContants.Chamber).Select(c => new ComponentItem(c.Id, c.Name));
                foreach (var item in chambers) Chambers.Add(item);

                if (Liquids.Count > 0) SelectedLiquid = Liquids.First();
                if (Chambers.Count > 0) SelectedChamber = Chambers.First();
            });
        }

        [RelayCommand(CanExecute = nameof(CanFill))]
        private async Task FillChamber()
        {
            IsBusy = true;
            StatusMessage = "";

            try
            {
                var fillResult = await _manifoldService.FillChamberAsync(SelectedLiquid.Id, SelectedChamber.Id);
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