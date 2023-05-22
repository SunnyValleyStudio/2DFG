using FarmGame.Agent;
using UnityEngine;

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
            if(ToolAnimator != null)
            {
                agent.AgentAnimation.ToolAnimation.SetAnimatorController(ToolAnimator);
                agent.AgentAnimation.ToolAnimation.PlayAnimation();
            }
            agent.AgentAnimation.PlayAnimation(AnimationType.Swing);
            return;
        }
    }
}
