using System;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using Photon.Pun;

namespace App.Scripts.Scenes.Gameplay.Timer
{
    public class TimerProvider : MonoBehaviourPun, IInitializable, IUpdatable
    {
        public event Action<double> OnTimerTick;
        public event Action OnTimerExpired;

        private readonly GameConfig _gameConfig;
        
        private double _startTime;
        private bool _timerRunning;

        public TimerProvider(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public void Initialize()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            photonView.RPC(nameof(StartTimer), RpcTarget.AllBuffered, PhotonNetwork.Time);
        }

        void IUpdatable.Update()
        {
            if (!_timerRunning)
            {
                return;
            }

            var remainingTime = _gameConfig.MatchDurationTime - (PhotonNetwork.Time - _startTime);
            if (remainingTime >= 0)
            {
                OnTimerTick?.Invoke(remainingTime);
                return;
            }

            OnTimerExpired?.Invoke();
            _timerRunning = false;
        }

        [PunRPC]
        public void StartTimer(double time)
        {
            _startTime = time;
            _timerRunning = true;
        }
    }
}