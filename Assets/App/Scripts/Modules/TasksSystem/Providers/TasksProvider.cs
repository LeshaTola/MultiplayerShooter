using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Modules.TasksSystem.Configs;
using App.Scripts.Modules.TasksSystem.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Modules.TasksSystem.Providers
{
    public class TasksProvider : ISavable, IUpdatable
    {
        public event Action<Dictionary<string, TaskContainerData>> OnTasksUpdated;
        public event Action<Dictionary<string, TaskContainerData>> OnTasksProgressUpdated;
        public event Action<TimeSpan> OnTimerUpdated;
        
        private readonly TaskProviderConfig _config;
        private readonly IDataProvider<TasksData> _dataProvider;
        
        private int _lastTaskId;
        private DateTime _nextUpdateTime;
        private TimeSpan _remainingTime;

        public Dictionary<string, TaskContainerData> ActiveTasks { get; } = new();
        
        public TasksProvider(TaskProviderConfig config, IDataProvider<TasksData> dataProvider)
        {
            _config = config;
            _dataProvider = dataProvider;
        }

        public void Update()
        {
            _remainingTime -= TimeSpan.FromSeconds(Time.deltaTime);
            OnTimerUpdated?.Invoke(_remainingTime);
            
            if (_remainingTime <= TimeSpan.Zero)
            {
                FillTasks();
                SaveState();
                CalculateNextUpdateTime();
            }
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
            OnTasksProgressUpdated?.Invoke(ActiveTasks);
        }

        public void SaveState()
        {
            _dataProvider.SaveData(GetState());
        }

        public void LoadState()
        {
            if (!_dataProvider.HasData())
            {
                FillTasks();
                SaveState();
                return;
            }
            SetState();
        }

        private void SetState()
        {
            ActiveTasks.Clear();
            var data = _dataProvider.GetData();
            if (IsCurrentDay(data.LastUpdateDate))
            {
                FillTasks();
                SaveState();
                return;
            }
            
            foreach (var task in data.Tasks)
            {
                AddTask(task);
            }
            OnTasksUpdated?.Invoke(ActiveTasks);
            CalculateNextUpdateTime();
        }

        private bool IsCurrentDay(long lastUpdateDateLong)
        {
            var lastUpdateDate = DateTimeOffset.FromUnixTimeSeconds(lastUpdateDateLong).UtcDateTime;
            return lastUpdateDate.Date != DateTime.Now.Date;
        }

        private TasksData GetState()
        {
            return new TasksData()
            {
                LastUpdateDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Tasks = ActiveTasks.Values.ToList()
            };
        }
        
        private void FillTasks()
        {
            ActiveTasks.Clear();
            for (int i = 0; i < _config.MaxTasksCount; i++)
            {
                GetTask();
            }
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

            AddTask(config);
        }

        private void AddTask(TaskConfig config)
        {
            ActiveTasks.Add(config.Id, new()
            {
                TaskConfig = config,
                TaskConfigId = config.Id,
                TasksData = null,
                Progress = config.Tasks.Count >= 2 ? new (0, 1) : config.Tasks[0].GetProgress()
            });
        }
        
        private void AddTask(TaskContainerData taskData)
        {
            taskData.TaskConfig = _config.TasksPool.Tasks.FirstOrDefault(x=>x.Id.Equals(taskData.TaskConfigId));
            ActiveTasks.Add(taskData.TaskConfigId, taskData);
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

        private void CalculateNextUpdateTime()
        {
            var now = DateTime.Now;
            _nextUpdateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1);
            _remainingTime = _nextUpdateTime - now;
        }
    }
}
