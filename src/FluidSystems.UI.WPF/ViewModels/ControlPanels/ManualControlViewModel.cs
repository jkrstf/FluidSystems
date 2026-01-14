using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Behaviors;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.ManifoldServices;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ManualControlViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly IManifoldService _manifoldService;
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

        public ManualControlViewModel(SimulationContext context, IManifoldService manifoldService)
        {
            _context = context;
            _manifoldService = manifoldService;
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
            StatusMessage = "";

            try
            {
                var fillResult = await _manifoldService.ToggleComponentAsync(_selectedComponentId);
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