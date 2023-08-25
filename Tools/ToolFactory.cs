using FarmGame.DataStorage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public static class ToolFactory
    { 
        public static Tool CreateTool(ItemDescription description, string data = null) 
        {
            Tool tool = description.ToolType switch
            {
                ToolType.Hand => new HandTool(description.ID,data),
                ToolType.Hoe => new HoeTool(description.ID, data),
                ToolType.SeedPlacer => new SeedPlacementTool(description.ID, data),
                ToolType.WatringCan => new WateringCanTool(description.ID, data),
                _ => throw new System.NotImplementedException(
                    $"ToolType is not defined in the ToolFactory {description.ToolType}")
            };
            tool.ToolAnimator = description.ToolAnimator;
            tool.ToolRange = description.ToolRange;
            return tool;
        }
    }
}
