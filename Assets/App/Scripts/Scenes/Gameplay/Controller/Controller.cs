using App.Scripts.Features.Input;
using App.Scripts.Scenes.Gameplay.Controller.Providers;
using App.Scripts.Scenes.Gameplay.Esc;
using App.Scripts.Scenes.Gameplay.Esc.Menu;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private LeaderBoard.LeaderBoard _leaderBoard;

        private IControllable _controllable;
        private GameInputProvider _gameInputProvider;
        private MouseSensivityProvider _mouseSensivityProvider;
        private EscScreenPresenter _escScreenPresenter;

        public bool IsBusy { get; set; } = false;

        public void Initialize(GameInputProvider gameInputProvider,
            IControllable controllable,
            MouseSensivityProvider mouseSensivityProvider, 
            EscScreenPresenter escScreenPresenter)
        {
            _gameInputProvider = gameInputProvider;
            _controllable = controllable;
            _mouseSensivityProvider = mouseSensivityProvider;
            _escScreenPresenter = escScreenPresenter;

            _gameInputProvider.OnLeftMouseStarted += AttackStarted;
            _gameInputProvider.OnLeftMouseCanceled += AttackCanceled;
            _gameInputProvider.OnSpace += JumpPerformed;
            _gameInputProvider.OnR += ReloadPerformed;
            _gameInputProvider.OnTabPerformed += OnTabPerformed;
            _gameInputProvider.OnTabCanceled += OnTabCanceled;
            _gameInputProvider.OnEsc += OnEscPreformed;
        }

        private void OnDisable()
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

        private void Update()
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
            
            _leaderBoard.Show();
        }

        private void OnTabCanceled()
        {
            if (IsBusy)
            {
                return;
            }
            
            _leaderBoard.Hide();
        }
    }
}