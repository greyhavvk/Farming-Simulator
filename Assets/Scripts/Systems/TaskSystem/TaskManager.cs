using System;
using System.Collections.Generic;
using Core.UIManager.Task;
using UnityEngine;

namespace Systems.TaskSystem
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<TaskModel> tutorialTasks;
        [SerializeField] private List<TaskModel> loopTasks;
        private ITaskUI _taskUI;
        private Action<TaskModel> _updateTaskProgressListener; 
        private Action _clearTaskListeners;

        private List<TaskModel> _currentTaskList;
    
        public void Initialize(ITaskUI taskUI, Action<TaskModel> updateTaskProgressListener, Action clearTaskListeners)
        {
            _taskUI = taskUI;
            _updateTaskProgressListener = updateTaskProgressListener;
            _clearTaskListeners = clearTaskListeners;
            _currentTaskList = tutorialTasks;
        
            SetTask();
        }

        public void DisableListeners()
        {
            _clearTaskListeners = null;
            _updateTaskProgressListener = null;
        }

        private void HandleTaskCompleted(TaskModel task)
        {
            _taskUI.ClearTaskUI();
            if (_currentTaskList.Count>0)
            {
                _currentTaskList.Remove(_currentTaskList[0]);
            }
            else
            {
                _currentTaskList = new List<TaskModel>(loopTasks);
            }
            _clearTaskListeners?.Invoke();
            SetTask();
        }

        private void SetTask()
        {
            var newTask = _currentTaskList[0];
            newTask.TaskSelected(_taskUI, HandleTaskCompleted);
        
            _updateTaskProgressListener?.Invoke(newTask);
        }

        public void UpdateTaskProgress(TaskData taskData)
        {
            _currentTaskList[0].UpdateTaskProgress(taskData);
        }
    }
}