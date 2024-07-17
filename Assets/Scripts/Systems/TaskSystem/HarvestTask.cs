using System;
using Core;
using Core.UIManager;
using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.TaskSystem
{
    [CreateAssetMenu(fileName = "NewHarvestTask", menuName = "Tasks/HarvestTask")]
    public class HarvestTask : TaskModel
    {
        [SerializeField] private PlantType requiredPlantType;
        [SerializeField] private int requiredHarvests;
        [SerializeField] private int currentHarvests;

        private const string TaskTypeText = "Harvest ";
        
        public override void UpdateTaskProgress(TaskData taskData)
        {
            if (taskData is not HarvestTaskData harvestTaskData) return;
            if (requiredPlantType != harvestTaskData.plantType) return;
            currentHarvests++;
            if (currentHarvests>=requiredHarvests)
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
            currentHarvests = 0;
            base.TaskSelected(taskUI, taskCompleted);
        }

        protected override void ShowTask()
        {
            taskUI.UpdateTaskUI(requiredPlantType, TaskTypeText+(requiredHarvests-currentHarvests));
        }
    }
}