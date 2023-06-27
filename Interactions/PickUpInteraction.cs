using FarmGame.Agent;
using FarmGame.DataStorage.Inventory;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Interactions
{
    public class PickUpInteraction : MonoBehaviour, IInteractable
    {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; }
            = new List<ToolType>();

        public UnityEvent OnPickUp;

        public bool CanInteract(IAgent agent)
            => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent)
        {
            agent.Inventory.AddItem(new InventoryItemData(0, 1, -1), 1);
            Debug.Log(agent.Inventory);
            OnPickUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
