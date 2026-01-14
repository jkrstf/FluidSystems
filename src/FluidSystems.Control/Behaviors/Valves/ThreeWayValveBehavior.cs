using FluidSystems.Control.Core;
using FluidSystems.Control.Resources;
using FluidSystems.Core.Constants;
using FluidSystems.Core.Models.System;

namespace FluidSystems.Control.Behaviors.Valves
{
    public class ThreeWayValveBehavior : IComponentBehavior, IActivatableBehavior
    {
        private bool _isDefaultPosition;

        public bool IsDefaultPosition => _isDefaultPosition;
        public bool IsAlternativePosition => !_isDefaultPosition;
        public Dictionary<string, string> GetState() => new()
        {
            { "State", IsDefaultPosition ? "De-energized" : "Energized" }
        };

        public ThreeWayValveBehavior(bool isDefaultPosition = true)
        {
            _isDefaultPosition = isDefaultPosition;
        }

        public void Activate(FluidComponent component, SimulationContext context)
        {
            _isDefaultPosition = !_isDefaultPosition;
        }

        public string GetDescription() => Messages.ThreeWayValveDescriptor;

        public bool IsPathActive(string fromId, string toId, FluidComponent component, bool simulateToggle = false)
        {
            bool effectiveIsDefault = simulateToggle ? !_isDefaultPosition : _isDefaultPosition;

            var commonEdge = component.Connectors.FirstOrDefault()?.ConnectedComponent.Id;
            component.Parameters.TryGetValue(FluidSystemContants.DefaultEdge, out var defaultEdge);
            component.Parameters.TryGetValue(FluidSystemContants.AlternativeEdge, out var altEdge);

            if (effectiveIsDefault)
            {
                return (fromId == commonEdge && toId == defaultEdge) ||
                       (fromId == defaultEdge && toId == commonEdge);
            }
            return (fromId == commonEdge && toId == altEdge) ||
                    (fromId == altEdge && toId == commonEdge);
        }
    }
}