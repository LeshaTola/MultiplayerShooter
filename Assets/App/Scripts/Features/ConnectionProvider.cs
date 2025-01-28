using System;
using App.Scripts.Features.Match.Maps;
using App.Scripts.Scenes.Gameplay.Timer;
using Photon.Pun;
using UnityEngine;
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

            var playerName = PlayerPrefs.HasKey(NAME_DATA)? PlayerPrefs.GetString(NAME_DATA): $"Player {Random.Range(0, 1000)}";
            PlayerPrefs.SetString(playerName, NAME_DATA);
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
            PhotonNetwork.LoadLevel("Gameplay");
        }

        public void QuickGame()
        {
            _mapsProvider.SetRandomMap();
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
    }
}