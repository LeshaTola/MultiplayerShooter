using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

public class PauseHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private int _lobbyExitTime = 60;
    private CancellationTokenSource _exitTimerCts;

    private void Awake()
    { 
         YG2.onFocusWindowGame += OnGamePaused;
    }

    private void OnGamePaused(bool isPaused)
    {
        if (!isPaused)
        {
            // Если таймер не был отменён, выходим из лобби
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                // Передаём хост другому игроку перед выходом
                PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
            }
            
            StartExitTimer().Forget();
        }
        else
        {
            // Если окно вернулось в фокус, отменяем таймер
            CancelExitTimer();
        }
    }

    private async UniTaskVoid StartExitTimer()
    {
        // Отменяем предыдущий таймер, если он был
        CancelExitTimer();

        _exitTimerCts = new CancellationTokenSource();

        try
        {
            // Ждём 30 секунд с возможностью отмены
            await UniTask.Delay((int)(_lobbyExitTime * 1000), cancellationToken: _exitTimerCts.Token);
            
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        catch (System.OperationCanceledException)
        {
            // Таймер был отменён (игрок вернулся в игру)
        }
    }

    private void CancelExitTimer()
    {
        _exitTimerCts?.Cancel();
        _exitTimerCts?.Dispose();
        _exitTimerCts = null;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"Новый хост: {newMasterClient.NickName}");
    }

    private void OnDestroy()
    {
        YG2.onFocusWindowGame -= OnGamePaused;
        CancelExitTimer(); // Очистка при уничтожении объекта
    }
}