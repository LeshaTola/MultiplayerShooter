using System;
using App.Scripts.Features.Match.Maps;
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

        private MapsProvider _mapsProvider;

        [Inject]
        public void Constructor(MapsProvider mapsProvider)
        {
            _mapsProvider = mapsProvider;
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

        public void QuickGame()
        {
            Debug.Log("Состояние сети: " + PhotonNetwork.NetworkClientState);
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
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public string PlayerName = string.Empty;
    }
}