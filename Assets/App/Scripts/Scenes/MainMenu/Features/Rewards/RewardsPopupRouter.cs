using System.Collections.Generic;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.Rewards.Configs;
using App.Scripts.Features.UserStats;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.MinMaxValue;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardsPopupRouter
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;

        public RewardsPopupRouter(
            IPopupController popupController,
            ILocalizationSystem localizationSystem)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
        }
        
        private RewardsPopup _popup;

        public async UniTask ShowPopup(List<RewardConfig> rewards,  List<ExpAnimationData> animationDatas)
        {
            if (_popup == null)
            {
                _popup = _popupController.GetPopup<RewardsPopup>();
            }

            var viewModule = new RewardsPopupVM(_localizationSystem,rewards,animationDatas);
            _popup.Setup(viewModule);

            await _popup.Show();
        }

        public async UniTask HidePopup()
        {
            if (_popup == null)
            {
                return;
            }

            await _popup.Hide();
            _popup = null;
        }
    }
}