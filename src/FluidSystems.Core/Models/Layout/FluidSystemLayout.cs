namespace FluidSystems.Core.Models.Layout
{
    public class FluidSystemLayout
    {
        public string SystemId { get; set; }
        public LayoutSettings Settings { get; set; }
        public Dictionary<string, FluidComponentLayout> Elements { get; set; }
    }
}
