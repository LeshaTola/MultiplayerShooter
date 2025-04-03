using System;
using System.Collections.Generic;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.TasksSystem.Providers;
using App.Scripts.Modules.TasksSystem.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen.DailyTasks
{
    public class DailyTaskViewPresenter : IInitializable, ICleanupable
    {
        private readonly DailyTasksView _dailyTasksView;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly TasksProvider _tasksProvider;

        public DailyTaskViewPresenter(ILocalizationSystem localizationSystem,
            DailyTasksView dailyTasksView,
            TasksProvider tasksProvider)
        {
            _localizationSystem = localizationSystem;
            _dailyTasksView = dailyTasksView;
            _tasksProvider = tasksProvider;
        }

        public void Initialize()
        {
            _dailyTasksView.Initialize(_localizationSystem);
            _tasksProvider.OnTasksUpdated += UpdateView;
            _tasksProvider.OnTasksProgressUpdated += UpdateViewProgress;
            _tasksProvider.OnTimerUpdated += OnTimerUpdated;
            UpdateView(_tasksProvider.ActiveTasks);
        }

        private void UpdateViewProgress(Dictionary<string, TaskContainerData> tasks)
        {
            UpdateView(tasks);
        }

        private void OnTimerUpdated(TimeSpan timeSpan)
        {
            _dailyTasksView.UpdateTimer(timeSpan);
        }

        public void Cleanup()
        {
            _dailyTasksView.Cleanup();
            _tasksProvider.OnTasksUpdated -= UpdateView;
            _tasksProvider.OnTasksProgressUpdated -= UpdateViewProgress;
            _tasksProvider.OnTimerUpdated -= OnTimerUpdated;
        }

        private void UpdateView(Dictionary<string, TaskContainerData> tasks)
        {
            _dailyTasksView.SetActiveTimer(tasks.Count < 3);
            _dailyTasksView.Setup(tasks);
        }
    }
}