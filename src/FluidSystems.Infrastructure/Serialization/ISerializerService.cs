namespace FluidSystems.Infrastructure.Serialization
{
    public interface ISerializerService
    {
        Task<T> DeserializeAsync<T>(Stream stream);
    }
}
