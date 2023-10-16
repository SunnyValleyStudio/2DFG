using FarmGame.CarryItemManager;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Farming;
using FarmGame.Hotbar;
using FarmGame.Input;
using FarmGame.Interactions;
using FarmGame.SaveSystem;
using FarmGame.StaminaSystem;
using FarmGame.Tools;
using FarmGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Agent
{
    public class Player : MonoBehaviour, IAgent, ISavable
    {
        [SerializeField]
        private AgentMover _agentMover;
        [SerializeField]
        private PlayerInputFarm _agentInput;
        [SerializeField]
        private AgentAnimation _agentAnimation;

        [SerializeField]
        private InteractionDetector _interactionDetector;

        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        [SerializeField]
        private FieldController _fieldController;

        [SerializeField]
        private ToolSelectionUI _toolSelectionUI;

        [SerializeField]
        private GameObject _playerUI;
        [SerializeField]
        private PlayerInformationSystem _informationSystem;

        [SerializeField]
        private HotbarController _hotbarController;

        [SerializeField]
        private CarryItemSystem _carryItemSystem;

        private bool _blocked = false;

        public bool Blocked
        {
            get
            {
                return _blocked;
            }
            set
            {
                _blocked = value;
                _agentMover.Stopped = _blocked;
                if (_blocked)
                    _agentAnimation.PlayMovementAnimation(false);
                _agentInput.BlockInput(_blocked);
            }
        }

        [field: SerializeField]
        public AgentStamina AgentStaminaSystem { get; private set; }

        [SerializeField]
        private Inventory _inventory;

        public Inventory Inventory
        {
            get => _inventory;
        }


        [SerializeField]
        private FieldDetector _fieldDetector;

        public FieldDetector FieldDetectorObject
        {
            get { return _fieldDetector; }
        }

        public AgentMover AgentMover
        {
            get => _agentMover;
        }
        public PlayerInputFarm AgentInput
        {
            get => _agentInput;
        }
        public AgentAnimation AgentAnimation
        {
            get => _agentAnimation;
        }
        public InteractionDetector InteractionDetector
        {
            get => _interactionDetector;
        }
        [field:SerializeField]
        public ToolsBag ToolsBag { get; private set; }

        [field: SerializeField]
        public AgentDataSO AgentData { get; private set; }

        public FieldController FieldController => _fieldController;

        public int SaveID => SaveIDRepositor.PLAYER_DATA_ID;

        public UnityEvent<Inventory> OnToggleInventory;

        private void OnEnable()
        {
            _agentInput.OnMoveInput.AddListener(_agentMover.SetMovementInput);
            _agentInput.OnMoveInput.AddListener(_agentAnimation.ChangeDirection);
            _agentInput.OnMoveInput.AddListener(_agentAnimation.ToolAnimation.ChangeDirection);
            _agentInput.OnMoveInput.AddListener(_interactionDetector.SetInteractionDirection);
            _agentInput.OnMoveInput.AddListener(_fieldDetector.SetInteractionDirection);
            _agentInput.OnPerformAction += PerformAction;
            _agentInput.OnSwapTool += SwapTool;
            _agentInput.OnToggleInventory += ToggleInventory;
            _agentInput.OnHotBarKey += HandleHotbarSelection;
            _agentInput.OnCancelSelection += CancelHotbarSelection;

            _agentMover.OnMove += _agentAnimation.PlayMovementAnimation;

            Inventory.OnUpdateInventory += _hotbarController.UpdateHotBar;
            
            ToolsBag.OnToolsBagUpdated += _toolSelectionUI.UpdateUI;

            _carryItemSystem.OnStartCarrying += StartCarryAction;
            _carryItemSystem.OnCancelCarrying += StopCarryAction;
        }

        private void StopCarryAction()
        {
            if (Blocked == false)
            {
                _agentAnimation.SetCarrying(false);
            }
        }

        private void StartCarryAction()
        {
            if(Blocked == false)
            {
                _agentAnimation.SetCarrying(true);
            }
        }

        private void CancelHotbarSelection()
        {
            _carryItemSystem.ResetSelection();
        }

        private void HandleHotbarSelection(int id)
        {
            if (Blocked)
                return;
            int index = id - 1;
            _carryItemSystem.StartCarrying(index, Inventory);
        }

        private void ToggleInventory()
        {
            OnToggleInventory?.Invoke(Inventory);
        }

        private void Start()
        {
            ToolsBag.Initialize(this);
            //Debug.Log("<color=red>Resetting Agent Data</color>");
            //AgentData.Money = 0;
            _agentAnimation.ChangeDirection(Vector2.down);
            _hotbarController.UpdateHotBar(Inventory.InventoryContent);
        }

        private void SwapTool()
        {
            ToolsBag.SelectNextTool(this);
        }

        private void PerformAction()
        {
            if (Blocked)
                return;
            if (_carryItemSystem.AmICarryingItem)
            {
                CarryItemInteraction();
            }
            else
            {
                Debug.Log("Interacting");
                ToolType type = ToolsBag.CurrentTool.ToolType;
                if (type == ToolType.Hand || AgentStaminaSystem.IsThereEnoughStamina())
                {
                    ToolsBag.CurrentTool.UseTool(this);
                }
            }
        }

        private void CarryItemInteraction()
        {
            InventoryItemData inventoryItem 
                = Inventory.GetItemDataAt(_carryItemSystem.CarriedItemIndex);
            ItemDescription description = _itemDatabase.GetItemData(inventoryItem.id);
            bool success = false;
            if (description.Consumable)
            {
                Debug.Log("Consuming the carried item");
                AgentStaminaSystem.ModifyStamina(description.EnergyBoost);
                success = true;
            }
            else if (description.CanThrowAway)
            {
                success = true;
            }

            if(success)
            {
                Inventory.RemoveOneItem(inventoryItem);
                _carryItemSystem.ResetSelection();
            }
        }

        private void OnDisable()
        {
            _agentInput.OnMoveInput.RemoveListener(_agentMover.SetMovementInput);
            _agentInput.OnMoveInput.RemoveListener(_agentAnimation.ChangeDirection);
            _agentInput.OnMoveInput.RemoveListener(_agentAnimation.ToolAnimation.ChangeDirection);
            _agentInput.OnMoveInput.RemoveListener(_interactionDetector.SetInteractionDirection);
            _agentInput.OnMoveInput.RemoveListener(_fieldDetector.SetInteractionDirection);
            _agentInput.OnPerformAction -= PerformAction;
            _agentInput.OnSwapTool -= SwapTool;
            _agentInput.OnToggleInventory -= ToggleInventory;
            _agentInput.OnHotBarKey -= HandleHotbarSelection;
            _agentInput.OnCancelSelection -= CancelHotbarSelection;

            _agentMover.OnMove -= _agentAnimation.PlayMovementAnimation;

            Inventory.OnUpdateInventory -= _hotbarController.UpdateHotBar;

            ToolsBag.OnToolsBagUpdated -= _toolSelectionUI.UpdateUI;

            _carryItemSystem.OnStartCarrying -= StartCarryAction;
            _carryItemSystem.OnCancelCarrying -= StopCarryAction;
        }

        public string GetData()
        {
            return AgentData.GetData();
        }

        public void RestoreData(string data)
        {
            AgentData.Inventory = Inventory;
            if(String.IsNullOrEmpty(data))
            {
                AgentData.SetDefaultData();
            }
            else
            {
                AgentData.RestoreData(data);
            }
        }

        public void SetUIVisibility(bool val)
        {
            _playerUI.SetActive(val);
        }

        public void ShowInformation(string infoText)
        {
            _informationSystem.ShowInformation(this, infoText);
        }
    }
}
