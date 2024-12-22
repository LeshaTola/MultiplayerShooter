using App.Scripts.Features.Input;
using App.Scripts.Modules.StateMachine.Services.CleanupService;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.Esc;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using App.Scripts.Scenes.Gameplay.LeaderBoard;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class PlayerController : IInitializable, ICleanupable, IUpdatable
    {
        private readonly LeaderBoardView _leaderBoardView;
        private readonly GameInputProvider _gameInputProvider;
        private readonly MouseSensivityProvider _mouseSensivityProvider;
        
        private IControllable _controllable;

        public bool IsBusy { get; set; } = false;

        public PlayerController(GameInputProvider gameInputProvider,
            MouseSensivityProvider mouseSensivityProvider, 
            LeaderBoardView leaderBoardView)
        {
            _gameInputProvider = gameInputProvider;
            _mouseSensivityProvider = mouseSensivityProvider;
            _leaderBoardView = leaderBoardView;
        }
        
        public void Initialize()
        {
            _gameInputProvider.OnLeftMouseStarted += AttackStarted;
            _gameInputProvider.OnLeftMouseCanceled += AttackCanceled;
            
            _gameInputProvider.OnRightMouseStarted += AttackAlternativeStarted;
            _gameInputProvider.OnRightMouseCanceled += AttackAlternativeCanceled;
            
            _gameInputProvider.OnSpace += JumpPerformed;
            _gameInputProvider.OnR += ReloadPerformed;
            _gameInputProvider.OnTabPerformed += OnTabPerformed;
            _gameInputProvider.OnTabCanceled += OnTabCanceled;
        }

        public void Setup(IControllable controllable)
        {
            _controllable = controllable;
        }

        public void Cleanup()
        {
            if (_gameInputProvider == null)
            {
                return;
            }

            _gameInputProvider.OnLeftMouseStarted -= AttackStarted;
            _gameInputProvider.OnLeftMouseCanceled -= AttackCanceled;
            
            _gameInputProvider.OnRightMouseStarted -= AttackAlternativeStarted;
            _gameInputProvider.OnRightMouseCanceled -= AttackAlternativeCanceled;
            
            _gameInputProvider.OnSpace -= JumpPerformed;
            _gameInputProvider.OnR -= ReloadPerformed;
            _gameInputProvider.OnTabPerformed -= OnTabPerformed;
            _gameInputProvider.OnTabCanceled -= OnTabCanceled;
        }

        private void JumpPerformed()
        {
            if (IsBusy)
            {
                return;
            }
            
            _controllable.Jump();
        }

        private void AttackStarted()
        {
            if (IsBusy)
            {
                return;
            }
            
            _controllable.StartAttack();
        }

        private void AttackCanceled()
        {
            _controllable.CancelAttack();
        }

        public void Update()
        {
            if (IsBusy)
            {
                _controllable.Move(Vector2.zero);
                _controllable.MoveCamera(Vector2.zero);
                return;
            }
         
            _controllable.Move(_gameInputProvider.GetMovementNormalized());
            _controllable.MoveCamera(_gameInputProvider.GetMouseLook() * _mouseSensivityProvider.Sensivity);
            Debug.Log(_mouseSensivityProvider.Sensivity);
        }


        private void ReloadPerformed()
        {
            if (IsBusy)
            {
                return;
            }
            
            _controllable.Reload();
        }

        private void OnTabPerformed()
        {
            if (IsBusy)
            {
                return;
            }
            
            _leaderBoardView.Show();
        }

        private void OnTabCanceled()
        {
            if (IsBusy)
            {
                return;
            }
            
            _leaderBoardView.Hide();
        }
        private void AttackAlternativeStarted()
        {
            if (IsBusy)
            {
                return;
            }
            _controllable.StartAttackAlternative();
        }

        private void AttackAlternativeCanceled()
        {
            _controllable.CancelAttackAlternative();
        }
    }
}