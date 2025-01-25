using System.Collections.Generic;
using App.Scripts.Features.PlayerStats;
using App.Scripts.Features.UserStats;
using App.Scripts.Modules.Localization;
using App.Scripts.Modules.PopupAndViews.General.Controllers;
using App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards.Configs;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Features.UserStats.Rewards
{
    public class RewardsPopupRouter
    {
        private readonly IPopupController _popupController;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly UserRankProvider _userRankProvider;

        public RewardsPopupRouter(
            IPopupController popupController,
            ILocalizationSystem localizationSystem,
            UserRankProvider userRankProvider)
        {
            _popupController = popupController;
            _localizationSystem = localizationSystem;
            _userRankProvider = userRankProvider;
        }
        
        private RewardsPopup _popup;

        public async UniTask ShowPopup(List<RewardConfig> rewards, int levels, float expValue)
        {
            if (_popup == null)
            {
                _popup = _popupController.GetPopup<RewardsPopup>();
            }

            var viewModule = new RewardsPopupVM(_localizationSystem, _userRankProvider,rewards,levels,expValue);
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