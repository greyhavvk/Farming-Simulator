using System;
using Core;
using Core.UIManager;
using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.TaskSystem
{
    [CreateAssetMenu(fileName = "NewSellTask", menuName = "Tasks/SellTask")]
    public class SellTask : TaskModel
    {
        [SerializeField] private PlantType requiredPlantType;
        [SerializeField] private int requiredSales;
        [SerializeField] private int currentSales;

        private const string TaskTypeText = "Sell ";
        
        public override void UpdateTaskProgress(TaskData taskData)
        {
            if (taskData is not SellTaskData sellTaskData) return;
            if (requiredPlantType != sellTaskData.plantType) return;
            currentSales+=sellTaskData.sellCount;
            if (currentSales>=requiredSales)
            {
                CompleteTask();
            }
            else
            {
                ShowTask();
            }
        }
        
        public override void TaskSelected(ITaskUI taskUI, Action<TaskModel> taskCompleted)
        {
            currentSales = 0;
            base.TaskSelected(taskUI, taskCompleted);
        }

        protected override void ShowTask()
        {
            taskUI.UpdateTaskUI(requiredPlantType, TaskTypeText+(requiredSales-currentSales));
        }
    }
}