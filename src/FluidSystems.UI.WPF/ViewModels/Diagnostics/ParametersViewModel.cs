using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.UI.WPF.Models;

namespace FluidSystems.UI.WPF.ViewModels.Diagnostics
{
    public partial class ParametersViewModel : ObservableObject
    {
        [ObservableProperty] private string _title;
        [ObservableProperty] private List<ParameterItem> _items = new();

        public void UpdateParameters(string title, Dictionary<string, string> parameters, List<string> boldKeys = null)
        {
            Title = title.ToUpper();
            var boldSet = boldKeys?.ToHashSet() ?? new HashSet<string>();

            Items = parameters?.Select(p => new ParameterItem
            {
                Key = p.Key,
                Value = p.Value,
                IsBold = boldSet.Contains(p.Key)
            }).ToList() ?? new List<ParameterItem>();
        }
    }
}