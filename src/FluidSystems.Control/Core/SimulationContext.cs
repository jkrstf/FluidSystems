using FluidSystems.Control.Behaviors;
using FluidSystems.Control.Models;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Models.Topology;

namespace FluidSystems.Control.Core
{
    public class SimulationContext
    {
        private readonly Dictionary<string, IComponentBehavior> _behaviors = new();

        public TopologyGraph Graph { get; }
        public FluidSystem System { get; }
        public FluidState FluidState { get; }

        public event EventHandler<string>? ComponentStateChanged;
        public event EventHandler<string>? ComponentBehaviorChanged;

        public SimulationContext(FluidSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            var builder = new FluidTopologyGraphBuilder();
            Graph = builder.Build(system);

            FluidState = new FluidState();

            InitializeBehaviors();
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