using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Control.Core;
using FluidSystems.Control.Services.ManifoldServices;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Services.ChamberHandling
{
    public class ManifoldCleaner : IManifoldCleaner
    {
        public void CleanManifold(string sinkComponentId, SimulationContext context)
        {
            CloseTwoWayValves(context);
            SetManifoldValvesToDefault(context);
            ConfigureWaterValve(sinkComponentId, context);
            context.ActivateComponent("valve_p1");
        }

        private void CloseTwoWayValves(SimulationContext context)
        {
            foreach (FluidComponent twoWayValve in context.System.Components.Where(component => component.Category == ComponentCategory.Valve && component.SubType == "TwoWay"))
            {
                if (context.GetBehavior(twoWayValve.Id) is TwoWayValveBehavior twoWay && twoWay.IsOpen)
                    context.ActivateComponent(twoWayValve.Id);
            }
        }

        private void SetManifoldValvesToDefault(SimulationContext context)
        {
            List<string> manifoldValves =
            [
                "valve_a1", "valve_a2", "valve_a3",
                "valve_b1", "valve_b2", "valve_b3"
            ];

            foreach (var valveId in manifoldValves)
                if (context.GetBehavior(valveId) is ThreeWayValveBehavior threeWay && threeWay.IsAlternativePosition)
                    context.ActivateComponent(valveId);
        }

        private void ConfigureWaterValve(string sinkComponentId, SimulationContext context)
        {
            const string valveId = "valve_w1";
            if (context.GetBehavior(valveId) is not ThreeWayValveBehavior valve) return;
            if (sinkComponentId == "source_hw" && valve.IsAlternativePosition) context.ActivateComponent(valveId);
            else context.ActivateComponent(valveId);
        }

    }
}