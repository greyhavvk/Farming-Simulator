using UnityEngine;

namespace Systems.TaskSystem
{
    [CreateAssetMenu(fileName = "NewMoveTask", menuName = "Tasks/MoveTask")]
    public class MoveTask : TaskModel
    {
        public bool movedLeft;
        public bool movedRight;
        public bool movedForward;
        public bool movedBack;

        public override void UpdateTaskProgress(TaskData taskData)
        {
            if (taskData is not MoveTaskData moveTaskData) return;

            var direction = moveTaskData.moveDirection;

            movedBack = direction.y < 0 || movedBack;
            movedLeft = direction.x < 0 || movedLeft;
            movedForward = direction.y > 0 || movedForward;
            movedRight = direction.x > 0 || movedRight;

            if (movedLeft && movedRight && movedForward && movedBack)
            {
                movedLeft = false;
                movedRight = false;
                movedForward = false;
                movedBack = false;
                CompleteTask();
            }
            else
            {
                ShowTask();
            }
        }

        protected override void ShowTask()
        {
            taskUI.UpdateMoveTaskUI();
        }
    }
}