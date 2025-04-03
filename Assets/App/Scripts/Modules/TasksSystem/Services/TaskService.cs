using System;
using System.Collections.Generic;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Modules.TasksSystem.Configs;
using App.Scripts.Modules.TasksSystem.Factories;
using App.Scripts.Modules.TasksSystem.Providers;
using App.Scripts.Modules.TasksSystem.Tasks;
using UnityEngine;

namespace App.Scripts.Modules.TasksSystem.Services
{
    public class TaskService : IInitializable, ICleanupable
    {
        public event Action<List<TasksContainer>> OnTasksUpdated;

        private readonly TasksContainerFactory _factory;
        private readonly TasksProvider _tasksProvider;

        public List<TasksContainer> ActiveTasks { get; } = new();

        public TaskService(TasksProvider tasksProvider, TasksContainerFactory factory)
        {
            _tasksProvider = tasksProvider;
            _factory = factory;
        }

        public void Initialize()
        {
            foreach (var containerData in _tasksProvider.ActiveTasks.Values)
            {
                GetTask(containerData);
            }
            OnTasksUpdated?.Invoke(ActiveTasks);
        }

        public void Cleanup()
        {
            ActiveTasks.Clear();
            OnTasksUpdated?.Invoke(ActiveTasks);
            _tasksProvider.SaveState();
        }

        private void GetTask(TaskContainerData taskData)
        {
            var taskConfig = taskData.TaskConfig;
            if (taskConfig == null)
            {
                return;
            }
            
            var tasksContainer = _factory.GetTaskContainer(taskConfig);
            tasksContainer.SetState(taskData.TasksData);
            RegisterTask(tasksContainer);
            ActiveTasks.Add(tasksContainer);
        }
        
        private void RegisterTask(TasksContainer task)
        {
            task.StartTask();
            task.OnTaskCompleted += UnregisterTask;
            task.OnProgressChanged += (progress) => OnProgressChanged(task);
        }

        private void OnProgressChanged(TasksContainer task)
        {
            Debug.Log($"Task Progress: {task.Progress}");
            _tasksProvider.UpdateProgress(task);
        }

        private void UnregisterTask(TasksContainer task)
        {
            task.OnTaskCompleted -= UnregisterTask;
            ActiveTasks.Remove(task);
            _tasksProvider.RemoveTask(task.Config.Id);
            _tasksProvider.SaveState();
        }
    }
}