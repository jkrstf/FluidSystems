using FluidSystems.Core.Models.Enums;

namespace FluidSystems.Core.Models.System
{
    public class FluidComponent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ComponentCategory Category { get; set; }
        public string SubType { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<FluidConnector> Connectors { get; set; }
    }
}
