using FarmGame.Agent;
using FarmGame.Tools;
using System.Collections.Generic;

namespace FarmGame.Interactions
{
    public interface IInteractable
    {
        List<ToolType> UsableTools { get; set; }

        bool CanInteract(IAgent agent);
        void Interact(IAgent agent);
    }
}