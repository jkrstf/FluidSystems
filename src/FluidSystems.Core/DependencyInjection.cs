using FluidSystems.Core.Features;
using FluidSystems.Core.Services;
using FluidSystems.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluidSystems.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IFluidSystemLoader, FluidSystemLoader>();
            services.AddSingleton<IFluidSystemLayoutLoader, FluidSystemLayoutLoader>();
            return services;
        }
    }
}