namespace FluidSystems.Control.Behaviors
{
    public interface IComponentBehavior
    {
        string GetDescription();
        Dictionary<string, string> GetState();
    }
}