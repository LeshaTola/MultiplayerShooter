using System;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Timer
{
    public class TimerProvider : MonoBehaviourPun
    {
        public event Action<double> OnTimerTick;
        public event Action OnTimerExpired;
        
        [SerializeField] private  double _matchDurationTime;
        
        private double _startTime;
        private bool _timerRunning;
        public void Initialize()
        {
            photonView.RPC(nameof(StartTimer), RpcTarget.AllBuffered, PhotonNetwork.Time);
        }

        [PunRPC]
        public void StartTimer(double time)
        {
            _startTime = time;
            _timerRunning = true;
        }
        
        private void Update()
        {
            if (!_timerRunning)
            {
                return;
            }
            
            var remainingTime =  _matchDurationTime - (PhotonNetwork.Time - _startTime);
            if (remainingTime >= 0)
            {
                OnTimerTick?.Invoke(remainingTime);
                return;
            }
            
            OnTimerExpired?.Invoke();
            _timerRunning = false;
        }
    }
}