namespace FluidSystems.UI.WPF.Models
{
    public record ComponentItem(string Id, string Name)
    {
        public override string ToString() => Name;
    }
}
