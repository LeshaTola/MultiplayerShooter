using System;
using System.Linq;
using System.Runtime.InteropServices;
using App.Scripts.Features.Commands;
using App.Scripts.Features.GameMods.Providers;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Modules.PopupAndViews.Popups.Info;
using App.Scripts.Scenes.MainMenu.Features.RoomsProviders;
using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
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
        public event Action OnConnectionFinished;
        public event Action OnJoinedRoomEvent;

        private MapsProvider _mapsProvider;
        private InfoPopupRouter _infoPopupRouter;
        private RoomsProvider _roomsProvider;
        private GameModProvider _gameModProvider;

        [Inject]
        public void Constructor(MapsProvider mapsProvider,
            GameModProvider gameModProvider,
            InfoPopupRouter infoPopupRouter,
            InputFieldPopupRouter inputFieldPopupRouter,
            RoomsProvider roomsProvider)
        {
            _mapsProvider = mapsProvider;
            _gameModProvider = gameModProvider;
            _roomsProvider = roomsProvider;
            _infoPopupRouter = infoPopupRouter;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                return;
            }

            SetupRegion();
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
            OnConnectionFinished?.Invoke();
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
                        && x.PlayerCount > 0
                        && x.PlayerCount < x.MaxPlayers
                        && !x.CustomProperties.TryGetValue("Password", out _));
            if (availableRoom == null)
            {
                OnJoinRandomFailed(300, "Ne smog sdelat komaty");
                return;
            }

            if (availableRoom.CustomProperties.TryGetValue("GameMode", out var gameMode))
            {
                _gameModProvider.SetGameMod(gameMode.ToString());
            }

            if (!PhotonNetwork.IsConnectedAndReady)
            {
                _infoPopupRouter.ShowPopup(ConstStrings.ERROR, ConstStrings.CANNOT_JOIN_ROOM).Forget();
                return;
            }

            PhotonNetwork.JoinRoom(availableRoom.Name);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            string roomName = $"Room_{Random.Range(0, 1000)}";
            _gameModProvider.SetRandomGameMod();
            _mapsProvider.SetRandomMap();
            var options = new RoomOptions
            {
                MaxPlayers = (byte) _gameModProvider.CurrentGameMod.Players.Max,
                IsOpen = true,
                IsVisible = true,
                EmptyRoomTtl = 0,
                CustomRoomProperties = new Hashtable
                {
                    {"Map", _mapsProvider.MapConfig.Name},
                    {"GameMode", _gameModProvider.CurrentGameMod.Name}
                },
                CustomRoomPropertiesForLobby = new[] {"Map", "GameMode"}
            };

            PhotonNetwork.CreateRoom(roomName, options);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Room {returnCode}: {message}");
            _infoPopupRouter.ShowPopup(ConstStrings.ERROR, ConstStrings.CANNOT_JOIN_ROOM).Forget();
        }

        private bool _wasInRoom;

        private async void TryReconnect()
        {
            _wasInRoom = PhotonNetwork.InRoom;

            if (PhotonNetwork.IsConnected && PhotonNetwork.NetworkClientState == ClientState.Joined)
            {
                Debug.Log("Попытка ReconnectAndRejoin");

                if (!PhotonNetwork.ReconnectAndRejoin())
                {
                    Debug.LogWarning("ReconnectAndRejoin не сработал, пробуем полное переподключение");
                    PhotonNetwork.Disconnect();
                    await WaitForDisconnect();
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private async UniTask WaitForDisconnect()
        {
            while (PhotonNetwork.IsConnected)
            {
                await UniTask.Delay(100);
            }
        }

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void ReloadPage();
#endif

        public void Reload()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ReloadPage();
#else
            Debug.LogWarning("Reload game");
#endif
        }

        private void SetupRegion()
        {
            var region = YG2.saves.Region;
            if (!string.IsNullOrEmpty(region))
            {
                PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
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