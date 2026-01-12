using FluidSystems.Infrastructure.Resources;
using FluidSystems.Shared.Common.Exceptions;

namespace FluidSystems.Infrastructure.FileSystem
{
    public class LocalDiskFileService : IFileHandlerService
    {
        public Task<Stream> GetStreamAsync(string filePath)
        {
            try
            {
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return Task.FromResult<Stream>(stream);
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
            {
                throw new FluidSystemsException(string.Format(Messages.Error_FileNotFound, filePath));
            }
            catch (Exception ex)
            {
                throw new FluidSystemsException(string.Format(Messages.Error_FileProcessing, filePath));
            }
        }
    }
}