using CommunityToolkit.Mvvm.ComponentModel;

namespace FluidSystems.UI.WPF.ViewModels
{
    public partial class ComponentOverviewViewModel : ObservableObject
    {
        [ObservableProperty] private string? _selectedComponentId;
        [ObservableProperty] private string? _material;
        [ObservableProperty] private Dictionary<string, string>? _componentParameters;
        [ObservableProperty] private Dictionary<string, string>? _behaviorParameters;

        public ComponentOverviewViewModel(string componentId, string material, Dictionary<string, string>? componentParameters, Dictionary<string, string>? behaviorParameters)
        {
            SelectedComponentId = componentId;
            Material = material;
            ComponentParameters = componentParameters;
            BehaviorParameters = behaviorParameters;
        }
    }
}
