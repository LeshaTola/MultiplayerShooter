using App.Scripts.Features.Commands;
using App.Scripts.Features.Screens;
using App.Scripts.Modules.Commands.General;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Features.Roulette.Configs;
using App.Scripts.Scenes.MainMenu.Features.Screens.MainScreen;
using App.Scripts.Scenes.MainMenu.Features.UserStats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Features.Roulette.Screen
{
    public class RouletteScreenPresentrer : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly RouletteConfig _rouletteConfig;
        private readonly RouletteScreen _rouletteScreen;
        private readonly Roulette _roulette;
        private readonly TicketsProvider _ticketsProvider;
        private readonly InfoPopupRouter _infoPopupRouter;

        public RouletteScreenPresentrer(RouletteConfig rouletteConfig,
            RouletteScreen rouletteScreen,
            Roulette roulette, 
            TicketsProvider ticketsProvider, 
            InfoPopupRouter infoPopupRouter)
        {
            _rouletteConfig = rouletteConfig;
            _rouletteScreen = rouletteScreen;
            _roulette = roulette;
            _ticketsProvider = ticketsProvider;
            _infoPopupRouter = infoPopupRouter;
        }

        public override void Initialize()
        {
            _rouletteScreen.SpinButtonPressed += OnSpinButtonPressed;
            
            _rouletteScreen.Setup(_rouletteConfig);
            _rouletteScreen.Initialize();
            _roulette.GenerateRoulette();
        }

        public override void Cleanup()
        {
            _rouletteScreen.SpinButtonPressed -= OnSpinButtonPressed;
            _rouletteScreen.Cleanup();
        }

        public override async UniTask Show()
        {
            _rouletteScreen.SetupTicketsCount(0);
            await _rouletteScreen.Show();
        }

        public override async UniTask Hide()
        {
            await _rouletteScreen.Hide();
        }

        private async void OnSpinButtonPressed()
        {
            if (!_ticketsProvider.IsEnough(1))
            {
                await _infoPopupRouter.ShowPopup("Внимание!", "Не достаточно билетов");
                return;
            }
            
            _rouletteScreen.SetBlockSreen(true);
            var angle = await _roulette.SpinRoulette();
            var result = _roulette.GetConfigByAngle(angle);
            Debug.Log($"Angle: {angle} Sector: {result.Name}");
            _rouletteScreen.SetBlockSreen(false);
        }
    }
}