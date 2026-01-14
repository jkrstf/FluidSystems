using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Control.Core;
using FluidSystems.Control.Resources;
using FluidSystems.Control.Services.ManifoldServices;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.System;
using FluidSystems.Shared.Common.Results;

namespace FluidSystems.Control.Services.ChamberHandling
{
    public class ChamberFiller : IChamberFiller
    {
        public Result<bool> FillChamber(string startComponentId, string endComponentId, SimulationContext context)
        {
            CloseTwoWayValves(context);
            if (IsMixDetected(GetRouteToChamber(startComponentId, endComponentId), GetMaterial(startComponentId, context), context))
            {
                return Result<bool>.Failure(string.Format(Messages.MixingFluidsText, "fill chamber"));
            }
            ConfigureValves(endComponentId, context);
            context.ActivateComponent(GetSourceValve(startComponentId));
            return Result<bool>.Success(true);
        }

        private void CloseTwoWayValves(SimulationContext context)
        {
            foreach (FluidComponent twoWayValve in context.System.Components.Where(component => component.Category == ComponentCategory.Valve && component.SubType == "TwoWay"))
            {
                if (context.GetBehavior(twoWayValve.Id) is TwoWayValveBehavior twoWay && twoWay.IsOpen)
                    context.ActivateComponent(twoWayValve.Id);
            }
        }

        private List<string> GetRouteToChamber(string startComponentId, string endComponentId)
        {
            var route = new List<string>();

            route.AddRange(GetSourceSegment(startComponentId));
            route.AddRange(GetTargetSegment(endComponentId));

            return route;
        }

        private bool IsMixDetected(List<string> route, string startMaterial, SimulationContext context)
        {
            foreach (var componentId in route)
            {
                var material = GetMaterial(componentId, context);
                if (material != startMaterial && material != "Air") return true;
            }
            return false;
        }

        private string GetMaterial(string componentId, SimulationContext context)
        {
            return context.FluidState.Materials[componentId];
        }

        private void ConfigureValves(string endComponentId, SimulationContext context)
        {
            var (requiredDefaults, requiredAlternates) = GetValveConfiguration(endComponentId);

            foreach (var valveId in requiredDefaults)
                if (context.GetBehavior(valveId) is ThreeWayValveBehavior threeWay && threeWay.IsAlternativePosition)
                    context.ActivateComponent(valveId);

            foreach (var valveId in requiredAlternates)
                if (context.GetBehavior(valveId) is ThreeWayValveBehavior threeWay && threeWay.IsDefaultPosition)
                    context.ActivateComponent(valveId);
        }

        private IEnumerable<string> GetSourceSegment(string sourceId)
        {
            return sourceId switch
            {
                "source_air" => ["pipe_10", "valve_p1"],
                "source_alcohol" => ["pipe_11", "valve_p2"],
                "source_water" => ["pipe_12", "valve_p3"],
                _ => new List<string>()
            };
        }

        private IEnumerable<string> GetTargetSegment(string chamberId)
        {
            return chamberId switch
            {
                "cointainer_chamber1" =>
                [
                    "valve_a3", "pipe_31", 
                    "valve_a2", "pipe_30",
                    "valve_a1", "pipe_40", 
                    "pipe_41", 
                    "valve_b1"
                ],
                "cointainer_chamber2" =>
                [
                    "valve_a3", "pipe_31", 
                    "valve_a2", "pipe_42",
                    "pipe_43", 
                    "valve_b2"
                ],
                "cointainer_chamber3" =>
                [
                    "valve_a3", "pipe_44",
                    "pipe_45", 
                    "valve_b3"
                ],
                _ => new List<string>()
            };
        }
        private (List<string> Defaults, List<string> Alternates) GetValveConfiguration(string endComponent)
        {
            return endComponent switch
            {
                "cointainer_chamber3" => (
                    new List<string>() { "valve_b3" },
                    new List<string> { "valve_a3" }
                ),
                "cointainer_chamber2" => (
                    new List<string> { "valve_a3", "valve_b2" },
                    new List<string> { "valve_a2" }
                ),
                "cointainer_chamber1" => (
                    new List<string> { "valve_a3", "valve_a2", "valve_b1" },
                    new List<string> { "valve_a1" }
                ),
                _ => (new List<string>(), new List<string>())
            };
        }
       
        private string GetSourceValve(string sourceId)
        {
            return sourceId switch
            {
                "source_air" => "valve_p1",
                "source_alcohol" => "valve_p2",
                "source_water" => "valve_p3",
                _ => ""
            };
        }
    }
}