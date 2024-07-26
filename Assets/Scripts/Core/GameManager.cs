using Systems.FarmingSystems;
using Systems.FinanceSystem;
using Systems.InventorySystem;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using Systems.MarketSystem;
using Systems.PlacementSystem;
using Systems.PlayerSystem;
using Systems.TaskSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private FinanceController financeController;
        [SerializeField] private MarketController marketController;

        private GameState _gameState;
        private GameState _previousState;

        private void Awake()
        {
            InitializeComponents();
            SetListeners();
            ChangeGameState(GameState.Normal);
            playerController.GainControls();
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

            farmingController.Initialize(ReduceStackableItemFromSeedStack,
                inventoryController.InstanceItemFromPosition);

            inventoryController.Initialize(uiManager);

            placementManager.Initialize(inputManager, farmingController, PlacementEnded);

            // PlayerController'ı başlat ve inputManager'ı ile birlikte initialize et
            playerController.Initialize(inputManager, uiManager, inventoryController.TryAddDroppedItem,
                farmingController, MarketInteracted);

            taskManager.Initialize(uiManager, SetTaskListener, ClearTaskListeners);

            financeController.Initialize(uiManager, marketController.MoneyValueChanged);

            marketController.Initialize(financeController, uiManager, StartPlacement,
                inventoryController.InstanceItemFromPosition, ItemSold);

        }
        
        private void Update()
        {
            ListenInputs();
        }

        private void ListenInputs()
        {
            switch (_gameState)
            {
                case GameState.Normal:
                    if (inputManager.GetInventoryUITriggerInput())
                    {
                        OpenInventory();
                    }
                    else if (inputManager.GetSettingUITriggerInput())
                    {
                        OpenSetting();
                    }

                    break;
                case GameState.Inventory:
                    if (inputManager.GetInventoryUITriggerInput())
                    {
                        CloseInventory();
                    }
                    else if (inputManager.GetSettingUITriggerInput())
                    {
                        OpenSetting();
                    }

                    break;
                case GameState.Market:
                    if (inputManager.GetSettingUITriggerInput())
                    {
                        OpenSetting();
                    }

                    break;
                case GameState.Setting:
                    if (inputManager.GetSettingUITriggerInput())
                    {
                        CloseSetting();
                    }

                    break;
                case GameState.Building:
                    if (inputManager.GetSettingUITriggerInput())
                    {
                        OpenSetting();
                    }

                    break;
            }
        }
        
        private void PlacementEnded()
        {
            ChangeGameState(GameState.Normal);
        }

        private void StartPlacement(int placeAbleItemID)
        {
            placementManager.StartPlacement(placeAbleItemID);
            marketController.CloseMarket();
            playerController.GainControls();
            ChangeGameState(GameState.Building);
        }

        private void OpenInventory()
        {
            playerController.LoseControls();
            inventoryController.InventoryUITriggered();
            ChangeGameState(GameState.Inventory);
        }

        public void CloseInventory()
        {
            playerController.GainControls();
            inventoryController.InventoryUITriggered();
            ChangeGameState(GameState.Normal);
        }

        public void OpenSetting()
        {
            Time.timeScale = 0;
            ChangeGameState(GameState.Setting);
            uiManager.OpenSettingPanel();
            playerController.LoseControls();
        }

        public void CloseSetting()
        {
            Time.timeScale = 1;
            if (_previousState==GameState.Normal || _previousState==GameState.Building)
            {
                playerController.GainControls();
            }
            ChangeGameState(_previousState);
            uiManager.CloseSettingPanel();
        }

        private void ChangeGameState(GameState newState)
        {
            _previousState = _gameState;
            _gameState = newState;
        }

        private void ItemSold(FarmingItemData farmingItemData)
        {
            inventoryController.RemoveItem(farmingItemData);
            playerController.RemoveItemFromHotBar(farmingItemData);
            RefreshInventory();
        }

        private void MarketInteracted()
        {
            playerController.LoseControls();
            ChangeGameState(GameState.Market);
            marketController.OpenMarket(inventoryController.GetItemsForSell());
        }

        public void CloseMarket()
        {
            marketController.CloseMarket();
            playerController.GainControls();
            ChangeGameState(GameState.Normal);
        }

        private void SetListeners()
        {
        }

        private void ClearListeners()
        {
            ClearTaskListeners();
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
            inventoryController.ReduceStackableItemFromSeedStack(stackableFarmingItem, reduceCount, out var removedItem);
            if (removedItem!=null)
            {
                playerController.RemoveItemFromHotBar(removedItem);
            }
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
            marketController.ClearTaskListeners();
        }

        private void SetTaskListener(TaskModel taskModel)
        {
            switch (taskModel)
            {
                case MoveTask:
                    playerController.TriggerTaskListener(taskManager.UpdateTaskProgress);
                    break;
                case SellTask:
                    marketController.TriggerTaskListener(taskManager.UpdateTaskProgress);
                    break;
                case PlantSeedTask:
                    farmingController.TriggerTaskListener(taskManager.UpdateTaskProgress);
                    break;
                case HarvestTask:
                    farmingController.TriggerTaskListener(taskManager.UpdateTaskProgress);
                    break;
            }
        }

        public void OnQuitButtonClicked()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}