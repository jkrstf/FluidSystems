using FluidSystems.Control.Behaviors;
using FluidSystems.Control.Models;
using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Models.Topology;

namespace FluidSystems.Control.Core
{
    public class SimulationContext
    {
        private readonly ISimulationManager _manager;
        private readonly Dictionary<string, IComponentBehavior> _behaviors = new();

        public TopologyGraph Graph { get; private set; }
        public FluidSystem System { get; private set; }
        public FluidSystemLayout Layout { get; private set; }
        public FluidState FluidState { get; private set; }

        public event EventHandler Initialized;
        public event EventHandler<string>? ComponentStateChanged;
        public event EventHandler<string>? ComponentBehaviorChanged;

        public SimulationContext(ISimulationManager manager)
        {
            _manager = manager;
        }

        public void Initialize(FluidSystem system, FluidSystemLayout layout)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            Layout = layout ?? throw new ArgumentNullException(nameof(layout));

            var builder = new FluidTopologyGraphBuilder();
            Graph = builder.Build(system);

            FluidState = new FluidState();

            InitializeBehaviors();
            _manager.SetContext(this);

            Initialized?.Invoke(this, null);
        }

        private void InitializeBehaviors()
        {
            foreach (var component in System.Components)
            {
                var behavior = ComponentBehaviorFactory.Create(component);
                if (behavior != null)
                {
                    _behaviors.Add(component.Id, behavior);
                }
            }
        }

        public IComponentBehavior? GetBehavior(string componentId)  => _behaviors.TryGetValue(componentId, out var b) ? b : null;

        public void ActivateComponent(string id)
        {
            if (!_behaviors.TryGetValue(id, out var behavior)) return;

            var component = System.Components.FirstOrDefault(c => c.Id == id);

            if (component != null && behavior is IActivatableBehavior activatable)
            {
                activatable.Activate(component, this);
                ComponentBehaviorChanged?.Invoke(this, id);
            }
        }

        public void SetMaterial(string id, string? material)
        {
            FluidState.Materials[id] = material;
            ComponentStateChanged?.Invoke(this, id);
        }
    }
}