using FluidSystems.Control.Core;
using FluidSystems.Shared.Common.Results;

namespace FluidSystems.Control.Services.ManifoldServices
{
    public interface IChamberFiller
    {
        Result<bool> FillChamber(string startComponentId, string endComponentId, SimulationContext context);
    }
}