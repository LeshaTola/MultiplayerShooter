using System;
using System.Linq;
using App.Scripts.Features.Commands;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.RoomsProviders;
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
        private RoomsProvider _roomsProvider;

        [Inject]
        public void Constructor(MapsProvider mapsProvider,
            InfoPopupRouter infoPopupRouter,
            RoomsProvider roomsProvider)
        {
            _mapsProvider = mapsProvider;
            _roomsProvider = roomsProvider;
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
            var playerName =
 PlayerPrefs.HasKey(NAME_DATA)? PlayerPrefs.GetString(NAME_DATA): $"Player {Random.Range(0, 1000)}";
            PlayerPrefs.SetString(playerName, NAME_DATA);
#endif
            PhotonNetwork.NickName = playerName;

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"Connected To Server. Region: {PhotonNetwork.CloudRegion}");
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
                Command = new CustomCommand(ConstStrings.RECСONECT, Reconnect)
            }).Forget();
        }

        public void Reconnect()
        {
            TryReconnect();
            _infoPopupRouter.HidePopup().Forget();
        }
        
        public void QuickGame()
        {
            var availableRoom
                = _roomsProvider
                    .Rooms
                    .FirstOrDefault(x =>
                        x.IsOpen
                        && x.IsVisible
                        && x.PlayerCount < 10
                        && !x.CustomProperties.TryGetValue("Password", out _));
            if (availableRoom == null)
            {
                OnJoinRandomFailed(300,"Ne smog sdelat komaty");
                return;
            }
        
            PhotonNetwork.JoinRoom(availableRoom.Name);
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            string roomName = $"Room_{Random.Range(0, 1000)}";
            _mapsProvider.SetRandomMap();
            var options = new RoomOptions
            {
                MaxPlayers = (byte) 10,
                IsOpen = true,
                IsVisible = true,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    {"Map", _mapsProvider.MapConfig.Name},
                    {"GameMode", "PVP"}
                },
                CustomRoomPropertiesForLobby = new[] {"Map", "GameMode"}
            };
        
            PhotonNetwork.CreateRoom(roomName, options);
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