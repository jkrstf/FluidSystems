using FluidSystems.Infrastructure.FileSystem;
using FluidSystems.Infrastructure.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace FluidSystems.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileHandlerService, LocalDiskFileService>();
            services.AddSingleton<ISerializerService, JsonSerializerService>();
            return services;
        }
    }
}
