using FarmGame.Agent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public class SeedPlacementTool : Tool
    {
        public int CropID { get; set; } = 0;
        private int _quantity = 1;

        public SeedPlacementTool(int itemID, string data) : base(itemID, data)
        {
            this.ToolType = ToolType.SeedPlacer;
        }

        public override string GetDataToSave()
        {
            return JsonUtility.ToJson(new SeedToolData
            {
                cropID = CropID,
                quantity = _quantity
            });
        }

        public override void RestoreSavedData(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new System.Exception("Failed to create tool becasue data passed is null");
            SeedToolData savedData = JsonUtility.FromJson<SeedToolData>(data);
            CropID = savedData.cropID;
            _quantity = savedData.quantity;
        }

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
            if (agent.FieldDetectorObject.ValidSelectionPositions.Count < 0)
                return;
            agent.Blocked = true;
            agent.AgentAnimation.PlayAnimation(AnimationType.PickUp);
            OnPerformedAction?.Invoke();
            agent.AgentAnimation.OnAnimationEnd.AddListener(() =>
            {
                foreach (var pos in agent.FieldDetectorObject.ValidSelectionPositions)
                {
                    if (agent.FieldController.CanIPlaceCropsHere(pos))
                    {
                        agent.FieldController.PlaceCropAt(pos, CropID);
                    }
                    else
                    {
                        Debug.Log($"Can't place crop at {pos}");
                    }
                }
                _quantity--;
                OnFinishedActon?.Invoke(agent);
                agent.Blocked = false;
            });
            agent.FieldController.PrintCropsStatus();
        }

        public override bool IsToolStillValid()
        {
            return _quantity > 0;
        }
    }
    [Serializable]
    public struct SeedToolData
    {
        public int cropID, quantity;
    }
}
