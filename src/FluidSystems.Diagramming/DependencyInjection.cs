using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Diagramming.Services.Builders;
using FluidSystems.Diagramming.Services.Routing;
using FluidSystems.Diagramming.Services.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace FluidSystems.Diagramming
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDiagrammingServices(this IServiceCollection services)
        {
            services.AddSingleton<IDiagramBuilder, DiagramBuilderService>();
            services.AddSingleton<IDiagramNodeBuilder, DiagramNodeBuilder>();
            services.AddSingleton<IDiagramConnectionBuilder, DiagramConnectionBuilder>();

            services.AddSingleton<IDiagramConnectionRouter, ManhattanRouter>();

            services.AddSingleton<CommonRailConnectionStrategy>();
            services.AddSingleton<PipeConnectionStrategy>();

            services.AddSingleton<Dictionary<string, IDiagramConnectionStrategy>>(sp =>
            {
                return new Dictionary<string, IDiagramConnectionStrategy>
                {
                    { "CommonRail", sp.GetRequiredService<CommonRailConnectionStrategy>() },
                    { "Pipe", sp.GetRequiredService<PipeConnectionStrategy>() }
                };
            });

            return services;
        }
    }
}