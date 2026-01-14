using FluidSystems.Core.Mappers;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Models.System.DTO;
using FluidSystems.Core.Resources;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Infrastructure.FileSystem;
using FluidSystems.Infrastructure.Serialization;
using FluidSystems.Shared.Common.Exceptions;
using FluidSystems.Shared.Common.Results;
using FluidSystems.Shared.Mappers;
using FluidSystems.Shared.Models.Documents;
using FluidSystems.Shared.Models.Documents.DTO;

namespace FluidSystems.Core.Features
{
    public class FluidSystemLoader : IFluidSystemLoader
    {
        private readonly IFileHandlerService _fileService;
        private readonly ISerializerService _serializer;

        public FluidSystemLoader(IFileHandlerService fileService, ISerializerService serializer)
        {
            _fileService = fileService;
            _serializer = serializer;
        }

        public async Task<Result<Document<FluidSystem>>> LoadAsync(string filePath)
        {
            try
            {
                using (Stream stream = await _fileService.GetStreamAsync(filePath))
                {
                    var dtoWrapper = await _serializer.DeserializeAsync<DocumentDTO<FluidSystemDTO>>(stream);

                    Document<FluidSystem> domainDocument = dtoWrapper.ToModel(contentDto => contentDto.ToModel());
                    domainDocument.Metadata.FilePath = filePath;

                    return Result<Document<FluidSystem>>.Success(domainDocument);
                }
            }
            catch (FluidSystemsException ex)
            {
                return Result<Document<FluidSystem>>.Failure(ex.Message, ex);
            }
            catch (Exception ex)
            {
                return Result<Document<FluidSystem>>.Failure(string.Format(Messages.SystemLoader_UnexpectedError, filePath), ex);
            }
        }
    }
}