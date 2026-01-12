using FluidSystems.Control.Core;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Behaviors.Valves
{
    public class ThreeWayValveBehavior : IComponentBehavior, IActivatableBehavior
    {
        private bool _isDefaultPosition;

        public bool IsDefaultPosition => _isDefaultPosition;
        public bool IsAlternativePosition => !_isDefaultPosition;
        public Dictionary<string, string> GetState() => new()
        {
            { "Routing", IsDefaultPosition ? "Towards default position" : "Towards alternative position" }
        };

        public ThreeWayValveBehavior(bool isDefaultPosition = true)
        {
            _isDefaultPosition = isDefaultPosition;
        }

        public void Activate(FluidComponent component, SimulationContext context)
        {
            _isDefaultPosition = !_isDefaultPosition;
        }
    }
}