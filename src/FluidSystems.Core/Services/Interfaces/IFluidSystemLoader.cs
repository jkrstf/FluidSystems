using FluidSystems.Core.Models.System;
using FluidSystems.Shared.Common.Results;
using FluidSystems.Shared.Models.Documents;

namespace FluidSystems.Core.Services.Interfaces
{
    public interface IFluidSystemLoader
    {
        Task<Result<Document<FluidSystem>>> LoadAsync(string filePath);
    }
}
