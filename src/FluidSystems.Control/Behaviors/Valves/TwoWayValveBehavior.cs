using FluidSystems.Control.Core;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Behaviors.Valves
{
    public class TwoWayValveBehavior : IComponentBehavior, IActivatableBehavior
    {
        private bool _isOpen;

        public bool IsOpen => _isOpen;
        public bool IsClosed => !_isOpen;
        public Dictionary<string, string> GetState() => new()
        {
            { "Status", IsOpen ? "Opened" : "Closed" }
        };

        public TwoWayValveBehavior(bool isOpen = false)
        {
            _isOpen = isOpen;
        }

        public void Activate(FluidComponent component, SimulationContext context)
        {
            _isOpen = !_isOpen;
        }
    }
}