namespace FluidSystems.Shared.Models.Documents
{
    public class Document<TContent>
    {
        public FileMetaData Metadata { get; set; }
        public TContent Content { get; set; }
    }
}