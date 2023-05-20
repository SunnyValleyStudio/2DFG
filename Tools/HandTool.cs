using FarmGame.Agent;
using FarmGame.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class HandTool : Tool
    {

        public HandTool(ToolType toolType) : base(toolType)
        {

        }

        public override void UseTool(Player agent)
        {
            IEnumerable<IInteractable> interactables = agent.FieldDetectorObject.IsNearField ?
                agent.InteractionDetector.PerformDetection(agent.FieldDetectorObject.DetectValidTiles()[0])
                : agent.InteractionDetector.PerformDetection();

            foreach (IInteractable item in interactables)
            {
                if (item.CanInteract(agent))
                {
                    agent.Blocked = true;
                    Debug.Log("Agent Stopped");
                    agent.AgentAnimation.OnAnimationEnd.AddListener(
                        () =>
                        {
                            item.Interact(agent);
                            agent.Blocked = false;
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
        Hand
    }
}
