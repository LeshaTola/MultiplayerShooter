using System;
using App.Scripts.Features.Match.Configs;
using App.Scripts.Modules.StateMachine.Services.InitializeService;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Timer
{
    public class TimerProvider : MonoBehaviourPunCallbacks, IInitializable, IUpdatable
    {
        public event Action<double> OnTimerTick;
        public event Action OnTimerExpired;

        [SerializeField] private GameConfig _gameConfig;
        
        private double _startTime;
        private bool _timerRunning;

        public double LocalStartTime {get; private set;}

        public void Initialize()
        {
            LocalStartTime = PhotonNetwork.Time;
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

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            if (_timerRunning && newMasterClient.IsMasterClient)
            {
                photonView.RPC(nameof(StartTimer), RpcTarget.AllBuffered, _startTime);
            }
        }
    }
}