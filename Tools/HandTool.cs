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
            foreach (PickUpInteraction item in agent.InteractionDetector.PerformDetection())
            {
                if (item.CanInteract())
                {
                    agent.AgentMover.Stopped = true;
                    Debug.Log("Agent Stopped");
                    agent.AgentAnimation.OnAnimationEnd.AddListener(
                        () =>
                        {
                            item.Interact(agent);
                            agent.AgentMover.Stopped = false;
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
