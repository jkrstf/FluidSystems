using FluidSystems.Shared.Models.Documents;
using FluidSystems.Shared.Models.Documents.DTO;

namespace FluidSystems.Shared.Mappers
{
    public static class DocumentMapper
    {
        public static FileMetaData ToModel(this FileMetadataDTO dto)
        {
            if (dto == null) return null;

            DateTime.TryParse(dto.LastModified, out var parsedDate);
            if (parsedDate == DateTime.MinValue) parsedDate = DateTime.UtcNow;

            return new FileMetaData
            {
                FilePath = dto.FilePath,
                Version = dto.Version,
                LastModified = parsedDate
            };
        }

        public static Document<TModel> ToModel<TDto, TModel>(this DocumentDTO<TDto> dto, Func<TDto, TModel> contentMapper)
        {
            if (dto == null) return null;
            if (contentMapper == null) return null;

            return new Document<TModel>
            {
                Metadata = dto.Metadata?.ToModel() ?? new FileMetaData(),
                Content = dto.Content != null ? contentMapper(dto.Content) : default
            };
        }
    }
}