namespace FluidSystems.Core.Models.System.DTO
{
    public class FluidSystemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public List<FluidComponentDTO> Components { get; set; }
    }
}