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
        [SerializeField]private UIManager.UIManager uiManager;
        [SerializeField]private PlacementManager placementManager;
        

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // InputManager'ı başlat
            inputManager.Initialize();
            
            placementManager.Initialize(inputManager);

            // PlayerController'ı başlat ve inputManager'ı ile birlikte initialize et
            playerController.Initialize(inputManager);
            
            taskManager.Initialize(uiManager, SetTaskListener, ClearTaskListeners);
        }
        
        public void ClearTaskListeners()
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