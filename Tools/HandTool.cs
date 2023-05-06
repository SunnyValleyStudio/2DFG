using FarmGame.Agent;
using FarmGame.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class HandTool
    {
        public ToolType ToolType { get;}

        public HandTool(ToolType toolType)
        {
            this.ToolType = toolType;
        }

        public void UseTool(Player agent)
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
