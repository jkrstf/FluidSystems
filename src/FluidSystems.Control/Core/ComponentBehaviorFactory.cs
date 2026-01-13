using FluidSystems.Control.Behaviors;
using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Core
{
    public static class ComponentBehaviorFactory
    {
        public static IComponentBehavior? Create(FluidComponent component) => 
            component.Category switch
            {
                ComponentCategory.Valve when component.SubType == "TwoWay" => new TwoWayValveBehavior(),
                ComponentCategory.Valve when component.SubType == "ThreeWay" => new ThreeWayValveBehavior(),
                _ => null
            };
    }
}