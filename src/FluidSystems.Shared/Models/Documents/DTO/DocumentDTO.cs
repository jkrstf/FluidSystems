namespace FluidSystems.Shared.Models.Documents.DTO
{
    public class DocumentDTO<TContentDTO>
    {
        public FileMetadataDTO Metadata { get; set; }
        public TContentDTO Content { get; set; }
    }
}