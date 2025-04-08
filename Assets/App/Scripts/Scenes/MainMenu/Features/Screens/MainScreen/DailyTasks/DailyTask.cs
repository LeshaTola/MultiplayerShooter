using System.Collections.Generic;
using System.Linq;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Elements.Texts;
using App.Scripts.Modules.TasksSystem.CompleteActions;
using App.Scripts.Modules.TasksSystem.Configs;
using App.Scripts.Modules.TasksSystem.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen.DailyTasks
{
    public class DailyTask : MonoBehaviour
    {
        [SerializeField] private LocalizedWithValue _task;
        [SerializeField] private Slider _progressSlider;

        [Header("Rewards")]
        [SerializeField] private DailyTaskReward _rewardPrefab;

        [SerializeField] private RectTransform _container;
        [SerializeField] private int _maxRewards = 2;

        private ILocalizationSystem _localizationSystem;
        private List<DailyTaskReward> _rewards = new();

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _task.Initialize(localizationSystem);
        }

        public void Setup(TaskContainerData task)
        {
            SetupHeader(task);
            SetupSlider(task);

            CleanupRewards();
            var rewards = GetRewardsData(task.TaskConfig);
            foreach (var rewardData in rewards)
            {
                var reward = Instantiate(_rewardPrefab, _container);
                reward.Initialize(_localizationSystem);
                reward.Setup(rewardData);
                _rewards.Add(reward);
            }
        }

        public void Cleanup()
        {
            _task.Cleanup();
            CleanupRewards();
        }

        private void SetupSlider(TaskContainerData task)
        {
            _progressSlider.value = ((float)task.Progress.Progress)/task.Progress.Target;
        }

        private void SetupHeader(TaskContainerData task)
        {
            var progressValues = task.Progress;
            var progressText = $"{progressValues.Progress}/{progressValues.Target}";
            _task.Setup(task.TaskConfig.Name, progressText);
        }

        private List<RewardData> GetRewardsData(TaskConfig config)
        {
            return config.CompleteActions
                .SelectMany(x => x.GetRewardData())
                .Take(_maxRewards)
                .ToList();
        }

        private void CleanupRewards()
        {
            foreach (var child in _rewards)
            {
                child.Cleanup();
                Destroy(child.gameObject);
            }

            _rewards.Clear();
        }
    }
}