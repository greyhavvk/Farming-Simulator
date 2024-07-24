using System;
using Systems.FarmingSystems;
using Systems.InventorySystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
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
        [SerializeField] private FarmingController farmingController;
        
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
            
            farmingController.Initialize(ReduceStackableItemFromSeedStack, inventoryController.InstanceItemFromPosition);
            
            inventoryController.Initialize(uiManager, inputManager);
            
            placementManager.Initialize(inputManager,farmingController);

            // PlayerController'ı başlat ve inputManager'ı ile birlikte initialize et
            playerController.Initialize(inputManager,uiManager, inventoryController.TryAddDroppedItem, farmingController, MarketInteracted);
            
            taskManager.Initialize(uiManager, SetTaskListener, ClearTaskListeners);
            
        }

        private void MarketInteracted()
        {
            //TODO market açmak için tetikleyici
        }

        private void SetListeners()
        {
        }
        
        private void ClearListeners()
        {
            
        }

        private void FillItemStackFromAnother(FarmingItemData target, FarmingItemData filler)
        {
            inventoryController.FillItemStackFromAnother(target, filler);
        }

        private void DropItemFromInventory(FarmingItemData itemData)
        {
            inventoryController.DropItemFromInventory(itemData);
            playerController.RemoveItemFromHotBar(itemData);
        }
        
        private void ReduceStackableItemFromSeedStack(StackableFarmingItem stackableFarmingItem, int reduceCount)
        {
            inventoryController.ReduceStackableItemFromSeedStack(stackableFarmingItem, reduceCount);
            RefreshInventory();
        }

        private void RefreshInventory()
        {
            inventoryController.RefreshInventory();
            playerController.RefreshHotBar();
        }
        
        private void ClearTaskListeners()
        {
            playerController.ClearTaskListeners();
            farmingController.ClearTaskListeners();
        }

        //todo taskler'a satma task'i de eklenecek.
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
                    farmingController.TriggerTaskListener(taskManager.UpdateTaskProgress);
                    break;
                case HarvestTask:
                    farmingController.TriggerTaskListener(taskManager.UpdateTaskProgress);
                    break;
            }
        }
    }
}