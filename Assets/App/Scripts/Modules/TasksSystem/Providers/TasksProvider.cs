using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.TasksSystem.Configs;
using App.Scripts.Modules.TasksSystem.Tasks;
using Random = UnityEngine.Random;

namespace App.Scripts.Modules.TasksSystem.Providers
{
    public class TasksProvider
    {
        public event Action<Dictionary<string, TaskContainerData>> OnTasksUpdated;

        private readonly TaskProviderConfig _config;

        private int _lastTaskId;

        public Dictionary<string, TaskContainerData> ActiveTasks { get; } = new();
        

        public TasksProvider(TaskProviderConfig config)
        {
            _config = config;
        }

        public void FillTasks()
        {
            for (int i = 0; i < _config.MaxTasksCount; i++)
            {
                GetTask();
            }
            OnTasksUpdated?.Invoke(ActiveTasks);
        }

        public void ClearTasks()
        {
            ActiveTasks.Clear();
            OnTasksUpdated?.Invoke(ActiveTasks);
        }

        private void GetTask()
        {
            TaskConfig config;
            do
            {
                var id = GetConfigId();
                config = _config.TasksPool.Tasks[id];
            } while (ActiveTasks.ContainsKey(config.Id));

            ActiveTasks.Add(config.Id, new()
            {
                TaskConfig = config,
                TaskConfigId = config.Id,
                TasksData = null,
                Progress = config.Tasks.Count >= 2 ? new (0, 1) : config.Tasks[0].GetProgress()
            });
        }

        private int GetConfigId()
        {
            var id = _config.IsRandom ? Random.Range(0, _config.TasksPool.Tasks.Count) : _lastTaskId++;
            if (id >= _config.TasksPool.Tasks.Count)
            {
                id = 0;
                _lastTaskId = 0;
            }
            return id;
        }

        public void RemoveTask(string configId)
        {
            if (!ActiveTasks.ContainsKey(configId))
            {
                return;
            }
            
            ActiveTasks.Remove(configId);
            OnTasksUpdated?.Invoke(ActiveTasks);
        }

        public void UpdateProgress(TasksContainer tasksContainer)
        {
            if (!ActiveTasks.TryGetValue(tasksContainer.Config.Id, out var containerData))
            {
                return;
            }
            containerData.Progress = tasksContainer.GetProgress();
            containerData.TasksData = tasksContainer.Config.Tasks.Select(x=>x.GetProgress()).ToList();
        }
    }
}