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
            IEnumerable<IInteractable> interactables = null;
            if (agent.FieldDetectorObject.IsNearField)
            {
                List<Vector2> detectedPositions = agent.FieldDetectorObject.DetectValidTiles();
                if (detectedPositions.Count > 0)
                    interactables = agent.InteractionDetector.PerformDetection(detectedPositions[0]);
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
        Hand,
        Hoe
    }
}
