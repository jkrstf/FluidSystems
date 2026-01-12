using FluidSystems.Control.Services.FlowSolvers;

namespace FluidSystems.Control.Core
{
    public class SimulationManager : ISimulationManager
    {
        private SimulationContext _context;
        private readonly IFlowSolver _solver;

        public SimulationManager(IFlowSolver solver)
        {
            _solver = solver;
        }

        public SimulationContext? CurrentContext => throw new NotImplementedException();

        public void SetContext(SimulationContext context)
        {
            if (_context != null) _context.ComponentBehaviorChanged -= context_ComponentBehaviorChanged;
            _context = context;
            _context.ComponentBehaviorChanged += context_ComponentBehaviorChanged;
        }

        private void context_ComponentBehaviorChanged(object? sender, string e)
        {
            _solver.UpdateFlows(_context);
        }
    }
}