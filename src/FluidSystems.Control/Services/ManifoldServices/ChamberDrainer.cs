using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Control.Core;
using FluidSystems.Control.Resources;
using FluidSystems.Control.Services.ManifoldServices;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.System;
using FluidSystems.Shared.Common.Results;

namespace FluidSystems.Control.Services.ChamberHandling
{
    public class ChamberDrainer : IChamberDrainer
    {
        public Result<bool> DrainChamber(string chamberId, string sinkId, SimulationContext context)
        {
            if (IsMixingFluid(chamberId, context)) return Result<bool>.Failure(string.Format(Messages.MixingFluidsText, "drain chamber"));
            CloseTwoWayValves(context);
            SetManifoldValvesToDefault(context);
            ConfigureSinkValve(sinkId, context);
            context.ActivateComponent(GetChamberOutletValve(chamberId));
            context.ActivateComponent("valve_p1");
            return Result<bool>.Success(true);
        }

        private void CloseTwoWayValves(SimulationContext context)
        {
            foreach (FluidComponent twoWayValve in context.System.Components.Where(c => c.Category == ComponentCategory.Valve && c.SubType == "TwoWay"))
                if (context.GetBehavior(twoWayValve.Id) is TwoWayValveBehavior twoWay && twoWay.IsOpen) context.ActivateComponent(twoWayValve.Id);
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

        private void ConfigureSinkValve(string sinkId, SimulationContext context)
        {
            const string valveId = "valve_w1";
            if (context.GetBehavior(valveId) is not ThreeWayValveBehavior valve) return;

            if (sinkId == "source_hw" && valve.IsAlternativePosition) context.ActivateComponent(valveId);
            else context.ActivateComponent(valveId);
        }

        private string GetChamberOutletValve(string chamberId)
        {
            return chamberId switch
            {
                "cointainer_chamber1" => "valve_b1",
                "cointainer_chamber2" => "valve_b2",
                "cointainer_chamber3" => "valve_b3",
                _ => ""
            };
        }

        private bool IsMixingFluid(string chamberId, SimulationContext context)
        {
            List<string> manifoldValves =
            [
                "pipe_20", "pipe_21", "pipe_22", "pipe_23",
                "pipe_30", "pipe_31", "pipe_32", "pipe_33",
                "pipe_13", "pipe_14"
               
            ];
            string chamberMaterial = context.FluidState.Materials[chamberId];

            foreach (var valve in manifoldValves)
            {
                string pipeMaterial = context.FluidState.Materials[valve];
                if (pipeMaterial != "Air" && pipeMaterial != chamberMaterial) return true;
            }
            return false;
        }
    }
}