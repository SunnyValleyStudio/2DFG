using FarmGame.Agent;
using FarmGame.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FarmGame.Tools
{
    public class WateringCanTool : Tool
    {
        private int _maxUses = 4;
        public int NumberOfUses { get; set; }
        public WateringCanTool(int itemID, string data) : base(itemID, data)
        {
            this.ToolType = ToolType.WatringCan;
            NumberOfUses = _maxUses;
        }

        public override bool IsToolStillValid()
            => true;

        public override void Equip(IAgent agent)
        {
            agent.FieldDetectorObject.StartChecking(ToolRange);
        }

        public override void PutAway(IAgent agent)
        {
            agent.FieldDetectorObject.StopChecking();
        }

        public override void UseTool(IAgent agent)
        {
            if(agent.FieldDetectorObject != null && agent.FieldDetectorObject.ValidSelectionPositions.Count > 0)
            {
                TryWateringCrop(agent);
            }
            else
            {
                TryInteractionWithSomething(agent);
            }
        }

        private void TryInteractionWithSomething(IAgent agent)
        {
            agent.Blocked = true;
            agent.AgentAnimation.PlayAnimation(AnimationType.Watering);
            if(ToolAnimator != null)
            {
                agent.AgentAnimation.OnAnimationOnce.AddListener(() =>
                {
                    foreach (IInteractable interactable 
                    in agent.InteractionDetector.PerformDetection())
                    {
                        if (interactable.CanInteract(agent))
                        {
                            interactable.Interact(agent);
                            agent.Blocked = true;
                            return;
                        }
                    }
                });
                agent.AgentAnimation.OnAnimationEnd.AddListener(() =>
                {
                    agent.Blocked = false;
                    OnFinishedActon?.Invoke(agent);
                });

                agent.AgentAnimation.ToolAnimation.SetAnimatorController(ToolAnimator);
                agent.AgentAnimation.ToolAnimation.PlayAnimation();
            }
        }

        private void TryWateringCrop(IAgent agent)
        {
            throw new NotImplementedException();
        }
    }
}
