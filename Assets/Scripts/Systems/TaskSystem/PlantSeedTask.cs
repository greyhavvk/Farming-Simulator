using System;
using Core;
using Core.UIManager;
using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.TaskSystem
{
    [CreateAssetMenu(fileName = "NewPlantSeedTask", menuName = "Tasks/PlantSeedTask")]
    public class PlantSeedTask : TaskModel
    {
        [SerializeField] private PlantType requiredPlantType;
        [SerializeField] private int requiredSeedsPlanted;
        [SerializeField] private int currentSeedsPlanted;

        private const string TaskTypeText = "Plant ";

        public override void UpdateTaskProgress(TaskData taskData)
        {
            if (taskData is not PlantSeedData plantSeedData) return;
            if (requiredPlantType != plantSeedData.plantType) return;
            currentSeedsPlanted++;
            if (currentSeedsPlanted>=requiredSeedsPlanted)
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
            currentSeedsPlanted = 0;
            base.TaskSelected(taskUI, taskCompleted);
        }

        protected override void ShowTask()
        {
            taskUI.UpdateTaskUI(requiredPlantType, TaskTypeText+(requiredSeedsPlanted-currentSeedsPlanted));
        }
    }
}