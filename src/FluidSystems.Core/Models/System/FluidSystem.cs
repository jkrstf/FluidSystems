namespace FluidSystems.Core.Models.System
{
    public class FluidSystem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public List<FluidComponent> Components { get; set; }
    }
}
