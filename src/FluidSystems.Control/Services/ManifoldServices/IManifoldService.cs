using FluidSystems.Shared.Common.Results;

namespace FluidSystems.Control.Services.ManifoldServices
{
    public interface IManifoldService
    {
        Task<Result<bool>> FillChamberAsync(string startComponentId, string endComponentId, CancellationToken cancellationToken = default);
        Task<Result<bool>> DrainChamberAsync(string chamberId, string sinkId, CancellationToken cancellationToken = default);
        Task<Result<bool>> CleanManifoldAsync(string sinkComponentId, CancellationToken cancellationToken = default);
        Task<Result<bool>> ToggleComponentAsync(string componentId, CancellationToken cancellationToken = default);
    }
}