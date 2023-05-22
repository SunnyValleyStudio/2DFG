using FarmGame.Agent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace FarmGame.Tools
{
    public class HoeTool : Tool
    {
        public HoeTool(ToolType toolType) : base(toolType)
        {

        }

        public override void UseTool(Player agent)
        {
            agent.Blocked = true;
            Debug.Log("Agent Stopped");
            agent.AgentAnimation.OnAnimationEnd.AddListener(
            () =>
            {
                    agent.Blocked = false;
                    Debug.Log("Agent Restarted");
                }
                );
            agent.AgentAnimation.PlayAnimation(AnimationType.Swing);
            return;
        }
    }
}
