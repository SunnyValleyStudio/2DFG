using FarmGame.Agent;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class HoeTool : Tool
    {
        public HoeTool(int itemID, string data) : base(itemID, data)
        {
            this.ToolType = ToolType.Hoe;
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
            if (agent.FieldDetectorObject.IsNearField == false)
                return;
            List<Vector2> detectedPosition = agent.FieldDetectorObject.ValidSelectionPositions;
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
                OnFinishedAction?.Invoke(agent);
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
