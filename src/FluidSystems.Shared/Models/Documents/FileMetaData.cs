namespace FluidSystems.Shared.Models.Documents
{
    public class FileMetaData
    {
        public string FilePath { get; set; }
        public string Version { get; set; } 
        public DateTime LastModified { get; set; }
    }
}