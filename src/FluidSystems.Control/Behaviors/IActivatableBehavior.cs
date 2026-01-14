using FluidSystems.Control.Core;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Behaviors
{
    internal interface IActivatableBehavior
    {
        void Activate(FluidComponent component, SimulationContext context);
    }
}
