using System;
using App.Scripts.Features.GameMods.Providers;
using App.Scripts.Modules.StateMachine.Services.UpdateService;
using Photon.Pun;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Timer
{
    public class TimerProvider : MonoBehaviourPunCallbacks, IUpdatable
    {
        public event Action<double> OnTimerTick;
        public event Action OnTimerExpired;

        private double _startTime;
        private bool _timerRunning;
        private GameModProvider _gameModProvider;

        public double LocalStartTime { get; private set; }

        [Inject]
        public void Construct(GameModProvider gameModProvider)
        {
            _gameModProvider = gameModProvider;
        }

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

            var remainingTime = GetRemainingTime();
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
            if (PhotonNetwork.IsMasterClient)
            {
                if (_timerRunning)
                {
                    photonView.RPC(nameof(StartTimer), RpcTarget.AllBuffered, _startTime);
                }
                else
                {
                    Initialize();
                }
            }
        }

        private double GetRemainingTime()
        {
            return _gameModProvider.CurrentGameMod.GameConfig.MatchDurationTime - (PhotonNetwork.Time - _startTime);
        }
    }
}