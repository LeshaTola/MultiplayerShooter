using System;
using System.Collections.Generic;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.MainMenu
{
    public class ConnectionProvider : MonoBehaviourPunCallbacks
    {
        public const string NAME_DATA = "playerName";

        private RoomsViewPresenter _roomsViewPresenter;
        private StateMachine _stateMachine;

        public event Action OnConnected; 

        [Inject]
        private void Construct(RoomsViewPresenter roomsViewPresenter, StateMachine stateMachine)
        {
            _roomsViewPresenter = roomsViewPresenter;
            _stateMachine = stateMachine;
        }

        private void Start()
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
            OnConnected?.Invoke();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Update rooms in provider");
            _roomsViewPresenter.UpdateRoomList(roomList);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Gameplay");
            //await _stateMachine.ChangeState<LoadSceneState>();
        }
    }
}