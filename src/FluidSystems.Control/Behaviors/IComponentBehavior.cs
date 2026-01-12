namespace FluidSystems.Control.Behaviors
{
    public interface IComponentBehavior
    {
        Dictionary<string, string> GetState();
    }
}