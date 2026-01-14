namespace FluidSystems.Core.Models.Layout
{
    public class FluidComponentLayout
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int? X2 { get; set; }
        public int? Y2 { get; set; }
        public int ColumnSpan => X2.HasValue ? Math.Max(1, X2.Value - X + 1) : 1;
        public int RowSpan => Y2.HasValue ? Math.Max(1, Y2.Value - Y + 1) : 1;
        public double Rotation { get; set; }
        public int ZIndex { get; set; }
    }
}
