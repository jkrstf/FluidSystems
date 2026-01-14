using CommunityToolkit.Mvvm.ComponentModel;
using FluidSystems.Control.Core;
using FluidSystems.UI.WPF.Models;
using FluidSystems.UI.WPF.Resources;
using System.Collections.ObjectModel;

namespace FluidSystems.UI.WPF.ViewModels.SystemLogs
{
    public partial class LogsViewModel : ObservableObject
    {
        private readonly SimulationContext _context;
        private readonly object _lock = new();
        public ObservableCollection<LogModel> Logs { get; } = new();

        public LogsViewModel(SimulationContext context) 
        {
            _context = context;
            _context.Initialized += OnSimulationContextInitialized;
            _context.ComponentBehaviorChanged += OnComponentBehaviorChanged;
        }

        private void OnSimulationContextInitialized(object? sender, EventArgs e)
        {
            lock (_lock)
            {
                Logs.Clear();
            }
        }

        private void OnComponentBehaviorChanged(object? sender, string componentId)
        {
            lock (_lock)
            {
                Logs.Insert(0, new LogModel(string.Format(Texts.BehaviorChangedText, componentId, string.Join(", ", _context.GetBehavior(componentId)?.GetState().Select(kvp => $"{kvp.Key}: {kvp.Value}") ?? Array.Empty<string>()))));
            }
        }
    }
}
