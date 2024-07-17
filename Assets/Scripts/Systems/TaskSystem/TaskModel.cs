using System;
using Core;
using Core.UIManager;
using UnityEngine;

namespace Systems.TaskSystem
{
    public abstract class TaskModel : ScriptableObject
    {
        private Action<TaskModel> _onTaskCompleted;
        protected ITaskUI taskUI;

        public abstract void UpdateTaskProgress(TaskData taskData);

        protected abstract void ShowTask();

        public virtual void TaskSelected(ITaskUI taskUI, Action<TaskModel> taskCompleted)
        {
            _onTaskCompleted = taskCompleted;
            this.taskUI = taskUI;
            ShowTask();
        }
        
        protected void CompleteTask()
        {
            _onTaskCompleted?.Invoke(this);
        }
    }
}