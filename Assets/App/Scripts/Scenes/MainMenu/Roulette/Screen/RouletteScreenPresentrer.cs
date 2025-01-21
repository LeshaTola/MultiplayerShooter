using App.Scripts.Features.Screens;
using App.Scripts.Scenes.MainMenu.Roulette.Configs;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.MainMenu.Roulette.Screen
{
    public class RouletteScreenPresentrer : GameScreenPresenter
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
            _rouletteScreen.Setup(_rouletteConfig);
            _rouletteScreen.Initialize();
            _roulette.GenerateRoulette();
        }

        public override void Cleanup()
        {
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