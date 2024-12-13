using App.Scripts.Modules.StateMachine.States.General;
using App.Scripts.Scenes.Gameplay.Player.Factories;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RespawnState : State
    {
        private PlayerProvider _playerProvider;
        
        public override async UniTask Enter()
        {
            _playerProvider.RespawnPlayer();
            StateMachine.ChangeState<GameplayState>();
        }
    }
}