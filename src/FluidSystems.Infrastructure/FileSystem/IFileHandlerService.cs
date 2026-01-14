namespace FluidSystems.Infrastructure.FileSystem
{
    public interface IFileHandlerService
    {
        Task<Stream> GetStreamAsync(string filePath);
    }
}
