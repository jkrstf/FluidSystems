namespace FluidSystems.Control.Core
{
    public interface ISimulationManager
    {
        SimulationContext? CurrentContext { get; }
        void SetContext(SimulationContext context);
    }
}
