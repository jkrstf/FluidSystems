using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.Control.Core;
using FluidSystems.UI.WPF.Resources;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.Diagnostics
{
    public partial class DiagnosticsViewModel : ObservableObject
    {
        private readonly SimulationContext _context;

        [ObservableProperty] private ObservableCollection<ParametersViewModel> _parametersList = new();

        public DiagnosticsViewModel(SimulationContext context)
        {
            _context = context;
            _context.ComponentBehaviorChanged += ComponentBehaviorChanged;
        }

        public void OnComponentSelected(string componentId)
        {
            ParametersList.Clear();

            var component = _context.System.Components.FirstOrDefault(c => c.Id == componentId);
            if (component == null) return;

            var summary = new ParametersViewModel();
            summary.UpdateParameters(Texts.ComponentInformation, new Dictionary<string, string>
            {
                { Texts.Name, $"{component.Name} ({component.Id})" },
                { Texts.Category, component.Category.ToString() },
                { Texts.SubType, component.SubType ?? "-" }
            }, boldKeys: new List<string> { Texts.Name });
            ParametersList.Add(summary);

            if (component.Parameters != null && component.Parameters.Count > 0)
            {
                var parametersVm = new ParametersViewModel();
                parametersVm.UpdateParameters(Texts.ComponentParameters, component.Parameters);
                ParametersList.Add(parametersVm);
            }

            var behaviorState = _context.GetBehavior(componentId)?.GetState();
            if (behaviorState != null && behaviorState.Count > 0)
            {
                var behaviorVm = new ParametersViewModel();
                behaviorVm.UpdateParameters(Texts.BehaviorParameters, behaviorState);
                ParametersList.Add(behaviorVm);
            }

            var materialValue = _context.FluidState.Materials.FirstOrDefault(p => p.Key == componentId).Value;
            if (materialValue != null)
            {
                var materialVm = new ParametersViewModel();
                var materialDict = new Dictionary<string, string>
                {
                    { Texts.Material, materialValue }
                };

                materialVm.UpdateParameters(Texts.FluidState, materialDict, boldKeys: new List<string> { Texts.Material });

                ParametersList.Add(materialVm);
            }
        }

        private void ComponentBehaviorChanged(object? sender, string componentId)
        {
            OnComponentSelected(componentId);
        }
    }
}