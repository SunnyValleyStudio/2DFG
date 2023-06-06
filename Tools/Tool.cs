using FarmGame.Agent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public abstract class Tool
    {
        public ToolType ToolType { get; }

        public RuntimeAnimatorController ToolAnimator { get; set; }
        public Vector2Int ToolRange { get; set; } = Vector2Int.one;

        protected Tool(ToolType toolType)
        {
            this.ToolType = toolType;
        }

        public virtual void PutAway(IAgent agent) { }
        public virtual void Equip(IAgent agent) { }

        public abstract void UseTool(IAgent agent);
    }
}
