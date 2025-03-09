using System;
using App.Scripts.Features.Commands;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Modules.Commands.General;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using YG;
using Zenject;
using Random = UnityEngine.Random;

namespace App.Scripts.Features
{
    public class ConnectionProvider : MonoBehaviourPunCallbacks
    {
        public const string NAME_DATA = "playerName";
        public event Action OnConnectionFinished;
        public event Action OnJoinedRoomEvent;

        private MapsProvider _mapsProvider;
        private InfoPopupRouter _infoPopupRouter;

        [Inject]
        public void Constructor(MapsProvider mapsProvider, InfoPopupRouter infoPopupRouter)
        {
            _mapsProvider = mapsProvider;
            _infoPopupRouter = infoPopupRouter;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                return;
            }

#if YANDEX
            var name = YG2.player.auth ? YG2.player.name : $"Player {Random.Range(0, 1000)}";
            var playerName = !string.IsNullOrEmpty(YG2.saves.PlayerName) ? YG2.saves.PlayerName : name;
            YG2.saves.PlayerName = playerName;
            YG2.SaveProgress();
#else
            var playerName = PlayerPrefs.HasKey(NAME_DATA)? PlayerPrefs.GetString(NAME_DATA): $"Player {Random.Range(0, 1000)}";
            PlayerPrefs.SetString(playerName, NAME_DATA);
#endif
            PhotonNetwork.NickName = playerName;

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Server");

            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Connected To Lobby");
            OnConnectionFinished?.Invoke();
        }
        
        public override void OnJoinedRoom()
        {
            OnJoinedRoomEvent?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _infoPopupRouter.ShowPopup(new InfoPopupData()
            {
                Header = ConstStrings.ERROR,
                Mesage = ConstStrings.CONNECTION_ERROR,
                #if YANDEX
                Command = new CustomCommand(ConstStrings.RECСONECT,TryReconnect)
                // Command = new CustomCommand(ConstStrings.RECСONECT, ReloadPage)
                #else
                Command = new CustomCommand(ConstStrings.RECСONECT,TryReconnect)
                #endif
            }).Forget();
        }

        public void QuickGame()
        {
            string roomName = $"Room_{Random.Range(0, 1000)}";
            var options = new RoomOptions
            {
                MaxPlayers = (byte) 10,
                IsOpen = true,
                IsVisible = true
            };
            _mapsProvider.SetRandomMap();
            PhotonNetwork.JoinRandomOrCreateRoom(roomName: roomName, roomOptions: options);
        }

        private void ReloadPage()
        {
            Application.OpenURL(Application.absoluteURL);
        }
        
        private void TryReconnect()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.ReconnectAndRejoin();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public string PlayerName = string.Empty;
    }
}