using System;
using System.Collections.Generic;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Elements.Texts;
using App.Scripts.Modules.TasksSystem.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen.DailyTasks
{
    public class DailyTasksView : MonoBehaviour
    {
        [SerializeField] private DailyTask _dailyTaskPrefab;
        [SerializeField] private Transform _container;

        [Header("Timer")]
        [SerializeField] private GameObject _timerPanel;
        [SerializeField] private LocalizedWithValue _timerText;
        [SerializeField] private string _localizedPart;

        
        private ILocalizationSystem _localizationSystem;
        private readonly List<DailyTask> _tasks = new ();

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _timerText.Initialize(localizationSystem);
        }

        public void Setup(Dictionary<string, TaskContainerData> tasks)
        {
            CleanupDailyTasks();
            foreach (var task in tasks.Values)
            {
                var dailyTask = Instantiate(_dailyTaskPrefab, _container);
                dailyTask.Initialize(_localizationSystem);
                dailyTask.Setup(task);
                _tasks.Add(dailyTask);
            }
        }

        public void UpdateView(Dictionary<string, TaskContainerData> tasks)
        {
            int i = 0;
            foreach (var task in tasks.Values)
            {
                if (i >= _tasks.Count)
                {
                    return;
                }
                _tasks[i].Setup(task);
                
                i++;
            }
        }

        public void SetActiveTimer(bool active)
        {
            if (_timerPanel == null)
            {
                return;
            }
            
            _timerPanel.SetActive(active);
        }
        
        public void UpdateTimer(TimeSpan time)
        {
            _timerText.Setup(_localizedPart, time.ToString(@"hh\:mm\:ss"));
        }

        public void Cleanup()
        {
            _timerText.Cleanup();
            CleanupDailyTasks();
        }

        private void CleanupDailyTasks()
        {
            foreach (var task in _tasks)
            {
                task.Cleanup();
                Destroy(task.gameObject);
            }
            _tasks.Clear();
        }
    }
}