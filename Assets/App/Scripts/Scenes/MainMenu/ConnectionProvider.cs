using System.Collections.Generic;
using App.Scripts.Scenes.MainMenu.Screens;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu
{
    public class ConnectionProvider : MonoBehaviourPunCallbacks
    {
        [SerializeField] private RoomsView _roomsView;
        public RoomsViewProvider ViewProvider { get; private set; }

        public static ConnectionProvider Instance { get; private set; }
        
        private void Start()
        {
            Instance = this;
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

            ViewProvider = new RoomsViewProvider(_roomsView);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Update rooms in provider");
            ViewProvider.UpdateRoomList(roomList);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }
    }
}
