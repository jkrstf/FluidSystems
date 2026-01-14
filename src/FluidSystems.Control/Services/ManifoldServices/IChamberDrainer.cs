using FluidSystems.Control.Core;
using FluidSystems.Shared.Common.Results;

namespace FluidSystems.Control.Services.ManifoldServices
{
    public interface IChamberDrainer
    {
        Result<bool> DrainChamber(string chamberId, string sinkId, SimulationContext context);
    }
}
