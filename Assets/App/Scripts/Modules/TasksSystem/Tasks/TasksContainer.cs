using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.TasksSystem.Configs;

namespace App.Scripts.Modules.TasksSystem.Tasks
{
    public class TasksContainer
    {
        public event Action<TasksContainer> OnTaskCompleted;
        public event Action<float> OnProgressChanged;

        public TaskConfig Config { get; }

        public float Progress { get; private set; }

        public TasksContainer(TaskConfig config)
        {
            this.Config = config;

            foreach (var configTask in config.Tasks)
            {
                configTask.OnProgressChanged += OnConfigProgressChanged;
                configTask.OnTaskCompleted += OnConfigTaskCompleted;
            }
        }

        public void StartTask()
        {
            foreach (var task in Config.Tasks)
            {
                task.Start();
            }
        }

        public void CompleteTask()
        {
            foreach (var configTask in Config.CompleteActions)
            {
                configTask.Execute();
            }

            OnTaskCompleted?.Invoke(this);
        }

        public ProgressPair GetProgress()
        {
            return Config.Tasks.Count >= 2 ? new (0, 1) : Config.Tasks[0].GetProgress();
        }

        public void SetState(List<ProgressPair> data)
        {
            if (data == null)
            {
                return;
            }
            
            for (int i = 0; i < Config.Tasks.Count; i++)
            {
                Config.Tasks[i].SetProgress(data[i]);
            }
        }

        public TaskContainerData GetState()
        {
            return new TaskContainerData()
            {
                TaskConfig = Config,
                TaskConfigId = Config.Id,
                TasksData = Config.Tasks.Select(x => x.GetProgress()).ToList(),
            };
        }

        private void OnConfigProgressChanged(float progress)
        {
            if (Config == null || Config.Tasks.Count == 0)
            {
                return;
            }

            Progress = Config.Tasks.Average(task => task.Progress);
            OnProgressChanged?.Invoke(Progress);
            if (progress >= 1)
            {
                CompleteTask();
            }
        }

        private void OnConfigTaskCompleted(ITask completedTask)
        {
            completedTask.OnProgressChanged -= OnConfigProgressChanged;
            completedTask.OnTaskCompleted -= OnConfigTaskCompleted;
        }
    }
}