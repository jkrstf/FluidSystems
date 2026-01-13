using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class EmptyingChamberViewModel : ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(EmptyChamberCommand))]
        [ObservableProperty]
        private string? _selectedChamber;
        [NotifyCanExecuteChangedFor(nameof(EmptyChamberCommand))]
        [ObservableProperty]
        private string? _selectedSink;
        [ObservableProperty] private string? _statusMessage;
        [ObservableProperty] private bool _isBusy;

        public ObservableCollection<string> Chambers { get; } = new();
        public ObservableCollection<string> Sinks { get; } = new();

        private bool CanEmpty => !string.IsNullOrEmpty(SelectedChamber) && !string.IsNullOrEmpty(SelectedSink) && !IsBusy;

        public void Initialize(List<string> chambers, List<string> sinks)
        {
            foreach (var chamber in chambers) Sinks.Add(chamber);
            foreach (var sink in sinks) Chambers.Add(sink);
        }

        [RelayCommand(CanExecute = nameof(CanEmpty))]
        private async Task EmptyChamber()
        {
            IsBusy = true;

            IsBusy = false;
        }
    }
}
