using System;
using Systems.InventorySystem;
using Systems.PlacementSystem;
using Systems.PlayerSystem;
using Systems.TaskSystem;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager.InputManager inputManager;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private TaskManager taskManager;
        [SerializeField] private UIManager.UIManager uiManager;
        [SerializeField] private PlacementManager placementManager;
        [SerializeField] private InventoryController inventoryController;

        private void Awake()
        {
            InitializeComponents();
            SetListeners();
        }

        private void OnDisable()
        {
            ClearListeners();
        }

        private void InitializeComponents()
        {
            // InputManager'ı başlat
            inputManager.Initialize();
            
            uiManager.Initialize(RefreshInventory, DropItemFromInventory, FillItemStackFromAnother, inputManager);
            
            inventoryController.Initialize(uiManager);
            
            placementManager.Initialize(inputManager);

            // PlayerController'ı başlat ve inputManager'ı ile birlikte initialize et
            playerController.Initialize(inputManager,uiManager, inventoryController.TryAddItem);
            
            taskManager.Initialize(uiManager, SetTaskListener, ClearTaskListeners);
            
        }

        private void SetListeners()
        {
        }
        
        private void ClearListeners()
        {
            
        }

        public void FillItemStackFromAnother(InventoryItemData target, InventoryItemData filler)
        {
            inventoryController.FillItemStackFromAnother(target, filler);
        }

        private void DropItemFromInventory(InventoryItemData itemData)
        {
            inventoryController.DropItemFromInventory(itemData);
            playerController.RemoveItemFromHotBar(itemData);
        }

        private void RefreshInventory()
        {
            inventoryController.RefreshInventory();
            playerController.RefreshHotBar();
        }
        
        private void ClearTaskListeners()
        {
            playerController.ClearTaskListeners();
        }

        private void SetTaskListener(TaskModel taskModel)
        {
            switch (taskModel)
            {
                case MoveTask:
                    playerController.TriggerTaskListener(taskManager.UpdateTaskProgress);
                    break;
                case SellTask:
                    break;
                case PlantSeedTask:
                    break;
                case HarvestTask:
                    break;
            }
        }
    }
}