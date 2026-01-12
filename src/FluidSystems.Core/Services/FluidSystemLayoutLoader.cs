using FluidSystems.Core.Mappers;
using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.Layout.DTO;
using FluidSystems.Core.Resources;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Infrastructure.FileSystem;
using FluidSystems.Infrastructure.Serialization;
using FluidSystems.Shared.Common.Exceptions;
using FluidSystems.Shared.Common.Results;
using FluidSystems.Shared.Mappers;
using FluidSystems.Shared.Models.Documents;
using FluidSystems.Shared.Models.Documents.DTO;

namespace FluidSystems.Core.Services
{
    public class FluidSystemLayoutLoader : IFluidSystemLayoutLoader
    {
        private readonly IFileHandlerService _fileService;
        private readonly ISerializerService _serializer;

        public FluidSystemLayoutLoader(IFileHandlerService fileService, ISerializerService serializer)
        {
            _fileService = fileService;
            _serializer = serializer;
        }

        public async Task<Result<Document<FluidSystemLayout>>> LoadAsync(string filePath)
        {
            try
            {
                using (Stream stream = await _fileService.GetStreamAsync(filePath))
                {
                    var dtoWrapper = await _serializer.DeserializeAsync<DocumentDTO<FluidSystemLayoutDTO>>(stream);

                    Document<FluidSystemLayout> domainDocument = dtoWrapper.ToModel(contentDto => contentDto.ToModel());
                    domainDocument.Metadata.FilePath = filePath;

                    return Result<Document<FluidSystemLayout>>.Success(domainDocument);
                }
            }
            catch (FluidSystemsException ex)
            {
                return Result<Document<FluidSystemLayout>>.Failure(ex.Message, ex);
            }
            catch (Exception ex)
            {
                return Result<Document<FluidSystemLayout>>.Failure(string.Format(Messages.SystemLoader_UnexpectedError, filePath), ex);
            }
        }
    }
}
