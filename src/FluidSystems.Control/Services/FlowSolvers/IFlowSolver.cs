using FluidSystems.Control.Core;

namespace FluidSystems.Control.Services.FlowSolvers
{
    public interface IFlowSolver
    {
        void InitializeFlow(SimulationContext context);
        void UpdateFlows(SimulationContext context);
    }
}