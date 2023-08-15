using FarmGame.Agent;
using FarmGame.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class HandTool : Tool
    {
        public HandTool(int itemID, string data) : base(itemID, data)
        {
            this.ToolType = ToolType.Hand;
        }

        public override void Equip(IAgent agent)
        {
            agent.FieldDetectorObject.StartChecking(ToolRange);
        }

        public override bool IsToolStillValid()
        {
            return true;
        }

        public override void PutAway(IAgent agent)
        {
            agent.FieldDetectorObject.StopChecking();
        }

        public override void UseTool(IAgent agent)
        {
            IEnumerable<IInteractable> interactables = null;
            if (agent.FieldDetectorObject.IsNearField)
            {
                if (agent.FieldDetectorObject.ValidSelectionPositions.Count > 0)
                    interactables = 
                        agent.InteractionDetector.
                        PerformDetection(agent.FieldDetectorObject.ValidSelectionPositions[0]);
            }
            if (interactables == null)
                interactables = agent.InteractionDetector.PerformDetection();

            foreach (IInteractable item in interactables)
            {
                if (item.CanInteract(agent))
                {
                    agent.Blocked = true;
                    Debug.Log("Agent Stopped");
                    agent.AgentAnimation.OnAnimationEnd.AddListener(
                        () =>
                        {
                            agent.Blocked = false;
                            item.Interact(agent);
                            Debug.Log("Agent Restarted");
                        }
                        );
                    agent.AgentAnimation.PlayAnimation(AnimationType.PickUp);
                    return;
                }
            }
        }
    }

    public enum ToolType
    {
        None,
        Hand,
        Hoe,
        SeedPlacer
    }
}
