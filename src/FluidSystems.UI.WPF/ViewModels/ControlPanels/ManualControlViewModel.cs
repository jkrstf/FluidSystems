using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.FluidSafetyValidators;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ManualControlViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly IFluidSafetyValidator _safetyValidator;
        private string _selectedComponentId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ActivateCommand))]
        private bool _hasBehavior;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ActivateCommand))]
        private bool _isBusy;
        [ObservableProperty] private string? _statusMessage;
        [ObservableProperty] private string? _description;

        private bool CanActivate => HasBehavior && !IsBusy;

        public ManualControlViewModel(SimulationContext context, IFluidSafetyValidator safetyValidator)
        {
            _context = context;
            _safetyValidator = safetyValidator;
        }


        public void Update(string selectedComopnentId)
        {
            _selectedComponentId = selectedComopnentId;
            IComponentBehavior? behavior = _context.GetBehavior(selectedComopnentId);
            HasBehavior = behavior != null;
            Description = behavior?.GetDescription() ?? "";
        }

        [RelayCommand(CanExecute = nameof(CanActivate))]
        private async Task Activate()
        {
            IsBusy = true;

            var validatorResult = _safetyValidator.ValidateToggle(_selectedComponentId, _context);
            if (!validatorResult.IsSuccess) StatusMessage = validatorResult.ErrorMessage;
            else
            {
                StatusMessage = "";
                _context.ActivateComponent(_selectedComponentId);
            }

            IsBusy = false;
        }
    }
}