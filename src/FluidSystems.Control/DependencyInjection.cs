using FluidSystems.Control.Core;
using FluidSystems.Control.Services.Flow;
using FluidSystems.Control.Services.FlowSolvers;
using FluidSystems.Control.Services.FluidSafetyValidators;
using Microsoft.Extensions.DependencyInjection;

namespace FluidSystems.Control
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddControlServices(this IServiceCollection services)
        {
            services.AddSingleton<IFlowSolver, FlowSolver>();
            services.AddSingleton<ISimulationManager, SimulationManager>();
            services.AddSingleton<IFluidSafetyValidator, FluidSafetyValidator>();
            services.AddSingleton<SimulationContext>();
            return services;
        }
    }
}