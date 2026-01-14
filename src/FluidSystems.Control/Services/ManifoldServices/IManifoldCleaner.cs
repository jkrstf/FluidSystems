using FluidSystems.Control.Core;

namespace FluidSystems.Control.Services.ManifoldServices
{
    public interface IManifoldCleaner
    {
        void CleanManifold(string sinkComponentId, SimulationContext context);
    }
}
