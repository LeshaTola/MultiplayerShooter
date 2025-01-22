using System.Collections.Generic;
using App.Scripts.Features.SceneTransitions;
using App.Scripts.Features.Screens.Providers;
using App.Scripts.Modules.CameraSwitchers;
using App.Scripts.Modules.Saves;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.MainMenu.Roulette.Screen;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.StateMachines.States
{
    public class RouletteState : State
    {
        private readonly RouletteScreenPresentrer _rouletteScreenPresentrer;

        public RouletteState(RouletteScreenPresentrer rouletteScreenPresentrer)
        {
            _rouletteScreenPresentrer = rouletteScreenPresentrer;
        }

        public override async UniTask Enter()
        {
            await _rouletteScreenPresentrer.Show();
        }

        public override async UniTask Exit()
        {
            await _rouletteScreenPresentrer.Hide();
        }
    }
}