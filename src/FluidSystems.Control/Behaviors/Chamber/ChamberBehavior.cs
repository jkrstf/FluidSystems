using FluidSystems.Control.Core;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Behaviors.Chamber
{
    public class ChamberBehavior : IComponentBehavior, IActivatableBehavior
    {
        private bool _isFilling;

        public bool IsFilling => _isFilling;
        public Dictionary<string, string> GetState() => new()
        {
            { "Operation", IsFilling ? "Filling" : "Emptying" }
        };

        public ChamberBehavior(bool isFilling = true)
        {
            _isFilling = isFilling;
        }

        public void Activate(FluidComponent component, SimulationContext context)
        {
            _isFilling = !_isFilling;
        }

    }
}