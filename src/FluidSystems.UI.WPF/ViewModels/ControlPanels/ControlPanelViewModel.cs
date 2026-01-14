using CommunityToolkit.Mvvm.ComponentModel;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ControlPanelViewModel : ObservableObject
    {
        [ObservableProperty] private FillChamberViewModel _fillChamberViewModel;
        [ObservableProperty] private EmptyingChamberViewModel _emptyChamberViewModel;
        [ObservableProperty] private ManualControlViewModel _manualControlViewModel;
        [ObservableProperty] private ManifoldCleanerViewModel _manifoldCleanerViewModel;

        public ControlPanelViewModel(FillChamberViewModel fillChamber, EmptyingChamberViewModel emptyChamber, ManualControlViewModel manualControl, ManifoldCleanerViewModel manifoldCleaner)
        {
            FillChamberViewModel = fillChamber;
            EmptyChamberViewModel = emptyChamber;
            ManualControlViewModel = manualControl;
            ManifoldCleanerViewModel = manifoldCleaner;
        }

        public void OnComponentSelected(string selectedComopnentId)
        {
            ManualControlViewModel.Update(selectedComopnentId);
        }
    }
}