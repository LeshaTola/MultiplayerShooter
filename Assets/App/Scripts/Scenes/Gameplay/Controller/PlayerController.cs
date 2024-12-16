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
        private readonly EscScreenPresenter _escScreenPresenter;
        
        private IControllable _controllable;

        public bool IsBusy { get; set; } = false;

        public PlayerController(GameInputProvider gameInputProvider,
            MouseSensivityProvider mouseSensivityProvider, 
            EscScreenPresenter escScreenPresenter,
            LeaderBoardView leaderBoardView)
        {
            _gameInputProvider = gameInputProvider;
            _mouseSensivityProvider = mouseSensivityProvider;
            _escScreenPresenter = escScreenPresenter;
            _leaderBoardView = leaderBoardView;
        }
        
        public void Initialize()
        {
            _gameInputProvider.OnLeftMouseStarted += AttackStarted;
            _gameInputProvider.OnLeftMouseCanceled += AttackCanceled;
            _gameInputProvider.OnSpace += JumpPerformed;
            _gameInputProvider.OnR += ReloadPerformed;
            _gameInputProvider.OnTabPerformed += OnTabPerformed;
            _gameInputProvider.OnTabCanceled += OnTabCanceled;
            _gameInputProvider.OnEsc += OnEscPreformed;
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
            _gameInputProvider.OnSpace -= JumpPerformed;
            _gameInputProvider.OnR -= ReloadPerformed;
            _gameInputProvider.OnTabPerformed -= OnTabPerformed;
            _gameInputProvider.OnTabCanceled -= OnTabCanceled;
            _gameInputProvider.OnEsc -= OnEscPreformed;
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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void AttackCanceled()
        {
            _controllable.CancelAttack();
        }

        public void Update()
        {
            if (IsBusy)
            {
                return;
            }
         
            _controllable.Move(_gameInputProvider.GetMovementNormalized());
            _controllable.MoveCamera(_gameInputProvider.GetMouseLook() * _mouseSensivityProvider.Sensivity);
        }

        private void ReloadPerformed()
        {
            if (IsBusy)
            {
                return;
            }
            
            _controllable.Reload();
        }

        private void OnEscPreformed()
        {
            IsBusy = !IsBusy;
            if (IsBusy)
            {
                _escScreenPresenter.Show();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                _escScreenPresenter.Hide();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
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
    }
}