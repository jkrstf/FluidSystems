using FluidSystems.Control.Core;
using FluidSystems.Shared.Common.Results;

namespace FluidSystems.Control.Services.FluidSafetyValidators
{
    public interface IFluidSafetyValidator
    {
        Result<bool> ValidateToggle(string toggleComponentId, SimulationContext context);
    }
}
