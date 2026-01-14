using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.ManifoldServices;
using FluidSystems.Core.Models.Enums;
using FluidSystems.UI.WPF.Models;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class FillChamberViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly IChamberFiller _chamberFiller;

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

        public FillChamberViewModel(SimulationContext context, IChamberFiller chamberFiller)
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
            _chamberFiller = chamberFiller;
        }

        private void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            Liquids.Clear();
            Chambers.Clear();

            if (_context?.System?.Components == null) return;

            var liquidSupplies = _context.System.Components
                .Where(c => c.Category == ComponentCategory.Source && c.SubType == "LiquidSupply")
                .Select(c => new ComponentItem(c.Id, c.Name));

            foreach (var item in liquidSupplies) Liquids.Add(item);

            var chambers = _context.System.Components
                .Where(c => c.Category == ComponentCategory.Container && c.SubType == "Chamber")
                .Select(c => new ComponentItem(c.Id, c.Name));

            foreach (var item in chambers) Chambers.Add(item);

            if (Liquids.Count > 0) SelectedLiquid = Liquids.First();
            if (Chambers.Count > 0) SelectedChamber = Chambers.First();
        }

        [RelayCommand(CanExecute = nameof(CanFill))]
        private async Task FillChamber()
        {
            IsBusy = true;

            var fillResult = _chamberFiller.FillChamber(SelectedLiquid.Id, SelectedChamber.Id, _context);
            StatusMessage = fillResult.ErrorMessage;

            IsBusy = false;
        }
    }
}