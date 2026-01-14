using FluidSystems.Infrastructure.Resources;
using FluidSystems.Shared.Common.Exceptions;
using System.Text.Json;

namespace FluidSystems.Infrastructure.Serialization
{
    public class JsonSerializerService : ISerializerService
    {
        private readonly JsonSerializerOptions _options;

        public JsonSerializerService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }
        public async Task<T> DeserializeAsync<T>(Stream stream)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, _options);
            }
            catch (JsonException ex)
            {
                string lineInfo = ex.LineNumber.HasValue ? string.Format(Messages.JsonError_LineInfo, ex.LineNumber) : string.Empty;
                string message = string.Format(Messages.JsonError_Detailed, Messages.JsonError_Simple, lineInfo, ex.Message);
                throw new FluidSystemsException(message, ex);
            }
            catch (Exception ex)
            {
                throw new FluidSystemsException(Messages.JsonError_Simple, ex);
            }
        }
    }
}