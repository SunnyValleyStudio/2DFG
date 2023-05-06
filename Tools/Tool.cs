using FarmGame.Agent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public abstract class Tool
    {
        public ToolType ToolType { get; }

        protected Tool(ToolType toolType)
        {
            this.ToolType = toolType;
        }

        public abstract void UseTool(Player agent);
    }
}
