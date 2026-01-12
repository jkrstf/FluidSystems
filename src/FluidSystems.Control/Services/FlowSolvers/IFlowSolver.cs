using FluidSystems.Control.Core;

namespace FluidSystems.Control.Services.FlowSolvers
{
    public interface IFlowSolver
    {
        void UpdateFlows(SimulationContext context);
    }
}