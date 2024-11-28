using System.Collections.Generic;
using App.Scripts.Scenes.MainMenu.Screens;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu
{
    public class ConnectionProvider : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                return;
            }
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
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Update rooms in provider");
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }
    }
}
