namespace FluidSystems.Core.Models.System.DTO
{
    public class FluidComponentDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SubType { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<ConnectorDTO> Connectors { get; set; }
    }
}
