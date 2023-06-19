using FarmGame.DataStorage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public static class ToolFactory
    { 
        public static Tool CreateTool(ItemDescription description) 
        {
            Tool tool = description.ToolType switch
            {
                ToolType.Hand => new HandTool(description.ToolType),
                ToolType.Hoe => new HoeTool(description.ToolType),
                _ => throw new System.NotImplementedException(
                    $"TOoType is not defined in the ToolFactory {description.ToolType}")
            };
            tool.ToolAnimator = description.ToolAnimator;
            return tool;
        }
    }
}
