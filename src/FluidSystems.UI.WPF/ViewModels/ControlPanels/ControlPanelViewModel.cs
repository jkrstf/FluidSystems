using CommunityToolkit.Mvvm.ComponentModel;

namespace FluidSystems.UI.WPF.ViewModels.ControlPanels
{
    public partial class ControlPanelViewModel : ObservableObject
    {
        [ObservableProperty] private FillChamberViewModel _fillChamberViewModel;
        [ObservableProperty] private EmptyingChamberViewModel _emptyChamberViewModel;
        [ObservableProperty] private ManualControlViewModel _manualControlViewModel;

        public ControlPanelViewModel(FillChamberViewModel fillChamber, EmptyingChamberViewModel emptyChamber, ManualControlViewModel manualControl)
        {
            FillChamberViewModel = fillChamber;
            EmptyChamberViewModel = emptyChamber;
            ManualControlViewModel = manualControl;
        }

        public void OnComponentSelected(string selectedComopnentId)
        {
            ManualControlViewModel.Update(selectedComopnentId);
        }
    }
}