using FluidSystems.Control.Behaviors;
using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Core.Constants;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Core
{
    public static class ComponentBehaviorFactory
    {
        public static IComponentBehavior? Create(FluidComponent component) => 
            component.Category switch
            {
                ComponentCategory.Valve when component.SubType == FluidSystemContants.TwoWayValve => new TwoWayValveBehavior(),
                ComponentCategory.Valve when component.SubType == FluidSystemContants.ThreeWayValve => new ThreeWayValveBehavior(),
                _ => null
            };
    }
}