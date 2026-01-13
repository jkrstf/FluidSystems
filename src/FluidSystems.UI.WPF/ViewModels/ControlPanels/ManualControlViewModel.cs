using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ManualControlViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ActivateCommand))]
        private bool _hasBehavior;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ActivateCommand))]
        private bool _isBusy;
        [ObservableProperty] private string? _statusMessage;

        private bool CanActivate => HasBehavior && !IsBusy;

        public void Initialize()
        {
        }

        [RelayCommand(CanExecute = nameof(CanActivate))]
        private async Task Activate()
        {
            IsBusy = true;

            IsBusy = false;
        }
    }
}