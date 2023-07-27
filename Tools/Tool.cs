using FarmGame.Agent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools
{
    public abstract class Tool
    {
        public ToolType ToolType { get; protected set; }
        public Action OnPerformedAction, OnStartedAction;
        public Action<IAgent> OnFinishedActon;

        public RuntimeAnimatorController ToolAnimator { get; set; }
        public Vector2Int ToolRange { get; set; } = Vector2Int.one;

        public int ItemIndex { get; set; }
        protected Tool(int itemID, string data)
        {
            this.ItemIndex = itemID;
            RestoreSavedData();
        }

        public virtual void RestoreSavedData()
        {
        }

        public virtual string GetDataToSave() => String.Empty;
        public abstract bool IsToolStillValid();

        public virtual void PutAway(IAgent agent) { }
        public virtual void Equip(IAgent agent) { }

        public abstract void UseTool(IAgent agent);
    }
}
