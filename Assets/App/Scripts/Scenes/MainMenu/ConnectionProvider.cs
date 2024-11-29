using System.Collections.Generic;
using App.Scripts.Modules.StateMachine;
using App.Scripts.Scenes.MainMenu.Screens.RoomsViews;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenu
{
    public class ConnectionProvider : MonoBehaviourPunCallbacks
    {
        private RoomsViewPresenter _roomsViewPresenter;
        private StateMachine _stateMachine;


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
            _roomsViewPresenter.UpdateRoomList(roomList);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Gameplay");
            //await _stateMachine.ChangeState<LoadSceneState>();
        }
    }
}