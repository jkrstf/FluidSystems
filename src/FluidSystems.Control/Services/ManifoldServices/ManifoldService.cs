using FluidSystems.Control.Behaviors.Valves;
using FluidSystems.Control.Core;
using FluidSystems.Control.Resources;
using FluidSystems.Control.Services.FluidSafetyValidators;
using FluidSystems.Core.Constants;
using FluidSystems.Core.Models.Enums;
using FluidSystems.Shared.Common.Results;
using System.Threading.Tasks;

namespace FluidSystems.Control.Services.ManifoldServices
{
    public class ManifoldService : IManifoldService
    {
        private readonly SimulationContext _context;
        private readonly IFluidSafetyValidator _fluidSafetyValidator;

        public ManifoldService(SimulationContext context, IFluidSafetyValidator safetyValidator)
        {
            _context = context;
            _fluidSafetyValidator = safetyValidator;
        }

        public Task<Result<bool>> FillChamberAsync(string startComponentId, string endComponentId, CancellationToken cancellationToken = default)
        {
            CloseTwoWayValves();

            var route = GetRouteToChamber(startComponentId, endComponentId);
            var sourceMaterial = GetMaterial(startComponentId);

            if (IsMixDetected(route, sourceMaterial))
            {
                return Task.FromResult(Result<bool>.Failure(string.Format(Messages.MixingFluidsText, "fill chamber")));
            }

            ConfigureValvesForFilling(endComponentId);
            _context.ActivateComponent(GetSourceValve(startComponentId));

            return Task.FromResult(Result<bool>.Success(true));
        }

        public Task<Result<bool>> DrainChamberAsync(string chamberId, string sinkId, CancellationToken cancellationToken = default)
        {
            if (IsChamberMixingFluid(chamberId))
            {
                return Task.FromResult(Result<bool>.Failure(string.Format(Messages.MixingFluidsText, "drain chamber")));
            }

            CloseTwoWayValves();
            SetManifoldValvesToDefault();
            ConfigureSinkValve(sinkId);

            _context.ActivateComponent(GetChamberOutletValve(chamberId));
            _context.ActivateComponent("valve_p1");

            return Task.FromResult(Result<bool>.Success(true));
        }

        public Task<Result<bool>> CleanManifoldAsync(string sinkComponentId, CancellationToken cancellationToken = default)
        {
            CloseTwoWayValves();
            SetManifoldValvesToDefault();
            ConfigureSinkValve(sinkComponentId);
            _context.ActivateComponent("valve_p1");
            return Task.FromResult(Result<bool>.Success(true));
        }

        public Task<Result<bool>> ToggleComponentAsync(string componentId, CancellationToken cancellationToken = default)
        {
            var validationResult = _fluidSafetyValidator.ValidateToggle(componentId, _context);
            if (!validationResult.IsSuccess) return Task.FromResult(Result<bool>.Failure(validationResult.ErrorMessage));

            _context.ActivateComponent(componentId);

            return Task.FromResult(Result<bool>.Success(true));
        }

        private void CloseTwoWayValves()
        {
            var twoWayValves = _context.System.Components
                .Where(c => c.Category == ComponentCategory.Valve && c.SubType == FluidSystemContants.TwoWayValve);

            foreach (var component in twoWayValves)
            {
                if (_context.GetBehavior(component.Id) is TwoWayValveBehavior twoWay && twoWay.IsOpen)
                    _context.ActivateComponent(component.Id);
            }
        }

        private void ConfigureValvesForFilling(string endComponentId)
        {
            var (requiredDefaults, requiredAlternates) = GetValveConfiguration(endComponentId);

            foreach (var valveId in requiredDefaults)
            {
                if (_context.GetBehavior(valveId) is ThreeWayValveBehavior threeWay && threeWay.IsAlternativePosition)
                    _context.ActivateComponent(valveId);
            }

            foreach (var valveId in requiredAlternates)
            {
                if (_context.GetBehavior(valveId) is ThreeWayValveBehavior threeWay && threeWay.IsDefaultPosition)
                    _context.ActivateComponent(valveId);
            }
        }

