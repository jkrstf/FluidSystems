using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class FillChamberViewModel : ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(FillChamberCommand))]
        [ObservableProperty]
        private string? _selectedLiquid;
        [NotifyCanExecuteChangedFor(nameof(FillChamberCommand))]
        [ObservableProperty] 
        private string? _selectedChamber;
        [ObservableProperty] private string? _statusMessage;
        [ObservableProperty] private bool _isBusy;

        public ObservableCollection<string> Liquids { get; } = new();
        public ObservableCollection<string> Chambers { get; } = new();

        private bool CanFill => !string.IsNullOrEmpty(SelectedLiquid) && !string.IsNullOrEmpty(SelectedChamber) && !IsBusy;

        public void Initialize(List<string> liquids, List<string> chambers)
        {
            foreach (var liquid in liquids) Liquids.Add(liquid);
            foreach (var chamber in chambers) Chambers.Add(chamber);
        }

        [RelayCommand(CanExecute = nameof(CanFill))]
        private async Task FillChamber()
        {
            IsBusy = true;

            IsBusy = false;
        }
    }
}