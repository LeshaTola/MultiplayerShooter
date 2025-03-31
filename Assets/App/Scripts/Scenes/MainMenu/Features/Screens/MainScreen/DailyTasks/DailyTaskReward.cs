using App.Scripts.Modules.Localization;
using App.Scripts.Modules.Localization.Elements.Texts;
using App.Scripts.Modules.TasksSystem.CompleteActions;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen.DailyTasks
{
    public class DailyTaskReward : MonoBehaviour
    {
        [SerializeField] private Image _rewardImage;
        [SerializeField] private LocalizedWithValue _rewardText;

        public void Initialize(ILocalizationSystem localizationSystem)
        {
            _rewardText.Initialize(localizationSystem);
        }

        public void Setup(RewardData rewardConfig)
        {
            _rewardImage.sprite = rewardConfig.Sprite;
            _rewardImage.color = rewardConfig.Color;
            _rewardText.Setup(rewardConfig.Text, rewardConfig.ValueText);
        }

        public void Cleanup()
        {
            _rewardText.Cleanup();
        }
    }
}