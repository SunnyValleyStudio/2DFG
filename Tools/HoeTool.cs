using FarmGame.Agent;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class HoeTool : Tool
    {
        public HoeTool(ToolType toolType) : base(toolType)
        {

        }

        public override void UseTool(IAgent agent)
        {
            if (agent.FieldDetectorObject.IsNearField == false)
                return;
            List<Vector2> detectedPosition = agent.FieldDetectorObject.DetectValidTiles();
            if(detectedPosition.Count <= 0)
                return;
            agent.Blocked = true;
            Debug.Log("Agent Stopped");
            agent.AgentAnimation.OnAnimationEnd.AddListener(
            () =>
            {
                foreach (Vector2 worldPositon in detectedPosition)
                {
                    agent.FieldController.PrepareFieldAt(worldPositon);
                }
                agent.Blocked = false;
                Debug.Log("Agent Restarted");
            }
                );
            if (ToolAnimator != null)
            {
                agent.AgentAnimation.ToolAnimation.SetAnimatorController(ToolAnimator);
                agent.AgentAnimation.ToolAnimation.PlayAnimation();
            }
            agent.AgentAnimation.PlayAnimation(AnimationType.Swing);
            return;
        }
    }
}
