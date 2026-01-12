using FluidSystems.Core.Models.Layout;
using FluidSystems.Shared.Common.Results;
using FluidSystems.Shared.Models.Documents;

namespace FluidSystems.Core.Services.Interfaces
{
    public interface IFluidSystemLayoutLoader
    {
        Task<Result<Document<FluidSystemLayout>>> LoadAsync(string filePath);
    }
}
