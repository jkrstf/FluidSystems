using FluidSystems.Core.Models.Enums;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Models.System.DTO;

namespace FluidSystems.Core.Mappers
{
    public static class FluidSystemMapper
    {
        public static FluidSystem ToModel(this FluidSystemDTO dto)
        {
            if (dto == null) return null;

            var system = new FluidSystem
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Version = dto.Version,
                Components = new List<FluidComponent>()
            };

            if (dto.Components == null || !dto.Components.Any()) return system;

            var componentLookup = dto.Components
                .Select(componentDto => componentDto.ToModelInternal())
                .ToDictionary(component => component.Id);

            foreach (var compDto in dto.Components)
            {
                var currentModel = componentLookup[compDto.Id];

                currentModel.Connectors = (compDto.Connectors ?? Enumerable.Empty<ConnectorDTO>())
                    .Select(c => c.ConnectedComponentId)
                    .Where(id => componentLookup.ContainsKey(id))
                    .Select(id => new FluidConnector
                    {
                        ConnectedComponent = componentLookup[id]
                    })
                    .ToList();

                system.Components.Add(currentModel);
            }

            return system;
        }

        private static FluidComponent ToModelInternal(this FluidComponentDTO dto)
        {
            if (dto == null) return null;

            return new FluidComponent
            {
                Id = dto.Id,
                Name = dto.Name,
                SubType = dto.SubType,
                Category = Enum.TryParse<ComponentCategory>(dto.Category, true, out var category) ? category : ComponentCategory.Unknown,
                Parameters = dto.Parameters?.ToDictionary(parameter => parameter.Key, parameter => parameter.Value) ?? new Dictionary<string, string>(),
                Connectors = new List<FluidConnector>()
            };
        }
    }
}