        private void SetManifoldValvesToDefault()
        {
            List<string> manifoldValves =
            [
                "valve_a1", "valve_a2", "valve_a3",
                "valve_b1", "valve_b2", "valve_b3"
            ];

            foreach (var valveId in manifoldValves)
            {
                if (_context.GetBehavior(valveId) is ThreeWayValveBehavior threeWay && threeWay.IsAlternativePosition)
                    _context.ActivateComponent(valveId);
            }
        }

        private void ConfigureSinkValve(string sinkId)
        {
            const string valveId = "valve_w1";
            if (_context.GetBehavior(valveId) is not ThreeWayValveBehavior valve) return;

            bool needsAlternative = (sinkId == "sink_nhw");
            if ((needsAlternative && !valve.IsAlternativePosition) || (!needsAlternative && valve.IsAlternativePosition))
            {
                _context.ActivateComponent(valveId);
            }
        }

        private bool IsMixDetected(List<string> route, string startMaterial)
        {
            foreach (var componentId in route)
            {
                var material = GetMaterial(componentId);
                if (material != startMaterial && material != FluidSystemContants.Air) return true;
            }
            return false;
        }
        private bool IsChamberMixingFluid(string chamberId)
        {
            List<string> manifoldPipesToCheck =
            [
                "pipe_20", "pipe_21", "pipe_22", "pipe_23",
                "pipe_30", "pipe_31", "pipe_32", "pipe_33",
                "pipe_13", "pipe_14"
                ];

            string chamberMaterial = _context.FluidState.Materials[chamberId];

            foreach (var pipeId in manifoldPipesToCheck)
            {
                string pipeMaterial = _context.FluidState.Materials[pipeId];
                if (pipeMaterial != FluidSystemContants.Air && pipeMaterial != chamberMaterial) return true;
            }
            return false;
        }

        private string GetMaterial(string componentId)
        {
            return _context.FluidState.Materials[componentId];
        }
        private List<string> GetRouteToChamber(string startComponentId, string endComponentId)
        {
            var route = new List<string>();
            route.AddRange(GetSourceSegment(startComponentId));
            route.AddRange(GetTargetSegment(endComponentId));
            return route;
        }

        private IEnumerable<string> GetSourceSegment(string sourceId) => sourceId switch
        {
            "source_air" => ["pipe_10", "valve_p1"],
            "source_alcohol" => ["pipe_11", "valve_p2"],
            "source_water" => ["pipe_12", "valve_p3"],
            _ => new List<string>()
        };

        private IEnumerable<string> GetTargetSegment(string chamberId) => chamberId switch
        {
            "cointainer_chamber1" => ["valve_a3", "pipe_31", "valve_a2", "pipe_30", "valve_a1", "pipe_40", "pipe_41", "valve_b1"],
            "cointainer_chamber2" => ["valve_a3", "pipe_31", "valve_a2", "pipe_42", "pipe_43", "valve_b2"],
            "cointainer_chamber3" => ["valve_a3", "pipe_44", "pipe_45", "valve_b3"],
            _ => new List<string>()
        };

        private (List<string> Defaults, List<string> Alternates) GetValveConfiguration(string endComponent) => endComponent switch
        {
            "cointainer_chamber3" => (new List<string> { "valve_b3" }, new List<string> { "valve_a3" }),
            "cointainer_chamber2" => (new List<string> { "valve_a3", "valve_b2" }, new List<string> { "valve_a2" }),
            "cointainer_chamber1" => (new List<string> { "valve_a3", "valve_a2", "valve_b1" }, new List<string> { "valve_a1" }),
            _ => (new List<string>(), new List<string>())
        };

        private string GetSourceValve(string sourceId) => sourceId switch
        {
            "source_air" => "valve_p1",
            "source_alcohol" => "valve_p2",
            "source_water" => "valve_p3",
            _ => ""
        };

        private string GetChamberOutletValve(string chamberId) => chamberId switch
        {
            "cointainer_chamber1" => "valve_b1",
            "cointainer_chamber2" => "valve_b2",
            "cointainer_chamber3" => "valve_b3",
            _ => ""
        };
    }
}