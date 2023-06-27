using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Interactions
{
    [RequireComponent(typeof(ItemData))]
    public class PickUpInteraction : MonoBehaviour, IInteractable
    {
        private ItemData _data;
        [SerializeField]
        private bool _destroyAfterPickup = true;
        [field: SerializeField]
        public ItemDatabaseSO ItemDatabase { get; set; }

        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; }
            = new List<ToolType>();

        public UnityEvent OnPickUp;

        private void Awake()
        {
            _data = GetComponent<ItemData>();
        }
        public bool CanInteract(IAgent agent)
            => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent)
        {
            //agent.Inventory.AddItem(new InventoryItemData(0, 1, -1), 1);
            InventoryItemData item = new InventoryItemData(_data.itemDatabaseIndex,
                _data.itemCount,
                _data.itemQuality);
            ItemDescription description = ItemDatabase.GetItemData(_data.itemDatabaseIndex);
            int stackSize = description.CanBeStacked ? description.StackQuantity : -1;
            if(agent.Inventory != null && _data.itemCount > 0
                && agent.Inventory.IsThereSpace(item, stackSize))
            {
                agent.Inventory.AddItem(item, stackSize);
                Debug.Log(agent.Inventory);
                OnPickUp?.Invoke();
                if(_destroyAfterPickup)
                {
                    Debug.Log($"Destroying {description.Name} {gameObject.name}");
                    Destroy(gameObject);
                }
                else
                {
                    _data.itemCount = 0;
                }
            }
        }
    }
}
