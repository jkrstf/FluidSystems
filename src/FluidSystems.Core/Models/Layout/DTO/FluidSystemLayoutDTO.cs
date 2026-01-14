namespace FluidSystems.Core.Models.Layout.DTO
{
    public class FluidSystemLayoutDTO
    {
        public string SystemId { get; set; }
        public LayoutSettingsDTO Settings { get; set; }
        public Dictionary<string, FluidComponentLayoutDTO> Elements { get; set; }
    }
}