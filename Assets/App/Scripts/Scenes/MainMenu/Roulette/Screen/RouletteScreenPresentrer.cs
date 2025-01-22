using App.Scripts.Features.Screens;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Scenes.MainMenu.Roulette.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.Roulette.Screen
{
    public class RouletteScreenPresentrer : GameScreenPresenter, IInitializable, ICleanupable
    {
        private readonly RouletteConfig _rouletteConfig;
        private readonly RouletteScreen _rouletteScreen;
        private readonly Roulette _roulette;

        public RouletteScreenPresentrer(RouletteConfig rouletteConfig,
            RouletteScreen rouletteScreen,
            Roulette roulette)
        {
            _rouletteConfig = rouletteConfig;
            _rouletteScreen = rouletteScreen;
            _roulette = roulette;
        }

        public override void Initialize()
        {
            _rouletteScreen.SpinButtonPressed += OnSpinButtonPressed;
            _rouletteScreen.Setup(_rouletteConfig);
            _rouletteScreen.Initialize();
            _roulette.GenerateRoulette();
        }

        private async void OnSpinButtonPressed()
        {
            _rouletteScreen.SetBlockSreen(true);
             var angle = await _roulette.SpinRoulette();
             var result = _roulette.GetConfigByAngle(angle);
             Debug.Log($"Angle: {angle} Sector: {result.Name}");
            _rouletteScreen.SetBlockSreen(false);
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
    }
}