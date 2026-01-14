using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Behaviors
{
    public interface IComponentBehavior
    {
        string GetDescription();
        Dictionary<string, string> GetState();
        bool IsPathActive(string fromId, string toId, FluidComponent component, bool simulateToggle = false);
    }
}