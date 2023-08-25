using FarmGame.Agent;
using FarmGame.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //private void TryInteractionWithSomething(IAgent agent)
        //{
        //    agent.Blocked = true;
        //    agent.AgentAnimation.PlayAnimation(AnimationType.Watering);
        //    if(ToolAnimator != null)
        //    {
        //        agent.AgentAnimation.OnAnimationOnce.AddListener(() =>
        //        {
        //            foreach (IInteractable interactable 
        //            in agent.InteractionDetector.PerformDetection())
        //            {
        //                if (interactable.CanInteract(agent))
        //                {
        //                    interactable.Interact(agent);
        //                    agent.Blocked = true;
        //                    return;
        //                }
        //            }
        //        });
        //        agent.AgentAnimation.OnAnimationEnd.AddListener(() =>
        //        {
        //            agent.Blocked = false;
        //            OnFinishedAction?.Invoke(agent);
        //        });

        //        agent.AgentAnimation.ToolAnimation.SetAnimatorController(ToolAnimator);
        //        agent.AgentAnimation.ToolAnimation.PlayAnimation();
        //    }
        //}

        private void TryInteractionWithSomething(IAgent agent)
        {
            foreach (var interactable in agent.InteractionDetector.PerformDetection())
            {
                if (interactable.CanInteract(agent))
                {
                    agent.Blocked = true;
                    agent.AgentAnimation.PlayAnimation(AnimationType.Watering);
                    if (ToolAnimator != null)
                    {
                        agent.AgentAnimation.ToolAnimation.SetAnimatorController(ToolAnimator);
                        agent.AgentAnimation.ToolAnimation.PlayAnimation();
                    }
                    agent.AgentAnimation.OnAnimationOnce.AddListener(() =>
                    {
                        interactable.Interact(agent);
                    });
                    agent.AgentAnimation.OnAnimationEnd.AddListener(() =>
                    {
                        agent.Blocked = false;
                        OnFinishedAction?.Invoke(agent);
                    }
                    );
                    return;
                }
            }

        }

        private void TryWateringCrop(IAgent agent)
        {
            List<Vector2> cropFields = agent.FieldDetectorObject.ValidSelectionPositions
                .Where(pos => agent.FieldController.IsThereCropAt(pos)).ToList();
            if(cropFields.Count <= 0)
            {
                Debug.Log("No crops to water here");
                return;
            }
            if(NumberOfUses <= 0)
            {
                Debug.Log("Watering can has NO water)");
            }
            agent.Blocked = true;
            agent.AgentAnimation.PlayAnimation(AnimationType.Watering);
            if (ToolAnimator != null)
            {
                agent.AgentAnimation.OnAnimationOnce.AddListener(() =>
                {
                    foreach (var pos in cropFields)
                    {
                        agent.FieldController.WaterCropAt(pos);
                    }
                    NumberOfUses--;
                });
                agent.AgentAnimation.OnAnimationEnd.AddListener(() =>
                {
                    agent.Blocked = false;
                    OnFinishedAction?.Invoke(agent);
                    agent.FieldController.PrintCropsStatus();
                });

                agent.AgentAnimation.ToolAnimation.SetAnimatorController(ToolAnimator);
                agent.AgentAnimation.ToolAnimation.PlayAnimation();
            }
        }
    }
}
