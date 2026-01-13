using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ManualControlViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private string _selectedComponentId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ActivateCommand))]
        private bool _hasBehavior;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ActivateCommand))]
        private bool _isBusy;
        [ObservableProperty] private string? _statusMessage;

        private bool CanActivate => HasBehavior && !IsBusy;

        public ManualControlViewModel(SimulationContext context)
        {
            _context = context;
        }


        public void Update(string selectedComopnentId)
        {
            _selectedComponentId = selectedComopnentId;
            HasBehavior = _context.GetBehavior(selectedComopnentId) != null;
        }

        [RelayCommand(CanExecute = nameof(CanActivate))]
        private async Task Activate()
        {
            IsBusy = true;

            _context.ActivateComponent(_selectedComponentId);

            IsBusy = false;
        }
    }
}