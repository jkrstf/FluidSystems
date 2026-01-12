using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.Layout.DTO;

namespace FluidSystems.Core.Mappers
{
    public static class FluidLayoutMapper
    {
        public static FluidSystemLayout ToModel(this FluidSystemLayoutDTO dto)
        {
            if (dto == null) return null;

            return new FluidSystemLayout
            {
                SystemId = dto.SystemId,
                Settings = dto.Settings?.ToModel() ?? new LayoutSettings(), 
                Elements = dto.Elements?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToModel()) ?? new Dictionary<string, FluidComponentLayout>()
            };
        }

        public static LayoutSettings ToModel(this LayoutSettingsDTO dto)
        {
            if (dto == null) return null; 

            return new LayoutSettings
            {
                Rows = dto.Rows,
                Columns = dto.Columns
            };
        }

        public static FluidComponentLayout ToModel(this FluidComponentLayoutDTO dto)
        {
            if (dto == null) return null;

            return new FluidComponentLayout
            {
                X = dto.X,
                Y = dto.Y,
                X2 = dto.X2,
                Y2 = dto.Y2,
                Rotation = dto.Rotation,
                ZIndex = dto.ZIndex
            };
        }
    }
}